import { Observable } from 'rxjs';
import { SelectItemsList } from './common/selectitem';
import { Result } from './common/result';

export class Role {
    name: string;
}

export class RolesLookup {
    id: number;
    name: string;
}

export class RolesList {
    roles: RolesLookup[];
}

export class AddRoleCommand {
    name: string;
}

export class UpdateRoleCommand {
    id: number;
    name: string;
}

export class RestoreRoleCommand {
    id: number;
}

export abstract class RoleData {
    abstract GetRole(id: number): Observable<Role>;
    abstract GetRoles(): Observable<RolesList>;
    abstract GetRolesDropdown(): Observable<SelectItemsList>;
    abstract AddRole(addRoleCommand: AddRoleCommand): Observable<Result>;
    abstract UpdateRole(updateRoleCommand: UpdateRoleCommand): Observable<Result>;
    abstract DeleteRole(id: number): Observable<Result>;
    abstract RestoreRole(restoreRoleCommand: RestoreRoleCommand): Observable<Result>;
}
