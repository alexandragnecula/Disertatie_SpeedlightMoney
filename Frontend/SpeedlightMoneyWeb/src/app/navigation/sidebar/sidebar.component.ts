import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../../auth/auth.service';
import { UserData } from 'src/app/@core/data/userclasses/user';
import { UIService } from 'src/app/shared/ui.service';
import { CurrentUser } from 'src/app/@core/data/userclasses/currentuser';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit, OnDestroy {
  @Output()
  closeSidenav = new EventEmitter<void>();

  isAuth = false;
  isAdmin = false;
  isUltimate = false;
  isPremium = false;
  isExplorer = false;
  currentUserId = -1;
  authSubscription: Subscription;
  adminSubscription: Subscription;
  ultimateSubscription: Subscription;
  premiumSubscription: Subscription;
  explorerSubscription: Subscription;
  currentUserIdSubscription: Subscription;
  currentUserSubscription: Subscription;
  currentUserName: string;

  constructor(private authService: AuthService,
              private userData: UserData,
              private uiService: UIService) { }

  ngOnInit(): void {
    this.authSubscription = this.authService.authChange.subscribe(authStatus => {
      this.isAuth = authStatus;
    });
    this.adminSubscription = this.authService.isAdmin.subscribe(isAdmin => {
      this.isAdmin = isAdmin;
    });
    this.ultimateSubscription = this.authService.isUltimate.subscribe(isUltimate => {
      this.isUltimate = isUltimate;
    });
    this.premiumSubscription = this.authService.isPremium.subscribe(isPremium => {
      this.isPremium = isPremium;
    });
    this.explorerSubscription = this.authService.isExplorer.subscribe(isExplorer => {
      this.isExplorer = isExplorer;
    });
    this.currentUserIdSubscription = this.authService.currentUserId.subscribe(userId => {
      this.currentUserId = userId;
    });
    this.currentUserSubscription = this.authService.currentUser.subscribe(currentUser => {
      if (currentUser !== null) {
        this.currentUserName = currentUser.email;
      }
    });
  }

  ngOnDestroy(): void {
    this.authSubscription.unsubscribe();
    this.adminSubscription.unsubscribe();
    this.ultimateSubscription.unsubscribe();
    this.premiumSubscription.unsubscribe();
    this.explorerSubscription.unsubscribe();
    this.currentUserIdSubscription.unsubscribe();
    this.currentUserSubscription.unsubscribe();
  }

  onClose() {
    this.closeSidenav.emit();
  }

  onLogout() {
    this.onClose();
    this.authService.logout();
  }
}
