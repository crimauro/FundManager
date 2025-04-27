import { v4 as uuidv4 } from 'uuid';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { FundsService } from '../../services/funds.service';
import { ApiFundsService } from '../../services/api-funds.service';
import { CustomersService } from '../../services/customers.service';
import { TransactionsService } from '../../services/transactions.service';
import { Fund } from '../../models/fund.model';
import { ApiTransaction } from '../../models/api-transaction.model';
import { Customer} from '../../models/customer.model';
import { ApiFund } from '../../models/api-fund.model';
import { NotificationType } from '../../models/transaction.model';
import {AppConfig} from '../../config/app-config';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-subscribe-fund',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './subscribe-fund.component.html',
  styleUrls: ['./subscribe-fund.component.css']
})
export class SubscribeFundComponent implements OnInit {
  fund?: ApiFund;
  availableBalance: number = 0;
  notificationType: NotificationType = 'EMAIL';
  errorMessage: string = '';
  successMessage: string = '';
  customerId: string = '';
  openingTransaction?: ApiTransaction; 

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fundsService: FundsService,
    private apiFundsService: ApiFundsService,
    private customersService: CustomersService,
    private transactionsService: TransactionsService,
  ) {
    this.customerId = (() => AppConfig.userKey)();
  }

  ngOnInit(): void {
    const fundId = Number(this.route.snapshot.paramMap.get('id'));
    
    this.apiFundsService.getFundById(fundId).subscribe({
      next: (fund) => {
       
        this.fund = fund;
        
        this.customersService.getCustomerById(this.customerId).pipe(
          map((customer: Customer) => customer.availableBalance)
        ).subscribe(balance => {
          this.availableBalance = balance;
        });
      },
      error: (err) => {
        console.error('Error al obtener el fondo:', err);
        this.router.navigate(['/dashboard']);
      }
    });
  }

  onSubmit(): void {
    if (!this.fund) return;

    if (this.availableBalance < this.fund.minimumAmount) {
      this.errorMessage = `No tiene saldo disponible para vincularse al fondo ${this.fund.name}.`;
      return;
    }

    this.openingTransaction = {
      pk: uuidv4(),
      transactionId: uuidv4(),
      customerId: this.customerId,
      fundId: this.fund.id,
      fundName: this.fund.name,
      amount: this.fund.minimumAmount,
      operationType: 'OPENING',
      timestamp: new Date(),
      notificationType: this.notificationType,
    };

    this.transactionsService.createTransaction(this.openingTransaction).subscribe({
      next: (response) => {
        console.log('Respuesta del servidor:', response);
        this.successMessage = `Se ha suscrito exitosamente al fondo ${this.fund?.name}`;
        setTimeout(() => {
          this.router.navigate(['/dashboard']);
        }, 2000);
      },
      error: (err) => {
        console.error('Error en la petición:', err);
        this.errorMessage = 'Ha ocurrido un error al procesar la suscripción';
      }
    });
    
  }
}
