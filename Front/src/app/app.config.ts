import { ApplicationConfig, provideBrowserGlobalErrorListeners, APP_INITIALIZER } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch, withInterceptors, HTTP_INTERCEPTORS } from '@angular/common/http';
import { routes } from './app.routes';
import { AuthInterceptor } from './services/auth.interceptor';
import { NotificationsSignalRService } from './services/notifications-signalr.service';
import { AuthService } from './services/auth.service';


export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withFetch()),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    {
      provide: APP_INITIALIZER,
      useFactory: (authService: AuthService, signalRService: NotificationsSignalRService) => {
        return async () => {
          
          const token = authService.getToken();
          const userId = authService.getCurrentUserId();

          
          if (token) {
            console.log('%c🔄 Počinjem inicijalizaciju SignalR-a...', 'color: cyan');
            try {
              await signalRService.connect();
              console.log('%c✓ APP_INITIALIZER: SignalR uspešno inicijaliziran', 'color: cyan; font-weight: bold');
            } catch (err) {
              console.error('%c✗ APP_INITIALIZER: Greška pri inicijalizaciji SignalR-a', 'color: red; font-weight: bold', err);
            }
          } else {
            console.warn('%c APP_INITIALIZER: Token nedostaje, preskačem SignalR inicijalizaciju', 'color: orange');
          }
          
          console.log('%c=== APP_INITIALIZER END ===', 'color: cyan; font-weight: bold');
        };
      },
      deps: [AuthService, NotificationsSignalRService],
      multi: true
    }
  ]
};
