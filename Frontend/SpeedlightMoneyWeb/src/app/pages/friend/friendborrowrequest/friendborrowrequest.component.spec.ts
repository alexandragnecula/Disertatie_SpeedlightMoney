import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendborrowrequestComponent } from './friendborrowrequest.component';

describe('FriendborrowrequestComponent', () => {
  let component: FriendborrowrequestComponent;
  let fixture: ComponentFixture<FriendborrowrequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FriendborrowrequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FriendborrowrequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
