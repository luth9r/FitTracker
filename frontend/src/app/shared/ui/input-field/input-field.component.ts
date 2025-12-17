import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-input-field',
  imports: [CommonModule],
  templateUrl: './input-field.component.html',
  styleUrls: ['./input-field.component.scss'],
})
export class InputFieldComponent {
  @Input() label?: string;
  @Input() placeholder?: string;
  @Input() type = 'text';
  @Input() name!: string;

  @Input() value = '';
  @Output() valueChange = new EventEmitter<string>();

  @Output() focus = new EventEmitter<string>();
  @Output() blur = new EventEmitter<void>();

  onInput(event: Event) {
    const target = event.target as HTMLInputElement;
    this.valueChange.emit(target.value);
  }

  onFocus() {
    this.focus.emit(this.name);
  }

  onBlur() {
    this.blur.emit();
  }
}
