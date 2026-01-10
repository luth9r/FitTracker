import 'package:flutter/material.dart';
import 'package:easy_localization/easy_localization.dart';
import '../../../core/app_colors.dart';
import '../../../core/auth_input_styles.dart';
import '../../../services/notification_service.dart';
import 'forgot_password_modal.dart';

class LoginForm extends StatefulWidget {
  final bool isLoading;
  final Function(String email, String password) onSubmit;
  final Function(String fieldName)? onFieldFocus;
  final VoidCallback? onFieldBlur;

  const LoginForm({
    super.key,
    required this.isLoading,
    required this.onSubmit,
    this.onFieldFocus,
    this.onFieldBlur,
  });

  @override
  State<LoginForm> createState() => _LoginFormState();
}

class _LoginFormState extends State<LoginForm> {
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _emailFocusNode = FocusNode();
  final _passwordFocusNode = FocusNode();
  final _notificationService = NotificationService();

  bool _emailTouched = false;
  bool _passwordTouched = false;
  bool _obscurePassword = true;

  @override
  void initState() {
    super.initState();
    _emailFocusNode.addListener(_handleEmailFocusChange);
    _passwordFocusNode.addListener(_handlePasswordFocusChange);
  }

  @override
  void dispose() {
    _emailController.dispose();
    _passwordController.dispose();
    _emailFocusNode.removeListener(_handleEmailFocusChange);
    _passwordFocusNode.removeListener(_handlePasswordFocusChange);
    _emailFocusNode.dispose();
    _passwordFocusNode.dispose();
    super.dispose();
  }

  void _handleEmailFocusChange() {
    if (_emailFocusNode.hasFocus) {
      widget.onFieldFocus?.call('email');
    } else {
      setState(() => _emailTouched = true);
      widget.onFieldBlur?.call();
    }
  }

  void _handlePasswordFocusChange() {
    if (_passwordFocusNode.hasFocus) {
      widget.onFieldFocus?.call('password');
    } else {
      setState(() => _passwordTouched = true);
      widget.onFieldBlur?.call();
    }
  }

  bool _validateEmail(String email) {
    return RegExp(r'^[^\s@]+@[^\s@]+\.[^\s@]+$').hasMatch(email);
  }

  bool _validatePassword(String password) {
    return password.isNotEmpty;
  }

  bool get _hasEmailError => _emailTouched && !_validateEmail(_emailController.text);
  bool get _hasPasswordError => _passwordTouched && !_validatePassword(_passwordController.text);

  void _handleSubmit() {
    setState(() {
      _emailTouched = true;
      _passwordTouched = true;
    });

    final email = _emailController.text.trim();
    final password = _passwordController.text.trim();

    if (email.isEmpty || password.isEmpty) {
      _notificationService.showToast(
          'LOGIN.ERRORS.EMPTY_FIELDS'.tr(),
          isError: true
      );
      return;
    }
    
    if (!_validateEmail(email) || !_validatePassword(password)) {
      _notificationService.showToast(
          'LOGIN.ERRORS.INVALID_FIELDS'.tr(),
          isError: true
      );
      return;
    }
    
    widget.onSubmit(email, password);
  }

  void _showForgotPasswordModal() {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (context) => const ForgotPasswordModal(),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        // Email Field
        TextField(
          controller: _emailController,
          focusNode: _emailFocusNode,
          keyboardType: TextInputType.emailAddress,
          onChanged: (_) => setState(() {}),
          style: const TextStyle(color: AppColors.colorTextPrimary),
          decoration: AuthInputStyles.authInputDecoration(
            labelText: 'LOGIN.EMAIL'.tr(),
            hintText: 'LOGIN.EMAIL_PLACEHOLDER'.tr(),
            errorText: _hasEmailError ? 'LOGIN.EMAIL_INVALID'.tr() : null,
            hasError: _hasEmailError,
            prefixIcon: Icon(
              Icons.email_outlined,
              color: _hasEmailError ? AppColors.colorAccentDanger : AppColors.colorTextSecondary,
            ),
          ),
        ),

        const SizedBox(height: 16),

        // Password Field
        TextField(
          controller: _passwordController,
          focusNode: _passwordFocusNode,
          obscureText: _obscurePassword,
          onChanged: (_) => setState(() {}),
          style: const TextStyle(color: AppColors.colorTextPrimary),
          decoration: AuthInputStyles.authInputDecoration(
            labelText: 'LOGIN.PASSWORD'.tr(),
            hintText: 'LOGIN.PASSWORD_PLACEHOLDER'.tr(),
            errorText: _hasPasswordError ? 'LOGIN.PASSWORD_REQUIRED'.tr() : null,
            hasError: _hasPasswordError,
            prefixIcon: Icon(
              Icons.lock_outline,
              color: _hasPasswordError ? AppColors.colorAccentDanger : AppColors.colorTextSecondary,
            ),
            suffixIcon: IconButton(
              icon: Icon(
                _obscurePassword ? Icons.visibility_outlined : Icons.visibility_off_outlined,
                color: AppColors.colorTextSecondary,
              ),
              onPressed: () => setState(() => _obscurePassword = !_obscurePassword),
            ),
          ),
        ),

        const SizedBox(height: 8),

        // Forgot Password
        Align(
          alignment: Alignment.centerRight,
          child: TextButton(
            onPressed: _showForgotPasswordModal,
            style: TextButton.styleFrom(
              foregroundColor: AppColors.colorPrimary,
              padding: EdgeInsets.zero,
              minimumSize: const Size(0, 0),
              tapTargetSize: MaterialTapTargetSize.shrinkWrap,
              textStyle: const TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.w500,
              ),
            ),
            child: Text('LOGIN.FORGOT_PASSWORD'.tr()),
          ),
        ),

        const SizedBox(height: 24),

        _buildSubmitButton(),
      ],
    );
  }

  Widget _buildSubmitButton() {
    return SizedBox(
      width: double.infinity,
      height: 44,
      child: ElevatedButton(
        onPressed: widget.isLoading ? null : _handleSubmit,
        style: ElevatedButton.styleFrom(
          backgroundColor: AppColors.colorPrimary,
          foregroundColor: AppColors.colorTextPrimary,
          disabledBackgroundColor: AppColors.colorPrimary.withOpacity(0.6),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12),
          ),
          elevation: 0,
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 12),
        ),
        child: widget.isLoading
            ? const SizedBox(
          height: 20,
          width: 20,
          child: CircularProgressIndicator(
            strokeWidth: 2,
            color: Colors.white,
          ),
        )
            : Text(
          'LOGIN.SUBMIT_BUTTON_LOGIN'.tr(),
          style: const TextStyle(
            fontSize: 14,
            fontWeight: FontWeight.w500,
          ),
        ),
      ),
    );
  }
}
