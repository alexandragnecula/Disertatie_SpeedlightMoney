import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BorrowrequestshistoryComponent } from './borrowrequestshistory.component';

describe('BorrowrequestshistoryComponent', () => {
  let component: BorrowrequestshistoryComponent;
  let fixture: ComponentFixture<BorrowrequestshistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BorrowrequestshistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BorrowrequestshistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
