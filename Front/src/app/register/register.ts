import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Api } from '../services/api';
import { userRole } from '../enums/userRols';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private api = inject(Api);
  private router = inject(Router);

  userName = '';
  email = '';
  password = '';
  confirmPassword = '';
  role: userRole = userRole.RegularUser;
  error = '';

  onSubmit() {
    if (this.password !== this.confirmPassword) {
      this.error = 'Lozinke se ne podudaraju';
      return;
    }

    this.api.register(this.userName, this.email, this.password, this.role).subscribe({
      next: (token) => {
        localStorage.setItem('token', token);
        this.router.navigate(['/home']);
      },
      error: () => {
        this.error = 'Registracija nije uspjela';
      }
    });
  }
}