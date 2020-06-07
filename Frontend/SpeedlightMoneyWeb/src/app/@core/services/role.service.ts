import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, throwError } from 'rxjs';
import { retry, catchError, map } from 'rxjs/operators';
import { SelectItemsList } from '../data/common/selectitem';
import { Result } from '../data/common/result';
import { ErrorService } from 'src/app/shared/error.service';
import { AuthService } from 'src/app/auth/auth.service';
// tslint:disable-next-line: max-line-length
import { CurrencyData, Currency, CurrenciesList, AddCurrencyCommand, UpdateCurrencyCommand, RestoreCurrencyCommand } from '../data/currency';
import { RoleData, Role, RolesList, AddRoleCommand, UpdateRoleCommand, RestoreRoleCommand } from '../data/role';

@Injectable()
export class RoleService extends RoleData {
    baseUrl = environment.baseURL + 'Role';

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


    GetRole(id: number): Observable<Role> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<Role>(this.baseUrl + '/' + id, this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetRoles(): Observable<RolesList> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<RolesList>(this.baseUrl, this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    GetRolesDropdown(): Observable<SelectItemsList> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
        })};
        return this.http.get<SelectItemsList>(this.baseUrl + '/rolesdropdown', this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    AddRole(addRoleCommand: AddRoleCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.post<Result>(this.baseUrl, JSON.stringify(addRoleCommand), this.httpOptions)
            .pipe(
                map((response: any) => response),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }
    UpdateRole(updateRoleCommand: UpdateRoleCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl, JSON.stringify(updateRoleCommand), this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
    DeleteRole(id: number): Observable<Result> {
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
    RestoreRole(restoreRoleCommand: RestoreRoleCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl + '/restore', JSON.stringify(restoreRoleCommand), this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }
}
