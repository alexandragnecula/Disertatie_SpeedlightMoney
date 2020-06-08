import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoanrequestsComponent } from './loanrequests/loanrequests.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MaterialModule } from 'src/app/material.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { LENDERROUTES } from './lender-routing';
import { CreditsComponent } from './credits/credits.component';
import { LoanrequestshistoryComponent } from './loanrequestshistory/loanrequestshistory.component';
import { CreditshistoryComponent } from './creditshistory/creditshistory.component';



@NgModule({
  declarations: [LoanrequestsComponent, CreditsComponent, LoanrequestshistoryComponent, CreditshistoryComponent],
  imports: [
    LENDERROUTES,
    CommonModule,
    LayoutModule,
    MaterialModule,
    SharedModule
  ]
})
export class LenderModule { }
