import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { FriendsLookup, FriendData, FriendsList, AddFriendCommand } from 'src/app/@core/data/friend';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { UIService } from 'src/app/shared/ui.service';
import { FriendComponent } from '../friend/friend.component';
import { Result } from 'src/app/@core/data/common/result';
import { AuthService } from 'src/app/auth/auth.service';
import { Subscription } from 'rxjs';
import { AddLoanCommand, LoanData } from 'src/app/@core/data/loan';
import { FriendborrowrequestComponent } from '../friendborrowrequest/friendborrowrequest.component';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.scss']
})
export class FriendsComponent implements OnInit, AfterViewInit, OnDestroy {
  isLoading = true;
  displayedColumns = ['userFriendName', 'userFriendPhoneNumber', 'actions'];
  dataSource = new MatTableDataSource<FriendsLookup>();
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  currentUserIdSubscription: Subscription;
  userId: number;
  borowerIdSubscription: Subscription;
  borrowerId: number;

  isExplorerSubscription: Subscription;
  isExplorer: boolean;

  mapPhoneNumberToUserFriendId: { [phoneNumber: string]: number; } = {};

  constructor(public dialog: MatDialog,
              private friendData: FriendData, private uiService: UIService,
              private authService: AuthService,
              private loanData: LoanData) { }

  ngOnInit(): void {
    this.getFriends();

    this.currentUserIdSubscription = this.authService.currentUserId.subscribe(userId => {
      this.userId = userId;
      });

    this.borowerIdSubscription = this.authService.currentUserId.subscribe(borrowerId => {
        this.borrowerId = borrowerId;
      });

    this.isExplorerSubscription = this.authService.isExplorer.subscribe(isExplorer => {
        this.isExplorer = isExplorer;
    });
  }

  getFriends() {
    this.friendData.GetFriendsForCurrentUser().subscribe((friendsList: FriendsLookup[]) => {
      this.isLoading = false;
      this.dataSource.data = friendsList;
      friendsList.forEach(x => {
        this.mapPhoneNumberToUserFriendId[x.userFriendPhoneNumber] = x.userFriendId;
      });
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }


  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  doFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  openDialogToAdd() {
    const dialogRef = this.dialog.open(FriendComponent, {
      width: '600px',
      data: {
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.addFriend(result);
      }
    });
  }

  addFriend(form){
    this.isLoading = true;
    const addFriendCommand: AddFriendCommand = {
      userFriendId: +form.value.userFriendId,
      userId: this.userId
    } as AddFriendCommand;

    this.friendData.AddFriendForCurrentUser(addFriendCommand).subscribe((res: Result) => {
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
      this.getFriends();
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  openDialogToRequestLoan(row) {
    const dialogRef = this.dialog.open(FriendborrowrequestComponent, {
      width: '600px',
      data: {
        lenderId: this.mapPhoneNumberToUserFriendId[row.userFriendPhoneNumber],
        lenderName: row.userFriendName
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.requestLoan(result);
      }
    });
  }

  requestLoan(form) {
    this.isLoading = true;
    const addLoanCommand: AddLoanCommand = {
      termId: +form.value.termId,
      currencyId: +form.value.currencyId,
      description: form.value.description,
      amount: +form.value.amount,
      lenderId: form.value.lenderId,
      borrowerId: this.userId
    } as AddLoanCommand;

    this.loanData.RequestLoan(addLoanCommand).subscribe((res: Result) => {
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
      // this.borrowreqests.getBorrowRequestsForCurrentUser();
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  ngOnDestroy(): void {
    this.currentUserIdSubscription.unsubscribe();
    this.isExplorerSubscription.unsubscribe();
    this.borowerIdSubscription.unsubscribe();
  }
}
