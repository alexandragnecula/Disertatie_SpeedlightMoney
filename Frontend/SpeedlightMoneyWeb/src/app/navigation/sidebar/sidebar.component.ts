import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../../auth/auth.service';

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

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.authSubscription = this.authService.authChange.subscribe(authStatus => {
      this.isAuth = authStatus;
    })
    this.adminSubscription = this.authService.isAdmin.subscribe(isAdmin => {
      this.isAdmin = isAdmin;
    })
    this.ultimateSubscription = this.authService.isUltimate.subscribe(isUltimate => {
      this.isUltimate = isUltimate;
    })
    this.premiumSubscription = this.authService.isPremium.subscribe(isPremium => {
      this.isPremium = isPremium;
    })
    this.explorerSubscription = this.authService.isExplorer.subscribe(isExplorer => {
      this.isExplorer = isExplorer;
    })
    this.currentUserIdSubscription = this.authService.currentUserId.subscribe(userId => {
      this.currentUserId = userId;
    });
  }

  ngOnDestroy(): void {
    this.authSubscription.unsubscribe();
    this.adminSubscription.unsubscribe();
    this.ultimateSubscription.unsubscribe();
    this.premiumSubscription.unsubscribe();
    this.explorerSubscription.unsubscribe();
    this.currentUserIdSubscription.unsubscribe();
  }

  onClose() {
    this.closeSidenav.emit();
  }

  onLogout() {
    this.onClose();
    this.authService.logout();
  }
}
