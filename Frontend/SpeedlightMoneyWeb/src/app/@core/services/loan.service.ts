import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, throwError } from 'rxjs';
import { retry, catchError, map } from 'rxjs/operators';
import { SelectItemsList } from '../data/common/selectitem';
import { Result } from '../data/common/result';
import { ErrorService } from 'src/app/shared/error.service';
import { AuthService } from 'src/app/auth/auth.service';
import { LoanData, Loan, LoansList, AddLoanCommand, UpdateLoanCommand, RestoreLoanCommand, LoansLookup, ManageLoanCommand } from '../data/loan';

@Injectable()
export class LoanService extends LoanData {
    baseUrl = environment.baseURL + 'Loan';

    // Http Headers
        httpOptions = {
        headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.authService.getToken()}`
        })
    };

    constructor(private http: HttpClient, private errService: ErrorService,
                private authService: AuthService) {
        super();
    }

    GetLoan(id: number): Observable<Loan> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<Loan>(this.baseUrl + '/' + id, this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetLoans(): Observable< LoansList> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<LoansList>(this.baseUrl, this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetLoansDropdown(): Observable<SelectItemsList> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<SelectItemsList>(this.baseUrl + '/loansdropdown', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    AddLoan(addLoanCommand: AddLoanCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.post<Result>(this.baseUrl + '/addloan', JSON.stringify(addLoanCommand), this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    UpdateLoan(updateLoanCommand: UpdateLoanCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl, JSON.stringify(updateLoanCommand), this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
    DeleteLoan(id: number): Observable<Result> {
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
    RestoreLoan(restoreLoanCommand: RestoreLoanCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl + '/restore', JSON.stringify(restoreLoanCommand), this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
    RequestLoan(addLoanCommand: AddLoanCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.post<Result>(this.baseUrl + '/requestloan', JSON.stringify(addLoanCommand), this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    ManageLoan(updateLoanCommand: ManageLoanCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl + '/manageloan', JSON.stringify(updateLoanCommand), this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetBorrowRequestsForCurrentUser(): Observable<LoansLookup[]> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<LoansList>(this.baseUrl + '/borrowrequests', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetLendRequestsForCurrentUser(): Observable<LoansLookup[]> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<LoansList>(this.baseUrl + '/lendrequests', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }

    GetBorrowRequestsHistoryForCurrentUser(): Observable<LoansLookup[]> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<LoansList>(this.baseUrl + '/borrowrequestshistory', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetLendRequestsHistoryForCurrentUser(): Observable<LoansLookup[]> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<LoansList>(this.baseUrl + '/lendrequestshistory', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    CancelLoanRequest(updateLoanCommand: ManageLoanCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl + '/cancelloanrequest', JSON.stringify(updateLoanCommand), this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
}
