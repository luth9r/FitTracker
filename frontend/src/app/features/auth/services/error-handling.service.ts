import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

export interface ErrorContext {
  type: 'login' | 'register' | 'google-login' | 'google-register' | 'general';
  error: unknown;
}

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlingService {
  /**
   * Handles errors and returns a translation key for display to the user
   */
  handleAuthError(err: unknown, context: string): string {
    console.error(`[ERROR] Error in context "${context}":`, err);

    // HTTP errors
    if (err instanceof HttpErrorResponse) {
      return this.handleHttpError(err);
    }

    // OAuth/Google errors
    const maybeError = (err as any)?.error;
    if (maybeError === 'popup_closed_by_user') {
      console.log('[INFO] User closed the popup without completing Google Sign-In.');
      return ''; // Empty string = do not show error
    }

    return 'LOGIN.ERRORS.UNKNOWN_GOOGLE';
  }

  /**
   * Handles HTTP errors
   */
  private handleHttpError(err: HttpErrorResponse): string {
    // 400 - Validation errors
    if (err.status === 400 && err.error && err.error.errors) {
      const errors = err.error.errors;
      const firstErrorKey = Object.keys(errors)[0];
      return errors[firstErrorKey][0];
    }

    // 0 - Network error (API unreachable)
    if (err.status === 0) {
      return 'LOGIN.ERRORS.API_UNREACHABLE';
    }

    // 401, 404, 409 - Auth errors
    if (err.status === 401 || err.status === 404 || err.status === 409) {
      return err.error?.messageKey || 'LOGIN.ERRORS.UNKNOWN';
    }

    // Other HTTP errors
    return 'LOGIN.ERRORS.UNKNOWN';
  }

  /**
   * Checks if the error is "popup_closed"
   * Returns true if the error should be ignored
   */
  isPopupClosedError(err: unknown): boolean {
    const oauthError = err as any;
    return oauthError?.type === 'popup_closed' || oauthError?.error === 'popup_closed_by_user';
  }
}
