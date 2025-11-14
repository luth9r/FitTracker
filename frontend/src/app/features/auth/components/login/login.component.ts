import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    TranslateModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  email = '';
  password = '';
  confirmPassword = '';
  showPassword = false;
  showConfirmPassword = false;
  isLogin = true;

  toggleMode() {
    this.isLogin = !this.isLogin;
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  handleSubmit() {
    if (!this.isLogin && this.password !== this.confirmPassword) {
      alert('Пароли не совпадают!');
      return;
    }

    console.log('Форма отправлена:', {
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword,
      mode: this.isLogin ? 'login' : 'signup'
    });
    alert(`${this.isLogin ? 'Вход' : 'Регистрация'} выполнена!\nEmail: ${this.email}`);
  }
}