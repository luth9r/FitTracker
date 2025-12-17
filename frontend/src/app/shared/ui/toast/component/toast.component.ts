import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { ToastService } from '../service/toast.service';

@Component({
  standalone: true,
  selector: 'app-toast-container',
  imports: [CommonModule, TranslateModule],
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss'],
})
export class ToastComponent {
  constructor(public toastService: ToastService) {}
}