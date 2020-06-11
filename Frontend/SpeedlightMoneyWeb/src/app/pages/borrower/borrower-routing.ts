import { RouterModule, Routes } from '@angular/router';
import { RoleGuardService as RoleGuard } from '../../auth/role-guard.service';
import { UsersComponent } from './users/users.component';
import { AuthGuardService as AuthGuard } from '../../auth/auth-guard.service';

const routes: Routes = [
  {
    path: '',
    component: UsersComponent,
    canActivate: [AuthGuard]
  },
];

export const BORROWERSROUTES = RouterModule.forChild(routes);

