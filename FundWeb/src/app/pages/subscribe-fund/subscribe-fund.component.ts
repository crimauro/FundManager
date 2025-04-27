import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FundsService } from '../../services/funds.service';
import { Fund } from '../../models/fund.model';
import { NotificationType } from '../../models/transaction.model';

@Component({
  selector: 'app-subscribe-fund',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './subscribe-fund.component.html',
  styleUrls: ['./subscribe-fund.component.css']
})
export class SubscribeFundComponent implements OnInit {
  fund?: Fund;
  availableBalance: number = 0;
  notificationType: NotificationType = 'Email';
  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fundsService: FundsService
  ) {}

  ngOnInit(): void {
    const fundId = Number(this.route.snapshot.paramMap.get('id'));
    this.fund = this.fundsService.getFundById(fundId);
    
    if (!this.fund) {
      this.router.navigate(['/dashboard']);
      return;
    }

    this.fundsService.getAvailableBalance().subscribe(balance => {
      this.availableBalance = balance;
    });
  }

  onSubmit(): void {
    if (!this.fund) return;

    if (this.availableBalance < this.fund.minimumSubscriptionAmount) {
      this.errorMessage = `No tiene saldo disponible para vincularse al fondo ${this.fund.name}.`;
      return;
    }

    const success = this.fundsService.subscribeFund(
      this.fund.id,
      this.fund.minimumSubscriptionAmount,
      this.notificationType
    );

    if (success) {
      this.successMessage = `Se ha suscrito exitosamente al fondo ${this.fund.name}`;
      setTimeout(() => {
        this.router.navigate(['/dashboard']);
      }, 2000);
    } else {
      this.errorMessage = 'Ha ocurrido un error al procesar la suscripción';
    }
  }
}
