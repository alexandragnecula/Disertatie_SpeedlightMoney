import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DebtshistoryComponent } from './debtshistory.component';

describe('DebtshistoryComponent', () => {
  let component: DebtshistoryComponent;
  let fixture: ComponentFixture<DebtshistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DebtshistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DebtshistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
