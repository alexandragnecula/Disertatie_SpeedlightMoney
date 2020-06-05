import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { throwIfAlreadyLoaded } from './module-import-guard';
import { UserData } from './data/userclasses/user';
import { UserService } from './services/user.service';
import { CurrencyData } from './data/currency';
import { CurrencyService } from './services/currency.service';
import { RoleData } from './data/role';
import { RoleService } from './services/role.service';

const DATA_SERVICES = [
  { provide: UserData, useClass: UserService },
  {provide: CurrencyData, useClass: CurrencyService},
  {provide: RoleData, useClass: RoleService}
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
