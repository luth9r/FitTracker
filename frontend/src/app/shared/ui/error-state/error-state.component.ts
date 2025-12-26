import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-error-state',
  standalone: true,
  imports: [CommonModule, MatIconModule, TranslateModule],
  templateUrl: './error-state.component.html',
  styleUrl: './error-state.component.scss',
})
export class ErrorStateComponent {
  /**
   * Icon to display (Material icon name)
   */
  @Input() icon: string = 'cloud_off';

  /**
   * Translation key for title
   */
  @Input() titleKey: string = 'COMMON.ERROR_STATE.TITLE';

  /**
   * Translation key for description
   */
  @Input() descriptionKey: string = 'COMMON.ERROR_STATE.DESCRIPTION';

  /**
   * Translation key for retry button
   */
  @Input() retryButtonKey: string = 'COMMON.ERROR_STATE.RETRY';

  /**
   * Show or hide retry button
   */
  @Input() showRetryButton: boolean = true;

  /**
   * Event emitted when retry button is clicked
   */
  @Output() retryClicked = new EventEmitter<void>();

  onRetry(): void {
    this.retryClicked.emit();
  }
}
