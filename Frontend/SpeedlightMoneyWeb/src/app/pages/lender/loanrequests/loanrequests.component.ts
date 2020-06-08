import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { LoanData, LoansLookup, ManageLoanCommand } from 'src/app/@core/data/loan';
import { UIService } from 'src/app/shared/ui.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { Result } from 'src/app/@core/data/common/result';
import { SelectItemsList } from 'src/app/@core/data/common/selectitem';
import { LoanStatusData } from 'src/app/@core/data/loanstatus';

@Component({
  selector: 'app-loanrequests',
  templateUrl: './loanrequests.component.html',
  styleUrls: ['./loanrequests.component.scss']
})
export class LoanrequestsComponent implements OnInit, AfterViewInit{
  isLoading = false;
  displayedColumns = ['borrowerName', 'amount', 'currencyName', 'termName', 'borrowDate', 'returnDate', 'dueDate', 'loanStatusName', 'manageRequest'];
  dataSource = new MatTableDataSource<LoansLookup>();
  loanstatusesSelectList: SelectItemsList = new SelectItemsList();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private loanData: LoanData,
              private loanStatusData: LoanStatusData,
              private uiService: UIService) { }

  ngOnInit(): void {
    this.getLoanRequestsForCurrentUser();
    this.getLoanStatusSelect();
  }

  getLoanRequestsForCurrentUser() {
    this.loanData.GetLendRequestsForCurrentUser().subscribe((loansList: LoansLookup[]) => {
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

  manageLoanRequest(loanStatusId: string, element: any){
    this.isLoading = true;
    const updateLoanCommand: ManageLoanCommand = {
      loanStatusId: +loanStatusId,
      id: +element.id,
      borrowerId: +element.borrowerId,
      currencyId: +element.currencyId,
      amount: +element.amount
    } as ManageLoanCommand;

    this.loanData.ApproveLoan(updateLoanCommand).subscribe((res: Result) => {
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  getLoanStatusSelect() {
    this.loanStatusData.GetLoanStatusesDropdown().subscribe((countries: SelectItemsList) => {
        this.loanstatusesSelectList = countries;
    },
        error => {
            this.uiService.showErrorSnackbar(error, null, 3000);
        });
  }
}
