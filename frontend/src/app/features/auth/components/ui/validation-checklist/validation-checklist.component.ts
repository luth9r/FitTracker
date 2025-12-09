import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

interface ValidationState {
  [key: string]: boolean;
}

interface ValidationCheck {
  key: string;
  isValid: boolean;
  label: string;
}

@Component({
  selector: 'app-validation-checklist',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './validation-checklist.component.html',
  styleUrls: ['./validation-checklist.component.scss'] 
})
export class ValidationChecklistComponent {
  private translate = inject(TranslateService);
  
  @Input() validationState!: ValidationState;
  @Input() translationPrefix = '';
  @Input() shouldShow = false;

  get validationChecks(): ValidationCheck[] {
    return Object.keys(this.validationState).map(key => ({
      key,
      isValid: this.validationState[key as keyof ValidationState] ?? false,
      label: this.translate.instant(`${this.translationPrefix}${key.toUpperCase()}`)
    })).filter(check => check.isValid !== undefined);
  }
}
