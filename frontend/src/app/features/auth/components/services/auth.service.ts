import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface LoginResponse {
  usenmame: string;
  email: string;
  jwt: string;
}

export interface RegisterPayload {
  username: string;
  email: string;
  password: string;
}

export interface RegisterPayload {
  username: string;
  email: string;
  password: string;
}

export interface GoogleNeedsRegistrationResponse {
  needsRegistration: true;
  email: string;
  firstName: string;
  lastName: string;
}


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5000/api/auth'; // Base URL for your API

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
   * 3. Google Sign-In (Step 1: Validation)
   * Returns EITHER LoginResponse (if user exists) OR GoogleNeedsRegistrationResponse
   */
  googleLogin(idToken: string): Observable<LoginResponse | GoogleNeedsRegistrationResponse> {
    const payload = { idToken };
    // The backend will return 200 OK in both cases, but with a different body
    return this.http.post<LoginResponse | GoogleNeedsRegistrationResponse>(`${this.apiUrl}/google-login`, payload);
  }

  /**
   * 4. Complete Google Registration (Step 2: Creation)
   */
  completeGoogleRegistration(idToken: string, userName: string): Observable<LoginResponse> {
    const payload = { idToken, userName };
    return this.http.post<LoginResponse>(`${this.apiUrl}/complete-google-registration`, payload);
  }
}