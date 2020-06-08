import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { LoansLookup, LoanData, ManageLoanCommand } from 'src/app/@core/data/loan';
import { SelectItemsList } from 'src/app/@core/data/common/selectitem';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { LoanStatusData } from 'src/app/@core/data/loanstatus';
import { UIService } from 'src/app/shared/ui.service';
import { Result } from 'src/app/@core/data/common/result';

@Component({
  selector: 'app-loanrequestshistory',
  templateUrl: './loanrequestshistory.component.html',
  styleUrls: ['./loanrequestshistory.component.scss']
})
export class LoanrequestshistoryComponent implements OnInit, AfterViewInit {
  isLoading = false;
  displayedColumns = ['borrowerName', 'amount', 'currencyName', 'termName', 'borrowDate', 'returnDate', 'dueDate', 'loanStatusName'];
  dataSource = new MatTableDataSource<LoansLookup>();
  loanstatusesSelectList: SelectItemsList = new SelectItemsList();
  isManaged =  false;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private loanData: LoanData,
              private loanStatusData: LoanStatusData,
              private uiService: UIService) { }

  ngOnInit(): void {
    this.getLoanRequestsHistoryForCurrentUser();
  }

  getLoanRequestsHistoryForCurrentUser() {
    this.loanData.GetLendRequestsHistoryForCurrentUser().subscribe((loansList: LoansLookup[]) => {
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
