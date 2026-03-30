import { Component, signal, inject, OnInit, OnDestroy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NotificationsSignalRService } from './services/notifications-signalr.service';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit, OnDestroy {
  protected readonly title = signal('Front');

  private signalR = inject(NotificationsSignalRService);
  private authService = inject(AuthService);

  ngOnInit() {
    if (this.authService.getToken()) {
      console.log('App: Konektovanje na SignalR...');
      this.signalR.connect();
    }
  }

  ngOnDestroy() {
    console.log('App: Diskonektovanje od SignalR...');
    this.signalR.disconnect();
  }
}
