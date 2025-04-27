import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiFundsService {
  private baseUrl = `${environment.apiUrl}/api/Funds`;

  constructor(private http: HttpClient) {}

  createFund(fund: any): Observable<any> {
    return this.http.post(`${this.baseUrl}`, fund);
  }

  getAllFunds(): Observable<any> {
    return this.http.get(`${this.baseUrl}`);
  }

  getFundById(fundId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${fundId}`);
  }

  updateFund(fundId: number, fund: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${fundId}`, fund);
  }

  deleteFund(fundId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${fundId}`);
  }
}