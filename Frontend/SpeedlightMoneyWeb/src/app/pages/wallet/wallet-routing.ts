import { RouterModule, Routes } from '@angular/router';
import { RoleGuardService as RoleGuard } from '../../auth/role-guard.service';
import {AuthGuardService as AuthGuard} from '../../auth/auth-guard.service';
import { DashboardComponent } from '../dashboard/dashboard.component';

const routes: Routes = [
  // {
  //   path: '',
  //   component: DashboardComponent,
  //   canActivate: [AuthGuard]
  // },
  // {
  //   path: 'location',
  //   loadChildren: './location/location.module#LocationModule', canLoad: [RoleGuard], data: { expectedRoles: ['Admin'] }
  // },
];


export const WALLETROUTES = RouterModule.forChild(routes);

