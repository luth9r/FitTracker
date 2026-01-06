import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';
import { ModalRef } from '../../../../../shared/utils/modal.service';
import { AuthInputFieldComponent} from '../auth-input-field/auth-input-field.component';
import {AuthService} from '../../../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-forgot-password-modal',
  templateUrl: './forgot-password-modal.component.html',
  styleUrls: ['./forgot-password-modal.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    MatIconModule,
    TranslateModule,
    AuthInputFieldComponent
  ],
})
export class ForgotPasswordModalComponent {
  private readonly modalRef = inject<ModalRef<unknown, void>>(ModalRef);
  private readonly authService = inject(AuthService);

  email = '';
  isSuccess = false;
  errorMessage = '';
  touched = false;

  get hasError(): boolean {
    return this.touched && (!this.email || !this.validateEmail(this.email));
  }

  validateEmail(email: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  }

  onFieldFocus(): void {
  }

  onFieldBlur(): void {
    this.touched = true;
  }

  onSubmit(event?: Event): void {
    event?.preventDefault();
    this.touched = true;

    if (!this.email || !this.validateEmail(this.email)) {
      return;
    }

    this.authService.forgotPassword(this.email).subscribe({
      next: () => {
        this.isSuccess = true;

        setTimeout(() => this.close(), 3000);
      },
    });
  }

  close(): void {
    this.modalRef.close();
  }
}
