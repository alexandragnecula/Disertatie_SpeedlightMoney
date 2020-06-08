import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MaterialModule } from '../material.module';
import { LayoutModule } from '@angular/cdk/layout';
import { PAGESROUTES } from './pages-routing';
import { SharedModule } from '../shared/shared.module';
import { AddmoneyComponent } from './wallet/addmoney/addmoney.component';
import { WalletModule } from './wallet/wallet.module';



@NgModule({
  declarations: [DashboardComponent],
  imports: [
    PAGESROUTES,
    LayoutModule,
    CommonModule,
    MaterialModule,
    SharedModule,
    WalletModule
  ]
})
export class PagesModule { }
