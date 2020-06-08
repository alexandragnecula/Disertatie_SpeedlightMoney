import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DebtsComponent } from './debts/debts.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MaterialModule } from 'src/app/material.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { DEBTROUTES } from './debt-routing';



@NgModule({
  declarations: [DebtsComponent],
  imports: [
    DEBTROUTES,
    CommonModule,
    LayoutModule,
    MaterialModule,
    SharedModule
  ]
})
export class DebtModule { }
