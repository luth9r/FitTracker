import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationChecklistComponent } from './validation-checklist.component';

describe('ValidationChecklistComponent', () => {
  let component: ValidationChecklistComponent;
  let fixture: ComponentFixture<ValidationChecklistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ValidationChecklistComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ValidationChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
