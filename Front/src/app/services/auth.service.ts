import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private platformId = inject(PLATFORM_ID);

  isLoggedIn(): boolean {
    if (!isPlatformBrowser(this.platformId)) {
      return false;
    }
    return !!localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    if (!this.isLoggedIn()) {
      return false;
    }
    // Provjeri da li je token istekao
    if (this.isTokenExpired()) {
      this.logout();
      return false;
    }
    return true;
  }

  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    try {
      const decoded = this.decodeToken(token);
      const expiry = decoded['exp'];
      
      if (!expiry) {
        // Nema exp claim-a, smatramo da je validan
        return false;
      }

      // exp je u sekundama, a Date.now() je u milisekundama
      const now = Math.floor(Date.now() / 1000);
      return expiry < now;
    } catch {
      return true;
    }
  }

  setToken(token: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('token', token);
    }
  }

  getToken(): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    return localStorage.getItem('token');
  }

  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('token');
    }
  }

  getCurrentUserId(): string {
    const token = this.getToken();
    if (!token) return '';
    
    try {
      const decoded = this.decodeToken(token);
      return decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || '';
    } catch {
      return '';
    }
  }

  getCurrentUserName(): string {
    const token = this.getToken();
    if (!token) return '';
    
    try {
      const decoded = this.decodeToken(token);
      return decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || '';
    } catch {
      return '';
    }
  }

  private decodeToken(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );
      return JSON.parse(jsonPayload);
    } catch (error) {
      console.error('Failed to decode token:', error);
      return {};
    }
  }
}
