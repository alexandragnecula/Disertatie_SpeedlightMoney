import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { UserData, AddUserCommand, User, UserProfile, UpdateUserCommand } from 'src/app/@core/data/userclasses/user';
import { UIService } from 'src/app/shared/ui.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CurrencyData } from 'src/app/@core/data/currency';
import { RoleData } from 'src/app/@core/data/role';
import { AuthService } from 'src/app/auth/auth.service';
import { SelectItemsList } from 'src/app/@core/data/common/selectitem';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  isLoading = false;
  userProfileForm: FormGroup;
  currentUserId: number;

  constructor(private userData: UserData,
              private uiService: UIService,
              private currencyData: CurrencyData,
              private roleData: RoleData,
              private router: Router,
              private authService: AuthService,
              private route: ActivatedRoute) { }

  currencySelectList: SelectItemsList = new SelectItemsList();
  roleSelectList: SelectItemsList = new SelectItemsList();
  currentStatusSelectList: SelectItemsList = new SelectItemsList();

  ngOnInit(): void {
    if (Number(this.route.snapshot.params.id)) {
      this.currentUserId = +this.route.snapshot.params.id;
    }
    this.initForm();
    this.getCurrenciesSelect();
    this.getCurrentStatusesSelect();
    this.getRolesSelect();
  }

  initForm() {
    this.userProfileForm = new FormGroup({
        email: new FormControl({value: '', disabled: true}),
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
        roleId: new FormControl('', [Validators.required])
    });
    this.getUserProfile();
  }

  getUserProfile() {
    this.isLoading = true;
    this.userData.getUserProfile(this.currentUserId).subscribe((user: UserProfile) => {
        this.userProfileForm.setValue({
            email: user.email,
            firstName: user.firstName,
            lastName: user.lastName,
            birthdate: user.birthdate,
            cnp: user.cnp,
            country: user.country,
            county: user.county,
            city: user.city,
            streetName: user.streetName,
            streetNumber: user.streetNumber,
            currentStatus: user.currentStatus,
            cardNumber: user.cardNumber,
            cvv: user.cvv,
            expireDate: user.expireDate,
            salary: user.salary,
            phoneNumber: user.phoneNumber,
            roleId: user.roleId.toString()
        });
        // this.getCurrenciesSelect();
        this.getCurrentStatusesSelect();
        this.getRolesSelect();
        this.isLoading = false;
    },
        error => {
            this.uiService.showErrorSnackbar(error, null, 3000);
            this.isLoading = false;
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

onSubmit() {
  this.isLoading = true;
  const updateUserCommand: UpdateUserCommand = {
      id: this.currentUserId,
      firstName: this.userProfileForm.value.firstName,
      lastName: this.userProfileForm.value.lastName,
      birthdate: this.userProfileForm.value.birthdate,
      cnp: this.userProfileForm.value.cnp,
      country: this.userProfileForm.value.country,
      county: this.userProfileForm.value.county,
      city: this.userProfileForm.value.city,
      streetName: this.userProfileForm.value.streetName,
      streetNumber: this.userProfileForm.value.streetNumber,
      currentStatus: this.userProfileForm.value.currentStatus,
      cardNumber: this.userProfileForm.value.cardNumber,
      cvv: this.userProfileForm.value.cvv,
      expireDate: this.userProfileForm.value.expireDate,
      salary: +this.userProfileForm.value.salary,
      phoneNumber: this.userProfileForm.value.phoneNumber,
      roleId: +this.userProfileForm.value.roleId
  } as UpdateUserCommand;
  this.userData.updateUser(updateUserCommand).subscribe(res => {
      this.isLoading = false;
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
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
}
