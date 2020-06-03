import { Observable } from 'rxjs';
import { SelectItemsList } from './common/selectitem';
import { Result } from './common/result';

export class Term {
    termName: string;
    periodInDays: number;
    deleted: boolean;
}

export class TermsLookup {
    id: number;
    termName: string;
    periodInDays: number;
    deleted: boolean;
}

export class TermsList {
    terms: TermsLookup[];
}

export class AddTermCommand {
    termName: string;
    periodInDays: number;
}

export class UpdateTermCommand {
    id: number;
    termName: string;
    periodInDays: number;
}

export class RestoreTermCommand {
    id: number;
}

export abstract class TermData {
    abstract GetTerm(id: number): Observable<Term>;
    abstract GetTerms(): Observable<TermsList>;
    abstract GetTermsDropdown(): Observable<SelectItemsList>;
    abstract AddTerm(addTermCommand: AddTermCommand): Observable<Result>;
    abstract UpdateTerm(updateTermCommand: UpdateTermCommand): Observable<Result>;
    abstract DeleteTerm(id: number): Observable<Result>;
    abstract RestoreTerm(restoreTermCommand: RestoreTermCommand): Observable<Result>;
}