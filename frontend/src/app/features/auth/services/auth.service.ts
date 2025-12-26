import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';

export interface LoginResponse {
  username: string;
  email: string;
  jwt: string;
}

export interface RegisterResponse {
  username: string;
  email: string;
}

export interface RegisterPayload {
  username: string;
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5000/api/auth'; // Base URL for API
  private userApiUrl = 'http://localhost:5000/api/user'; // Base URL for user API

  /**
   * Regular Login
   */
  login(email: string, password: string): Observable<LoginResponse> {
    const payload = { email, password };
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, payload, {
      withCredentials: true,
    });
  }

  /**
   * Regular Registration
   */
  register(payload: RegisterPayload): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/register`, payload, {
      withCredentials: true,
    });
  }

  verifyEmail(token: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.apiUrl}/verify-email`,
      null,
      {
        params: { token },
        withCredentials: true,
      }
    );
  }

  /**
   * Google Sign-In (Login ONLY)
   */
  googleLogin(code: string, codeVerifier: string): Observable<LoginResponse> {
    const payload = { code, codeVerifier };
    return this.http.post<LoginResponse>(`${this.apiUrl}/google-login`, payload, {
      withCredentials: true,
    });
  }

  /**
   * Google Sign-Up (Register ONLY)
   */
  googleRegister(code: string, codeVerifier: string): Observable<LoginResponse> {
    const payload = { code, codeVerifier };
    return this.http.post<LoginResponse>(`${this.apiUrl}/google-register`, payload, {
      withCredentials: true,
    });
  }

  checkAuth(): Observable<boolean> {
    return this.http
      .get<{ userId: string | null }>(`${this.userApiUrl}/me`, {
        withCredentials: true,
      })
      .pipe(
        map((res) => !!res.userId),
        catchError(() => of(false))
      );
  }

  resendVerificationEmail(email: string): Observable<void> {
    return this.http.post<void>(
      `${this.apiUrl}/resend-verification`,
      { email }
    );
  }
}
