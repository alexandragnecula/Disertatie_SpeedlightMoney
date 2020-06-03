import { Observable } from 'rxjs';
import { SelectItemsList } from './common/selectitem';
import { Result } from './common/result';

export class Wallet {
    totalAmount: number;
    userId: number;
    currencyId: number;
    deleted: boolean;
}

export class WalletsLookup {
    id: number;
    totalAmount: number;
    deleted: boolean;
}

export class WalletsList {
    wallets: WalletsLookup[];
}

export class AddWalletCommand {
    totalAmount: number;
    userId: number;
    currencyId: number;
}

export class UpdateWalletCommand {
    id: number;
    totalAmount: number;
    userId: number;
    currencyId: number;
}

export class RestoreWalletCommand {
    id: number;
}

export abstract class WalletData {
    abstract GetWallet(id: number): Observable<Wallet>;
    abstract GetWallets(): Observable<WalletsList>;
    abstract GetWalletsDropdown(): Observable<SelectItemsList>;
    abstract AddWallet(addWalletCommand: AddWalletCommand): Observable<Result>;
    abstract UpdateWallet(updateWalletCommand: UpdateWalletCommand): Observable<Result>;
    abstract DeleteWallet(id: number): Observable<Result>;
    abstract RestoreWallet(restoreWalletCommand: RestoreWalletCommand): Observable<Result>;
}