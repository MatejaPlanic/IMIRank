import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Api } from '../services/api';
import { ReviewItem } from '../dto/reviewList';
import { ReviewCommentsComponent } from '../review-comments/review-comments';

@Component({
  selector: 'app-review-detail',
  imports: [CommonModule, ReviewCommentsComponent],
  templateUrl: './review-detail.html',
  styleUrl: './review-detail.css'
})
export class ReviewDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private api = inject(Api);
  private cdr = inject(ChangeDetectorRef);

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