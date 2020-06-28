import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DebtsLookup, DebtData } from 'src/app/@core/data/debt';
import { Subscription } from 'rxjs';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { UIService } from 'src/app/shared/ui.service';
import { Result } from 'src/app/@core/data/common/result';

@Component({
  selector: 'app-debtshistory',
  templateUrl: './debtshistory.component.html',
  styleUrls: ['./debtshistory.component.scss']
})
export class DebtshistoryComponent implements OnInit, AfterViewInit {
  isLoading = false;
  displayedColumns = ['lenderName', 'loanAmount', 'currencyName', 'termName', 'borrowDate', 'returnDate', 'dueDate', 'debtStatusName'];
  dataSource = new MatTableDataSource<DebtsLookup>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private debtData: DebtData,
              private uiService: UIService) { }

  ngOnInit(): void {
    this.getDebtsHistoryForCurrentUser();
  }

  getDebtsHistoryForCurrentUser() {
    this.isLoading = true;
    this.debtData.GetDebtsHistoryForCurrentUser().subscribe((debtsList: DebtsLookup[]) => {
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
