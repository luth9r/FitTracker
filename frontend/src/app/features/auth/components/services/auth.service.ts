import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface LoginResponse {
  username: string;
  email: string;
  jwt: string;
}

export interface RegisterPayload {
  username: string;
  email: string;
  password: string;
}


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5000/api/auth'; // Base URL for API

  /**
   * 1. Regular Login
   */
  login(email: string, password: string): Observable<LoginResponse> {
    const payload = { email, password };
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, payload);
  }

  /**
   * 2. Regular Registration
   */
  register(payload: RegisterPayload): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/register`, payload);
  }

  /**
   * 3. Google Sign-In (Login ONLY)
   */
  googleLogin(code: string, codeVerifier: string): Observable<LoginResponse> {
    const payload = { code, codeVerifier };
    return this.http.post<LoginResponse>(`${this.apiUrl}/google-login`, payload);
  }

  /**
   * 4. Google Sign-Up (Register ONLY)
   */
  googleRegister(code: string, codeVerifier: string): Observable<LoginResponse> {
    const payload = { code, codeVerifier };
    return this.http.post<LoginResponse>(`${this.apiUrl}/google-register`, payload);
  }
}