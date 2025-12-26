import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService, LoginResponse } from '../../services/auth.service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  imports: [TranslateModule, CommonModule],
  styleUrls: ['./verify-email.component.scss']
})
export class VerifyEmailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private translate = inject(TranslateService);

  isLoading = true;
  isSuccess = false;
  errorMessage = '';
  statusTitle = '';
  statusSubtitle = '';
  userName?: string;

  ngOnInit(): void {
    // Set initial loading state
    this.translate.get('VERIFY_EMAIL.VERIFYING_TITLE').subscribe(title => {
      this.statusTitle = title;
    });
    this.translate.get('VERIFY_EMAIL.PLEASE_WAIT').subscribe(subtitle => {
      this.statusSubtitle = subtitle;
    });

    this.route.queryParams.subscribe(params => {
      const token = params['token'];
      
      if (!token) {
        this.translate.get('VERIFY_EMAIL.ERROR_NO_TOKEN').subscribe(msg => {
          this.handleError(msg);
        });
        return;
      }

      this.verifyEmail(token);
    });
  }

  verifyEmail(token: string): void {
    this.authService.verifyEmail(token).subscribe({
      next: (response: LoginResponse) => {
        this.isLoading = false;
        this.isSuccess = true;
        this.userName = response.username;
        
        this.translate.get('VERIFY_EMAIL.SUCCESS_TITLE').subscribe(title => {
          this.statusTitle = title;
        });
        this.translate.get('VERIFY_EMAIL.SUCCESS_SUBTITLE').subscribe(subtitle => {
          this.statusSubtitle = subtitle;
        });
        
        // Store user info if needed
        localStorage.setItem('user', JSON.stringify({
          username: response.username,
          email: response.email
        }));
        
        // Redirect to dashboard after 2 seconds
        setTimeout(() => {
          this.router.navigate(['/dashboard']);
        }, 2000);
      },
      error: (error: { message: string; status: number }) => {
        this.handleError(error.message);
      }
    });
  }

  handleError(message: string): void {
    this.isLoading = false;
    this.isSuccess = false;
    this.errorMessage = message;
    
    this.translate.get('VERIFY_EMAIL.ERROR_TITLE').subscribe(title => {
      this.statusTitle = title;
    });
    this.translate.get('VERIFY_EMAIL.ERROR_SUBTITLE').subscribe(subtitle => {
      this.statusSubtitle = subtitle;
    });
  }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }

  navigateToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }
}
