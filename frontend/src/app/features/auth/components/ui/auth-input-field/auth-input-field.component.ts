import { Component, Input, Output, EventEmitter, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

export interface InputValidationState {
  minLength: boolean;
  noSpaces?: boolean;
  oneLetter?: boolean;
  oneNumber?: boolean;
}

@Component({
  selector: 'app-auth-input-field',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslateModule],
  templateUrl: './auth-input-field.component.html',
  styleUrls: ['./auth-input-field.component.scss'],
})
export class AuthInputFieldComponent {
  @Input() labelKey!: string;
  @Input() placeholderKey!: string;
  @Input() fieldName!: string;
  @Input() type: 'text' | 'email' | 'password' = 'text';
  @Input() value = '';
  @Input() showPassword = false;
  @Input() validationState?: InputValidationState;
  @Input() activeField?: string | null;
  @Input() isFormPristine = true;
  @Input() validationType?: 'username' | 'password';

  @Output() valueChange = new EventEmitter<string>();
  @Output() showPasswordChange = new EventEmitter<boolean>();
  @Output() focus = new EventEmitter<void>();
  @Output() blur = new EventEmitter<void>();
  @Output() validateOnChange = new EventEmitter<string>();

  togglePassword() {
    this.showPassword = !this.showPassword;
    this.showPasswordChange.emit(this.showPassword);
  }

  onFocus() {
    this.focus.emit();
  }
  onBlur() {
    this.blur.emit();
  }

  onValueChange(value: string) {
    this.value = value;
    this.valueChange.emit(value);
    if (this.validateOnChange.observers.length) {
      this.validateOnChange.emit(value);
    }
  }
}
