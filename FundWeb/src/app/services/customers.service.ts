import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; // Adjust the path as necessary

@Injectable({
  providedIn: 'root'
})
export class CustomersService {
  private baseUrl = `${environment.apiUrl}/api/Customers`; 

  constructor(private http: HttpClient) {}

  createCustomer(customer: any): Observable<any> {
    return this.http.post(`${this.baseUrl}`, customer);
  }

  getCustomerById(customerId: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/${customerId}`);
  }

  updateCustomer(customerId: string, customer: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${customerId}`, customer);
  }

  deleteCustomer(customerId: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${customerId}`);
  }
}