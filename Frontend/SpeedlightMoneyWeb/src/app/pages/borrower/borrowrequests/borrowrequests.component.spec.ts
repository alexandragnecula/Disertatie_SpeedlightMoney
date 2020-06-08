import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BorrowrequestsComponent } from './borrowrequests.component';

describe('BorrowrequestsComponent', () => {
  let component: BorrowrequestsComponent;
  let fixture: ComponentFixture<BorrowrequestsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BorrowrequestsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BorrowrequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
