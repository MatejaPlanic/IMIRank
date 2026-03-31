import { NotificationsSignalRService } from './notifications-signalr.service';
import { AuthService } from './auth.service';

export function setupSignalRDebug(signalRService: NotificationsSignalRService, authService: AuthService) {
  const debug = {
    getFullStatus: () => {
      
      const token = authService.getToken();
      const userId = authService.getCurrentUserId();

      if (token) {
        const decoded = (authService as any).decodeToken(token);
        console.log('   JWT Claims:', decoded);
      }
      
      // SIGNALR CONNECTION INFO
      console.log('SIGNALR CONNECTION:');
      const hub = (signalRService as any).hubConnection;
      console.log(`   HubConnection postoji: ${hub ? '✓ DA' : '✗ NE'}`);
      if (hub) {
        console.log(`   State: ${hub.state || 'unknown'}`);
        console.log(`   ConnectionId: ${hub.connectionId || 'nedostaje'}`);
        console.log(`   URL: ${hub.baseUrl || 'nedostaje'}`);
      }
      
      // TEST ACTIONS
      console.log('TEST KOMANDE:');
      console.log('   window.__signalRDebug.testConnect() - Re-konektuj');
      console.log('   window.__signalRDebug.testJoinGroup() - Pridruži se grupi');
      console.log('   window.__signalRDebug.testSendNotification() - Simuliraj notifikaciju');
      console.log('');
    },
    
    testConnect: async () => {
      console.log('%c TEST: Pokušavam re-konekt...', 'color: orange; font-weight: bold');
      try {
        await signalRService.connect();
        console.log('%c Re-konekt USPEŠAN', 'color: green; font-weight: bold');
      } catch (err) {
        console.error('%c Re-konekt FAILED:', 'color: red; font-weight: bold', err);
      }
    },
    
    testJoinGroup: async () => {
      const userId = authService.getCurrentUserId();
      const hub = (signalRService as any).hubConnection;
      
      if (!userId) {
        console.error(' userId nedostaje');
        return;
      }
      
      if (!hub) {
        console.error(' HubConnection nedostaje');
        return;
      }
      
      console.log(`%c TEST: Pokušavam join u grupu: notifications-${userId}`, 'color: orange; font-weight: bold');
      try {
        await hub.invoke('JoinUserGroup', userId);
        console.log(`%c Join USPEŠAN u grupu: notifications-${userId}`, 'color: green; font-weight: bold');
      } catch (err) {
        console.error(`%c Join FAILED:`, 'color: red; font-weight: bold', err);
      }
    },
    
    testSendNotification: () => {
      console.log('%c TEST: Simuliram notifikaciju...', 'color: orange; font-weight: bold');
      const mockNotification = {
        id: `test-${Date.now()}`,
        message: ' TEST NOTIFIKACIJA',
        actorUserName: 'Test User',
        reviewId: 'test-review-id',
        isRead: false,
        createdAt: new Date().toISOString()
      };
      
      (signalRService as any).notificationReceivedSubject.next(mockNotification);
      console.log('%c TEST notifikacija poslana subjects-u', 'color: green; font-weight: bold');
    }
  };
  
  (window as any).__signalRDebug = debug;
  console.log('%c DEBUG OBJEKTI DOSTUPNI: window.__signalRDebug.*', 'color: green');
}
