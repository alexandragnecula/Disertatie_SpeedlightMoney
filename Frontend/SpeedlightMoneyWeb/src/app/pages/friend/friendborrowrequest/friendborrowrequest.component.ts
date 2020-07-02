import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { SelectItemsList } from 'src/app/@core/data/common/selectitem';
import { FormGroup, FormControl, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TermData } from 'src/app/@core/data/term';
import { CurrencyData } from 'src/app/@core/data/currency';
import { UIService } from 'src/app/shared/ui.service';
import { AuthService } from 'src/app/auth/auth.service';
import { LoanData } from 'src/app/@core/data/loan';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-friendborrowrequest',
  templateUrl: './friendborrowrequest.component.html',
  styleUrls: ['./friendborrowrequest.component.scss']
})
export class FriendborrowrequestComponent implements OnInit, OnDestroy  {

  termSelectList: SelectItemsList = new SelectItemsList();
  currencySelectList: SelectItemsList = new SelectItemsList();
  isLoading = false;
  loanRequestForm: FormGroup;
  isExplorerSubscription: Subscription;
  isExplorer: boolean;

  constructor(@Inject(MAT_DIALOG_DATA)  public loanRequest: any,
              private termData: TermData,
              private currencyData: CurrencyData, private uiService: UIService,
              private authService: AuthService,
              private loanData: LoanData,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.isExplorerSubscription = this.authService.isExplorer.subscribe(isExplorer => {
      this.isExplorer = isExplorer;
    });

    this.getTerms();
    this.getCurrencies();
    this.initForm();
  }

  initForm() {
    this.loanRequestForm = new FormGroup({
      lenderName: new FormControl({value: this.loanRequest.lenderName, disabled: true}),
      description: new FormControl(''),
      amount: new FormControl('', [Validators.required, this.amountValidator()]),
      currencyId: new FormControl('', [Validators.required]),
      termId: new FormControl('', [Validators.required]),
      lenderId: new FormControl(this.loanRequest.lenderId)
    });
  }

  getTerms() {
    this.termData.GetTermsDropdown().subscribe((result: SelectItemsList) => {
      this.termSelectList = result;
      this.isLoading = false;
    }, error => {
        this.isLoading = false;
        this.uiService.showErrorSnackbar(error, null, 3000);
       });
  }

  getCurrencies(){
    this.currencyData.GetCurrenciesDropdown().subscribe((result: SelectItemsList) => {
      this.currencySelectList = result;
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
     });
  }

  amountValidator(): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any} | null => {
      const invalid = this.validateAmount(control.value);
      return !invalid ? {invalidError: {value: control.value}} : null;
    };
  }

  private validateAmount( pAmount: number ) {
    if (this.currencySelectList.selectItems){
      const item = this.currencySelectList.selectItems.find(x => x.value === this.loanRequestForm.value.currencyId);
      if (item.label === 'RON'){
        if (pAmount > 25000){
          return false;
        }
      }

      if (item.label === 'EUR'){
        if (pAmount > 5000){
          return false;
        }
      }
    }
    return pAmount;
  }

  ngOnDestroy(): void {
    this.isExplorerSubscription.unsubscribe();
  }

}
