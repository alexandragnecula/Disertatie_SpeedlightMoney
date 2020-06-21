import { Observable } from 'rxjs';
import { Result } from './common/result';

export class TransactionHistory {
    amount: number;
    senderId: number;
    beneficiarId: number;
    currencyId: number;
    createdOn: Date;
}

export class TransactionsHistoryLookup {
    id: number;
    amount: number;
    createdOn: Date;
}

export class TransactionsHistoryList {
    transactionsHistory: TransactionsHistoryLookup[];
}

export class AddTransactionHistoryCommand {
    amount: number;
    senderId: number;
    beneficiarId: number;
    currencyId: number;
}

export class UpdateTransactionHistoryCommand {
    id: number;
    amount: number;
    senderId: number;
    beneficiarId: number;
    currencyId: number;
}

export class RestoreTransactionHistoryCommand {
    id: number;
}

export abstract class TransactionHistoryData {
    abstract GetTransactionHistory(id: number): Observable<TransactionHistory>;
    abstract GetTransactionsHistory(): Observable<TransactionsHistoryList>;
    abstract GetTransactionsHistoryForCurrentUser(): Observable<TransactionsHistoryLookup[]>;
    abstract AddTransactionHistory(addTransactionHistoryCommand: AddTransactionHistoryCommand): Observable<Result>;
    abstract UpdateTransactionHistory(updateTransactionHistoryCommand: UpdateTransactionHistoryCommand): Observable<Result>;
    abstract DeleteTransactionHistory(id: number): Observable<Result>;
    abstract RestoreTransactionHistory(restoreTransactionHistoryCommand: RestoreTransactionHistoryCommand): Observable<Result>;
}
