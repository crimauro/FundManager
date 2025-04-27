import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ActivatedRoute, Router } from '@angular/router';
import { FundsService } from '../../services/funds.service';
import { UserFund } from '../../models/fund.model';

@Component({
  selector: 'app-cancel-fund',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './cancel-fund.component.html',
  styleUrls: ['./cancel-fund.component.css']
})
export class CancelFundComponent implements OnInit {
  userFund?: UserFund;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fundsService: FundsService
  ) {}

  ngOnInit(): void {
    const fundId = Number(this.route.snapshot.paramMap.get('id'));
    this.fundsService.getUserFunds().subscribe(userFunds => {
      this.userFund = userFunds.find(f => f.id === fundId);
      if (!this.userFund) {
        this.router.navigate(['/dashboard']);
      }
    });
  }

  onConfirm(): void {
    if (!this.userFund) return;

    const success = this.fundsService.cancelSubscription(this.userFund.id);

    if (success) {
      this.successMessage = `Se ha cancelado exitosamente la suscripción al fondo ${this.userFund.name}`;
      setTimeout(() => {
        this.router.navigate(['/dashboard']);
      }, 2000);
    } else {
      this.errorMessage = 'Ha ocurrido un error al procesar la cancelación';
    }
  }
}
