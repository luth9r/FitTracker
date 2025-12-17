import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  standalone: true,
  selector: 'app-page-header',
  imports: [CommonModule, MatIconModule, TranslateModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class PageHeaderComponent {
  @Input() icon = 'fitness_center';
  @Input() title!: string;
  @Input() subtitle!: string;
}
