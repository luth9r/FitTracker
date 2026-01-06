import { Injectable } from '@angular/core';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export interface UsernameValidationState {
  minLength: boolean;
  noSpaces: boolean;
}

export interface PasswordValidationState {
  minLength: boolean;
  oneLetter: boolean;
  oneNumber: boolean;
}

export interface ConfirmPasswordValidationState {
  matches: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class ValidationService {
  // Username validation
  validateUsername(username: string): UsernameValidationState {
    return {
      minLength: username.length >= 3,
      noSpaces: !/\s/.test(username),
    };
  }

  // Password validation
  validatePassword(password: string): PasswordValidationState {
    return {
      minLength: password.length >= 8,
      oneLetter: /[a-zA-Z]/.test(password),
      oneNumber: /\d/.test(password),
    };
  }

  validateEmail(email: string): boolean {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(email);
  }

  // Confirm password validation
  validateConfirmPassword(
    password: string,
    confirmPassword: string
  ): ConfirmPasswordValidationState {
    return {
      matches: confirmPassword === password && confirmPassword.length > 0,
    };
  }

  // Custom validators for Reactive Forms (for future use)
  usernameValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value || '';
      const state = this.validateUsername(value);

      if (!state.minLength || !state.noSpaces) {
        return { username: state };
      }
      return null;
    };
  }

  // Password validator
  passwordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value || '';
      const state = this.validatePassword(value);

      if (!state.minLength || !state.oneLetter || !state.oneNumber) {
        return { password: state };
      }
      return null;
    };
  }

  // Confirm password validator
  confirmPasswordValidator(passwordControl: AbstractControl): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = passwordControl.value || '';
      const confirmPassword = control.value || '';
      const state = this.validateConfirmPassword(password, confirmPassword);

      if (!state.matches) {
        return { confirmPassword: state };
      }
      return null;
    };
  }
}
