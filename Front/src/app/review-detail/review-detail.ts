import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Api } from '../services/api';
import { ReviewItem } from '../dto/reviewList';
import { ReviewCommentsComponent } from '../review-comments/review-comments';
import { CreateReviewModal } from '../create-review-modal/create-review-modal';

@Component({
  selector: 'app-review-detail',
  imports: [CommonModule, ReviewCommentsComponent, MatDialogModule],
  templateUrl: './review-detail.html',
  styleUrl: './review-detail.css'
})
export class ReviewDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  api = inject(Api);
  private cdr = inject(ChangeDetectorRef);
  private dialog = inject(MatDialog);

  review: ReviewItem | null = null;
  loading = true;
  notFound = false;

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.api.getReviewById(id).subscribe({
      next: (res) => {
        this.review = res;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.notFound = true;
        this.cdr.detectChanges();
      }
    });
  }

  goBack() {
    this.router.navigate(['/home']);
  }

  openReviewModal() {
    if (!this.review) return;

    const gameData = {
      id: this.review.gameId,
      title: this.review.gameTitle,
      genre: this.review.gameGenre,
      coverImageUrl: this.review.gameCoverUrl,
      developer: '',
      releaseYear: 0
    };

    const ref = this.dialog.open(CreateReviewModal, {
      panelClass: 'dark-dialog',
      backdropClass: 'dark-backdrop',
      data: { game: gameData }
    });

    ref.afterClosed().subscribe(success => {
      if (success) {
        this.api.getReviewById(this.route.snapshot.paramMap.get('id')!).subscribe({
          next: (res) => {
            this.review = res;
            this.cdr.detectChanges();
          }
        });
      }
    });
  }

  getStars(rating: number): string {
    const full = Math.round(rating / 2);
    return '★'.repeat(full) + '☆'.repeat(5 - full);
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('sr-Latn-RS', {
      day: 'numeric', month: 'long', year: 'numeric'
    });
  }
}