import { Component } from '@angular/core';
import {
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
  FormBuilder,
  AbstractControl
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent {
  signUpForm: FormGroup;
  errorMessage: string | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.signUpForm = this.fb.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
      },
      { validators: this.passwordMatchValidator() }
    );
  }


  passwordMatchValidator(): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      const password = group.get('password')?.value;
      const confirmPassword = group.get('confirmPassword')?.value;
      return password === confirmPassword ? null : { passwordMismatch: true };
    };
  }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      //this.router.navigate(['/dashboard']); -- Redirect to products if already logged in
    }
  }

  onSubmit(): void {
    this.errorMessage = null;
    if (!this.signUpForm.valid) return;
    const signUpUser = this.signUpForm.value;
    this.loading = true;
    this.authService.signUp(signUpUser).subscribe({
      next: (res: any) => {
        this.loading = false;
        this.signUpForm.get('password')?.reset();
        this.signUpForm.get('confirmPassword')?.reset();
        if (res?.isSuccess || res?.authResult?.isSuccess) {
          this.router.navigate(['/products']);
        } else {
          this.errorMessage = res?.failureReason || res?.authResult?.failureReason || 'Sign up failed. Please try again.';
        }
      },
      error: (err: any) => {
        this.loading = false;
        this.errorMessage = err?.message ?? 'Sign up failed. Please try again.';
      }
    });
  }
}
