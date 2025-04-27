import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Fund, UserFund } from '../models/fund.model';
import { Transaction, NotificationType } from '../models/transaction.model';

@Injectable({
  providedIn: 'root'
})
export class FundsService {
  private availableBalance = new BehaviorSubject<number>(10000000); // Initial balance: 10M COP
  private funds: Fund[] = [
    { id: 1, name: 'Fondo Conservador', minimumSubscriptionAmount: 500000 },
    { id: 2, name: 'Fondo Moderado', minimumSubscriptionAmount: 1000000 },
    { id: 3, name: 'Fondo Agresivo', minimumSubscriptionAmount: 2000000 }
  ];
  
  private userFunds = new BehaviorSubject<UserFund[]>([]);
  private transactions = new BehaviorSubject<Transaction[]>([]);

  constructor() {}

  getAvailableBalance(): Observable<number> {
    return this.availableBalance.asObservable();
  }

  getAvailableFunds(): Fund[] {
    return this.funds.filter(fund => 
      !this.userFunds.value.some(userFund => userFund.id === fund.id)
    );
  }

  getUserFunds(): Observable<UserFund[]> {
    return this.userFunds.asObservable();
  }

  getTransactions(): Observable<Transaction[]> {
    return this.transactions.asObservable();
  }

  getFundById(fundId: number): Fund | undefined {
    return this.funds.find(fund => fund.id === fundId);
  }

  subscribeFund(fundId: number, amount: number, notificationType: NotificationType): boolean {
    const fund = this.getFundById(fundId);
    if (!fund) return false;

    const currentBalance = this.availableBalance.value;
    if (currentBalance < amount) return false;

    // Update balance
    this.availableBalance.next(currentBalance - amount);

    // Add to user funds
    const userFund: UserFund = {
      ...fund,
      subscribedAmount: amount
    };
    this.userFunds.next([...this.userFunds.value, userFund]);

    // Create transaction
    const transaction: Transaction = {
      id: this.transactions.value.length + 1,
      fundId,
      fundName: fund.name,
      operationType: 'Apertura',
      amount,
      dateTime: new Date(),
      notificationType
    };
    this.transactions.next([...this.transactions.value, transaction]);

    return true;
  }

  cancelSubscription(fundId: number): boolean {
    const userFund = this.userFunds.value.find(f => f.id === fundId);
    if (!userFund) return false;

    // Return amount to balance
    this.availableBalance.next(this.availableBalance.value + userFund.subscribedAmount);

    // Remove from user funds
    this.userFunds.next(this.userFunds.value.filter(f => f.id !== fundId));

    // Create transaction
    const transaction: Transaction = {
      id: this.transactions.value.length + 1,
      fundId,
      fundName: userFund.name,
      operationType: 'Cancelación',
      amount: userFund.subscribedAmount,
      dateTime: new Date(),
      notificationType: 'Email' // Default notification type for cancellations
    };
    this.transactions.next([...this.transactions.value, transaction]);

    return true;
  }
}
