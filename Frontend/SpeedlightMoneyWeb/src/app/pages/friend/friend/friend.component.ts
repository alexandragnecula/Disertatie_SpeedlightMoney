import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SelectItemsList, SelectItem } from 'src/app/@core/data/common/selectitem';
import { FriendData } from 'src/app/@core/data/friend';
import { UIService } from 'src/app/shared/ui.service';
import { UserData, UserLookup } from 'src/app/@core/data/userclasses/user';
import { ReplaySubject, Subject } from 'rxjs';
import { MatSelect } from '@angular/material/select';
import { takeUntil, take } from 'rxjs/operators';

@Component({
  selector: 'app-friend',
  templateUrl: './friend.component.html',
  styleUrls: ['./friend.component.scss']
})
export class FriendComponent implements OnInit {
  addFriendForm: FormGroup;
  isLoading = false;
  usersSelectList: SelectItemsList = new SelectItemsList();
  userFriendId: number;

  @ViewChild('singleSelect') singleSelect: MatSelect;

  // public userFriendFilter: FormControl = new FormControl();

  //   /** list of banks filtered by search keyword */
  //   public filteredUsers: ReplaySubject<SelectItem[]> = new ReplaySubject<SelectItem[]>(1);

  // /** Subject that emits when the component has been destroyed. */
  // protected onDestroy = new Subject<void>();

  constructor(private friendData: FriendData,
              private uiService: UIService,
              private userData: UserData) { }

  ngOnInit(): void {
    this.initForm();
    this.GetUsersNotInFriendsList();

    //this.userFriendFilter.setValue(this.addFriendForm.get('userFriendId'));
    //console.log(this.userFriendFilter);
    // listen for search field value changes
    // this.addFriendForm.get('userFriendId').valueChanges
    //   .pipe(takeUntil(this.onDestroy))
    //   .subscribe(() => {
    //     this.filterUsers();
    //   });
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

  // ngOnDestroy() {
  //   this.onDestroy.next();
  //   this.onDestroy.complete();
  // }

  // ngAfterViewInit() {
  //   this.setInitialValue();
  // }

  // protected setInitialValue() {
  //   this.filteredUsers
  //     .pipe(take(1), takeUntil(this.onDestroy))
  //     .subscribe(() => {
  //       // setting the compareWith property to a comparison function
  //       // triggers initializing the selection according to the initial value of
  //       // the form control (i.e. _initializeSelection())
  //       // this needs to be done after the filteredBanks are loaded initially
  //       // and after the mat-option elements are available
  //       this.singleSelect.compareWith = (a: SelectItem, b: SelectItem) => a && b && a.value === b.value;
  //     });
  // }

  // protected filterUsers() {
  //   if (!this.usersSelectList.selectItems) {
  //     return;
  //   }
  //   // get the search keyword
  //   let search = this.addFriendForm.get('userFriendId').value;
  //   // let search = this.userFriendFilter.value;
  //   if (!search) {
  //     this.filteredUsers.next(this.usersSelectList.selectItems.slice());
  //     return;
  //   } else {
  //     search = search.toLowerCase();
  //   }
  //   // filter the banks
  //   this.filteredUsers.next(
  //     this.usersSelectList.selectItems.filter(user => user.label.toLowerCase().indexOf(search) > -1)
  //   );
  // }

}
