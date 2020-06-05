import { Observable } from 'rxjs/internal/Observable';
import { AuthSuccessResponse } from '../common/authresponse';
import { CurrentUser } from './currentuser';
import { Result } from '../common/result';
import { SelectItemsList} from '../common/selectitem';

export class User {
    email: string;
    firstName: string;
    lastName: string;
    birthdate: Date;
    cnp: string;
    country: string;
    county: string;
    city: number;
    streetName: string;
    streetNumber: string;
    currentStatus: string;
    cardNumber: string;
    cvv: string;
    expireDate: Date;
    salary: number;
    phoneNumber: string;
    isActive?: boolean;
}

export class LoginUser {
    email: string;
    password: string;
}

export class UserLookup {
    id: number;
    email: string;
    name: string;
    birthdate: Date;
    cnp: string;
    country: string;
    county: string;
    city: number;
    streetName: string;
    streetNumber: string;
    currentStatus: string;
    cardNumber: string;
    cvv: string;
    expireDate: Date;
    salary: number;
    phoneNumber: string;
    isActive?: boolean;
}

export class UserList {
    users: UserLookup[];
}

export class AddUserCommand {
    password: string;
    email: string;

    firstName: string;
    lastName: string;
    birthdate: Date;
    cnp: string;
    country: string;
    county: string;
    city: number;
    streetName: string;
    streetNumber: string;
    currentStatus: string;
    cardNumber: string;
    cvv: string;
    expireDate: Date;
    salary: number;
    phoneNumber: string;

    totalAmount: number;
    currencyId: number;
    roleId: number;
}

export class UpdateUserCommand {
    id: number;
    email: string;
    password: string;

    firstName: string;
    lastName: string;
    birthdate: Date;
    cnp: string;
    country: string;
    county: string;
    city: string;
    streetName: string;
    streetNumber: string;
    currentStatus: string;
    cardNumber: string;
    cvv: string;
    expireDate: Date;
    salary: number;
    phoneNumber: string;
    isActive?: boolean;
}

export abstract class UserData {
    abstract AddUser(addUserCommand: User): Observable<AuthSuccessResponse>;
    abstract AddUserWithWallets(addUserCommand: AddUserCommand): Observable<AuthSuccessResponse>;
    abstract LoginUser(loginUserCommand: LoginUser): Observable<AuthSuccessResponse>;
    abstract GetCurrentUser(): Observable<CurrentUser>;

    abstract getUsers(): Observable<UserList>;
    abstract getUser(id: number): Observable<User>;
    abstract getUsersDropdown(): Observable<SelectItemsList>;
    abstract getCurrentStatusesDropdown(): Observable<SelectItemsList>;
    abstract updateUser(user: UpdateUserCommand): Observable<Result>;
}
