import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDutyComponent } from './add-duty.component';

describe('AddDutyComponent', () => {
  let component: AddDutyComponent;
  let fixture: ComponentFixture<AddDutyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddDutyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddDutyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
