import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DebtsLookup, DebtData } from 'src/app/@core/data/debt';
import { Subscription } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { UIService } from 'src/app/shared/ui.service';

@Component({
  selector: 'app-debts',
  templateUrl: './debts.component.html',
  styleUrls: ['./debts.component.scss']
})
export class DebtsComponent implements OnInit, AfterViewInit {
  isLoading = false;
  displayedColumns = ['lenderName', 'amount', 'borrowDate', 'returnDate', 'dueDate', 'debtStatusName', 'actions'];
  dataSource = new MatTableDataSource<DebtsLookup>();
  currentUserIdSubscription: Subscription;
  currentUserId = -1;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private debtData: DebtData,
              private uiService: UIService) { }

  ngOnInit(): void {
    this.getBorrowRequestsForCurrentUser();
  }

  getBorrowRequestsForCurrentUser() {
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
}
