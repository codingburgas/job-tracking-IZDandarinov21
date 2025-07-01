import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class JobAdvertisementService {
  private apiUrl = 'https://localhost:7255/api/jobadvertisements'; 

  constructor(private http: HttpClient, private authService: AuthService) { }

  // Метод за получаване на HTTP хедъри с JWT токен
  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  // Взима всички обяви
  getJobAdvertisements(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl, { headers: this.getAuthHeaders() });
  }

  // Взима обява по ID
  getJobAdvertisementById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`, { headers: this.getAuthHeaders() });
  }

  // Добавя нова обява
  addJobAdvertisement(jobAd: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, jobAd, { headers: this.getAuthHeaders() });
  }

  // Обновява съществуваща обява
  updateJobAdvertisement(id: number, jobAd: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, jobAd, { headers: this.getAuthHeaders() });
  }

  // Изтрива обява
  deleteJobAdvertisement(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`, { headers: this.getAuthHeaders() });
  }
}
