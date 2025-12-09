import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { finalize } from 'rxjs/operators';
import { AuthService, LoginResponse, RegisterPayload } from '../../services/auth.service';
import { OAuthService } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';
import { ValidationService } from '../../services/validation.service';
import { ErrorHandlingService } from '../../services/error-handling.service';
import { LoginFormComponent, LoginFormData } from './forms/login-form/login-form.component';
import { RegisterFormComponent, RegisterFormData } from './forms/register-form/register-form.component';

type AuthMode = 'login' | 'register';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    LoginFormComponent,
    RegisterFormComponent
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  private authService = inject(AuthService);
  private translate = inject(TranslateService);
  private oauthService = inject(OAuthService);
  private router = inject(Router);
  private validationService = inject(ValidationService);
  private errorHandlingService = inject(ErrorHandlingService);

  // UI state
  authMode: AuthMode = 'login';
  isLoading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  activeField: string | null = null;

  // Validation states (только для register)
  usernameValidationState = { minLength: false, noSpaces: false };
  passwordValidationState = { minLength: false, oneLetter: false, oneNumber: false };
  confirmPasswordValidationState = { matches: false };

  // Temporary state for validation
  private registerPassword = '';
  private registerConfirmPassword = '';

  private googleIntent: AuthMode = 'login';

  constructor() {
    console.log('[INFO] LoginComponent initialized, OAuthService already configured');

    window.addEventListener('message', (event) => {
      if (event.origin !== window.location.origin) return;

      const message = event.data;
      const queryString = message.startsWith('?') || message.startsWith('??')
        ? message.replace(/^\?+/, '')
        : message;
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

        action.pipe(
          finalize(() => {
            this.isLoading = false;
            sessionStorage.removeItem('pkce_verifier');
          })
        ).subscribe({
          next: (response) => this.handleLoginSuccess(response, isRegistration),
          error: (err) => this.handleError(err, `google-${this.googleIntent}`)
        });
      } else {
        if (!code) console.warn('[WARN] Google Flow: "code" not found in message.');
        if (!codeVerifier) console.warn('[WARN] Google Flow: "codeVerifier" not found in sessionStorage.');
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
      this.errorMessage = 'LOGIN.ERRORS.EMPTY_FIELDS';
      this.isLoading = false;
      return;
    }

    this.authService.login(formData.email, formData.password)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: (response) => this.handleLoginSuccess(response),
        error: (err) => this.handleError(err, 'login')
      });
  }

  // Register form submit
  onRegisterSubmit(formData: RegisterFormData) {
    this.resetState(true);

    if (!formData.username.trim() || !formData.email.trim() || 
        !formData.password.trim() || !formData.confirmPassword.trim()) {
      this.errorMessage = 'LOGIN.ERRORS.EMPTY_FIELDS';
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

    const isUsernameValid = Object.values(this.usernameValidationState).every(v => v);
    const isPasswordValid = Object.values(this.passwordValidationState).every(v => v);
    const isConfirmPasswordValid = this.confirmPasswordValidationState.matches;

    if (!isUsernameValid || !isPasswordValid || !isConfirmPasswordValid) {
      this.errorMessage = 'LOGIN.ERRORS.VALIDATION_FAILED';
      this.isLoading = false;
      return;
    }

    const payload: RegisterPayload = { 
      username: formData.username, 
      email: formData.email, 
      password: formData.password 
    };
    
    this.authService.register(payload)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: (response) => this.handleLoginSuccess(response, true),
        error: (err) => this.handleError(err, 'register')
      });
  }

  handleGoogleLogin(intent: 'login' | 'register') {
    this.resetState(true);
    this.googleIntent = intent;

    if (!this.oauthService.issuer) {
      console.error('[ERROR] OAuthService not configured! Check app.config.ts');
      this.errorMessage = 'LOGIN.ERRORS.GOOGLE_CONFIG_ERROR';
      this.isLoading = false;
      return;
    }

    console.log(`[INFO] Starting Google Sign-In (PKCE) with intent: ${intent}`);
    
    this.oauthService.initLoginFlowInPopup()
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
    this.successMessage = isRegistration ? 'LOGIN.SUCCESS.REGISTER' : 'LOGIN.SUCCESS.LOGIN';
    setTimeout(() => {
      this.router.navigate(['/']);
    }, 1000);
  }

  private handleError(err: unknown, context: string) {
    const errorMessage = this.errorHandlingService.handleAuthError(err, context);
    
    if (errorMessage) {
      this.errorMessage = errorMessage;
    } else {
      this.isLoading = false;
    }
  }

  private resetState(isLoading = false) {
    this.isLoading = isLoading;
    this.errorMessage = null;
    this.successMessage = null;
  }

  private resetValidationState() {
    this.registerPassword = '';
    this.registerConfirmPassword = '';
    this.usernameValidationState = { minLength: false, noSpaces: false };
    this.passwordValidationState = { minLength: false, oneLetter: false, oneNumber: false };
    this.confirmPasswordValidationState = { matches: false };
  }
}
