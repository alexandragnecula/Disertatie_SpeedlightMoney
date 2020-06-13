import { Observable } from 'rxjs';
import { Result } from './common/result';
import { SelectItemsList } from './common/selectitem';

export class Friend {
    nickname: string;
    userId: number;
    userFriendId: number;
    deleted: boolean;
}

export class FriendsLookup {
    id: number;
    nickname: string;
    userName: string;
    userFriendName: string;
    userFriendPhoneNumber: string;
    deleted: boolean;
}

export class FriendsList {
    friends: FriendsLookup[];
}

export class AddFriendCommand {
    nickname: string;
    userId: number;
    userFriendId: number;
}

export class UpdateFriendCommand {
    id: number;
    nickname: string;
    userId: number;
    userFriendId: number;
}

export class RestoreFriendCommand {
    id: number;
}

export abstract class FriendData {
    abstract GetFriend(id: number): Observable<Friend>;
    abstract GetFriends(): Observable<FriendsList>;
    abstract GetFriendsDropdown(): Observable<SelectItemsList>;
    abstract AddFriend(addFriendCommand: AddFriendCommand): Observable<Result>;
    abstract UpdateFriend(updateFriendCommand: UpdateFriendCommand): Observable<Result>;
    abstract DeleteFriend(id: number): Observable<Result>;
    abstract RestoreFriend(restoreFriendCommand: RestoreFriendCommand): Observable<Result>;
    abstract AddFriendForCurrentUser(addFriendCommand: AddFriendCommand): Observable<Result>;
    abstract GetFriendsForCurrentUser(): Observable<FriendsLookup[]>;
}

