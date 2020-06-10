import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersComponent } from './users/users.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { LayoutModule } from '@angular/cdk/layout';
import { MaterialModule } from 'src/app/material.module';
import { BORROWERSROUTES } from './borrower-routing';
import { BorrowrequestComponent } from './borrowrequest/borrowrequest.component';
import { BorrowrequestsComponent } from './borrowrequests/borrowrequests.component';
import { BorrowrequestshistoryComponent } from './borrowrequestshistory/borrowrequestshistory.component';
import {MatExpansionModule} from '@angular/material/expansion';
@NgModule({
  declarations: [UsersComponent, BorrowrequestComponent, BorrowrequestsComponent, BorrowrequestshistoryComponent],
  imports: [
    BORROWERSROUTES,
    LayoutModule,
    CommonModule,
    MaterialModule,
    SharedModule,
    MatExpansionModule
  ],
  entryComponents: [UsersComponent, BorrowrequestComponent, BorrowrequestsComponent]
})
export class BorrowerModule { }
