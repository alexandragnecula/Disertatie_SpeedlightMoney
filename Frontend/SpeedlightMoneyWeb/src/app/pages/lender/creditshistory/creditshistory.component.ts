import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DebtsLookup, DebtData } from 'src/app/@core/data/debt';
import { Subscription } from 'rxjs';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { UIService } from 'src/app/shared/ui.service';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-creditshistory',
  templateUrl: './creditshistory.component.html',
  styleUrls: ['./creditshistory.component.scss']
})
export class CreditshistoryComponent implements OnInit, AfterViewInit {

  isLoading = false;
  displayedColumns = ['borrowerName', 'loanAmount', 'currencyName', 'termName', 'borrowDate', 'returnDate', 'dueDate', 'loanStatusName', 'debtStatusName'];
  dataSource = new MatTableDataSource<DebtsLookup>();
  currentUserIdSubscription: Subscription;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private debtData: DebtData,
              private uiService: UIService,
              private authService: AuthService) { }

  ngOnInit(): void {
    this.getCreditsHistoryForCurrentUser();
  }

  getCreditsHistoryForCurrentUser() {
    this.debtData.GetCreditsHistoryForCurrentUser().subscribe((debtsList: DebtsLookup[]) => {
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

}
