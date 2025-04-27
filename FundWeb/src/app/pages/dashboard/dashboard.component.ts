import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FundsService } from '../../services/funds.service';
import { Fund, UserFund } from '../../models/fund.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  availableBalance$: Observable<number>;
  availableFunds: Fund[] = [];
  userFunds$: Observable<UserFund[]>;

  constructor(private fundsService: FundsService) {
    this.availableBalance$ = this.fundsService.getAvailableBalance();
    this.userFunds$ = this.fundsService.getUserFunds();
  }

  ngOnInit(): void {
    this.availableFunds = this.fundsService.getAvailableFunds();
  }
}
