import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, throwError } from 'rxjs';
import { retry, catchError, map } from 'rxjs/operators';
import { SelectItemsList } from '../data/common/selectitem';
import { Result } from '../data/common/result';
import { ErrorService } from 'src/app/shared/error.service';
import { AuthService } from 'src/app/auth/auth.service';
import { DebtData, Debt, DebtsList, AddDebtCommand, UpdateDebtCommand, RestoreDebtCommand, DebtsLookup} from '../data/debt';

@Injectable()
export class DebtService extends DebtData {
    baseUrl = environment.baseURL + 'Debt';

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

    GetDebt(id: number): Observable<Debt> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<Debt>(this.baseUrl + '/' + id, this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetDebts(): Observable<DebtsList> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<DebtsList>(this.baseUrl, this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetDebtsForCurrentUser(): Observable<DebtsLookup[]> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<DebtsList>(this.baseUrl + '/userdebts', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetCreditsForCurrentUser(): Observable<DebtsLookup[]> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<DebtsList>(this.baseUrl + '/usercredits', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetDebtsHistoryForCurrentUser(): Observable<DebtsLookup[]> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<DebtsList>(this.baseUrl + '/userdebtshistory', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetCreditsHistoryForCurrentUser(): Observable<DebtsLookup[]> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<DebtsList>(this.baseUrl + '/usercreditshistory', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetDebtsDropdown(): Observable<SelectItemsList> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<SelectItemsList>(this.baseUrl + '/debtsdropdown', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    AddDebt(addDebtCommand: AddDebtCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.post<Result>(this.baseUrl + '/adddebt', JSON.stringify(addDebtCommand), this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    UpdateDebt(updateDebtCommand: UpdateDebtCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl, JSON.stringify(updateDebtCommand), this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
    DeleteDebt(id: number): Observable<Result> {
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
    RestoreDebt(restoreDebtCommand: RestoreDebtCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl + '/restore', JSON.stringify(restoreDebtCommand), this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
    PayDebt(updateDebtCommand: UpdateDebtCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl + '/paydebt', JSON.stringify(updateDebtCommand), this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
    DeferPayment(updateDebtCommand: UpdateDebtCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl + '/deferpayment', JSON.stringify(updateDebtCommand), this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
}
