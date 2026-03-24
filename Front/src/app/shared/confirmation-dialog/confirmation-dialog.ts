import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface ConfirmationDialogData {
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  isDangerous?: boolean;
}

@Component({
  selector: 'app-confirmation-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule],
  template: `
    <div class="dialog-wrap">
      <div class="dialog-icon" [class.danger]="data.isDangerous">
        {{ data.isDangerous ? '⚠️' : '❓' }}
      </div>
      <h2 class="dialog-title">{{ data.title }}</h2>
      <p class="dialog-message">{{ data.message }}</p>
      <div class="dialog-actions">
        <button class="btn-cancel" (click)="onCancel()">
          {{ data.cancelText || 'Otkaži' }}
        </button>
        <button
          class="btn-confirm"
          [class.danger]="data.isDangerous"
          (click)="onConfirm()">
          {{ data.confirmText || 'Potvrdi' }}
        </button>
      </div>
    </div>
  `,
  styles: [`
    .dialog-wrap {
      background: #0f0f1a;
      border: 1px solid rgba(124,58,237,0.3);
      border-radius: 16px;
      padding: 32px 28px 24px;
      width: 340px;
      max-width: calc(100vw - 48px);
      box-sizing: border-box;
      overflow: hidden;
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 12px;
      font-family: 'Nunito', sans-serif;
    }

    .dialog-icon {
      width: 52px;
      height: 52px;
      border-radius: 50%;
      background: rgba(124,58,237,0.1);
      border: 1px solid rgba(124,58,237,0.25);
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1.4rem;
      margin-bottom: 4px;
    }

    .dialog-icon.danger {
      background: rgba(239,68,68,0.1);
      border-color: rgba(239,68,68,0.25);
    }

    .dialog-title {
      margin: 0;
      color: #f1f1f5;
      font-family: 'Rajdhani', sans-serif;
      font-size: 1.3rem;
      font-weight: 700;
      letter-spacing: 0.5px;
      text-align: center;
    }

    .dialog-message {
      margin: 0;
      color: #7070a0;
      font-size: 0.9rem;
      line-height: 1.6;
      text-align: center;
      max-width: 280px;
    }

    .dialog-actions {
      display: flex;
      gap: 10px;
      margin-top: 8px;
      width: 100%;
    }

    .btn-cancel {
      flex: 1;
      padding: 10px;
      background: #1a1a2e;
      border: 1px solid #2a2a45;
      border-radius: 8px;
      color: #7070a0;
      font-family: 'Nunito', sans-serif;
      font-size: 0.88rem;
      font-weight: 700;
      cursor: pointer;
      transition: all 0.2s;
    }

    .btn-cancel:hover {
      border-color: #5a5a7a;
      color: #e0e0f0;
    }

    .btn-confirm {
      flex: 1;
      padding: 10px;
      background: linear-gradient(135deg, #7c3aed, #3b82f6);
      border: none;
      border-radius: 8px;
      color: #fff;
      font-family: 'Rajdhani', sans-serif;
      font-size: 0.95rem;
      font-weight: 700;
      letter-spacing: 0.5px;
      cursor: pointer;
      transition: opacity 0.2s, transform 0.1s;
    }

    .btn-confirm.danger {
      background: linear-gradient(135deg, #ef4444, #dc2626);
    }

    .btn-confirm:hover {
      opacity: 0.88;
      transform: translateY(-1px);
    }
  `]
})
export class ConfirmationDialogComponent {
  data = inject(MAT_DIALOG_DATA) as ConfirmationDialogData;
  private dialogRef = inject(MatDialogRef<ConfirmationDialogComponent>);

  onConfirm() { this.dialogRef.close(true); }
  onCancel() { this.dialogRef.close(false); }
}