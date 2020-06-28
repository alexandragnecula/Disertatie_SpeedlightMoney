import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DebtsLookup, DebtData } from 'src/app/@core/data/debt';
import { Subscription } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UIService } from 'src/app/shared/ui.service';
import { Result } from 'src/app/@core/data/common/result';
import { DebtshistoryComponent } from '../debtshistory/debtshistory.component';

@Component({
  selector: 'app-debts',
  templateUrl: './debts.component.html',
  styleUrls: ['./debts.component.scss']
})
export class DebtsComponent implements OnInit, AfterViewInit {
  isLoading = false;
  displayedColumns = ['lenderName', 'loanAmount', 'currencyName', 'termName', 'borrowDate', 'returnDate', 'dueDate', 'debtStatusName', 'loanStatusName', 'actions', 'deferpayment'];
  dataSource = new MatTableDataSource<DebtsLookup>();
  currentUserId = -1;
  currentDate = new Date();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(DebtshistoryComponent) debtshistorycomponent: DebtshistoryComponent;

  constructor(private debtData: DebtData,
              private uiService: UIService) { }

  ngOnInit(): void {
    this.getDebtsForCurrentUser();
  }

  getDebtsForCurrentUser() {
    this.isLoading = true;
    this.debtData.GetDebtsForCurrentUser().subscribe((debtsList: DebtsLookup[]) => {
      this.isLoading = false;
      this.dataSource.data = debtsList;
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

  payDebt(debt){
    this.isLoading = true;
    this.debtData.PayDebt(debt).subscribe((res: Result) => {
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
      this.debtshistorycomponent.getDebtsHistoryForCurrentUser();
      this.isLoading = false;
      this.getDebtsForCurrentUser();
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  deferPayment(row){
    this.isLoading = true;
    this.debtData.DeferPayment(row).subscribe((res: Result) => {
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
      this.debtshistorycomponent.getDebtsHistoryForCurrentUser();
      this.getDebtsForCurrentUser();
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  compareDates(dueDate: string) {
    const dateToCheck = new Date(dueDate);
    return this.currentDate.getFullYear() >= dateToCheck.getFullYear() &&
    this.currentDate.getMonth() >= dateToCheck.getMonth() &&
    this.currentDate.getDate() >= dateToCheck.getDate();
  }
}
