import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Api } from '../services/api';
import { HomeResponse, GameCard, RecentReview } from '../dto/homeResponse';

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private api = inject(Api);

  data: HomeResponse | null = null;
  loading = true;
    private cdr = inject(ChangeDetectorRef);

 ngOnInit() {
  this.api.getHomeData().subscribe({
    next: (res) => {
      this.data = res;
      this.loading = false;
      this.cdr.detectChanges();
    },
    error: (err) => {
      console.error('Error:', err); 
      this.loading = false;
      this.cdr.detectChanges();
    }
  });
}

  getStars(rating: number): string {
    const full = Math.round(rating / 2);
    return '★'.repeat(full) + '☆'.repeat(5 - full);
  }
}