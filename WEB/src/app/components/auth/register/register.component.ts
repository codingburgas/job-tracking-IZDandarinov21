import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormGroup, FormControl, Validators, ReactiveFormsModule, AbstractControl, ValidatorFn } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      username: new FormControl('', [Validators.required]),
      firstName: new FormControl('', [Validators.required]),
      middleName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)]),
      confirmPassword: new FormControl('', [Validators.required])
    });

    this.registerForm.setValidators(RegisterComponent.passwordMatchValidator());
  }

  static passwordMatchValidator(): ValidatorFn {
    return (form: AbstractControl): { [key: string]: boolean } | null => {
      const passwordControl = form.get('password');
      const confirmPasswordControl = form.get('confirmPassword');

      if (!passwordControl || !confirmPasswordControl) {
        return null;
      }

      if (passwordControl.value === confirmPasswordControl.value) {
        confirmPasswordControl.setErrors(null);
        return null;
      } else {
        confirmPasswordControl.setErrors({ mismatch: true });
        return { mismatch: true };
      }
    };
  }

  onSubmit(): void {
    this.registerForm.markAllAsTouched();

    if (this.registerForm.valid) {
      const { email, username, firstName, middleName, lastName, password } = this.registerForm.value;

      this.authService.register({ email, username, firstName, middleName, lastName, password }).subscribe({
        next: (response) => {
          console.log('Registration successful', response);
          this.router.navigate(['/login']);
          alert('Registration successful! You can now log in.');
        },
        error: (error) => {
          console.error('Registration failed', error);
          let errorMessage = 'Unknown error occurred.';
          if (error.error && typeof error.error === 'string') {
            errorMessage = error.error;
          } else if (error.error && error.error.errors) {
            const validationErrors = error.error.errors;
            errorMessage = Object.keys(validationErrors)
              .map(key => validationErrors[key].join(', '))
              .join('; ');
          } else if (error.message) {
            errorMessage = error.message;
          }
          alert('Registration failed: ' + errorMessage);
        }
      });
    } else {
      alert('Please fill in all required fields correctly and ensure passwords match.');
    }
  }
}
