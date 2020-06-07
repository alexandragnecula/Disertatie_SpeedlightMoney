import { of as observableOf,  Observable, throwError, pipe } from 'rxjs';
import { Injectable } from '@angular/core';
import { CurrentUser } from '../data/userclasses/currentuser';
import { AuthSuccessResponse } from '../data/common/authresponse';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { retry, catchError, map } from 'rxjs/operators';
import { AuthService } from '../../auth/auth.service';
import { ErrorService } from 'src/app/shared/error.service';
import { User, UserData, AddUserCommand, LoginUser, UserList, UpdateUserCommand } from '../data/userclasses/user';
import { Result } from '../data/common/result';
import { SelectItemsList } from '../data/common/selectitem';

@Injectable()
export class UserService extends UserData {
    baseUrl = environment.baseURL + 'User';

    // Http Headers
    httpOptions = {
        headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${this.authService.getToken()}`
        })
    };

    constructor(private http: HttpClient, private authService: AuthService, private errService: ErrorService) {
        super();
    }

    AddUser(addUserCommand: AddUserCommand): Observable<AuthSuccessResponse> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.post<AuthSuccessResponse>(this.baseUrl + '/adduser', JSON.stringify(addUserCommand), this.httpOptions)
            .pipe(
                map(res => {
                    return res;
                }),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }

    AddUserWithWallets(addUserCommand: AddUserCommand): Observable<AuthSuccessResponse> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.post<AuthSuccessResponse>(this.baseUrl + '/adduserandwallets', JSON.stringify(addUserCommand), this.httpOptions)
            .pipe(
                map(res => {
                    return res;
                }),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }

    LoginUser(loginUserCommand: LoginUser): Observable<AuthSuccessResponse> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.post<AuthSuccessResponse>(this.baseUrl + '/login', JSON.stringify(loginUserCommand), this.httpOptions)
            .pipe(
                map(res => {
                    return res;
                }),
                retry(1),
                catchError(this.errService.errorHandl)
            );
    }

    getUsers(): Observable<UserList> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<UserList>(this.baseUrl, this.httpOptions)
            .pipe(
                map((response: any) => response),
                catchError(this.errService.errorHandl)
            );
    }
    getUser(id: number): Observable<User> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<User>(this.baseUrl + '/' + id, this.httpOptions)
            .pipe(
                map((response: any) => response),
                catchError(this.errService.errorHandl)
            );
    }
    updateUser(userProfile: UpdateUserCommand): Observable<Result> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.put<Result>(this.baseUrl, JSON.stringify(userProfile), this.httpOptions)
            .pipe(
                map((response: any) => response),
                catchError(this.errService.errorHandl)
            );
    }
    getUsersDropdown(): Observable<SelectItemsList> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<SelectItemsList>(this.baseUrl + '/usersdropdown', this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }

    getCurrentStatusesDropdown(): Observable<SelectItemsList> {
        this.httpOptions = {
            headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.authService.getToken()}`
        })};
        return this.http.get<SelectItemsList>(this.baseUrl + '/currentstatusesdropdown', this.httpOptions)
        .pipe(
            map((response: any) => response),
            retry(1),
            catchError(this.errService.errorHandl)
        );
    }

}
