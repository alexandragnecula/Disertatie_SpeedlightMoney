import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddmoneyComponent } from './addmoney/addmoney.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MaterialModule } from 'src/app/material.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { WALLETROUTES } from './wallet-routing';


@NgModule({
  declarations: [AddmoneyComponent],
  imports: [
    WALLETROUTES,
    CommonModule,
    LayoutModule,
    MaterialModule,
    SharedModule
  ],
  entryComponents: [AddmoneyComponent]
})
export class WalletModule { }
