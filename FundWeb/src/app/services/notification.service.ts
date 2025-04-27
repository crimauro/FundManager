import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private baseUrl = `${environment.apiUrl}/api/Notification`;

  constructor(private http: HttpClient) {}

  sendEmail(email: string, subject: string, message: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/send-email`, null, {
      params: { email, subject, message }
    });
  }

  sendSms(phoneNumber: string, message: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/send-sms`, null, {
      params: { phoneNumber, message }
    });
  }

  sendToTopic(subject: string, message: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/send-to-topic`, null, {
      params: { subject, message }
    });
  }
}