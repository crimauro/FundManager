import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FundsService } from '../../services/funds.service';
import { TransactionsService } from '../../services/transactions.service';
import { Transaction } from '../../models/transaction.model';
import { ApiTransaction } from '../../models/api-transaction.model';
import { Observable } from 'rxjs';
import {AppConfig} from '../../config/app-config';

@Component({
  selector: 'app-transactions-history',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './transactions-history.component.html',
  styleUrls: ['./transactions-history.component.css']
})
export class TransactionsHistoryComponent {
  transactions$: Observable<ApiTransaction[]>;
  customerId: string;

  constructor(
    private fundsService: FundsService,
    private transactionsService: TransactionsService,) {
    this.customerId = (() => AppConfig.userKey)();
    this.transactions$ = this.transactionsService.getTransactionsByCustomerId(this.customerId);
  }

}
