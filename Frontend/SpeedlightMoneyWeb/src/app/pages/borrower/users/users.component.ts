import { Component, OnInit, ViewChild, Input, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { UserLookup, UserData, UserList, User } from 'src/app/@core/data/userclasses/user';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UIService } from 'src/app/shared/ui.service';
import { MatDialog } from '@angular/material/dialog';
import { BorrowrequestComponent } from '../borrowrequest/borrowrequest.component';
import { AddLoanCommand, LoanData } from 'src/app/@core/data/loan';
import { Result } from 'src/app/@core/data/common/result';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { repeatWhen } from 'rxjs/operators';
import { BorrowrequestsComponent } from '../borrowrequests/borrowrequests.component';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit, AfterViewInit {
  isLoading = true;
  displayedColumns = ['email', 'firstName', 'country', 'city', 'currentStatus', 'phoneNumber', 'actions'];
  dataSource = new MatTableDataSource<UserLookup>();
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(BorrowrequestsComponent) borrowreqests: BorrowrequestsComponent;

  lenderId: number;
  borrowerId: number;

  loanRequestForm: FormGroup;

  currentUserIdSubscription: Subscription;

  constructor(public dialog: MatDialog,
              private userData: UserData,
              private loanData: LoanData,
              private uiService: UIService,
              private authService: AuthService) { }

  ngOnInit(): void {
    this.getAllLenders();

    this.currentUserIdSubscription = this.authService.currentUserId.subscribe(userId => {
      this.borrowerId = userId;
      });
  }

  getAllLenders() {
    this.userData.getUsers().subscribe((usersList: UserLookup[]) => {
      this.isLoading = false;
      this.dataSource.data = usersList;
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

  openDialogToAdd(row) {
    const dialogRef = this.dialog.open(BorrowrequestComponent, {
      width: '600px',
      data: {
        lenderId: row.id,
        borrowerId: this.borrowerId,
        lenderName: row.firstName + ' ' + row.lastName
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.requestLoan(result, row.id);
      }
    });
  }

  requestLoan(form, id: number) {
    this.isLoading = true;
    const addLoanCommand: AddLoanCommand = {
      termId: +form.value.termId,
      currencyId: +form.value.currencyId,
      description: form.value.description,
      amount: +form.value.amount,
      lenderId: id
    } as AddLoanCommand;

    this.loanData.RequestLoan(addLoanCommand).subscribe((res: Result) => {
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
      this.borrowreqests.getBorrowRequestsForCurrentUser();
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }
}
