import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MaterialModule } from '../material.module';
import { LayoutModule } from '@angular/cdk/layout';
import { PAGESROUTES } from './pages-routing';
import { SharedModule } from '../shared/shared.module';
import { WalletModule } from './wallet/wallet.module';
import { ProfileComponent } from './profile/profile.component';
import { TransactionshistoryComponent } from './transactionshistory/transactionshistory.component';

@NgModule({
  declarations: [DashboardComponent, ProfileComponent, TransactionshistoryComponent],
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
