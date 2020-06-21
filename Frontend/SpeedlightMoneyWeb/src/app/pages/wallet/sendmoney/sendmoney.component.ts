import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SelectItemsList } from 'src/app/@core/data/common/selectitem';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CurrencyData } from 'src/app/@core/data/currency';
import { UIService } from 'src/app/shared/ui.service';
import { UserData } from 'src/app/@core/data/userclasses/user';

@Component({
  selector: 'app-sendmoney',
  templateUrl: './sendmoney.component.html',
  styleUrls: ['./sendmoney.component.scss']
})
export class SendmoneyComponent implements OnInit {
  sendMoneyForm: FormGroup;
  isLoading = false;
  currenciesSelectList: SelectItemsList = new SelectItemsList();
  beneficiarsSelectList: SelectItemsList = new SelectItemsList();
  currencyId: number;
  beneficiarId: number;

  constructor(@Inject(MAT_DIALOG_DATA) public passedData: any,
              private currencyData: CurrencyData,
              private userdData: UserData,
              private uiService: UIService) { }

  ngOnInit(): void {
    this.initForm();
    this.getBeneficiarsSelect();
    this.getCurrenciesSelect();
  }

  initForm() {
    this.sendMoneyForm = new FormGroup({
        beneficiarId: new FormControl('', [Validators.required]),
        currencyId: new FormControl('', [Validators.required]),
        totalAmount: new FormControl('', [Validators.required])
    });
  }

  getCurrenciesSelect(){
    this.currencyData.GetCurrenciesDropdown().subscribe((users: SelectItemsList) => {
      this.currenciesSelectList = users;
      if (this.currenciesSelectList.selectItems.length > 0) {
        this.currencyId = +this.currenciesSelectList.selectItems[0].value;
        this.sendMoneyForm.patchValue({currencyId: this.currenciesSelectList.selectItems[0].value});
      }
    },
    error => {
        this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  getBeneficiarsSelect(){
    this.userdData.getUsersDropdown().subscribe((users: SelectItemsList) => {
      this.beneficiarsSelectList = users;
    },
    error => {
        this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

}
