import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { NotFoundComponent } from './notfound/notfound.component';

const routes: Routes = [
  {
    path: '',
    component: LoginComponent
  },
  // {
  //   path: 'register',
  //   component: RegisterComponent
  // },
  // { 
  //   path: '',
  //   loadChildren: './pages/pages.module#PagesModule', canLoad: [AuthGuard] 
  // },
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
