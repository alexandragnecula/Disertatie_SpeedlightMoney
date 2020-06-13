import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgForm, FormGroup, FormControl, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { UIService } from 'src/app/shared/ui.service';
import { UserService } from 'src/app/@core/services/user.service';
import { AddUserCommand, UserData } from 'src/app/@core/data/userclasses/user';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { SelectItemsList} from 'src/app/@core/data/common/selectitem';
import { CurrencyData } from 'src/app/@core/data/currency';
import { RoleData } from 'src/app/@core/data/role';
import { Subscription } from 'rxjs';
import { IfStmt } from '@angular/compiler';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {
  isLoading = false;
  authChange = true;
  registerForm: FormGroup;
  currencySelectList: SelectItemsList = new SelectItemsList();
  roleSelectList: SelectItemsList = new SelectItemsList();
  currentStatusSelectList: SelectItemsList = new SelectItemsList();
  isExplorer = false;

  authChangeSubscription: Subscription;

  constructor(private userData: UserData,
              private currencyData: CurrencyData,
              private roleData: RoleData,
              private uiService: UIService,
              private router: Router,
              private authService: AuthService) { }

  ngOnInit(): void {
    this.authChangeSubscription = this.authService.authChange.subscribe((authChange: boolean) => {
      if (authChange === true) {
          this.router.navigate(['']);
      } else {
        this.authChange = false;
        this.initForm();
        this. getCurrenciesSelect();
        this.getRolesSelect();
        this.getCurrentStatusesSelect();
      }
    });
  }

  initForm() {
    this.registerForm = new FormGroup({
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', [Validators.required, Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$')]),
        firstName: new FormControl('', [Validators.required]),
        lastName: new FormControl('', [Validators.required]),
        birthdate: new FormControl(new Date(), [Validators.required]),
        cnp: new FormControl('', [Validators.required, this.CnpValidator()]),
        country: new FormControl('', [Validators.required]),
        county: new FormControl('', [Validators.required]),
        city: new FormControl('', [Validators.required]),
        streetName: new FormControl('', [Validators.required]),
        streetNumber: new FormControl('', [Validators.required]),
        currentStatus: new FormControl('', [Validators.required]),
        cardNumber: new FormControl('', [Validators.required]),
        cvv: new FormControl('', [Validators.required]),
        expireDate: new FormControl(new Date(), [Validators.required]),
        salary: new FormControl('', [Validators.required]),
        phoneNumber: new FormControl('', [Validators.required, Validators.pattern('^(07[0-8]{1}[0-9]{1}|02[0-9]{2}|03[0-9]{2}){1}?([0-9]{3}){2}$')]),
        totalAmount: new FormControl('', [Validators.required, this.amountValidator()]),
        currencyId: new FormControl('', [Validators.required]),
        roleId: new FormControl('', [Validators.required])
    });
  }

  CnpValidator(): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any} | null => {
      const invalid = this.validateCNP(control.value);
      return !invalid ? {invalidError: {value: control.value}} : null;
    };
  }

  private validateCNP( pCnp: string ) {
    let i = 0;
    let year = 0;
    let hashResult = 0;
    const cnp = [];
    const hashTable = [2, 7, 9, 1, 4, 6, 3, 5, 8, 2, 7, 9];
    if ( pCnp.length !== 13 ) { return false; }
    for ( i = 0 ; i < 13 ; i++ ) {
        cnp[i] = parseInt( pCnp.charAt(i) , 10 );
        if ( isNaN( cnp[i] ) ) { return false; }
        if ( i < 12 ) { hashResult = hashResult + ( cnp[i] * hashTable[i] ); }
    }
    hashResult = hashResult % 11;
    if ( hashResult === 10 ) { hashResult = 1; }
    year = (cnp[1] * 10) + cnp[2];
    switch ( cnp[0] ) {
        case 1  : case 2 : { year += 1900; } break;
        case 3  : case 4 : { year += 1800; } break;
        case 5  : case 6 : { year += 2000; } break;
        case 7  : case 8 : case 9 : { year += 2000;
                                      if ( year > ( new Date().getFullYear() - 14 ) ) { year -= 100; } }
                                    break;
        default : { return false; }
    }
    if ( year < 1800 || year > 2099 ) { return false; }
    return ( cnp[12] === hashResult );
  }

  amountValidator(): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any} | null => {
      const invalid = this.validateAmount(control.value);
      return !invalid ? {invalidError: {value: control.value}} : null;
    };
  }

  private validateAmount( pAmount: number ) {
        if (this.currencySelectList.selectItems) {
          const item = this.currencySelectList.selectItems.find(x => x.value === this.registerForm.value.currencyId);
          if (item) {
          if (item.label === 'RON'){
            if (pAmount < 30){
              return false;
            }
          }

          if (item.label === 'EUR'){
            if (pAmount < 5){
              return false;
            }
          }
        }
      }
        return pAmount;
  }

  disableWallet(){
    if (this.roleSelectList.selectItems) {
      const item = this.roleSelectList.selectItems.find(x => x.value === this.registerForm.value.roleId);
      console.log(item);
      if (item.label === 'EXPLORER'){
        this.registerForm.patchValue({
          currencyId: '1',
          totalAmount: 0.0
        });
        this.registerForm.get('currencyId').disable();
        this.registerForm.get('totalAmount').disable();
        this.isExplorer = true;
        console.log(this.isExplorer);
      } else {
        this.isExplorer = false;
        this.registerForm.get('currencyId').enable();
        this.registerForm.get('totalAmount').enable();
        console.log(this.isExplorer);
      }
    }
  }

  onSubmit() {
    this.isLoading = true;
    const addUserCommand: AddUserCommand = {
        email: this.registerForm.value.email,
        password: this.registerForm.value.password,
        firstName: this.registerForm.value.firstName,
        lastName: this.registerForm.value.lastName,
        birthdate: this.registerForm.value.birthdate,
        cnp: this.registerForm.value.cnp,
        country: this.registerForm.value.country,
        county: this.registerForm.value.county,
        city: this.registerForm.value.city,
        streetName: this.registerForm.value.streetName,
        streetNumber: this.registerForm.value.streetNumber,
        currentStatus: this.registerForm.value.currentStatus,
        cardNumber: this.registerForm.value.cardNumber,
        cvv: this.registerForm.value.cvv,
        expireDate: this.registerForm.value.expireDate,
        salary: this.registerForm.value.salary,
        phoneNumber: this.registerForm.value.phoneNumber,
        totalAmount: this.registerForm.value.totalAmount,
        currencyId: +this.registerForm.value.currencyId,
        roleId: +this.registerForm.value.roleId
    } as AddUserCommand;
    if (this.isExplorer) {
      addUserCommand.totalAmount = this.registerForm.getRawValue().totalAmount;
      addUserCommand.currencyId = +this.registerForm.getRawValue().currencyId;
    }
    this.userData.AddUserWithWallets(addUserCommand).subscribe(res => {
        this.isLoading = false;
        this.authService.setToken(res.token);
        this.uiService.showSuccessSnackbar('You successfully registered! Welcome!', null, 3000);
        this.authService.initAuthListener();
        this.router.navigate(['']);
    }, error => {
        this.isLoading = false;
        this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  getCurrenciesSelect() {
    this.currencyData.GetCurrenciesDropdown().subscribe((currencies: SelectItemsList) => {
      this.currencySelectList = currencies;
    },
    error => {
        this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  getRolesSelect() {
    this.roleData.GetRolesDropdown().subscribe((roles: SelectItemsList) => {
      this.roleSelectList = roles;
    },
    error => {
        this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  getCurrentStatusesSelect() {
    this.userData.getCurrentStatusesDropdown().subscribe((currentStatuses: SelectItemsList) => {
      this.currentStatusSelectList = currentStatuses;
    },
    error => {
        this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  ngOnDestroy(): void {
    this.authChangeSubscription.unsubscribe();
  }
}
