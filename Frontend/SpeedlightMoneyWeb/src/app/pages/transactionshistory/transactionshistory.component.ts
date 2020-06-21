import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DebtsLookup, DebtData } from 'src/app/@core/data/debt';
import { Subscription } from 'rxjs';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { UIService } from 'src/app/shared/ui.service';
import { TransactionHistoryData, TransactionsHistoryLookup } from 'src/app/@core/data/transactionhistory';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-transactionshistory',
  templateUrl: './transactionshistory.component.html',
  styleUrls: ['./transactionshistory.component.scss']
})
export class TransactionshistoryComponent implements OnInit, AfterViewInit {

  isLoading = false;
  displayedColumns = ['createdOn', 'senderBeneficiarName', 'amount'];
  dataSource = new MatTableDataSource<TransactionsHistoryLookup>();
  currentUserIdSubscription: Subscription;
  userId: number;
  isSender = false;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private transactionHistoryData: TransactionHistoryData,
              private uiService: UIService,
              private authService: AuthService) { }

  ngOnInit(): void {
    this.currentUserIdSubscription = this.authService.currentUserId.subscribe(userId => {
      this.userId = userId;
      });

    this.getTransactionsHistoryForCurrentUser();
  }

  getTransactionsHistoryForCurrentUser() {
    this.isLoading = true;
    this.transactionHistoryData.GetTransactionsHistoryForCurrentUser().subscribe((transactionsHistoryList: TransactionsHistoryLookup[]) => {
      this.isLoading = false;
      this.dataSource.data = transactionsHistoryList;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  isSentMoney(senderId){
    if (senderId === this.userId){
      this.isSender = true;
      return true;
    }
    return false;
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  doFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

}
