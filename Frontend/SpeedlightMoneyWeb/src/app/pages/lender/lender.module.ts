import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoanrequestsComponent } from './loanrequests/loanrequests.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MaterialModule } from 'src/app/material.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { LENDERROUTES } from './lender-routing';



@NgModule({
  declarations: [LoanrequestsComponent],
  imports: [
    LENDERROUTES,
    CommonModule,
    LayoutModule,
    MaterialModule,
    SharedModule
  ]
})
export class LenderModule { }
