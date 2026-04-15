import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { SignUpRequest, AuthResponse, SignInRequest } from 'src/app/models/models';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  getToken(): string | null {
    return localStorage.getItem(environment.tokenKey);
  }

  setToken(token: string): void {
    localStorage.setItem(environment.tokenKey, token);
  }

  removeToken(): void {
    localStorage.removeItem(environment.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  signIn(credentials: SignInRequest): Observable<string> {
    const body = { email: credentials.email, password: credentials.password };
    return this.http
      .post(`${environment.apiBase}/auth/login`, body, { responseType: 'text' as const })
      .pipe(catchError(this.handleError));
  }


  signUp(signUpUser: SignUpRequest): Observable<AuthResponse> {
    const body = {
      firstName: signUpUser.firstName,
      lastName: signUpUser.lastName,
      email: signUpUser.email,
      password: signUpUser.password,
      confirmPassword: signUpUser.confirmPassword
    };
    return this.http
      .post<any>(`${environment.apiBase}/auth/register`, body)
      .pipe(
        // Custom error handling for EmailAlreadyExists
        catchError(this.handleError),
      );
  }

  private handleError(error: any) {
    let message = 'Request failed';
    if (error?.status === 0) {
      message = 'Cannot reach the API. Check that it is running and that the HTTPS certificate is trusted.';
    } else if (error?.status === 400) {
      // Check for EmailAlreadyExists in the error response
      if (error?.error?.authResult?.failureReason === 'EmailAlreadyExists') {
        message = 'This email is already registered. Please use a different email or sign in.';
      } else {
        message = error?.error?.failureReason || error?.error || 'Invalid request. Please check your inputs.';
      }
    }
    return throwError(() => ({ status: error?.status, error: error?.error, message }));
  }
}
