import { RouterModule, Routes } from '@angular/router';
import { RoleGuardService as RoleGuard } from '../../auth/role-guard.service';

const routes: Routes = [
//   {
//     path: '',
//     component: DashboardComponent,
//     canActivate: [AuthGuard]
//   },
  // {
  //   path: 'location',
  //   loadChildren: './location/location.module#LocationModule', canLoad: [RoleGuard], data: { expectedRoles: ['Admin'] }
  // },
];


export const WALLETROUTES = RouterModule.forChild(routes);

