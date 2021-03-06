import { Observable } from 'rxjs';
import { SelectItemsList } from './common/selectitem';
import { Result } from './common/result';

export class Loan {
    description: string;
    amount: number;
    borrowDate?: Date;
    returnDate?: Date;
    dueDate?: Date;
    borrowerId: number;
    lenderId: number;
    currencyId: number;
    loadStatusId: number;
    termId: number;
    deleted: boolean;
}

export class LoansLookup {
    id: number;
    description: string;
    amount: number;
    borrowDate?: Date;
    returnDate?: Date;
    dueDate?: Date;
    borrowerName: string;
    lenderName: string;
    currencyName: string;
    loanStatusName: string;
    termName: string;
    deleted: boolean;
}

export class LoansList {
    loans: LoansLookup[];
}

export class AddLoanCommand {
    description: string;
    amount: number;
    borrowDate?: Date;
    returnDate?: Date;
    dueDate?: Date;
    borrowerId: number;
    lenderId: number;
    currencyId: number;
    loadStatusId: number;
    termId: number;
}

export class UpdateLoanCommand {
    id: number;
    description: string;
    amount: number;
    borrowDate?: Date;
    returnDate?: Date;
    dueDate?: Date;
    borrowerId: number;
    lenderId: number;
    currencyId: number;
    loadStatusId: number;
    termId: number;
}

export class ManageLoanCommand {
    id: number;
    loanStatusId: number;
    borrowerId: number;
    lenderId: number;
    currencyId: number;
    amount: number;
}

export class RestoreLoanCommand {
    id: number;
}

export abstract class LoanData {
    abstract GetLoan(id: number): Observable<Loan>;
    abstract GetLoans(): Observable<LoansList>;
    abstract GetLoansDropdown(): Observable<SelectItemsList>;
    abstract AddLoan(addLoanCommand: AddLoanCommand): Observable<Result>;
    abstract UpdateLoan(updateLoanCommand: UpdateLoanCommand): Observable<Result>;
    abstract DeleteLoan(id: number): Observable<Result>;
    abstract RestoreLoan(restoreLoanCommand: RestoreLoanCommand): Observable<Result>;
    abstract RequestLoan(addLoanCommand: AddLoanCommand): Observable<Result>;
    abstract ManageLoan(updateLoanCommand: ManageLoanCommand): Observable<Result>;
    abstract GetBorrowRequestsForCurrentUser(): Observable<LoansLookup[]>;
    abstract GetLendRequestsForCurrentUser(): Observable<LoansLookup[]>;
    abstract GetBorrowRequestsHistoryForCurrentUser(): Observable<LoansLookup[]>;
    abstract GetLendRequestsHistoryForCurrentUser(): Observable<LoansLookup[]>;
    abstract CancelLoanRequest(updateLoanCommand: ManageLoanCommand): Observable<Result>;
}
