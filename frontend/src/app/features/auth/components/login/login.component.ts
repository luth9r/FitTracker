import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs/operators';
import { AuthService, LoginResponse, RegisterPayload } from '../services/auth.service';
import { OAuthService } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';

type AuthMode = 'login' | 'register';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  private authService = inject(AuthService);
  private translate = inject(TranslateService);
  private oauthService = inject(OAuthService);
  private router = inject(Router);

  email = '';
  password = '';
  confirmPassword = '';
  username = '';

  showPassword = false;
  showConfirmPassword = false;
  authMode: AuthMode = 'login';
  isLoading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  isFormPristine = true;

  usernameValidationState = { minLength: false, noSpaces: false };
  passwordValidationState = { minLength: false, oneLetter: false, oneNumber: false };

  private googleIntent: AuthMode = 'login';

  constructor() {
    console.log('[INFO] LoginComponent initialized, OAuthService already configured');

    window.addEventListener('message', (event) => {
      if (event.origin !== window.location.origin) {
        return; 
      }

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

  toggleMode(mode: 'login' | 'register') {
    this.authMode = mode;
    this.resetState();
    this.isFormPristine = true;
    this.email = '';
    this.password = '';
    this.confirmPassword = '';
    this.username = '';
  }

  togglePassword() { this.showPassword = !this.showPassword; }
  toggleConfirmPassword() { this.showConfirmPassword = !this.showConfirmPassword; }

  validateUsernameOnTheFly(username: string) {
    this.isFormPristine = false;
    this.usernameValidationState.minLength = username.length >= 3;
    this.usernameValidationState.noSpaces = !/\s/.test(username);
  }

  validatePasswordOnTheFly(password: string) {
    this.isFormPristine = false;
    this.passwordValidationState.minLength = password.length >= 8;
    this.passwordValidationState.oneLetter = /[a-zA-Z]/.test(password);
    this.passwordValidationState.oneNumber = /\d/.test(password);
  }

  handleSubmit() {
    this.isFormPristine = false;
    this.resetState(true);

    if (this.authMode === 'login') {
      if (!this.email.trim() || !this.password.trim()) {
        this.errorMessage = 'LOGIN.ERRORS.EMPTY_FIELDS';
        this.isLoading = false;
        return;
      }

      this.authService.login(this.email, this.password)
        .pipe(finalize(() => this.isLoading = false))
        .subscribe({
          next: (response) => this.handleLoginSuccess(response),
          error: (err) => this.handleError(err, 'login')
        });

    } else if (this.authMode === 'register') {
      if (!this.username.trim() || !this.email.trim() || !this.password.trim()) {
        this.errorMessage = 'LOGIN.ERRORS.EMPTY_FIELDS';
        this.isLoading = false;
        return;
      }

      this.validateUsernameOnTheFly(this.username);
      this.validatePasswordOnTheFly(this.password);

      const isUsernameValid = Object.values(this.usernameValidationState).every(v => v);
      const isPasswordValid = Object.values(this.passwordValidationState).every(v => v);

      if (this.password !== this.confirmPassword) {
        this.errorMessage = 'LOGIN.ERRORS.PASSWORDS_DO_NOT_MATCH';
        this.isLoading = false;
        return;
      }

      if (!isUsernameValid || !isPasswordValid) {
        this.errorMessage = 'LOGIN.ERRORS.VALIDATION_FAILED';
        this.isLoading = false;
        return;
      }

      const payload: RegisterPayload = { username: this.username, email: this.email, password: this.password };
      this.authService.register(payload)
        .pipe(finalize(() => this.isLoading = false))
        .subscribe({
          next: (response) => this.handleLoginSuccess(response, true),
          error: (err) => this.handleError(err, 'register')
        });
    }
  }

  // ===================================================================
  // This method now ONLY opens the window and saves the "intent".
  // It does not process idToken.
  // ===================================================================
  handleGoogleLogin(intent: 'login' | 'register') {
    this.resetState(true);
    // 1. Save the "intent"
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
        // Doing nothing here, as the message event listener will handle the rest.
      })
      .catch((err) => {
        const oauthError = err as any;
        if (oauthError?.type === 'popup_closed') {
          console.log('[INFO] Google Sign-In (init): Popup closed by user (err).');
        } else {
          console.error('[ERROR] Google Sign-In (init) Error:', err); 
        }
      });
  }

  private handleLoginSuccess(response: LoginResponse, isRegistration = false) {
    this.successMessage = isRegistration ? 'LOGIN.SUCCESS.REGISTER' : 'LOGIN.SUCCESS.LOGIN';

    setTimeout(() => {
      this.router.navigate(['/home']);
    }, 1000);
  }

  private handleError(err: unknown, context: string) {
    console.error(`[ERROR] Error in context "${context}":`, err);

    if (err instanceof HttpErrorResponse) {
      if (err.status === 400 && err.error && err.error.errors) {
        // .Net validation error
        const errors = err.error.errors;
        const firstErrorKey = Object.keys(errors)[0];
        this.errorMessage = errors[firstErrorKey][0];
      } else if (err.status === 0) {
        // API unreachable
        this.errorMessage = 'LOGIN.ERRORS.API_UNREACHABLE';
      } else if (err.status === 401 || err.status === 404 || err.status === 409) {
        this.errorMessage = err.error?.messageKey || 'LOGIN.ERRORS.UNKNOWN';
      } else {
        this.errorMessage = 'LOGIN.ERRORS.UNKNOWN';
      }
    } else {
      const maybeError = (err as any)?.error;
      if (maybeError === 'popup_closed_by_user') {
        console.log('[INFO] User closed the popup without completing Google Sign-In.');
        this.isLoading = false;
        return;
      }
      this.errorMessage = 'LOGIN.ERRORS.UNKNOWN_GOOGLE';
    }
  }

  private resetState(isLoading = false) {
    this.isLoading = isLoading;
    this.errorMessage = null;
    this.successMessage = null;
  }
}
