import { Observable } from 'rxjs';
import { Result } from './common/result';
import { SelectItemsList } from './common/selectitem';

export class LoanStatus {
    loanStatusName: string;
    deleted: boolean;
}

export class LoanStatusesLookup {
    id: number;
    loanStatusName: string;
    deleted: boolean;
}

export class LoanStatusesList {
    loanStatuses: LoanStatusesLookup[];
}

export class AddLoanStatusCommand {
    loanStatusName: string;
}

export class UpdateLoanStatusCommand {
    id: number;
    loanStatusName: string;
}

export class RestoreLoanStatusCommand {
    id: number;
}

export abstract class LoanStatusData {
    abstract GetLoanStatus(id: number): Observable<LoanStatus>;
    abstract GetLoanStatuses(): Observable<LoanStatusesList>;
    abstract GetLoanStatusesDropdown(): Observable<SelectItemsList>;
    abstract AddLoanStatus(addLoanStatusCommand: AddLoanStatusCommand): Observable<Result>;
    abstract UpdateLoanStatus(updateLoanStatusCommand: UpdateLoanStatusCommand): Observable<Result>;
    abstract DeleteLoanStatus(id: number): Observable<Result>;
    abstract RestoreLoanStatus(restoreLoanStatusCommand: RestoreLoanStatusCommand): Observable<Result>;
}
