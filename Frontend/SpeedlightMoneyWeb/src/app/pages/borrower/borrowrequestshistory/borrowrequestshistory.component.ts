import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { LoansLookup, LoanData } from 'src/app/@core/data/loan';
import { Subscription } from 'rxjs';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { UIService } from 'src/app/shared/ui.service';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-borrowrequestshistory',
  templateUrl: './borrowrequestshistory.component.html',
  styleUrls: ['./borrowrequestshistory.component.scss']
})
export class BorrowrequestshistoryComponent implements OnInit, AfterViewInit{
  isLoading = false;
  displayedColumns = ['lenderName', 'amount', 'currencyName', 'termName', 'borrowDate', 'returnDate', 'dueDate', 'loanStatusName'];
  dataSource = new MatTableDataSource<LoansLookup>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private loanData: LoanData,
              private uiService: UIService) { }

  ngOnInit(): void {
    this.getBorrowRequestsHistoryForCurrentUser();
  }

  getBorrowRequestsHistoryForCurrentUser() {
    this.loanData.GetBorrowRequestsHistoryForCurrentUser().subscribe((loansList: LoansLookup[]) => {
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

}
