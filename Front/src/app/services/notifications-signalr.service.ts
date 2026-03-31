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
      if (this.hubConnection.state === 'Connected') {
        return;
      }
    }

    const token = this.authService.getToken() || '';
    const userId = this.authService.getCurrentUserId();
    
    if (!token) {
      throw new Error('Token nedostaje');
    }
    
    if (!userId) {
      console.error('✗ userId nedostaje - ne mogu da se pristupim grupi!');
    }
    
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5062/notificationsHub', {
        accessTokenFactory: () => {
          return token;
        }
      })
      .withAutomaticReconnect([0, 0, 1000, 3000, 5000])
      .configureLogging(LogLevel.Information)
      .build();

    this.hubConnection.on('NotificationReceived', (notification: NotificationItem) => {      
      this.notificationReceivedSubject.next(notification);
    });

    this.hubConnection.onreconnecting((error) => {
      console.warn('⚠️ SignalR: Pokušavam rekonektivanje...', error?.message);
    });

    this.hubConnection.onreconnected((connectionId) => {
      if (userId) {
        this.hubConnection?.invoke('JoinUserGroup', userId)
          .then(() => console.log('   ✓ Re-join uspešan'))
          .catch(err => console.error('   ❌ Re-join greška:', err));
      }
    });

    this.hubConnection.onclose((error) => {
      console.error('%c❌ SignalR: Konekcija zatvorena', 'color: red; font-weight: bold', error?.message);
    });

    const checkState = () => {
      if (this.hubConnection) {
      }
    };
    setInterval(checkState, 30000);

    try {
      await this.hubConnection.start();

      if (userId) {
        try {
          await this.hubConnection.invoke('JoinUserGroup', userId);
        } catch (joinErr) {
          throw joinErr;
        }
      } else {
        console.warn('⚠️ userId nedostaje - neću se pridružiti grupi!');
      }
    } catch (err) {
      console.error('%c✗ GREŠKA PRI connect():', 'color: red; font-weight: bold', err);
      this.hubConnection = null;
      throw err;
    }
  }

  async disconnect() {
    if (!this.hubConnection) return;
    await this.hubConnection.stop().catch(() => {});
    this.hubConnection = null;
  }
}
