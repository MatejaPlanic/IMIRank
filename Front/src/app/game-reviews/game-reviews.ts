import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Api } from '../services/api';
import { ReviewItem } from '../dto/reviewList';

@Component({
  selector: 'app-game-reviews',
  imports: [CommonModule],
  templateUrl: './game-reviews.html',
  styleUrl: './game-reviews.css'
})
export class GameReviews implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private api = inject(Api);
  private cdr = inject(ChangeDetectorRef);

  gameId = '';
  game: any = null;
  reviews: ReviewItem[] = [];
  loading = true;

  ngOnInit() {
    this.gameId = this.route.snapshot.paramMap.get('id') || '';
    if (!this.gameId) {
      this.router.navigate(['/home']);
      return;
    }

    this.api.getGameById(this.gameId).subscribe({
      next: (res) => {
        this.game = res;
        this.cdr.detectChanges();
      },
      error: () => {
        this.game = null;
      }
    });

    this.api.getReviewsByGame(this.gameId).subscribe({
      next: (res) => {
        this.reviews = res.reviews || [];
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  goToReview(id: string) {
    this.router.navigate(['/review', id]);
  }

  goBack() {
    this.router.navigate(['/home']);
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('sr-Latn-RS', { day: 'numeric', month: 'long', year: 'numeric' });
  }

  getStars(rating: number): string {
    const full = Math.round(rating / 2);
    return '★'.repeat(full) + '☆'.repeat(5 - full);
  }
}
