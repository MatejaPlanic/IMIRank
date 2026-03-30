import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Api } from '../services/api';
import { AuthService } from '../services/auth.service';
import { ProfileResponse } from '../dto/profile';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class ProfilePage implements OnInit {
  api: Api;
  private cdr: ChangeDetectorRef;
  private router: Router;
  private route: ActivatedRoute;
  private authService: AuthService;

  constructor(api: Api, cdr: ChangeDetectorRef, router: Router, route: ActivatedRoute, authService: AuthService) {
    this.api = api;
    this.cdr = cdr;
    this.router = router;
    this.route = route;
    this.authService = authService;
  }

  profile: ProfileResponse | null = null;
  loading = true;
  publicView = false;
  currentId: string | null = null;

  isFollowing = false;
  followersCount = 0;
  followingCount = 0;
  followLoading = false;

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
      error: (err) => {
        // Ako je 401, token je istekao
        if (err.status === 401) {
          console.log('Token istekao, odloga se na login');
          this.authService.logout();
          this.router.navigate(['/login']);
        } else {
          // Ostale greške, idi na home
          this.router.navigate(['/home']);
        }
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
        
        // Učitaj follow status
        this.loadFollowStatus(userId);
        this.cdr.detectChanges();
      },
      error: () => {
        this.router.navigate(['/home']);
      }
    });
  }

  loadFollowStatus(userId: string) {
    this.api.getFollowStatus(userId).subscribe({
      next: (res: any) => {
        this.isFollowing = res.isFollowing;
        this.followersCount = res.followersCount;
        this.followingCount = res.followingCount;
        this.cdr.detectChanges();
      },
      error: () => {
        console.log('Greška pri učitavanju follow statusa');
      }
    });
  }

  toggleFollow() {
    if (!this.currentId) return;
    this.followLoading = true;

    const action = this.isFollowing ? 'unfollow' : 'follow';
    const method = this.isFollowing ? 
      this.api.unfollow(this.currentId) : 
      this.api.follow(this.currentId);

    method.subscribe({
      next: (res: any) => {
        this.isFollowing = res.isFollowing;
        this.followersCount = res.followersCount;
        this.followingCount = res.followingCount;
        this.followLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.followLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

onPictureChange(event: Event) {
  if (this.publicView) return;
  const file = (event.target as HTMLInputElement).files?.[0];
  if (!file) return;

  if (file.size > 5 * 1024 * 1024) {
    this.pictureError = 'Slika ne sme biti veća od 5MB';
    this.cdr.detectChanges();
    return;
  }

  this.api.updateProfilePicture(file).subscribe({
    next: () => {
      this.pictureSuccess = 'Slika uspešno promenjena!';
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
        this.usernameSuccess = 'Korisničko ime uspešno promenjeno!';
        this.loadProfile();
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.usernameError = err.error?.message || 'Greška pri promeni';
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
        this.passwordSuccess = 'Lozinka uspešno promijenjena!';
        this.oldPassword = '';
        this.newPassword = '';
        this.confirmPassword = '';
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.passwordError = err.error?.message || 'Greška pri promeni lozinke';
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

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  canShowFollowButton(): boolean {
    if (!this.publicView) return false;
    if (!this.currentId) return false;
    const loggedInUserId = this.authService.getCurrentUserId();
    return this.currentId !== loggedInUserId;
  }

  goBack() { this.router.navigate(['/home']); }
}