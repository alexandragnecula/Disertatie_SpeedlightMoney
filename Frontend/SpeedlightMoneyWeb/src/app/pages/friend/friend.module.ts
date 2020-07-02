import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutModule } from '@angular/cdk/layout';
import { SharedModule } from 'src/app/shared/shared.module';
import { FRIENDROUTES } from './friend-routing';
import { FriendsComponent } from './friends/friends.component';
import { FriendComponent } from './friend/friend.component';
import { FriendborrowrequestComponent } from './friendborrowrequest/friendborrowrequest.component';


@NgModule({
  declarations: [FriendsComponent, FriendComponent, FriendborrowrequestComponent],
  imports: [
    FRIENDROUTES,
    CommonModule,
    LayoutModule,
    SharedModule,
  ],
  entryComponents: [FriendComponent]
})
export class FriendModule { }
