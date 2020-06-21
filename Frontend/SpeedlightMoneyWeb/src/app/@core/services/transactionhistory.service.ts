import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, throwError } from 'rxjs';
import { retry, catchError, map } from 'rxjs/operators';
import { SelectItemsList } from '../data/common/selectitem';
import { Result } from '../data/common/result';
import { ErrorService } from 'src/app/shared/error.service';
import { AuthService } from 'src/app/auth/auth.service';
import { TransactionHistoryData, TransactionHistory, TransactionsHistoryList,
    AddTransactionHistoryCommand, UpdateTransactionHistoryCommand, RestoreTransactionHistoryCommand, TransactionsHistoryLookup } from '../data/transactionhistory';

@Injectable()
export class TransactionHistoryService extends TransactionHistoryData {
    baseUrl = environment.baseURL + 'TransactionHistory';

    // Http Headers
        httpOptions = {
        headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.authService.getToken()}`
        })
    };

    constructor(private http: HttpClient, private errService: ErrorService, private authService: AuthService) {
        super();
    }

    GetTransactionHistory(id: number): Observable<TransactionHistory> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<TransactionHistory>(this.baseUrl + '/' + id, this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetTransactionsHistory(): Observable<TransactionsHistoryList> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<TransactionsHistoryList>(this.baseUrl, this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    AddTransactionHistory(addTransactionHistoryCommand: AddTransactionHistoryCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.post<Result>(this.baseUrl
                        + 'addwtransactionhistory', JSON.stringify(addTransactionHistoryCommand), this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    UpdateTransactionHistory(updateTransactionHistoryCommand: UpdateTransactionHistoryCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl, JSON.stringify(updateTransactionHistoryCommand), this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
    DeleteTransactionHistory(id: number): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.delete<Result>(this.baseUrl + '/' + id, this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
    RestoreTransactionHistory(restoreTransactionHistoryCommand: RestoreTransactionHistoryCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl + '/restore', JSON.stringify(restoreTransactionHistoryCommand), this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
    GetTransactionsHistoryForCurrentUser(): Observable<TransactionsHistoryLookup[]> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<TransactionsHistoryList>(this.baseUrl + '/usertransactionshistory', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
}
