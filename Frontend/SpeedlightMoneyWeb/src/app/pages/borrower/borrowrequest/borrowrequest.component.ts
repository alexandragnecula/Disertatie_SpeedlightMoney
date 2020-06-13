import { Component, OnInit, Inject } from '@angular/core';
import { SelectItemsList } from 'src/app/@core/data/common/selectitem';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormControl, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { LoanData } from 'src/app/@core/data/loan';
import { UIService } from 'src/app/shared/ui.service';
import { TermData } from 'src/app/@core/data/term';
import { CurrencyData } from 'src/app/@core/data/currency';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-borrowrequest',
  templateUrl: './borrowrequest.component.html',
  styleUrls: ['./borrowrequest.component.scss']
})
export class BorrowrequestComponent implements OnInit {

  termSelectList: SelectItemsList = new SelectItemsList();
  currencySelectList: SelectItemsList = new SelectItemsList();
  isLoading = false;
  loanRequestForm: FormGroup;

  constructor(@Inject(MAT_DIALOG_DATA)  public loanRequest: any,
              private termData: TermData,
              private currencyData: CurrencyData, private uiService: UIService) { }

  ngOnInit(): void {
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
      termId: new FormControl('', [Validators.required])
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

}
