import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RetiredComponent } from './retired.component';

describe('RetiredComponent', () => {
  let component: RetiredComponent;
  let fixture: ComponentFixture<RetiredComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RetiredComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RetiredComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
