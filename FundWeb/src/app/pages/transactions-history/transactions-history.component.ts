import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FundsService } from '../../services/funds.service';
import { Transaction } from '../../models/transaction.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-transactions-history',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './transactions-history.component.html',
  styleUrls: ['./transactions-history.component.css']
})
export class TransactionsHistoryComponent {
  transactions$: Observable<Transaction[]>;

  constructor(private fundsService: FundsService) {
    this.transactions$ = this.fundsService.getTransactions();
  }
}
