import 'package:easy_localization/easy_localization.dart';
import 'package:flutter/material.dart';

import '../../../core/app_colors.dart';
import '../../../services/auth_service.dart';
import '../../../services/error_service.dart';
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
  final _errorService = ErrorService.instance;

  bool _isSending = false;
  bool _emailSent = false;
  bool _touched = false;
  int _countdown = 60;
  bool _canResend = true;

  @override
  void dispose() {
    _emailController.dispose();
    super.dispose();
  }

  bool _validateEmail(String email) {
    return RegExp(r'^[^\s@]+@[^\s@]+\.[^\s@]+$').hasMatch(email);
  }

  bool get _hasError =>
      _touched && !_validateEmail(_emailController.text.trim());

  void _startCountdown() {
    setState(() {
      _countdown = 60;
      _canResend = false;
    });

    Future.doWhile(() async {
      await Future.delayed(const Duration(seconds: 1));
      if (!mounted) return false;

      setState(() {
        _countdown--;
      });

      if (_countdown <= 0) {
        setState(() {
          _canResend = true;
        });
        return false;
      }
      return true;
    });
  }

  Future<void> _sendResetEmail() async {
    if (_emailSent && !_canResend) return;

    setState(() => _touched = true);

    final email = _emailController.text.trim();
    if (email.isEmpty || !_validateEmail(email)) {
      _notificationService.showToast('Login.EmailInvalid'.tr(), isError: true);
      return;
    }

    setState(() => _isSending = true);

    try {
      await _authService.forgotPassword(email);

      if (mounted) {
        setState(() {
          _emailSent = true;
          _isSending = false;
        });

        _notificationService.showToast('ForgotPassword.EmailSent'.tr());
        _startCountdown();
      }
    } catch (e) {
      print('[ERROR] Forgot password failed: $e');

      if (mounted) {
        setState(() => _isSending = false);

        if (_errorService.isErrorCode(e, 'User.RateLimitExceeded')) {
          _notificationService.showToast(
            'Errors.User.RateLimitExceeded'.tr(),
            isError: true,
          );
        } else if (_errorService.isErrorCode(e, 'User.NotFound')) {
          setState(() {
            _emailSent = true;
          });
          _notificationService.showToast('ForgotPassword.EmailSent'.tr());
          _startCountdown();
        } else {
          _notificationService.showToast(
            _errorService.handleError(e),
            isError: true,
          );
        }
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: const BoxDecoration(
        color: AppColors.colorCardBg,
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      padding: EdgeInsets.only(
        top: 24,
        left: 24,
        right: 24,
        bottom: MediaQuery.of(context).viewInsets.bottom + 24,
      ),
      child: SingleChildScrollView(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            // Drag indicator
            Container(
              width: 40,
              height: 4,
              decoration: BoxDecoration(
                color: AppColors.colorTextSecondary.withOpacity(0.3),
                borderRadius: BorderRadius.circular(2),
              ),
            ),
            const SizedBox(height: 24),

            // Icon
            Container(
              width: 80,
              height: 80,
              decoration: BoxDecoration(
                color: _emailSent
                    ? AppColors.colorPrimary.withOpacity(0.15)
                    : AppColors.colorPrimary.withOpacity(0.1),
                borderRadius: BorderRadius.circular(20),
              ),
              child: Icon(
                _emailSent
                    ? Icons.mark_email_read_outlined
                    : Icons.lock_reset_outlined,
                size: 40,
                color: AppColors.colorPrimary,
              ),
            ),
            const SizedBox(height: 24),

            // Title
            Text(
              _emailSent
                  ? 'ForgotPassword.TitleSent'.tr()
                  : 'ForgotPassword.Title'.tr(),
              style: const TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.bold,
                color: AppColors.colorTextPrimary,
              ),
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 12),

            // Description
            Text(
              _emailSent
                  ? 'ForgotPassword.DescriptionSent'.tr()
                  : 'ForgotPassword.Description'.tr(),
              style: const TextStyle(
                fontSize: 15,
                color: AppColors.colorTextSecondary,
                height: 1.5,
              ),
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 24),

            if (!_emailSent) ...[
              TextField(
                controller: _emailController,
                keyboardType: TextInputType.emailAddress,
                onChanged: (_) => setState(() {}),
                style: const TextStyle(color: AppColors.colorTextPrimary),
                decoration: InputDecoration(
                  labelText: 'Login.Email'.tr(),
                  hintText: 'Login.EmailPlaceholder'.tr(),
                  hintStyle: TextStyle(
                    color: AppColors.colorTextSecondary.withOpacity(0.5),
                  ),
                  errorText: _hasError ? 'Login.EmailInvalid'.tr() : null,
                  prefixIcon: Icon(
                    Icons.email_outlined,
                    color: _hasError
                        ? AppColors.colorAccentDanger
                        : AppColors.colorTextSecondary,
                  ),
                  filled: true,
                  fillColor: AppColors.colorInputBg,
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12),
                    borderSide: BorderSide(
                      color: _hasError
                          ? AppColors.colorAccentDanger
                          : AppColors.colorBorderStrong,
                    ),
                  ),
                  enabledBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12),
                    borderSide: BorderSide(
                      color: _hasError
                          ? AppColors.colorAccentDanger
                          : AppColors.colorBorderStrong,
                    ),
                  ),
                  focusedBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12),
                    borderSide: BorderSide(
                      color: _hasError
                          ? AppColors.colorAccentDanger
                          : AppColors.colorPrimary,
                      width: 2,
                    ),
                  ),
                  errorBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12),
                    borderSide: const BorderSide(
                      color: AppColors.colorAccentDanger,
                    ),
                  ),
                  focusedErrorBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(12),
                    borderSide: const BorderSide(
                      color: AppColors.colorAccentDanger,
                      width: 2,
                    ),
                  ),
                ),
              ),
              const SizedBox(height: 24),
            ],

            if (_emailSent) ...[
              Container(
                padding: const EdgeInsets.symmetric(
                  horizontal: 16,
                  vertical: 12,
                ),
                decoration: BoxDecoration(
                  color: AppColors.colorInputBg,
                  borderRadius: BorderRadius.circular(12),
                  border: Border.all(
                    color: AppColors.colorBorderStrong.withOpacity(0.5),
                    width: 1,
                  ),
                ),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    const Icon(
                      Icons.email,
                      size: 18,
                      color: AppColors.colorPrimary,
                    ),
                    const SizedBox(width: 8),
                    Flexible(
                      child: Text(
                        _emailController.text.trim(),
                        style: const TextStyle(
                          fontSize: 15,
                          fontWeight: FontWeight.w600,
                          color: AppColors.colorTextPrimary,
                        ),
                        textAlign: TextAlign.center,
                        overflow: TextOverflow.ellipsis,
                      ),
                    ),
                  ],
                ),
              ),
              const SizedBox(height: 24),

              // Info box
              Container(
                padding: const EdgeInsets.all(16),
                decoration: BoxDecoration(
                  color: AppColors.colorInputBg,
                  borderRadius: BorderRadius.circular(12),
                  border: Border.all(
                    color: AppColors.colorBorderStrong,
                    width: 1,
                  ),
                ),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      children: [
                        Icon(
                          Icons.info_outline,
                          size: 20,
                          color: AppColors.colorPrimary,
                        ),
                        const SizedBox(width: 8),
                        Text(
                          'ForgotPassword.DidntReceive'.tr(),
                          style: const TextStyle(
                            fontSize: 14,
                            fontWeight: FontWeight.w600,
                            color: AppColors.colorTextPrimary,
                          ),
                        ),
                      ],
                    ),
                    const SizedBox(height: 12),
                    _buildCheckItem('ForgotPassword.CheckSpam'.tr()),
                    _buildCheckItem('ForgotPassword.WaitMinutes'.tr()),
                    _buildCheckItem('ForgotPassword.CheckEmailCorrect'.tr()),
                  ],
                ),
              ),
              const SizedBox(height: 24),
            ],

            // Main action button
            SizedBox(
              width: double.infinity,
              height: 52,
              child: ElevatedButton(
                onPressed: _emailSent
                    ? (_canResend && !_isSending ? _sendResetEmail : null)
                    : (_isSending ? null : _sendResetEmail),
                style: ElevatedButton.styleFrom(
                  backgroundColor: _emailSent
                      ? (_canResend
                            ? AppColors.colorPrimary
                            : AppColors.colorInputBg)
                      : AppColors.colorPrimary,
                  disabledBackgroundColor: AppColors.colorInputBg,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12),
                  ),
                  elevation: 0,
                ),
                child: _isSending
                    ? const SizedBox(
                        width: 22,
                        height: 22,
                        child: CircularProgressIndicator(
                          strokeWidth: 2.5,
                          color: Colors.white,
                        ),
                      )
                    : Text(
                        _emailSent
                            ? (_canResend
                                  ? 'ForgotPassword.ResendEmail'.tr()
                                  : 'ForgotPassword.ResendCountdown'.tr(
                                      args: ['$_countdown'],
                                    ))
                            : 'ForgotPassword.SendEmail'.tr(),
                        style: TextStyle(
                          fontSize: 16,
                          fontWeight: FontWeight.w600,
                          color: (_emailSent && !_canResend)
                              ? AppColors.colorTextSecondary
                              : Colors.black,
                        ),
                      ),
              ),
            ),
            const SizedBox(height: 12),

            // Secondary button
            SizedBox(
              width: double.infinity,
              height: 48,
              child: TextButton(
                onPressed: () => Navigator.pop(context),
                style: TextButton.styleFrom(
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12),
                  ),
                ),
                child: Text(
                  _emailSent
                      ? 'ForgotPassword.Close'.tr()
                      : 'ForgotPassword.Cancel'.tr(),
                  style: const TextStyle(
                    fontSize: 15,
                    fontWeight: FontWeight.w500,
                    color: AppColors.colorTextSecondary,
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildCheckItem(String text) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 8, left: 4),
      child: Row(
        children: [
          Container(
            width: 6,
            height: 6,
            decoration: BoxDecoration(
              color: AppColors.colorTextSecondary,
              shape: BoxShape.circle,
            ),
          ),
          const SizedBox(width: 10),
          Expanded(
            child: Text(
              text,
              style: const TextStyle(
                fontSize: 13,
                color: AppColors.colorTextSecondary,
                height: 1.4,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
