import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';

(window as any).SignalRDebug = {
  getInfo: () => {
    const signalRService = (window as any).__signalR_service;
    if (signalRService) {
      const hub = signalRService.hubConnection;
      if (hub) {
        console.log('   - State:', hub.state);
        console.log('   - ConnectionId:', hub.connectionId);
      }
    }
  }
};

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
