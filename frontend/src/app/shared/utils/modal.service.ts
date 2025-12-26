import { Injectable, Injector, inject } from '@angular/core';
import { Overlay, OverlayConfig, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal, ComponentType } from '@angular/cdk/portal';
import { Subject } from 'rxjs';

export interface ModalConfig<T = any> {
  data?: T;
  panelClass?: string | string[];
  hasBackdrop?: boolean;
  backdropClass?: string;
  disableClose?: boolean;
}

export class ModalRef<T = any, R = any> {
  private afterClosedSubject = new Subject<R | undefined>();
  afterClosed$ = this.afterClosedSubject.asObservable();

  constructor(private overlayRef: OverlayRef, public data?: T) {
    // Close on backdrop click
    this.overlayRef.backdropClick().subscribe(() => {
      if (!this.config?.disableClose) {
        this.close();
      }
    });

    // Close on ESC key
    this.overlayRef.keydownEvents().subscribe((event) => {
      if (event.key === 'Escape' && !this.config?.disableClose) {
        this.close();
      }
    });
  }

  config?: ModalConfig;

  close(result?: R): void {
    this.overlayRef.dispose();
    this.afterClosedSubject.next(result);
    this.afterClosedSubject.complete();
  }
}

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  private overlay = inject(Overlay);
  private injector = inject(Injector);

  open<T = any, R = any>(component: ComponentType<any>, config?: ModalConfig<T>): ModalRef<T, R> {
    const overlayConfig = this.getOverlayConfig(config);
    const overlayRef = this.overlay.create(overlayConfig);
    const modalRef = new ModalRef<T, R>(overlayRef, config?.data);
    modalRef.config = config;

    // Create custom injector with ModalRef
    const injector = Injector.create({
      parent: this.injector,
      providers: [{ provide: ModalRef, useValue: modalRef }],
    });

    // Attach component to overlay
    const portal = new ComponentPortal(component, null, injector);
    overlayRef.attach(portal);

    return modalRef;
  }

  private getOverlayConfig(config?: ModalConfig): OverlayConfig {
    const positionStrategy = this.overlay
      .position()
      .global()
      .centerHorizontally()
      .centerVertically();

    return new OverlayConfig({
      hasBackdrop: config?.hasBackdrop ?? true,
      backdropClass: config?.backdropClass ?? 'modal-backdrop',
      panelClass: config?.panelClass ?? 'modal-panel',
      scrollStrategy: this.overlay.scrollStrategies.block(),
      positionStrategy,
    });
  }
}
