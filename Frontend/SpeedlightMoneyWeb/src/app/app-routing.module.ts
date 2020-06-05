import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { NotFoundComponent } from './notfound/notfound.component';
import {AuthGuardService as AuthGuard} from './auth/auth-guard.service';
import { RegisterComponent } from './auth/register/register.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: '',
    // loadChildren: './pages/pages.module#PagesModule', canLoad: [AuthGuard]
    loadChildren: () => import('./pages/pages.module').then(m => m.PagesModule), canLoad: [AuthGuard]
  },
  {
    path: '**',
    component: NotFoundComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
