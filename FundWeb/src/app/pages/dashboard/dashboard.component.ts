import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FundsService } from '../../services/funds.service';
import { CustomersService } from '../../services/customers.service';
import { ApiFundsService } from '../../services/api-funds.service';
import  {ActiveLinkagesService} from '../../services/active-linkages.service';
import { Fund, UserFund } from '../../models/fund.model';
import { ApiFund } from '../../models/api-fund.model';
import { Customer} from '../../models/customer.model';
import { ActiveLinkage } from '../../models/active-linkage.model'; 
import { Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import {AppConfig} from '../../config/app-config';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  availableBalance$: Observable<number>;
  availableFunds$: Observable<ApiFund[]>;
  userFunds$: Observable<ActiveLinkage[]>;
  customerId: string;

  constructor(
    private customersService: CustomersService,
    private fundsService: FundsService,
    private apiFundsService: ApiFundsService,
    private activeLinkagesService: ActiveLinkagesService,) {
    this.customerId = (() => AppConfig.userKey)();

    this.availableBalance$ = this.customersService.getCustomerById(this.customerId).pipe(
      map((customer: Customer) => customer.availableBalance)
    );

    this.userFunds$ = this.activeLinkagesService.getLinkagesByCustomer(this.customerId).pipe(
      (map((userFunds: ActiveLinkage[]) => userFunds.filter((f: ActiveLinkage) => f.linkedAmount > 0))
    ));

    
    this.availableFunds$ = combineLatest([
      this.apiFundsService.getAllFunds(),
      this.userFunds$
    ]).pipe(
      map(([allFunds, userFunds]) => {
        return allFunds.filter((fund: ApiFund) => 
          !userFunds.some(userFund => userFund.fundId === fund.id)
        );
      })
    );
  }
  
  ngOnInit(): void {}

}
