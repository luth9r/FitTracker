import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthInputFieldComponent } from '../../../ui/auth-input-field/auth-input-field.component';
import {ModalService} from '../../../../../../shared/utils/modal.service';
import {ForgotPasswordModalComponent} from '../../../ui/forgot-password-modal/forgot-password-modal.component'

export interface LoginFormData {
  email: string;
  password: string;
}

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslateModule, AuthInputFieldComponent],
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss'],
})
export class LoginFormComponent {
  private readonly modalService = inject(ModalService);

  @Input() isLoading = false;
  @Output() submitForm = new EventEmitter<LoginFormData>();
  @Output() fieldFocus = new EventEmitter<string>();
  @Output() fieldBlur = new EventEmitter<void>();

  openForgotPasswordModal(): void {
    this.modalService.open(ForgotPasswordModalComponent, {
      hasBackdrop: true,
      backdropClass: 'modal-backdrop-blur',
      panelClass: 'modal-panel-forgot-password',
    });
  }

  formData: LoginFormData = {
    email: '',
    password: '',
  };

  showPassword = false;

  onFieldFocus(fieldName: string) {
    this.fieldFocus.emit(fieldName);
  }

  onFieldBlur() {
    this.fieldBlur.emit();
  }

  onSubmit() {
    this.submitForm.emit(this.formData);
  }
}
