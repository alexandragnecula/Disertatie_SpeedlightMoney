import { Component, OnInit, Inject } from '@angular/core';
import { SelectItemsList } from 'src/app/@core/data/common/selectitem';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-addmoney',
  templateUrl: './addmoney.component.html',
  styleUrls: ['./addmoney.component.scss']
})
export class AddmoneyComponent implements OnInit {
  addMoneyForm: FormGroup;
  isLoading = false;
  currentUserWalletsSelectList: SelectItemsList = new SelectItemsList();
  walletId: number;

  constructor(@Inject(MAT_DIALOG_DATA) public passedData: any) { }

  ngOnInit(): void {
    this.initForm();
    this.currentUserWalletsSelectList = this.passedData.walletsSelectList;
  }

  initForm() {
    this.addMoneyForm = new FormGroup({
        walletId: new FormControl(this.passedData.walletId, [Validators.required]),
        totalAmount: new FormControl('', [Validators.required])
    });
  }
}
