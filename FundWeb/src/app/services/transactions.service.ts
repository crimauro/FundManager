import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class TransactionsService {
  private baseUrl = `${environment.apiUrl}/api/Transactions`; 

  constructor(private http: HttpClient) {}

  createTransaction(transaction: any): Observable<any> {
    return this.http.post(`${this.baseUrl}`, transaction);
  }

  getAllTransactions(): Observable<any> {
    return this.http.get(`${this.baseUrl}`);
  }

  getTransactionById(transactionId: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/${transactionId}`);
  }

  deleteTransaction(transactionId: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${transactionId}`);
  }

  getTransactionsByFundId(fundId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/fund/${fundId}`);
  }

  getTransactionsByCustomerId(customerId: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/customer/${customerId}`);
  }
}