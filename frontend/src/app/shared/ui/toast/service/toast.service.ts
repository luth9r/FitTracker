import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type ToastType = 'success' | 'error';

export interface ToastMessage {
  type: ToastType;
  textKey: string;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  private _toast$ = new BehaviorSubject<ToastMessage | null>(null);
  toast$ = this._toast$.asObservable();

  private hideTimeoutId: any = null;

  show(type: ToastType, textKey: string, durationMs = 3000): void {
    if (this.hideTimeoutId) {
      clearTimeout(this.hideTimeoutId);
      this.hideTimeoutId = null;
    }

    this._toast$.next({ type, textKey });

    if (durationMs > 0) {
      this.hideTimeoutId = setTimeout(() => {
        this._toast$.next(null);
        this.hideTimeoutId = null;
      }, durationMs);
    }
  }

  clear(): void {
    if (this.hideTimeoutId) {
      clearTimeout(this.hideTimeoutId);
      this.hideTimeoutId = null;
    }
    this._toast$.next(null);
  }
}