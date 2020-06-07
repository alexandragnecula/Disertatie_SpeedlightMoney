import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { Subject, of as observableOf, Observable, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { CurrentUser } from '../@core/data/userclasses/currentuser';

@Injectable()
export class AuthService {

  authChange = new BehaviorSubject<boolean>(false);
  isAdmin = new BehaviorSubject<boolean>(false);
  isUltimate = new BehaviorSubject<boolean>(false);
  isPremium = new BehaviorSubject<boolean>(false);
  isExplorer = new BehaviorSubject<boolean>(false);
  currentUserId = new BehaviorSubject<number>(-1);
  currentUser = new BehaviorSubject<CurrentUser>(null);

  private isUserAuthenticated = false;

  constructor(private router: Router,
              public jwtHelper: JwtHelperService) {}
  // ...

  initAuthListener() {
    this.isAuthenticated().subscribe(user => {
        if (user) {
            this.isUserAuthenticated = true;
            this.authChange.next(true);
            const token = this.getDecodedToken();
            const roles = [];
            if (Array.isArray(token.role)) {
              roles.push(...token.role);
            } else {
              roles.push(token.role);
            }

            if (roles.includes('Admin')) {
              this.isAdmin.next(true);
            }
            if (roles.includes('Ultimate')) {
              this.isUltimate.next(true);
            }
            if (roles.includes('Premium')) {
              this.isPremium.next(true);
            }
            if (roles.includes('Explorer')) {
              this.isExplorer.next(true);
            }
            this.currentUserId.next(+token.id);
            this.currentUser.next(this.GetCurrentUser());
        } else {
            this.logout();
        }
    });
}

  public isAuthenticated(): Observable<boolean> {
    const token = this.getToken();
    // Check whether the token is expired and return
    // true or false
    if (token == null) {
        return observableOf(false);
    } else {
        return observableOf(!this.jwtHelper.isTokenExpired(token));
    }
  }

  public isAuth(): boolean {
    return this.isUserAuthenticated;
  }

  public setToken(token: string) {
    localStorage.setItem(environment.authToken, token);
  }

  public getToken(): string {
    return localStorage.getItem(environment.authToken);
  }

  public getDecodedToken(): any {
    return this.jwtHelper.decodeToken(this.getToken());
  }

  public GetCurrentUser(): CurrentUser {
    const token = this.getDecodedToken();
    const user = new CurrentUser();
    user.email = token.email;
    user.firstName = token.firstName;
    user.lastName = token.lastName;
    user.id = token.id;
    return user;
}

  public logout(): void {
    localStorage.removeItem(environment.authToken);
    this.isUserAuthenticated = false;
    this.authChange.next(false);
    this.isAdmin.next(false);
    this.isUltimate.next(false);
    this.isPremium.next(false);
    this.isExplorer.next(false);
    this.currentUserId.next(-1);
    this.currentUser.next(null);
    this.router.navigate(['/login']);
  }
}
