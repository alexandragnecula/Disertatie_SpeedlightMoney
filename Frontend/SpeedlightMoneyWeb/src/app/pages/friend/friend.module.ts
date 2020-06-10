import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutModule } from '@angular/cdk/layout';
import { SharedModule } from 'src/app/shared/shared.module';
import { FRIENDROUTES } from './friend-routing';
import { FriendsComponent } from './friends/friends.component';



@NgModule({
  declarations: [FriendsComponent],
  imports: [
    FRIENDROUTES,
    CommonModule,
    LayoutModule,
    SharedModule
  ]
})
export class FriendModule { }
