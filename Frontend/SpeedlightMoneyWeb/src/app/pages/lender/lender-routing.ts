import { RouterModule, Routes } from '@angular/router';
import { RoleGuardService as RoleGuard } from '../../auth/role-guard.service';
import { LoanrequestsComponent } from './loanrequests/loanrequests.component';
import { AuthGuardService as AuthGuard } from '../../auth/auth-guard.service';

const routes: Routes = [
  {
    path: '',
    component: LoanrequestsComponent,
    canActivate: [AuthGuard]
  },
  // {
  //   path: 'location',
  //   loadChildren: './location/location.module#LocationModule', canLoad: [RoleGuard], data: { expectedRoles: ['Admin'] }
  // },
];


export const LENDERROUTES = RouterModule.forChild(routes);

