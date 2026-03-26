import { ChangeDetectorRef, Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { Api } from '../services/api';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';
import { NotificationsSignalRService } from '../services/notifications-signalr.service';
import { HomeResponse } from '../dto/homeResponse';
import { ReviewItem } from '../dto/reviewList';
import { GameItem } from '../dto/gameSearch';
import { CreateReviewModal } from '../create-review-modal/create-review-modal';
import { SuggestGameModal } from '../suggest-game-modal/suggest-game-modal';

interface UserResult {
  id: string;
  userName: string;
  profilePictureUrl?: string;
  role: string;
  totalReviews: number;
}

@Component({
  selector: 'app-home',
  imports: [CommonModule, FormsModule, MatDialogModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit, OnDestroy {
  private api = inject(Api);
  public cdr = inject(ChangeDetectorRef);
  private dialog = inject(MatDialog);
  private router = inject(Router);
  public authService = inject(AuthService);

  data: HomeResponse | null = null;
  loading = true;

  notificationsCount = 0;

  private notificationService = inject(NotificationService);
  private notificationsSignalR = inject(NotificationsSignalRService);

  reviews: ReviewItem[] = [];
  totalReviews = 0;
  currentPage = 1;
  pageSize = 6;
  selectedGenre = '';
  minRating = 0;
  selectedSort = 'newest';
  reviewsLoading = false;

  genres: string[] = [];
  ratings = [0, 5, 6, 7, 8, 9];

  get totalPages() {
    return Math.ceil(this.totalReviews / this.pageSize);
  }

  featuredReviewIndex = 0;
  private autoScrollInterval: any;

  searchFocused = false;

  onSearchBlur() {
    setTimeout(() => {
      this.searchFocused = false;
      this.cdr.detectChanges();
    }, 200);
  }

  ngOnInit() {
    if (this.authService.getUserRole() === 'Admin') {
      this.router.navigate(['/admin']);
      return;
    }

    this.api.getHomeData().subscribe({
      next: (res) => {
        this.data = res;
        this.genres = [''].concat(res.genres || []);
        this.loading = false;
        this.startAutoScroll();
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });

    this.loadNotifications();
    this.notificationsSignalR.connect();

    this.notificationsSignalR.notificationReceived$.subscribe(() => {
      this.notificationsCount++;
      this.cdr.detectChanges();
    });

    this.loadReviews();
  }

  ngOnDestroy() {
    this.notificationsSignalR.disconnect();
    if (this.autoScrollInterval) {
      clearInterval(this.autoScrollInterval);
    }
  }

  startAutoScroll() {
    this.autoScrollInterval = setInterval(() => {
      if (this.data?.recentReviews && this.data.recentReviews.length > 1) {
        this.nextReview();
      }
    }, 5000);
  }

  nextReview() {
    if (this.data?.recentReviews) {
      this.featuredReviewIndex = (this.featuredReviewIndex + 1) % this.data.recentReviews.length;
      this.cdr.detectChanges();
    }
  }

  prevReview() {
    if (this.data?.recentReviews) {
      this.featuredReviewIndex = this.featuredReviewIndex === 0 ? this.data.recentReviews.length - 1 : this.featuredReviewIndex - 1;
      this.cdr.detectChanges();
    }
  }

  loadReviews() {
    this.reviewsLoading = true;
    this.api.getReviews(this.selectedGenre, this.minRating, this.selectedSort, this.currentPage, this.pageSize)
      .subscribe({
        next: (res) => {
          this.reviews = res.reviews;
          this.totalReviews = res.total;
          this.reviewsLoading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.reviewsLoading = false;
          this.cdr.detectChanges();
        }
      });
  }

  applyFilter() {
    this.currentPage = 1;
    this.loadReviews();
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadReviews();
  }

  getPageNumbers(): number[] {
    const pages = [];
    const start = Math.max(1, this.currentPage - 1);
    const end = Math.min(this.totalPages, start + 2);
    for (let i = start; i <= end; i++) pages.push(i);
    return pages;
  }

  openReview(id: string) {
    this.router.navigate(['/review', id]);
  }

  openReviewModal() {
    const ref = this.dialog.open(CreateReviewModal, {
      panelClass: 'dark-dialog',
      backdropClass: 'dark-backdrop'
    });

    ref.afterClosed().subscribe(success => {
      if (success) {
        this.loadReviews();
        this.api.getHomeData().subscribe({
          next: (res) => { this.data = res; this.cdr.detectChanges(); }
        });
      }
    });
  }

  searchQuery = '';
  searchType: 'all' | 'games' | 'users' | 'reviewers' = 'all';
  gameResults: GameItem[] = [];
  userResults: UserResult[] = [];
  searchLoading = false;
  searchTimeout: any = null;
  minSearchQueryLength = 2;

  isSearchAvailable(): boolean {
    return this.searchQuery.trim().length >= this.minSearchQueryLength;
  }

  onSearchInput(): void {
    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout);
    }

    if (!this.isSearchAvailable()) {
      this.searchLoading = false;
      this.gameResults = [];
      this.userResults = [];
      this.cdr.detectChanges();
      return;
    }

    this.searchLoading = true;
    this.searchTimeout = setTimeout(() => {
      this.runSearch();
    }, 260);
  }

  getStars(rating: number): string {
    const full = Math.round(rating / 2);
    return '★'.repeat(full) + '☆'.repeat(5 - full);
  }

  runSearch() {
    if (!this.isSearchAvailable()) {
      this.searchLoading = false;
      this.gameResults = [];
      this.userResults = [];
      this.cdr.detectChanges();
      return;
    }

    this.searchLoading = true;
    this.gameResults = [];
    this.userResults = [];

    const searchGame = this.searchType === 'all' || this.searchType === 'games';
    const searchUsers = this.searchType === 'all' || this.searchType === 'users' || this.searchType === 'reviewers';

    const tasks: any[] = [];

    if (searchGame) {
      tasks.push(
        firstValueFrom(this.api.searchGames(this.searchQuery.trim(), 1, 10)).then((res: any) => {
          this.gameResults = res.games || [];
        }).catch(() => {})
      );
    }

    if (searchUsers) {
      tasks.push(
        firstValueFrom(this.api.searchUsers(this.searchQuery.trim(), 1, 10)).then((res: any) => {
          this.userResults = (res.users || []).map((u: any) => ({
            id: u.id,
            userName: u.userName,
            profilePictureUrl: u.profilePictureUrl,
            role: u.role,
            totalReviews: u.totalReviews ?? 0
          }));
        }).catch(() => {})
      );
    }

    Promise.all(tasks).finally(() => {
      this.searchLoading = false;
      this.cdr.detectChanges();
    });
  }

  openGameReview(game: GameItem) {
    this.router.navigate(['/game', game.id]);
  }

  getFeaturedBg(coverUrl?: string): string {
    return coverUrl ? `url(${coverUrl})` : 'transparent';
  }

  openProfile(user: UserResult) {
    this.router.navigate(['/profile', user.id]);
  }

  clickTrendingGame(game: any) {
    this.openGameReview(game);
  }

  clickEditor(user: any) {
    this.openProfile({ id: user.id, userName: user.userName, profilePictureUrl: user.profilePictureUrl, role: user.role, totalReviews: user.reviewCount });
  }

  loadNotifications() {
    this.notificationService.getNotifications().subscribe({
      next: (res: any) => {
        this.notificationsCount = (res.notifications || []).filter((n: any) => !n.isRead).length;
      },
      error: () => {
        this.notificationsCount = 0;
      }
    });
  }

  openNotifications() {
    this.router.navigate(['/notifications']);
  }

  goToProfile() {
    if (this.authService.isTokenExpired()) {
      console.log('Token je istekao, odloga se na login');
      this.authService.logout();
      this.router.navigate(['/login']);
    } else {
      this.router.navigate(['/profile']);
    }
  }

  goToAdmin() {
    this.router.navigate(['/admin']);
  }

  openSuggestModal() {
    this.dialog.open(SuggestGameModal, {
      panelClass: 'dark-dialog',
      backdropClass: 'dark-backdrop'
    });
  }
}