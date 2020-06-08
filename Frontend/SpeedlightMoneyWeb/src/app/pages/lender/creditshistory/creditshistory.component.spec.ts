import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreditshistoryComponent } from './creditshistory.component';

describe('CreditshistoryComponent', () => {
  let component: CreditshistoryComponent;
  let fixture: ComponentFixture<CreditshistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreditshistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreditshistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
