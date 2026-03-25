import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';
import { NotificationsSignalRService } from '../services/notifications-signalr.service';
import { NotificationItem } from '../dto/notification';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notifications.html',
  styleUrl: './notifications.css'
})
export class NotificationsComponent implements OnInit {
  private notificationService = inject(NotificationService);
  private signalR = inject(NotificationsSignalRService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  notifications: NotificationItem[] = [];
  loading = true;

  ngOnInit() {
    this.loadNotifications();
    this.signalR.connect();

    this.signalR.notificationReceived$.subscribe(notification => {
      this.notifications.unshift(notification);
      this.cdr.detectChanges();
    });
  }

  loadNotifications() {
    this.loading = true;
    this.notificationService.getNotifications().subscribe({
      next: (result: any) => {
        this.notifications = result.notifications ?? [];
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.notifications = [];
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  openReview(notification: NotificationItem) {
    this.markAsRead(notification);
    this.router.navigate(['/review', notification.reviewId]);
  }

  markAsRead(notification: NotificationItem) {
    if (notification.isRead) return;
    this.notificationService.markAsRead(notification.id).subscribe({
      next: () => {
        notification.isRead = true;
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  markAllAsRead() {
    this.notifications
      .filter(n => !n.isRead)
      .forEach(n => this.markAsRead(n));
  }

  get unreadCount(): number {
    return this.notifications.filter(n => !n.isRead).length;
  }

  goBack() {
    this.router.navigate(['/home']);
  }
}