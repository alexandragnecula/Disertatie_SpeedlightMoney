import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { LoanData, LoansLookup } from 'src/app/@core/data/loan';
import { MatTableDataSource } from '@angular/material/table';
import { UIService } from 'src/app/shared/ui.service';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { Result } from 'src/app/@core/data/common/result';
import { BorrowrequestshistoryComponent } from '../borrowrequestshistory/borrowrequestshistory.component';

@Component({
  selector: 'app-borrowrequests',
  templateUrl: './borrowrequests.component.html',
  styleUrls: ['./borrowrequests.component.scss']
})
export class BorrowrequestsComponent implements OnInit, AfterViewInit{
  isLoading = false;
  displayedColumns = ['lenderName', 'amount', 'currencyName', 'termName', 'borrowDate', 'returnDate', 'dueDate', 'loanStatusName', 'cancelloanrequest'];
  dataSource = new MatTableDataSource<LoansLookup>();
  currentUserIdSubscription: Subscription;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(BorrowrequestshistoryComponent) borrowrequestshistoryComponent: BorrowrequestshistoryComponent;

  constructor(private loanData: LoanData,
              private uiService: UIService,
              private authService: AuthService) { }

  ngOnInit(): void {
    this.getBorrowRequestsForCurrentUser();
  }

  getBorrowRequestsForCurrentUser() {
    this.loanData.GetBorrowRequestsForCurrentUser().subscribe((loansList: LoansLookup[]) => {
      this.isLoading = false;
      this.dataSource.data = loansList;
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

  cancelLoanRequest(id){
    this.isLoading = true;

    this.loanData.CancelLoanRequest(id).subscribe((res: Result) => {
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
      this.getBorrowRequestsForCurrentUser();
      this.borrowrequestshistoryComponent.getBorrowRequestsHistoryForCurrentUser();

      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }
}
