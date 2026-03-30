import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { NotificationItem } from '../dto/notification';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class NotificationsSignalRService {
  private hubConnection: HubConnection | null = null;

  private notificationReceivedSubject = new Subject<NotificationItem>();

  public notificationReceived$ = this.notificationReceivedSubject.asObservable();

  constructor(private authService: AuthService) {}

  async connect() {
    if (this.hubConnection) {
      console.log('SignalR već konnektan');
      return;
    }

    const token = this.authService.getToken() || '';

    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5062/notificationsHub', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    this.hubConnection.on('NotificationReceived', (notification: NotificationItem) => {
      console.log('Notifikacija primljena:', notification);
      this.notificationReceivedSubject.next(notification);
    });

    await this.hubConnection.start().catch(err => console.error('SignalR notification connection error', err));
    console.log('SignalR konekcija uspešna');

    const userId = this.authService.getCurrentUserId();
    if (userId) {
      console.log('Pridruživanje user grupi:', userId);
      this.hubConnection.invoke('JoinUserGroup', userId).catch(err => console.error('Join user group error', err));
    }
  }

  async disconnect() {
    if (!this.hubConnection) return;
    await this.hubConnection.stop().catch(() => {});
    this.hubConnection = null;
  }
}
