import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { Api } from '../services/api';

@Component({
  selector: 'app-suggest-game-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, MatDialogModule],
  templateUrl: './suggest-game-modal.html',
  styleUrl: './suggest-game-modal.css'
})
export class SuggestGameModal {
  private api = inject(Api);
  private cdr = inject(ChangeDetectorRef);
  private dialogRef = inject(MatDialogRef<SuggestGameModal>);

  title = '';
  genre = '';
  developer = '';
  note = '';
  submitting = false;
  error = '';

  genres = ['Action', 'Adventure', 'RPG', 'Action RPG', 'Strategy', 'Simulation',
    'Sports', 'Racing', 'Fighting', 'Puzzle', 'Platformer', 'Shooter', 'Horror', 'Indie'];

  onSubmit() {
    if (!this.title.trim()) {
      this.error = 'Naziv igre je obavezan';
      return;
    }

    this.submitting = true;
    this.error = '';

    this.api.createGameSuggestion({
      title: this.title,
      genre: this.genre,
      developer: this.developer,
      note: this.note
    }).subscribe({
      next: () => {
        this.submitting = false;
        this.dialogRef.close(true);
        this.cdr.detectChanges();
      },
      error: () => {
        this.submitting = false;
        this.error = 'Greška pri slanju prijedloga';
        this.cdr.detectChanges();
      }
    });
  }

  close() { this.dialogRef.close(); }
}