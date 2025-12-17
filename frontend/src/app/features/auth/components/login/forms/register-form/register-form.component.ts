import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AuthInputFieldComponent } from '../../../ui/auth-input-field/auth-input-field.component';
import { ValidationChecklistComponent } from '../../../ui/validation-checklist/validation-checklist.component';

export interface RegisterFormData {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}

@Component({
  selector: 'app-register-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    AuthInputFieldComponent,
    ValidationChecklistComponent,
  ],
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.scss'],
})
export class RegisterFormComponent {
  @Input() isLoading = false;
  @Input() usernameValidationState = { minLength: false, noSpaces: false };
  @Input() passwordValidationState = { minLength: false, oneLetter: false, oneNumber: false };
  @Input() confirmPasswordValidationState = { matches: false };
  @Input() activeField: string | null = null;
  @Input() value = '';

  @Output() valueChange = new EventEmitter<string>();
  @Output() submitForm = new EventEmitter<RegisterFormData>();
  @Output() fieldFocus = new EventEmitter<string>();
  @Output() fieldBlur = new EventEmitter<void>();
  @Output() validateChange = new EventEmitter<{ field: string; value: string }>();

  formData: RegisterFormData = {
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  };

  showPassword = false;
  showConfirmPassword = false;

  get showUsernameValidation(): boolean {
    return (
      this.activeField === 'username' &&
      this.formData.username.length > 0 &&
      !Object.values(this.usernameValidationState).every((v) => v)
    );
  }

  get showPasswordValidation(): boolean {
    return (
      this.activeField === 'password' &&
      this.formData.password.length > 0 &&
      !Object.values(this.passwordValidationState).every((v) => v)
    );
  }

  get showConfirmPasswordValidation(): boolean {
    return (
      this.activeField === 'confirmPassword' &&
      this.formData.confirmPassword.length > 0 &&
      !this.confirmPasswordValidationState.matches
    );
  }

  onInput(event: Event) {
    const target = event.target as HTMLInputElement;
    this.valueChange.emit(target.value);
  }

  onFieldFocus(fieldName: string) {
    this.fieldFocus.emit(fieldName);
  }

  onFieldBlur() {
    this.fieldBlur.emit();
  }

  onValidateChange(field: string, value: string) {
    this.validateChange.emit({ field, value });
  }

  onSubmit() {
    this.submitForm.emit(this.formData);
  }
}
