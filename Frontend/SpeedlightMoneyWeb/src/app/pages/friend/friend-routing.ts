import { RouterModule, Routes } from '@angular/router';
import { RoleGuardService as RoleGuard } from '../../auth/role-guard.service';
import { FriendsComponent } from './friends/friends.component';
import { AuthGuardService as AuthGuard } from '../../auth/auth-guard.service';

const routes: Routes = [
  {
    path: '',
    component: FriendsComponent,
    canActivate: [AuthGuard]
  },
];


export const FRIENDROUTES = RouterModule.forChild(routes);

