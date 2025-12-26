import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailVerificationModalComponent } from './email-verification-modal.component';

describe('EmailVerificationModalComponent', () => {
  let component: EmailVerificationModalComponent;
  let fixture: ComponentFixture<EmailVerificationModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmailVerificationModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmailVerificationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
