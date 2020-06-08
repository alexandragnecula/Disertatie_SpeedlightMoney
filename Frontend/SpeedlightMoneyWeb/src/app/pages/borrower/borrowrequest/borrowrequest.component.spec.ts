import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BorrowrequestComponent } from './borrowrequest.component';

describe('BorrowrequestComponent', () => {
  let component: BorrowrequestComponent;
  let fixture: ComponentFixture<BorrowrequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BorrowrequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BorrowrequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
