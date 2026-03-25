import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NotificationListResponse } from '../dto/notification';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private url = 'http://localhost:5062/api/';
  private httpClient = inject(HttpClient);

  getNotifications(page = 1, pageSize = 50) {
    const token = localStorage.getItem('token');
    return this.httpClient.get<NotificationListResponse>(
      `${this.url}notification/me?page=${page}&pageSize=${pageSize}`,
      { headers: { Authorization: `Bearer ${token}` } }
    );
  }

  markAsRead(notificationId: string) {
    const token = localStorage.getItem('token');
    return this.httpClient.put(`${this.url}notification/${notificationId}/read`, {}, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }
}
