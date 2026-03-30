import { Component, Input, inject, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Api } from '../services/api';
import { ReviewCommentsSignalRService } from '../services/review-comments-signalr.service';
import { ReviewCommentResponse, ReviewCommentsListResponse, CreateReviewCommentRequest, UpdateReviewCommentRequest } from '../dto/reviewComment';
import { AuthService } from '../services/auth.service';
import { ConfirmationDialogComponent } from '../shared/confirmation-dialog/confirmation-dialog';

@Component({
  selector: 'app-review-comments',
  standalone: true,
  imports: [CommonModule, FormsModule, MatDialogModule],
  templateUrl: './review-comments.html',
  styleUrl: './review-comments.css'
})
export class ReviewCommentsComponent implements OnInit, OnDestroy {
  @Input() reviewId!: string;
  @Input() reviewUserId!: string;

  api = inject(Api);
  private signalR = inject(ReviewCommentsSignalRService);
  private auth = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);
  private dialog = inject(MatDialog);

  comments: ReviewCommentResponse[] = [];
  newCommentText = '';
  loading = true;
  isAuthenticated = false;
  currentUserId = '';
  editingCommentId: string | null = null;
  editingCommentText = '';
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  totalCount = 0;

  get canComment(): boolean {
    return this.isAuthenticated && !!this.currentUserId && !!this.reviewUserId && this.currentUserId !== this.reviewUserId;
  }

  async ngOnInit() {
    this.isAuthenticated = this.auth.isAuthenticated();
    this.currentUserId = this.auth.getCurrentUserId();
    
    try {
      await this.loadComments();
      this.setupSignalR();
    } catch (error) {
      console.error('Error during initialization:', error);
    }
  }

  ngOnDestroy() {
    this.signalR.leaveReviewGroup(this.reviewId);
  }

  loadComments(page = 1): Promise<void> {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.api.getReviewComments(this.reviewId, page, this.pageSize).subscribe({
        next: (res: ReviewCommentsListResponse) => {
          this.comments = res.comments;
          this.totalCount = res.totalCount;
          this.totalPages = res.totalPages;
          this.currentPage = page;
          this.loading = false;
          this.cdr.detectChanges();
          resolve();
        },
        error: (error) => {
          this.loading = false;
          console.error('Error loading comments:', error);
          this.cdr.detectChanges();
          reject(error);
        }
      });
    });
  }

setupSignalR() {
    this.signalR.commentReceived$.subscribe((comment: ReviewCommentResponse) => {
      if (!this.comments.find(c => c.id === comment.id)) {
        this.comments.unshift(comment);
        this.totalCount++;
        this.cdr.detectChanges();
      }
    });

    this.signalR.commentUpdated$.subscribe((comment: ReviewCommentResponse) => {
      const index = this.comments.findIndex(c => c.id === comment.id);
      if (index !== -1) {
        this.comments[index] = comment;
        this.cdr.detectChanges(); 
      }
    });

    this.signalR.commentDeleted$.subscribe((commentId: string) => {
      this.comments = this.comments.filter(c => c.id !== commentId);
      this.totalCount--;
      this.cdr.detectChanges(); 
    });

    this.signalR.joinReviewGroup(this.reviewId).catch(err =>
      console.log('Failed to join SignalR group:', err)
    );
  }

  submitComment() {
    if (!this.canComment) {
      alert('Ne možete komentarisati sopstvenu recenziju.');
      return;
    }

    if (!this.newCommentText.trim()) return;

    const request: CreateReviewCommentRequest = {
      reviewId: this.reviewId,
      content: this.newCommentText
    };

    this.api.createReviewComment(request).subscribe({
      next: () => {
        this.newCommentText = '';
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to create comment:', err);
        alert('Greška pri kreiranju komentara');
      }
    });
  }

  editComment(comment: ReviewCommentResponse) {
    if (comment.userId !== this.currentUserId) {
      alert('Možete editovati samo svoje komentare');
      return;
    }
    this.editingCommentId = comment.id;
    this.editingCommentText = comment.content;
  }

  cancelEdit() {
    this.editingCommentId = null;
    this.editingCommentText = '';
  }

  saveEdit() {
    if (!this.editingCommentText.trim() || !this.editingCommentId) return;

    const request: UpdateReviewCommentRequest = {
      content: this.editingCommentText
    };

    this.api.updateReviewComment(this.editingCommentId, request).subscribe({
      next: () => {
        this.editingCommentId = null;
        this.editingCommentText = '';
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Failed to update comment:', err);
        alert('Greška pri izmjeni komentara');
      }
    });
  }

  deleteComment(comment: ReviewCommentResponse) {
    if (comment.userId !== this.currentUserId) {
      this.dialog.open(ConfirmationDialogComponent, {
        width: '400px',
        data: {
          title: 'Greška',
          message: 'Možete brisati samo svoje komentare',
          confirmText: 'OK',
          cancelText: 'Nazad',
          isDangerous: false
        }
      });
      return;
    }

    this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: '⚠️ Obriši komentar',
        message: 'Sigurni ste da želite obrisati ovaj komentar? Ova akcija se ne može poništiti.',
        confirmText: 'Obriši',
        cancelText: 'Otkaži',
        isDangerous: true
      }
    }).afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        this.api.deleteReviewComment(comment.id).subscribe({
          next: () => {},
          error: (err: any) => {
            console.error('Failed to delete comment:', err);
            alert('Greška pri brisanju komentara');
          }
        });
      }
    });
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.loadComments(this.currentPage - 1).catch(err => 
        console.error('Error loading previous page:', err)
      );
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.loadComments(this.currentPage + 1).catch(err => 
        console.error('Error loading next page:', err)
      );
    }
  }

  isOwnComment(comment: ReviewCommentResponse): boolean {
    return comment.userId === this.currentUserId;
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('sr-Latn-RS', {
      day: 'numeric', month: 'short', year: 'numeric', hour: '2-digit', minute: '2-digit'
    });
  }
}
