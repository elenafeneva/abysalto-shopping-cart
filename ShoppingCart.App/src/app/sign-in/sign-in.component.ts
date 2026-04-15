import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {
  signInForm: FormGroup;
  errorMessage: string | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.signInForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/dashboard']);
    }
  }

  onSubmit(): void {
    this.errorMessage = null;
    if (!this.signInForm.valid) return;
    const credentials = {
      email: this.signInForm.get('email')!.value,
      password: this.signInForm.get('password')!.value,
    };
    this.loading = true;
    this.authService.signIn(credentials).subscribe({
      next: (token: string) => {
        this.loading = false;
        this.cdr.detectChanges();
        this.signInForm.get('password')?.reset();
        this.authService.setToken(token);
        this.router.navigate(['/products']);
      },
      error: (err : any) => {
        this.loading = false;
        this.errorMessage = err?.message ?? 'Sign in failed. Please try again.';
        this.cdr.detectChanges();
      },
    });
  }
}