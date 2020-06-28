import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SelectItemsList, SelectItem } from 'src/app/@core/data/common/selectitem';
import { FriendData } from 'src/app/@core/data/friend';
import { UIService } from 'src/app/shared/ui.service';
import { UserData, UserLookup } from 'src/app/@core/data/userclasses/user';
import { ReplaySubject, Subject, Subscription } from 'rxjs';
import { MatSelect } from '@angular/material/select';
import { takeUntil, take } from 'rxjs/operators';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-friend',
  templateUrl: './friend.component.html',
  styleUrls: ['./friend.component.scss']
})
export class FriendComponent implements OnInit, OnDestroy {
  addFriendForm: FormGroup;
  isLoading = false;
  usersSelectList: SelectItemsList = new SelectItemsList();
  userFriendId: number;
  isExplorerSubscription: Subscription;
  isExplorer: boolean;

  @ViewChild('singleSelect') singleSelect: MatSelect;

  // public userFriendFilter: FormControl = new FormControl();

  //   /** list of banks filtered by search keyword */
  //   public filteredUsers: ReplaySubject<SelectItem[]> = new ReplaySubject<SelectItem[]>(1);

  // /** Subject that emits when the component has been destroyed. */
  // protected onDestroy = new Subject<void>();

  constructor(private friendData: FriendData,
              private uiService: UIService,
              private userData: UserData,
              private authService: AuthService) { }

  ngOnInit(): void {
    this.isExplorerSubscription = this.authService.isExplorer.subscribe(isExplorer => {
      this.isExplorer = isExplorer;
    });
    this.initForm();
    this.GetUsersNotInFriendsList();
  }

  initForm() {
    this.addFriendForm = new FormGroup({
        userFriendId: new FormControl('', [Validators.required])
    });
  }

  GetUsersNotInFriendsList(){
    this.userData.GetUsersNotInFriendsListDropdown().subscribe((users: SelectItemsList) => {
      this.usersSelectList = users;
    },
    error => {
        this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  ngOnDestroy(): void {
    this.isExplorerSubscription.unsubscribe();
  }

}
