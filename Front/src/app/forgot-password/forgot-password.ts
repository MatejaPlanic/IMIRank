import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Api } from '../services/api';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password',
  imports: [FormsModule, CommonModule],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.css',
})
export class ForgotPassword {
  private api = inject(Api);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  email = '';
  error = '';
  loading = false;
  passwordRetrieved = false;
  tempPassword = '';
  copied = false;

  onSubmit() {
    if (!this.email.trim()) {
      this.error = 'Molimo unesite email adresu';
      return;
    }

    this.loading = true;
    this.error = '';

    this.api.forgotPassword(this.email).subscribe({
      next: (response: any) => {
        console.log('Response primljen:', response);
        if (response && response.tempPassword) {
          this.tempPassword = response.tempPassword;
          this.passwordRetrieved = true;
        } else if (typeof response === 'string') {
          // Ako je string, to je sama lozinka
          this.tempPassword = response;
          this.passwordRetrieved = true;
        } else {
          this.error = 'Greška pri pronalaženju lozinke';
        }
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.log('Error primljen:', err);
        this.error = err.error?.message || err.message || 'Greška pri pronalaženju korisnika';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  copyToClipboard() {
    navigator.clipboard.writeText(this.tempPassword).then(() => {
      this.copied = true;
      setTimeout(() => {
        this.copied = false;
      }, 2000);
    });
  }
}
