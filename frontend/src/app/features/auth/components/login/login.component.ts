import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import {  HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs/operators';
import { AuthService, GoogleNeedsRegistrationResponse, LoginResponse, RegisterPayload } from '../services/auth.service';

// Define the component's possible modes
type AuthMode = 'login' | 'register' | 'completeGoogle';

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
  // --- Services ---
  private authService = inject(AuthService);
  private translate = inject(TranslateService);

  // --- Form Data ---
  email = '';
  password = '';
  confirmPassword = '';
  username = ''; 
  
  // --- UI State ---
  showPassword = false;
  showConfirmPassword = false;
  authMode: AuthMode = 'login';
  isLoading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  isFormPristine = true; // To prevent errors before typing

  // Live validation state objects
  usernameValidationState = {
    minLength: false,
    noSpaces: false
  };
  passwordValidationState = {
    minLength: false,
    oneLetter: false,
    oneNumber: false
  };

  // Data received from Google (step 1) and stored for step 2
  googleData = {
    idToken: '',
    email: '',
    firstName: '',
    lastName: ''
  };

  toggleMode(mode: 'login' | 'register') {
    this.authMode = mode;
    this.resetState();
    this.isFormPristine = true; // Reset pristine state
    
    // NEW (UX Fix): Clear fields ONLY when switching modes
    this.email = '';
    this.password = '';
    this.confirmPassword = '';
    this.username = '';
  }

  togglePassword() { this.showPassword = !this.showPassword; }
  toggleConfirmPassword() { this.showConfirmPassword = !this.showConfirmPassword; }

  // --- Live Validation Triggers ---
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

  /**
   * Handles EITHER regular login OR regular registration
   */
  handleSubmit() {
    this.isFormPristine = false; // Mark form as "touched"
    this.resetState(true); // isLoading = true

    if (this.authMode === 'login') {
      
      // --- NEW: Check for empty login fields ---
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
      
      // --- NEW: Check for empty registration fields ---
      if (!this.username.trim() || !this.email.trim() || !this.password.trim()) {
        this.errorMessage = 'LOGIN.ERRORS.EMPTY_FIELDS';
        this.isLoading = false;
        return;
      }

      // 1. Run final validation checks
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
      
      const payload: RegisterPayload = {
        username: this.username,
        email: this.email,
        password: this.password
      };
      
      this.authService.register(payload)
        .pipe(finalize(() => this.isLoading = false))
        .subscribe({
          next: (response) => this.handleLoginSuccess(response, true),
          error: (err) => this.handleError(err, 'register')
        });
    }
  }

  /**
   * Starts the Google Sign-In process (Step 1)
   */
  handleGoogleLogin() {
    // ... (logic remains the same)
    this.resetState(true);
    console.log('TODO: Call Google Sign-In Library...');
    this.errorMessage = 'LOGIN.ERRORS.GOOGLE_NOT_IMPLEMENTED';
    this.isLoading = false;
  }

  /**
   * Processes the token received from Google (Step 1)
   */
  private processGoogleToken(idToken: string) {
    // ... (logic remains the same)
  }

  /**
   * Completes Google registration (Step 2)
   */
  handleCompleteGoogleRegistration() {
    this.isFormPristine = false; // Mark form as "touched"
    this.resetState(true);

    // Run live validation just in case
    this.validateUsernameOnTheFly(this.username);

    // --- NEW: Check for empty username ---
    if (!this.username.trim()) {
      this.errorMessage = 'LOGIN.ERRORS.EMPTY_FIELDS';
      this.isLoading = false;
      return;
    }
    
    if (!this.usernameValidationState.minLength || !this.usernameValidationState.noSpaces) {
      this.errorMessage = 'LOGIN.ERRORS.VALIDATION_FAILED';
      this.isLoading = false;
      return;
    }

    this.authService.completeGoogleRegistration(this.googleData.idToken, this.username)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: (response) => this.handleLoginSuccess(response, true),
        error: (err) => this.handleError(err, 'complete-google')
      });
  }

  /**
   * Generic handler for successful login/registration
   */
  private handleLoginSuccess(response: LoginResponse, isRegistration = false) {
    // ... (logic remains the same)
    this.successMessage = isRegistration ? 'LOGIN.SUCCESS.REGISTER' : 'LOGIN.SUCCESS.LOGIN';
  }

  /**
   * Generic handler for API errors
   */
  private handleError(err: HttpErrorResponse, context: string) {
    if (err.status === 400 && err.error && err.error.errors) {
      const errors = err.error.errors;
      const firstErrorKey = Object.keys(errors)[0];
      this.errorMessage = errors[firstErrorKey][0];
    } else if (err.status === 0) {
      this.errorMessage = 'LOGIN.ERRORS.API_UNREACHABLE';
    } else {
      this.errorMessage = 'LOGIN.ERRORS.UNKNOWN';
    }
  }

  private resetState(isLoading = false) {
    this.isLoading = isLoading;
    this.errorMessage = null;
    this.successMessage = null;
  }
}