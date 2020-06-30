import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddmoneyComponent } from './addmoney/addmoney.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MaterialModule } from 'src/app/material.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { WALLETROUTES } from './wallet-routing';
import { SendmoneyComponent } from './sendmoney/sendmoney.component';
import { BanktransferComponent } from './banktransfer/banktransfer.component';

@NgModule({
  declarations: [AddmoneyComponent, SendmoneyComponent, BanktransferComponent],
  imports: [
    WALLETROUTES,
    CommonModule,
    LayoutModule,
    SharedModule
  ],
  entryComponents: [AddmoneyComponent, SendmoneyComponent]
})
export class WalletModule { }
