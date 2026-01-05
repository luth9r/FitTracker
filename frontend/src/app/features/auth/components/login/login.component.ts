import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { finalize } from 'rxjs/operators';
import { AuthService, LoginResponse, RegisterPayload } from '../../services/auth.service';
import { OAuthService } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';
import { ValidationService } from '../../services/validation.service';
import { ErrorHandlingService } from '../../services/error-handling.service';
import { ToastService } from '../../../../shared/ui/toast/service/toast.service';
import { LoginFormComponent, LoginFormData } from './forms/login-form/login-form.component';
import {
  RegisterFormComponent,
  RegisterFormData,
} from './forms/register-form/register-form.component';
import { ToastComponent } from '../../../../shared/ui/toast/component/toast.component';
import { ModalService } from '../../../../shared/utils/modal.service';
import {
  EmailVerificationModalComponent,
  EmailVerificationData
} from '../ui/email-verification-modal/email-verification-modal.component';

type AuthMode = 'login' | 'register';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    LoginFormComponent,
    RegisterFormComponent,
    ToastComponent,
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  private authService = inject(AuthService);
  private toast = inject(ToastService);
  private oauthService = inject(OAuthService);
  private router = inject(Router);
  private validationService = inject(ValidationService);
  private errorHandlingService = inject(ErrorHandlingService);
  private modalService = inject(ModalService);

  // UI state
  authMode: AuthMode = 'login';
  isLoading = false;
  activeField: string | null = null;

  // Validation states (только для register)
  usernameValidationState = { minLength: false, noSpaces: false };
  passwordValidationState = { minLength: false, oneLetter: false, oneNumber: false };
  confirmPasswordValidationState = { matches: false };

  // Temporary state for validation
  private registerPassword = '';
  private registerConfirmPassword = '';
  private registeredEmail = '';

  private googleIntent: AuthMode = 'login';

  constructor() {
    console.log('[INFO] LoginComponent initialized, OAuthService already configured');

    window.addEventListener('message', (event) => {
      if (event.origin !== window.location.origin) return;

      const message = event.data;
      const queryString =
        message.startsWith('?') || message.startsWith('??') ? message.replace(/^\?+/, '') : message;
      const params = new URLSearchParams(queryString);
      const code = params.get('code');
      const codeVerifier = sessionStorage.getItem('PKCE_verifier');

      console.log('[DEBUG] Extracted authorization code (PKCE code):', code);
      console.log('[DEBUG] Extracted codeVerifier from sessionStorage:', codeVerifier);

      if (code && codeVerifier) {
        console.log(`[INFO] Google Flow: received code and verifier. Intent: ${this.googleIntent}`);

        const isRegistration = this.googleIntent === 'register';
        const action = isRegistration
          ? this.authService.googleRegister(code, codeVerifier)
          : this.authService.googleLogin(code, codeVerifier);

        action
          .pipe(
            finalize(() => {
              this.isLoading = false;
              sessionStorage.removeItem('pkce_verifier');
            })
          )
          .subscribe({
            next: (response) => this.handleLoginSuccess(response, isRegistration),
            error: (err) => this.handleError(err, `google-${this.googleIntent}`),
          });
      } else {
        if (!code) console.warn('[WARN] Google Flow: "code" not found in message.');
        if (!codeVerifier)
          console.warn('[WARN] Google Flow: "codeVerifier" not found in sessionStorage.');
        this.isLoading = false;
      }
    });
  }

  // Field management
  setActiveField(fieldName: string) {
    this.activeField = fieldName;
  }

  clearActiveField() {
    this.activeField = null;
  }

  toggleMode(mode: AuthMode) {
    this.authMode = mode;
    this.resetState();
    this.clearActiveField();
    this.resetValidationState();
  }

  // Validation handler for register form
  onValidateChange(field: string, value: string) {
    switch (field) {
      case 'username':
        this.usernameValidationState = this.validationService.validateUsername(value);
        break;
      case 'password':
        this.registerPassword = value;
        this.passwordValidationState = this.validationService.validatePassword(value);

        if (this.registerConfirmPassword) {
          this.confirmPasswordValidationState = this.validationService.validateConfirmPassword(
            this.registerPassword,
            this.registerConfirmPassword
          );
        }
        break;
      case 'confirmPassword':
        this.registerConfirmPassword = value;
        this.confirmPasswordValidationState = this.validationService.validateConfirmPassword(
          this.registerPassword,
          value
        );
        break;
    }
  }

  // Login form submit
  onLoginSubmit(formData: LoginFormData) {
    this.resetState(true);

    if (!formData.email.trim() || !formData.password.trim()) {
      this.toast.show('error', 'LOGIN.ERRORS.EMPTY_FIELDS');
      this.isLoading = false;
      return;
    }

    this.authService
      .login(formData.email, formData.password)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (response) => this.handleLoginSuccess(response),
        error: (err) => this.handleError(err, 'login'),
      });
  }

  // Register form submit
  onRegisterSubmit(formData: RegisterFormData) {
    this.resetState(true);

    if (
      !formData.username.trim() ||
      !formData.email.trim() ||
      !formData.password.trim() ||
      !formData.confirmPassword.trim()
    ) {
      this.toast.show('error', 'LOGIN.ERRORS.EMPTY_FIELDS');
      this.isLoading = false;
      return;
    }

    // Validate
    this.usernameValidationState = this.validationService.validateUsername(formData.username);
    this.passwordValidationState = this.validationService.validatePassword(formData.password);
    this.confirmPasswordValidationState = this.validationService.validateConfirmPassword(
      formData.password,
      formData.confirmPassword
    );

    const isUsernameValid = Object.values(this.usernameValidationState).every((v) => v);
    const isPasswordValid = Object.values(this.passwordValidationState).every((v) => v);
    const isConfirmPasswordValid = this.confirmPasswordValidationState.matches;

    if (!isUsernameValid || !isPasswordValid || !isConfirmPasswordValid) {
      this.toast.show('error', 'LOGIN.ERRORS.VALIDATION_FAILED');
      this.isLoading = false;
      return;
    }

    const payload: RegisterPayload = {
      username: formData.username,
      email: formData.email,
      password: formData.password,
    };

    this.registeredEmail = formData.email;

    this.authService
      .register(payload)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (response) => {
          this.openEmailVerificationModal(formData.email);
          this.toast.show('success', 'LOGIN.SUCCESS.REGISTER');
        },
        error: (err) => this.handleError(err, 'register'),
      });
  }

  private openEmailVerificationModal(email: string) {
    const modalRef = this.modalService.open<EmailVerificationData>(
      EmailVerificationModalComponent,
      {
        data: { email },
        disableClose: false,
        panelClass: 'email-verification-modal',
        backdropClass: 'modal-backdrop-blur',
      }
    );

    modalRef.afterClosed$.subscribe((result) => {
      console.log('[INFO] Email verification modal closed', result);

      if (result?.action === 'resend') {
        this.handleResendVerification(email);
      }
    });
  }

  handleResendVerification(email: string) {
    console.log('[INFO] Resending verification email to:', email);

    this.isLoading = true;

    this.authService
      .resendVerificationEmail(email)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => {
          this.toast.show('success', 'VERIFICATION.RESEND_SUCCESS');
          console.log('[SUCCESS] Verification email resent successfully');
        },
        error: (err) => {
          console.error('[ERROR] Failed to resend verification email:', err);
          this.handleError(err, 'resend-verification');
        },
      });
  }

  handleGoogleLogin(intent: 'login' | 'register') {
    this.resetState(true);
    this.googleIntent = intent;

    if (!this.oauthService.issuer) {
      console.error('[ERROR] OAuthService not configured! Check app.config.ts');
      this.toast.show('error', 'LOGIN.ERRORS.GOOGLE_CONFIG_ERROR');
      this.isLoading = false;
      return;
    }

    console.log(`[INFO] Starting Google Sign-In (PKCE) with intent: ${intent}`);

    this.oauthService
      .initLoginFlowInPopup()
      .then((result) => {
        if (!result) {
          console.warn('[WARN] Google Sign-In: Popup closed or unsuccessful.');
          this.isLoading = false;
        }
      })
      .catch((err) => {
        if (this.errorHandlingService.isPopupClosedError(err)) {
          console.log('[INFO] Google Sign-In (init): Popup closed by user.');
          this.isLoading = false;
        } else {
          console.error('[ERROR] Google Sign-In (init) Error:', err);
        }
      });
  }

  private handleLoginSuccess(response: LoginResponse, isRegistration = false) {
    if (!isRegistration) {
      this.toast.show('success', 'LOGIN.SUCCESS.LOGIN');
      setTimeout(() => {
        this.router.navigate(['/']);
      }, 1000);
    }
  }

  private handleError(err: unknown, context: string) {
    const errorMessage = this.errorHandlingService.handleAuthError(err, context);

    if (errorMessage) {
      this.toast.show('error', errorMessage);
    } else {
      this.isLoading = false;
    }
  }

  private resetState(isLoading = false) {
    this.isLoading = isLoading;
  }

  private resetValidationState() {
    this.registerPassword = '';
    this.registerConfirmPassword = '';
    this.registeredEmail = '';
    this.usernameValidationState = { minLength: false, noSpaces: false };
    this.passwordValidationState = { minLength: false, oneLetter: false, oneNumber: false };
    this.confirmPasswordValidationState = { matches: false };
  }
}
