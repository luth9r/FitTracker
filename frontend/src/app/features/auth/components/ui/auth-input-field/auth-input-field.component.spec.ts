import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthInputFieldComponent } from './auth-input-field.component';

describe('AuthInputFieldComponent', () => {
  let component: AuthInputFieldComponent;
  let fixture: ComponentFixture<AuthInputFieldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthInputFieldComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthInputFieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
