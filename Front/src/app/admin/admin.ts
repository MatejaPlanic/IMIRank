import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Api } from '../services/api';
import { AuthService } from '../services/auth.service';
import { AdminStatsResponse } from '../dto/adminStatsResponse';
import { GameSuggestionResponse } from '../dto/gameSuggestion';

@Component({
  selector: 'app-admin',
  imports: [
    CommonModule,
    FormsModule,
  ],
  templateUrl: './admin.html',
  styleUrl: './admin.css',
})
export class Admin implements OnInit {
  private api = inject(Api);
  private router = inject(Router);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  suggestions: GameSuggestionResponse[] = [];
  suggestionsLoading = false;

  addSuccess = false;

  stats: AdminStatsResponse | null = null;
  loading = true;

  gameForm = {
    title: '',
    genre: '',
    developer: '',
    description: '',
    releaseYear: new Date().getFullYear(),
    coverImage: null as File | null
  };

  genres = [
    'Action', 'Adventure', 'RPG', 'Strategy', 'Simulation',
    'Sports', 'Racing', 'Fighting', 'Puzzle', 'Platformer',
    'Shooter', 'Horror', 'MMORPG', 'Indie'
  ];

  addingGame = false;

  ngOnInit() {
    this.loadStats();
    this.loadSuggestions();
  }

  loadSuggestions() {
    this.suggestionsLoading = true;
    this.api.getGameSuggestions().subscribe({
      next: (res) => {
        this.suggestions = res.suggestions;
        this.suggestionsLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.suggestionsLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  markSuggestionReviewed(suggestion: GameSuggestionResponse) {
    this.api.markSuggestionReviewed(suggestion.id).subscribe({
      next: () => {
        suggestion.isReviewed = true;
        this.cdr.detectChanges();
      }
    });
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('sr-Latn-RS', {
      day: 'numeric', month: 'short', year: 'numeric'
    });
  }

  loadStats() {
    this.loading = true;
    this.api.getAdminStats().subscribe({
      next: (res) => {
        this.stats = res;
        this.loading = false;
         this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
         this.cdr.detectChanges();
        this.router.navigate(['/']);
      }
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.gameForm.coverImage = file;
    }
  }

  addGame() {
  if (!this.gameForm.title || !this.gameForm.genre || !this.gameForm.developer) return;

  this.addingGame = true;
  this.api.addGame(this.gameForm).subscribe({
    next: () => {
      this.addingGame = false;
      this.gameForm = {
        title: '', genre: '', developer: '', description: '',
        releaseYear: new Date().getFullYear(), coverImage: null
      };
      this.addSuccess = true;
      this.cdr.detectChanges(); // dodaj ovdje
      setTimeout(() => { this.addSuccess = false; this.cdr.detectChanges(); }, 3000);
      this.loadStats();
    },
    error: (err) => {
      this.addingGame = false;
      this.cdr.detectChanges();
      console.error('Error adding game:', err);
    }
  });
}

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  getStars(rating: number): string {
    const full = Math.round(rating / 2);
    return '★'.repeat(full) + '☆'.repeat(5 - full);
  }

  objectKeys(obj: any): string[] {
    return Object.keys(obj);
  }

  objectValues(obj: any): number[] {
    return Object.values(obj);
  }

  getBarWidth(value: number, maxValue: number): number {
    return maxValue > 0 ? (value / maxValue) * 100 : 0;
  }

  maxValue(obj: { [key: string]: number }): number {
    return Math.max(...this.objectValues(obj));
  }

  formatMonth(monthKey: string): string {
    const [year, month] = monthKey.split('-');
    const date = new Date(parseInt(year), parseInt(month) - 1);
    return date.toLocaleDateString('en-US', { month: 'short', year: '2-digit' });
  }
}
