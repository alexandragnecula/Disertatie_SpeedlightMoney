import { Observable } from 'rxjs';
import { Result } from './common/result';
import { SelectItemsList } from './common/selectitem';

export class DebtStatus {
    debtStatusName: string;
    deleted: boolean;
}

export class DebtStatusesLookup {
    id: number;
    debtStatusName: string;
    deleted: boolean;
}

export class DebtStatusesList {
    debtStatuses: DebtStatusesLookup[];
}

export class AddDebtStatusCommand {
    debtStatusName: string;
}

export class UpdateDebtStatusCommand {
    id: number;
    debtStatusName: string;
}

export class RestoreDebtStatusCommand {
    id: number;
}

export abstract class DebtStatusData {
    abstract GetDebtStatus(id: number): Observable<DebtStatus>;
    abstract GetDebtStatuses(): Observable<DebtStatusesList>;
    abstract GetDebtStatusesDropdown(): Observable<SelectItemsList>;
    abstract AddDebtStatus(addDebtStatusCommand: AddDebtStatusCommand): Observable<Result>;
    abstract UpdateDebtStatus(updateDebtStatusCommand: UpdateDebtStatusCommand): Observable<Result>;
    abstract DeleteDebtStatus(id: number): Observable<Result>;
    abstract RestoreDebtStatus(restoreDebtStatusCommand: RestoreDebtStatusCommand): Observable<Result>;
}
