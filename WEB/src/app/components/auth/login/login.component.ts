import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)])
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      this.authService.login({ username, password }).subscribe({
        next: (response) => {
          console.log('Login successful', response);
          this.router.navigate(['/home']);
          alert('Login successful!');
        },
        error: (error) => {
          console.error('Login failed', error);
          let errorMessage = 'Login failed: Unknown error occurred.';
          if (error.error && typeof error.error === 'string') {
            errorMessage = 'Login failed: ' + error.error;
          } else if (error.error && error.error.errors) {
            const validationErrors = error.error.errors;
            errorMessage = 'Login failed: ' + Object.keys(validationErrors)
              .map(key => validationErrors[key].join(', '))
              .join('; ');
          } else if (error.message) {
            errorMessage = 'Login failed: ' + error.message;
          }
          alert(errorMessage);
        }
      });
    } else {
      alert('Please enter your username and password.');
    }
  }
}
