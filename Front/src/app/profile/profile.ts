import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Api } from '../services/api';
import { ProfileResponse } from '../dto/profile';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class ProfilePage implements OnInit {
  private api = inject(Api);
  private cdr = inject(ChangeDetectorRef);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  profile: ProfileResponse | null = null;
  loading = true;
  publicView = false;
  currentId: string | null = null;

  newUserName = '';
  usernameSuccess = '';
  usernameError = '';

  oldPassword = '';
  newPassword = '';
  confirmPassword = '';
  passwordSuccess = '';
  passwordError = '';

  pictureError = '';
  pictureSuccess = '';

  activeTab: 'info' | 'username' | 'password' = 'info';

  ngOnInit() {
    this.currentId = this.route.snapshot.paramMap.get('id');
    if (this.currentId) {
      this.publicView = true;
      this.loadPublicProfile(this.currentId);
    } else {
      this.publicView = false;
      this.loadProfile();
    }
  }

  loadProfile() {
    this.api.getProfile().subscribe({
      next: (res) => {
        this.profile = res;
        this.newUserName = res.userName;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.router.navigate(['/login']);
      }
    });
  }

  loadPublicProfile(userId: string) {
    this.api.getUserById(userId).subscribe({
      next: (res: any) => {
        this.profile = {
          id: res.id,
          userName: res.userName,
          email: '',
          role: res.role,
          profilePictureUrl: res.profilePictureUrl,
          totalReviews: res.totalReviews,
          createdAt: new Date().toISOString(),
          recentReviews: []
        } as any;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.router.navigate(['/home']);
      }
    });
  }

onPictureChange(event: Event) {
  if (this.publicView) return;
  const file = (event.target as HTMLInputElement).files?.[0];
  if (!file) return;

  if (file.size > 5 * 1024 * 1024) {
    this.pictureError = 'Slika ne smije biti veća od 5MB';
    this.cdr.detectChanges();
    return;
  }

  this.api.updateProfilePicture(file).subscribe({
    next: () => {
      this.pictureSuccess = 'Slika uspješno promijenjena!';
      this.pictureError = '';
      this.loadProfile();
      this.cdr.detectChanges();
    },
    error: (err) => {
      this.pictureError = 'Greška: ' + (err.error?.message || err.status);
      this.cdr.detectChanges();
    }
  });
}

  saveUsername() {
    if (this.publicView) return;
    this.usernameError = '';
    this.usernameSuccess = '';
    this.api.updateUsername(this.newUserName).subscribe({
      next: () => {
        this.usernameSuccess = 'Korisničko ime uspješno promijenjeno!';
        this.loadProfile();
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.usernameError = err.error?.message || 'Greška pri promjeni';
        this.cdr.detectChanges();
      }
    });
  }

  savePassword() {
    if (this.publicView) return;
    this.passwordError = '';
    this.passwordSuccess = '';
    this.api.updatePassword(this.oldPassword, this.newPassword, this.confirmPassword).subscribe({
      next: () => {
        this.passwordSuccess = 'Lozinka uspješno promijenjena!';
        this.oldPassword = '';
        this.newPassword = '';
        this.confirmPassword = '';
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.passwordError = err.error?.message || 'Greška pri promjeni lozinke';
        this.cdr.detectChanges();
      }
    });
  }

  getRoleBadge(role: string): string {
    const map: Record<string, string> = {
      'Admin': '👑 Admin',
      'Editor': '✏️ Urednik',
      'RegularUser': '👤 Korisnik'
    };
    return map[role] ?? role;
  }

  goToReview(reviewId: string) {
    this.router.navigate(['/review', reviewId]);
  }

  formatDate(date: string): string {
    if (!date) return 'Nepoznat datum';
    const d = new Date(date);
    if (isNaN(d.getTime())) return 'Nevažeći datum';
    return d.toLocaleDateString('sr-Latn-RS', {
      day: 'numeric', month: 'long', year: 'numeric'
    });
  }

  goBack() { this.router.navigate(['/home']); }
}