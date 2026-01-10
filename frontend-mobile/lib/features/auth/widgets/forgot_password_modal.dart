import 'package:flutter/material.dart';
import 'package:easy_localization/easy_localization.dart';
import '../../../core/app_colors.dart';
import '../../../core/auth_input_styles.dart';
import '../../../services/auth_service.dart';
import '../../../services/notification_service.dart';

class ForgotPasswordModal extends StatefulWidget {
  const ForgotPasswordModal({super.key});

  @override
  State<ForgotPasswordModal> createState() => _ForgotPasswordModalState();
}

class _ForgotPasswordModalState extends State<ForgotPasswordModal> {
  final _authService = AuthService();
  final _notificationService = NotificationService();
  final _emailController = TextEditingController();

  bool _isSuccess = false;
  bool _isLoading = false;
  bool _touched = false;

  bool _validateEmail(String email) {
    return RegExp(r'^[^\s@]+@[^\s@]+\.[^\s@]+$').hasMatch(email);
  }

  bool get _hasError => _touched && (!_validateEmail(_emailController.text));

  Future<void> _onSubmit() async {
    setState(() => _touched = true);

    final email = _emailController.text.trim();
    if (email.isEmpty || !_validateEmail(email)) return;

    setState(() => _isLoading = true);

    try {
      await _authService.forgotPassword(email);
      if (mounted) {
        setState(() {
          _isSuccess = true;
          _isLoading = false;
        });

        Future.delayed(const Duration(seconds: 3), () {
          if (mounted) Navigator.pop(context);
        });
      }
    } catch (e) {
      if (mounted) {
        setState(() => _isLoading = false);
        _notificationService.showToast(e.toString(), isError: true);
      }
    }
  }

  @override
  void dispose() {
    _emailController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsets.only(
        left: 24,
        right: 24,
        top: 24,
        bottom: MediaQuery.of(context).viewInsets.bottom + 24,
      ),
      decoration: const BoxDecoration(
        color: AppColors.colorCardBg,
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      child: AnimatedSwitcher(
        duration: const Duration(milliseconds: 300),
        child: _isSuccess ? _buildSuccessContent() : _buildFormContent(),
      ),
    );
  }

  Widget _buildFormContent() {
    return Column(
      mainAxisSize: MainAxisSize.min,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            const SizedBox(width: 32),
            Container(
              padding: const EdgeInsets.all(12),
              decoration: BoxDecoration(
                color: AppColors.colorPrimary.withOpacity(0.1),
                borderRadius: BorderRadius.circular(12),
              ),
              child: const Icon(Icons.lock_outline, color: AppColors.colorPrimary, size: 28),
            ),
            IconButton(
              onPressed: () => Navigator.pop(context),
              icon: const Icon(Icons.close, color: AppColors.colorTextSecondary),
            ),
          ],
        ),
        const SizedBox(height: 16),
        Text(
          'LOGIN.FORGOTPASSWORD_TITLE'.tr(),
          style: const TextStyle(
              fontSize: 20,
              fontWeight: FontWeight.bold,
              color: AppColors.colorTextPrimary
          ),
        ),
        const SizedBox(height: 8),
        Text(
          'LOGIN.FORGOTPASSWORD_SUBTITLE'.tr(),
          textAlign: TextAlign.center,
          style: const TextStyle(fontSize: 14, color: AppColors.colorTextSecondary),
        ),
        const SizedBox(height: 24),

        TextField(
          controller: _emailController,
          keyboardType: TextInputType.emailAddress,
          onChanged: (_) => setState(() {}),
          style: const TextStyle(color: AppColors.colorTextPrimary),
          decoration: AuthInputStyles.authInputDecoration(
            labelText: 'LOGIN.EMAIL'.tr(),
            hintText: 'LOGIN.EMAIL_PLACEHOLDER'.tr(),
            errorText: _hasError ? 'LOGIN.EMAIL_INVALID'.tr() : null,
            hasError: _hasError,
            prefixIcon: Icon(
              Icons.email_outlined,
              color: _hasError ? AppColors.colorAccentDanger : AppColors.colorTextSecondary,
            ),
          ),
        ),

        const SizedBox(height: 24),
        SizedBox(
          width: double.infinity,
          height: 50,
          child: ElevatedButton(
            onPressed: _isLoading ? null : _onSubmit,
            style: ElevatedButton.styleFrom(
              backgroundColor: AppColors.colorPrimary,
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
              elevation: 0,
            ),
            child: _isLoading
                ? const SizedBox(
                width: 24,
                height: 24,
                child: CircularProgressIndicator(strokeWidth: 2, color: Colors.black)
            )
                : Text(
                'LOGIN.FORGOTPASSWORD_SUBMIT'.tr(),
                style: const TextStyle(color: Colors.white, fontWeight: FontWeight.bold)
            ),
          ),
        ),
        const SizedBox(height: 12),
        TextButton(
          onPressed: () => Navigator.pop(context),
          child: Text(
              'LOGIN.BACK_TO_LOGIN'.tr(),
              style: const TextStyle(color: AppColors.colorTextSecondary)
          ),
        ),
      ],
    );
  }

  Widget _buildSuccessContent() {
    return Column(
      mainAxisSize: MainAxisSize.min,
      children: [
        const Icon(Icons.check_circle_outline, color: Color(0xFF4CAF50), size: 64),
        const SizedBox(height: 16),
        Text(
          'LOGIN.FORGOTPASSWORD_SUCCESS_TITLE'.tr(),
          style: const TextStyle(fontSize: 20, fontWeight: FontWeight.bold, color: AppColors.colorTextPrimary),
        ),
        const SizedBox(height: 8),
        Text(
          'LOGIN.FORGOTPASSWORD_SUCCESS_TEXT'.tr(),
          textAlign: TextAlign.center,
          style: const TextStyle(fontSize: 14, color: AppColors.colorTextSecondary),
        ),
        const SizedBox(height: 32),
      ],
    );
  }
}
