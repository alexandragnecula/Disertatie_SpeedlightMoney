import { Observable } from 'rxjs';
import { Result } from './common/result';
import { SelectItemsList } from './common/selectitem';

export class Debt {
    loanId: number;
    debtStatusId: number;
    deleted: boolean;
}

export class DebtsLookup {
    id: number;
    loanAmount: number;
    borrowDate?: Date;
    returnDate?: Date;
    dueDate?: Date;
    borrowerName: string;
    lenderName: string;
    debtStatusName: string;
    loanStatusName: string;
    currencyName: string;
    termName: string;
    deleted: boolean;
}

export class DebtsList {
    debts: DebtsLookup[];
}

export class AddDebtCommand {
    loanId: number;
    debtStatusId: number;
}

export class UpdateDebtCommand {
    id: number;
    loanId: number;
    debtStatusId: number;
}

export class RestoreDebtCommand {
    id: number;
}

export abstract class DebtData {
    abstract GetDebt(id: number): Observable<Debt>;
    abstract GetDebts(): Observable<DebtsList>;
    abstract GetDebtsForCurrentUser(): Observable<DebtsLookup[]>;
    abstract GetCreditsForCurrentUser(): Observable<DebtsLookup[]>;
    abstract GetDebtsHistoryForCurrentUser(): Observable<DebtsLookup[]>;
    abstract GetCreditsHistoryForCurrentUser(): Observable<DebtsLookup[]>;
    abstract GetDebtsDropdown(): Observable<SelectItemsList>;
    abstract AddDebt(addDebtCommand: AddDebtCommand): Observable<Result>;
    abstract UpdateDebt(updateDebtCommand: UpdateDebtCommand): Observable<Result>;
    abstract DeleteDebt(id: number): Observable<Result>;
    abstract RestoreDebt(restoreDebtCommand: RestoreDebtCommand): Observable<Result>;
    abstract PayDebt(updateDebtCommand: UpdateDebtCommand): Observable<Result>;
    abstract DeferPayment(updateDebtCommand: UpdateDebtCommand): Observable<Result>;
    abstract SendReminderToBorrower(id: number): Observable<Result>;
}
