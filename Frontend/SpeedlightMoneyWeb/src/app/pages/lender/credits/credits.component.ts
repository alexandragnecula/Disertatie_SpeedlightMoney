import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { DebtsLookup, DebtData } from 'src/app/@core/data/debt';
import { UIService } from 'src/app/shared/ui.service';
import { AuthService } from 'src/app/auth/auth.service';
import { Result } from 'src/app/@core/data/common/result';

@Component({
  selector: 'app-credits',
  templateUrl: './credits.component.html',
  styleUrls: ['./credits.component.scss']
})
export class CreditsComponent implements OnInit, AfterViewInit {
  isLoading = false;
  displayedColumns = ['borrowerName', 'loanAmount', 'currencyName', 'termName', 'borrowDate', 'returnDate', 'dueDate', 'loanStatusName', 'debtStatusName', 'sendReminder'];
  dataSource = new MatTableDataSource<DebtsLookup>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private debtData: DebtData,
              private uiService: UIService,
              private authService: AuthService) { }

  ngOnInit(): void {
    this.getCreditsForCurrentUser();
  }

  getCreditsForCurrentUser() {
    this.debtData.GetCreditsForCurrentUser().subscribe((debtsList: DebtsLookup[]) => {
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

  sendReminderEmailToBorrower(id){
    this.isLoading = true;

    this.debtData.SendReminderToBorrower(id).subscribe((res: Result) => {
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }
}
