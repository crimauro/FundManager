import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ActiveLinkagesService {
  private baseUrl = `${environment.apiUrl}/api/ActiveLinkages`;

  constructor(private http: HttpClient) {}

  createLinkage(linkage: any): Observable<any> {
    return this.http.post(`${this.baseUrl}`, linkage);
  }

  getLinkage(customerId: string, fundId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${customerId}/${fundId}`);
  }

  deleteLinkage(customerId: string, fundId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${customerId}/${fundId}`);
  }

  getLinkagesByCustomer(customerId: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/${customerId}`);
  }

  getLinkagesByCustomerAndCategory(customerId: string, category: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/${customerId}/category/${category}`);
  }
}