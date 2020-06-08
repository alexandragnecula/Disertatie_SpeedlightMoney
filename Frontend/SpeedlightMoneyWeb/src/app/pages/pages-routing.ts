import { RouterModule, Routes } from '@angular/router';
import { RoleGuardService as RoleGuard } from '../auth/role-guard.service';
import { AuthGuardService as AuthGuard } from '../auth/auth-guard.service';
import { DashboardComponent } from './dashboard/dashboard.component';
import { NotFoundComponent } from '../notfound/notfound.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'lenders',
    loadChildren: () => import('./borrower/borrower.module').then(m => m.BorrowerModule), canLoad: [AuthGuard]
  },
  {
  path: 'debts',
  loadChildren: () => import('./debt/debt.module').then(m => m.DebtModule), canLoad: [AuthGuard]
  },
  {
  path: 'credits',
  loadChildren: () => import('./lender/lender.module').then(m => m.LenderModule), canLoad: [AuthGuard]
  },
  {
    path: '**',
    component: NotFoundComponent,
  }
];


export const PAGESROUTES = RouterModule.forChild(routes);

