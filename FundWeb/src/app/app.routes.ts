import { Routes } from '@angular/router';
// Importar los componentes de la carpeta pages
import { CancelFundComponent } from './pages/cancel-fund/cancel-fund.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { SubscribeFundComponent } from './pages/subscribe-fund/subscribe-fund.component';
import { TransactionsHistoryComponent } from './pages/transactions-history/transactions-history.component';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'cancel/:id', component: CancelFundComponent },
  { path: 'subscribe/:id', component: SubscribeFundComponent },
  { path: 'history', component: TransactionsHistoryComponent }
];