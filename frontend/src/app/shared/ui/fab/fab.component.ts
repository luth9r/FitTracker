import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  standalone: true,
  selector: 'app-fab',
  imports: [CommonModule, MatIconModule],
  templateUrl: './fab.component.html',
  styleUrls: ['./fab.component.scss'],
})
export class FabComponent {
  @Input() icon: string = 'add';
  @Output() clicked = new EventEmitter<void>();
}
