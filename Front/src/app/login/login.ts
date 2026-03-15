import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Api } from '../services/api';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private api = inject(Api);
  private router = inject(Router);

  email = '';
  password = '';
  error = '';

  onSubmit() {
    this.api.login(this.email, this.password).subscribe({
      next: (token) => {
        localStorage.setItem('token', token);
        this.router.navigate(['/home']);
      },
      error: () => {
        this.error = 'Pogrešan email ili lozinka';
      }
    });
  }
}