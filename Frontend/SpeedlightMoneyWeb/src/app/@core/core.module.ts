import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { throwIfAlreadyLoaded } from './module-import-guard';
import { UserData } from './data/userclasses/user';
import { UserService } from './services/user.service';
import { CurrencyData } from './data/currency';
import { CurrencyService } from './services/currency.service';
import { RoleData } from './data/role';
import { RoleService } from './services/role.service';
import { WalletData } from './data/wallet';
import { WalletService } from './services/wallet.service';
import { DebtData } from './data/debt';
import { DebtService } from './services/debt.service';
import { DebtStatusData } from './data/debtstatus';
import { DebtStatusService } from './services/debtstatus.service';
import { FriendData } from './data/friend';
import { FriendService } from './services/friend.service';
import { LoanData } from './data/loan';
import { LoanService } from './services/loan.service';
import { LoanStatusData } from './data/loanstatus';
import { LoanStatusService } from './services/loanstatus.service';
import { TermData } from './data/term';
import { TermService } from './services/term.service';
import { TransactionHistoryData } from './data/transactionhistory';
import { TransactionHistoryService } from './services/transactionhistory.service';

const DATA_SERVICES = [
  {provide: CurrencyData, useClass: CurrencyService},
  {provide: DebtData, useClass: DebtService},
  {provide: DebtStatusData, useClass: DebtStatusService},
  {provide: FriendData, useClass: FriendService},
  {provide: LoanData, useClass: LoanService},
  {provide: LoanStatusData, useClass: LoanStatusService},
  {provide: RoleData, useClass: RoleService},
  {provide: TermData, useClass: TermService},
  { provide: UserData, useClass: UserService },
  {provide: WalletData, useClass: WalletService},
  {provide: TransactionHistoryData, useClass: TransactionHistoryService}
];

export const NB_CORE_PROVIDERS = [
  ...DATA_SERVICES,
];

@NgModule({
  imports: [
    CommonModule,
  ],
  exports: [
  ],
  declarations: [],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }

  static forRoot(): ModuleWithProviders {
    return  {
      ngModule: CoreModule,
      providers: [
        ...NB_CORE_PROVIDERS,
      ],
    } as ModuleWithProviders;
  }
}
