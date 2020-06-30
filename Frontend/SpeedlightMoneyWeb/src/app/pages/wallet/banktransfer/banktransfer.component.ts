import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SelectItemsList } from 'src/app/@core/data/common/selectitem';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-banktransfer',
  templateUrl: './banktransfer.component.html',
  styleUrls: ['./banktransfer.component.scss']
})
export class BanktransferComponent implements OnInit {
  bankTransferForm: FormGroup;
  isLoading = false;
  currentUserWalletsSelectList: SelectItemsList = new SelectItemsList();
  walletId: number;

  constructor(@Inject(MAT_DIALOG_DATA) public passedData: any) { }

  ngOnInit(): void {
    this.initForm();
    this.currentUserWalletsSelectList = this.passedData.walletsSelectList;
  }

  initForm() {
    this.bankTransferForm = new FormGroup({
        walletId: new FormControl(this.passedData.walletId, [Validators.required]),
        totalAmount: new FormControl('', [Validators.required])
    });
  }
}
