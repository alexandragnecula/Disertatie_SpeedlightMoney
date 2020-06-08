import { Component, OnInit, AfterViewInit } from '@angular/core';
import { WalletData, UpdateWalletCommand } from 'src/app/@core/data/wallet';
import { UIService } from 'src/app/shared/ui.service';
import { SelectItemsList, SelectItem } from 'src/app/@core/data/common/selectitem';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AddmoneyComponent } from '../wallet/addmoney/addmoney.component';
import { Result } from 'src/app/@core/data/common/result';
import { UserData } from 'src/app/@core/data/userclasses/user';
import { SendmoneyComponent } from '../wallet/sendmoney/sendmoney.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  currentUserWalletsSelectList: SelectItemsList = new SelectItemsList();
  beneficiarsSelectList: SelectItemsList = new SelectItemsList();
  walletForm: FormGroup;
  walletId: number;
  isLoading = true;

  constructor(private walletData: WalletData,
              private uiService: UIService,
              public dialog: MatDialog,
              public dialogToSendMoney: MatDialog) { }

  ngOnInit(): void {
    this.initForm();
    this.getWalletsForCurrentUserSelect();
  }

  initForm() {
    this.walletForm = new FormGroup({
        walletId: new FormControl(''),
        totalAmount: new FormControl('')
    });
  }

  getWalletsForCurrentUserSelect() {
    this.walletData.GetWalletsForCurrentUserDropdown().subscribe((users: SelectItemsList) => {
      this.currentUserWalletsSelectList = users;
      if (this.currentUserWalletsSelectList.selectItems.length > 0) {
        this.walletId = +this.currentUserWalletsSelectList.selectItems[0].value;
        this.walletForm.patchValue({walletId: this.currentUserWalletsSelectList.selectItems[0].value});
        // const toSelect = this.currentUserWalletsSelectList.selectItems.find(c => c.value === '1');
      }
    },
    error => {
        this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  openDialogToEdit() {

    const dialogRef = this.dialog.open(AddmoneyComponent, {
      width: '600px',
      data: {
        walletsSelectList: this.currentUserWalletsSelectList,
        walletId: this.walletForm.value.walletId
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.addMoneyToWallet(result);
      }
    });

  }

  openDialogToSendMoney() {
    const dialogRef = this.dialog.open(SendmoneyComponent, {
      width: '600px',
      data: {
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.sendMoney(result);
      }
    });
  }

  addMoneyToWallet(form) {
    this.isLoading = true;
    const updateWalletCommand: UpdateWalletCommand = {
      id: +form.value.walletId,
      totalAmount: form.value.totalAmount,

    } as UpdateWalletCommand;

    this.walletData.AddMoneyToWallet(updateWalletCommand).subscribe((res: Result) => {
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
      this.getWalletsForCurrentUserSelect();
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }

  sendMoney(form){
    this.isLoading = true;
    const updateWalletCommand: UpdateWalletCommand = {
      userId: +form.value.beneficiarId,
      currencyId: +form.value.currencyId,
      totalAmount: form.value.totalAmount,
    } as UpdateWalletCommand;

    this.walletData.SendMoney(updateWalletCommand).subscribe((res: Result) => {
      this.uiService.showSuccessSnackbar(res.successMessage, null, 3000);
      this.getWalletsForCurrentUserSelect();
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.uiService.showErrorSnackbar(error, null, 3000);
    });
  }
}
