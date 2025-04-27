import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ActivatedRoute, Router } from '@angular/router';
import { FundsService } from '../../services/funds.service';
import { TransactionsService } from '../../services/transactions.service';
import  {ActiveLinkagesService} from '../../services/active-linkages.service';
import { ApiTransaction } from '../../models/api-transaction.model';
import { ActiveLinkage } from '../../models/active-linkage.model'; 
import { UserFund } from '../../models/fund.model';
import {AppConfig} from '../../config/app-config';

@Component({
  selector: 'app-cancel-fund',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './cancel-fund.component.html',
  styleUrls: ['./cancel-fund.component.css']
})
export class CancelFundComponent implements OnInit {
  userFund?: ActiveLinkage;
  errorMessage: string = '';
  successMessage: string = '';
  customerId: string = '';
  closureTransaction?: ApiTransaction; 

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fundsService: FundsService,
    private activeLinkagesService: ActiveLinkagesService,
    private transactionsService: TransactionsService
  ) {
    this.customerId = (() => AppConfig.userKey)();
  }

  ngOnInit(): void {
    const fundId = Number(this.route.snapshot.paramMap.get('id'));

    this.activeLinkagesService.getLinkagesByCustomer(this.customerId).subscribe((userFunds: ActiveLinkage[]) => {
      this.userFund = userFunds.find((f: ActiveLinkage) => f.fundId === fundId);
      if (!this.userFund) {
        this.router.navigate(['/dashboard']);
      }
    });
  }

  onConfirm(): void {
    if (!this.userFund) return;

    this.closureTransaction = {
      pk: uuidv4(),
      transactionId: uuidv4(),
      customerId: this.customerId,
      fundId: this.userFund.fundId,
      fundName: this.userFund.fundName,
      amount: this.userFund.linkedAmount,
      operationType: 'CLOSURE',
      timestamp: new Date(),
      notificationType: '',
    };

    const success = this.transactionsService.createTransaction(this.closureTransaction);

    if (success) {
      this.successMessage = `Se ha cancelado exitosamente la suscripción al fondo ${this.userFund.fundName}`;
      setTimeout(() => {
        this.router.navigate(['/dashboard']);
      }, 2000);
    } else {
      this.errorMessage = 'Ha ocurrido un error al procesar la cancelación';
    }
  }
}
function uuidv4(): string {
  throw new Error('Function not implemented.');
}

