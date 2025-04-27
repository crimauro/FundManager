import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';


// Importar los componentes de la carpeta pages
import { CancelFundComponent } from './pages/cancel-fund/cancel-fund.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { SubscribeFundComponent } from './pages/subscribe-fund/subscribe-fund.component';
import { TransactionsHistoryComponent } from './pages/transactions-history/transactions-history.component';

// Importar los servicios de la carpeta services
import { FundsService } from './services/funds.service';


@NgModule({
  declarations: [],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'cancel/:id', component: CancelFundComponent },
      { path: 'subscribe/:id', component: SubscribeFundComponent },
      { path: 'history', component: TransactionsHistoryComponent }
    ]),
    AppComponent,
    CancelFundComponent,
    DashboardComponent,
    SubscribeFundComponent,
    TransactionsHistoryComponent,
    DashboardComponent
  ],
  providers: [FundsService]
})
export class AppModule { }