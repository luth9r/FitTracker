import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { ModalRef } from '../../../../../shared/utils/modal.service';
import { AuthService } from '../../../services/auth.service';

export interface EmailVerificationData {
  email: string;
}

export interface EmailVerificationResult {
  action: 'resend' | 'close';
}

@Component({
  selector: 'app-email-verification-modal',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './email-verification-modal.component.html',
  styleUrls: ['./email-verification-modal.component.scss'],
})
export class EmailVerificationModalComponent implements OnInit, OnDestroy {
  private modalRef = inject(ModalRef<EmailVerificationData, EmailVerificationResult>);
  private authService = inject(AuthService);

  email = '';
  isResending = false;
  
  // Timer properties
  canResend = false;
  countdown = 60;
  private timerInterval?: number;

  ngOnInit() {
    this.email = this.modalRef.data?.email || '';
    this.startTimer();
  }

  ngOnDestroy() {
    this.clearTimer();
  }

  onClose() {
    this.modalRef.close({ action: 'close' });
  }

  async onResend() {
    if (!this.canResend) return;

    this.isResending = true;
    
    // Reset timer
    this.canResend = false;
    this.countdown = 60;
    this.startTimer();

    this.authService.resendVerificationEmail(this.email).subscribe({
      next: () => {
        console.log('✅ Verification email resent successfully');
        this.isResending = false;
        // Modal stays open, timer continues
      },
      error: (error) => {
        console.error('❌ Failed to resend verification email:', error);
        this.isResending = false;
        // Timer continues anyway
      }
    });
  }

  private startTimer() {
    this.canResend = false;
    this.countdown = 60;
    
    this.timerInterval = window.setInterval(() => {
      this.countdown--;
      
      if (this.countdown <= 0) {
        this.canResend = true;
        this.clearTimer();
      }
    }, 1000);
  }

  private clearTimer() {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
      this.timerInterval = undefined;
    }
  }
}
