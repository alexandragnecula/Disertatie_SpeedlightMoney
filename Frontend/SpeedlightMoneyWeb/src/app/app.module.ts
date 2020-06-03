import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtModule } from '@auth0/angular-jwt';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from './shared/shared.module';
import { NotFoundComponent } from './notfound/notfound.component';
import { AuthModule } from './auth/auth.module';
import { CoreModule } from './@core/core.module';
import { AuthService } from './auth/auth.service';
import { AuthGuardService } from './auth/auth-guard.service';
import { RoleGuardService } from './auth/role-guard.service';
import { UIService } from './shared/ui.service';
import { ErrorService } from './shared/error.service';

@NgModule({
  declarations: [
    AppComponent,
    NotFoundComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem('access_token');
        },
        whitelistedDomains: ['localhost:4200/login', 'localhost:4200/register']
      }
    }),
    SharedModule,
    AuthModule,
    CoreModule.forRoot(),
  ],
  providers: [AuthService, AuthGuardService, RoleGuardService, UIService, ErrorService],
  bootstrap: [AppComponent]
})
export class AppModule { }
