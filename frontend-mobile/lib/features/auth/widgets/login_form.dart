import 'package:flutter/material.dart';
import 'package:easy_localization/easy_localization.dart';
import '../../../core/app_colors.dart';
import '../../../shared/widgets/auth_input_field.dart';

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

  @override
  void dispose() {
    _emailController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  void _openForgotPasswordModal() {
    showDialog(
      context: context,
      barrierDismissible: true,
      barrierColor: Colors.black.withOpacity(0.6),
      builder: (context) => const Center(
        child: Text(
          "Here would be modal for forgot password",
          style: TextStyle(color: Colors.white),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        // Email Field
        AuthInputField(
          label: 'LOGIN.EMAIL'.tr(),
          placeholder: 'LOGIN.EMAIL_PLACEHOLDER'.tr(),
          fieldName: 'email',
          type: 'email',
          controller: _emailController,
          onFocus: () => widget.onFieldFocus?.call('email'),
          onBlur: widget.onFieldBlur,
        ),

        const SizedBox(height: 16),

        // Password Field
        AuthInputField(
          label: 'LOGIN.PASSWORD'.tr(),
          placeholder: 'LOGIN.PASSWORD_PLACEHOLDER'.tr(),
          fieldName: 'password',
          type: 'password',
          controller: _passwordController,
          onFocus: () => widget.onFieldFocus?.call('password'),
          onBlur: widget.onFieldBlur,
        ),

        const SizedBox(height: 8),

        // Forgot Password
        Align(
          alignment: Alignment.centerRight,
          child: TextButton(
            onPressed: _openForgotPasswordModal,
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
        onPressed: widget.isLoading
            ? null
            : () => widget.onSubmit(_emailController.text, _passwordController.text),
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
