import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

export type NavItemKey = 'home' | 'exercises' | 'progress' | 'profile';

@Component({
  standalone: true,
  selector: 'app-bottom-nav',
  imports: [CommonModule, MatIconModule],
  templateUrl: './bottom-nav.component.html',
  styleUrls: ['./bottom-nav.component.scss']
})
export class BottomNavComponent {
  @Input() active: NavItemKey = 'home';
  @Output() navigate = new EventEmitter<NavItemKey>();

  constructor(private readonly router: Router) {}

  navigateTo(section: 'home' | 'exercises' | 'progress' | 'profile'): void {
    if (section === this.active) {
      return;
    }

    switch (section) {
      case 'home':
        this.router.navigate(['/']);
        break;
      case 'exercises':
        this.router.navigate(['/exercises']);
        break;
      case 'progress':
        this.router.navigate(['/progress']);
        break;
      case 'profile':
        this.router.navigate(['/profile']);
        break;
    }
  }
}
