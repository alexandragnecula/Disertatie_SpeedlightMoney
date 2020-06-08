import { RouterModule, Routes } from '@angular/router';
import { RoleGuardService as RoleGuard } from '../../auth/role-guard.service';
import { AuthGuardService as AuthGuard } from '../../auth/auth-guard.service';
import { DebtsComponent } from './debts/debts.component';

const routes: Routes = [
  {
    path: '',
    component: DebtsComponent,
    canActivate: [AuthGuard]
  },
];


export const DEBTROUTES = RouterModule.forChild(routes);

