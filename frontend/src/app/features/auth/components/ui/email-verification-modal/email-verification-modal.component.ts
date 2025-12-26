import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { ModalRef } from '../../../../../shared/utils/modal.service';

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
export class EmailVerificationModalComponent implements OnInit {
  private modalRef = inject(ModalRef<EmailVerificationData, EmailVerificationResult>);

  email = '';
  isResending = false;

  ngOnInit() {
    this.email = this.modalRef.data?.email || '';
  }

  onClose() {
    this.modalRef.close({ action: 'close' });
  }

  async onResend() {
    this.isResending = true;

    // Emit resend action
    this.modalRef.close({ action: 'resend' });
  }
}
