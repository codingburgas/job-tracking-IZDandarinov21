import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  private apiUrl = 'https://localhost:7255/api/auth';

  private userSubject = new BehaviorSubject<any | null>(null);
  public user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    this.loadCurrentUser();
  }

  // Метод за регистрация на потребител
  register(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }

  // Метод за вход на потребител
  login(credentials: { username: string, password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials).pipe(
      tap((response: any) => {
        if (response && response.token) {
          localStorage.setItem('jwtToken', response.token);
          this.userSubject.next(this.decodeToken(response.token));
        }
      })
    );
  }

  // Метод за изход (логаут) на потребител
  logout(): void {
    localStorage.removeItem('jwtToken');
    this.userSubject.next(null);
    this.router.navigate(['/login']); // Пренасочване към страницата за вход след изход
  }

  // Проверява дали потребителят е влязъл
  isLoggedIn(): boolean {
    return !!localStorage.getItem('jwtToken');
  }

  // Връща JWT токена
  getToken(): string | null {
    return localStorage.getItem('jwtToken');
  }

  // Декодира JWT токена за извличане на информация за потребителя
  private decodeToken(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join(''));
      return JSON.parse(jsonPayload);
    } catch (e) {
      console.error("Error decoding token:", e);
      return null;
    }
  }

  // Зарежда информацията за текущия потребител при стартиране на услугата (ако има токен)
  private loadCurrentUser(): void {
    const token = this.getToken();
    if (token) {
      const decodedUser = this.decodeToken(token);
      if (decodedUser) {
        const expirationTime = decodedUser.exp * 1000; // JWT exp е в секунди, трябва да е в милисекунди
        if (Date.now() < expirationTime) { // Проверява дали токенът не е изтекъл
          this.userSubject.next(decodedUser);
        } else {
          this.logout(); // Ако токенът е изтекъл, потребителят се отписва
        }
      }
    }
  }
}
