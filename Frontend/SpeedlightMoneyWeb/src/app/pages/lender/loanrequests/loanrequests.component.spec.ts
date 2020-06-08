import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoanrequestsComponent } from './loanrequests.component';

describe('LoanrequestsComponent', () => {
  let component: LoanrequestsComponent;
  let fixture: ComponentFixture<LoanrequestsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoanrequestsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoanrequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
