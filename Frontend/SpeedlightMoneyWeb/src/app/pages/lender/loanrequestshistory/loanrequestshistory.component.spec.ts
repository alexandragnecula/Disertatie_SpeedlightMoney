import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoanrequestshistoryComponent } from './loanrequestshistory.component';

describe('LoanrequestshistoryComponent', () => {
  let component: LoanrequestshistoryComponent;
  let fixture: ComponentFixture<LoanrequestshistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoanrequestshistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoanrequestshistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
