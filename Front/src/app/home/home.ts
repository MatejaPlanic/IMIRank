import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Api } from '../services/api';
import { HomeResponse } from '../dto/homeResponse';
import { ReviewItem } from '../dto/reviewList';
import { CreateReviewModal } from '../create-review-modal/create-review-modal';

@Component({
  selector: 'app-home',
  imports: [CommonModule, FormsModule, MatDialogModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private api = inject(Api);
  private cdr = inject(ChangeDetectorRef);
  private dialog = inject(MatDialog);
  private router = inject(Router);

  data: HomeResponse | null = null;
  loading = true;

  // Filters
  reviews: ReviewItem[] = [];
  totalReviews = 0;
  currentPage = 1;
  pageSize = 6;
  selectedGenre = '';
  minRating = 0;
  selectedSort = 'newest';
  reviewsLoading = false;

  genres = ['', 'RPG', 'Action RPG', 'Action-Adventure', 'Shooter', 'Strategy', 'Sports'];
  ratings = [0, 5, 6, 7, 8, 9];

  get totalPages() {
    return Math.ceil(this.totalReviews / this.pageSize);
  }

  ngOnInit() {
    this.api.getHomeData().subscribe({
      next: (res) => {
        this.data = res;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });

    this.loadReviews();
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

  getStars(rating: number): string {
    const full = Math.round(rating / 2);
    return '★'.repeat(full) + '☆'.repeat(5 - full);
  }
}