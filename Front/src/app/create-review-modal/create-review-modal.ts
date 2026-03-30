import { Component, inject, OnInit, ChangeDetectorRef, Inject, Optional } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { Api } from '../services/api';
import { GameItem } from '../dto/gameSearch';

@Component({
  selector: 'app-create-review-modal',
  imports: [CommonModule, FormsModule, MatDialogModule, MatButtonModule],
  templateUrl: './create-review-modal.html',
  styleUrl: './create-review-modal.css'
})
export class CreateReviewModal implements OnInit {
  api = inject(Api);
  private cdr = inject(ChangeDetectorRef);
  private dialogRef = inject(MatDialogRef<CreateReviewModal>);
  private initialData = inject(MAT_DIALOG_DATA, { optional: true });

  games: GameItem[] = [];
  searchQuery = '';
  currentPage = 1;
  pageSize = 5;
  totalGames = 0;
  selectedGame: GameItem | null = null;
  showDropdown = false;
  searchTimeout: any;

  title = '';
  content = '';
  rating = 0;
  hoveredStar = 0;
  submitting = false;
  error = '';

  get totalPages() {
    return Math.ceil(this.totalGames / this.pageSize);
  }

  ngOnInit() {
    if (this.initialData?.game) {
      this.selectedGame = this.initialData.game;
      this.searchQuery = this.selectedGame?.title || '';
    } else {
      this.loadGames();
    }
  }

  loadGames() {
    this.api.searchGames(this.searchQuery, this.currentPage, this.pageSize).subscribe({
      next: (res) => {
        this.games = res.games;
        this.totalGames = res.total;
        this.cdr.detectChanges();
      }
    });
  }

  onSearch() {
    clearTimeout(this.searchTimeout);
    this.searchTimeout = setTimeout(() => {
      this.currentPage = 1;
      this.loadGames();
    }, 300);
  }

  selectGame(game: GameItem) {
    this.selectedGame = game;
    this.showDropdown = false;
    this.searchQuery = game?.title || '';
  }

  openDropdown() {
    this.showDropdown = true;
    this.searchQuery = '';
    this.currentPage = 1;
    this.loadGames();
  }

  clearGame() {
    this.selectedGame = null;
    this.searchQuery = '';
    this.showDropdown = false;
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadGames();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadGames();
    }
  }

  setRating(value: number) {
    this.rating = value;
  }

  onSubmit() {
    if (!this.selectedGame || !this.title || !this.content || !this.rating) {
      this.error = 'Popunite sva polja';
      return;
    }

    this.submitting = true;
    this.error = '';

    this.api.createReview({
      gameId: this.selectedGame.id,
      title: this.title,
      content: this.content,
      rating: this.rating
    }).subscribe({
      next: () => {
        this.submitting = false;
        this.dialogRef.close(true);
        this.cdr.detectChanges();
      },
      error: () => {
        this.submitting = false;
        this.error = 'Greška pri kreiranju recenzije. Prijavite se prvo.';
        this.cdr.detectChanges();
      }
    });
  }

  close() {
    this.dialogRef.close();
  }
}