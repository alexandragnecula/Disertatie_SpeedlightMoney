import { Observable } from 'rxjs';
import { SelectItemsList } from './common/selectitem';
import { Result } from './common/result';

export class Currency {
    currencyName: string;
    deleted: boolean;
}

export class CurrenciesLookup {
    id: number;
    currencyName: string;
    deleted: boolean;
}

export class CurrenciesList {
    currencies: CurrenciesLookup[];
}

export class AddCurrencyCommand {
    currencyName: string;
}

export class UpdateCurrencyCommand {
    id: number;
    currencyName: string;
}

export class RestoreCurrencyCommand {
    id: number;
}

export abstract class CurrencyData {
    abstract GetCurrency(id: number): Observable<Currency>;
    abstract GetCurrencies(): Observable<CurrenciesList>;
    abstract GetCurrenciesDropdown(): Observable<SelectItemsList>;
    abstract AddCurrency(addCurrencyCommand: AddCurrencyCommand): Observable<Result>;
    abstract UpdateCurrency(updateCurrencyCommand: UpdateCurrencyCommand): Observable<Result>;
    abstract DeleteCurrency(id: number): Observable<Result>;
    abstract RestoreCurrency(restoreCurrencyCommand: RestoreCurrencyCommand): Observable<Result>;
}