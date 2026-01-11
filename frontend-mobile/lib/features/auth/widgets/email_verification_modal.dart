import 'package:easy_localization/easy_localization.dart';
import 'package:flutter/material.dart';

import '../../../core/app_colors.dart';
import '../../../services/auth_service.dart';
import '../../../services/error_service.dart';
import '../../../services/notification_service.dart';

class EmailVerificationModal extends StatefulWidget {
  final String email;

  const EmailVerificationModal({super.key, required this.email});

  @override
  State<EmailVerificationModal> createState() => _EmailVerificationModalState();
}

class _EmailVerificationModalState extends State<EmailVerificationModal> {
  final _authService = AuthService();
  final _notificationService = NotificationService();
  final _errorService = ErrorService.instance;

  int _countdown = 60;
  bool _canResend = false;
  bool _isResending = false;

  @override
  void initState() {
    super.initState();
    _startCountdown();
  }

  void _startCountdown() {
    setState(() {
      _canResend = false;
      _countdown = 60;
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

  Future<void> _handleResend() async {
    if (!_canResend || _isResending) return;

    setState(() => _isResending = true);

    try {
      await _authService.resendVerificationEmail(widget.email);

      if (mounted) {
        _notificationService.showToast('EmailVerification.ResendSuccess'.tr());
        _startCountdown();
      }
    } catch (e) {
      print('[ERROR] Resend verification failed: $e');

      if (mounted) {
        if (_errorService.isErrorCode(e, 'User.RateLimitExceeded')) {
          _notificationService.showToast(
            'Errors.User.RateLimitExceeded'.tr(),
            isError: true,
          );
        } else {
          _notificationService.showToast(
            _errorService.handleError(e),
            isError: true,
          );
        }
      }
    } finally {
      if (mounted) {
        setState(() => _isResending = false);
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
              color: AppColors.colorPrimary.withOpacity(0.15),
              borderRadius: BorderRadius.circular(20),
            ),
            child: const Icon(
              Icons.mark_email_read_outlined,
              size: 40,
              color: AppColors.colorPrimary,
            ),
          ),
          const SizedBox(height: 24),

          // Title
          Text(
            'Verification.Title'.tr(),
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
            'Verification.Message'.tr(),
            style: const TextStyle(
              fontSize: 15,
              color: AppColors.colorTextSecondary,
              height: 1.5,
            ),
            textAlign: TextAlign.center,
          ),
          const SizedBox(height: 16),

          // Email
          Container(
            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
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
                    widget.email,
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

          // Instructions
          Text(
            'Verification.Instruction'.tr(),
            style: const TextStyle(
              fontSize: 14,
              color: AppColors.colorTextSecondary,
              height: 1.5,
            ),
            textAlign: TextAlign.center,
          ),
          const SizedBox(height: 24),

          // Info box
          Container(
            padding: const EdgeInsets.all(16),
            decoration: BoxDecoration(
              color: AppColors.colorInputBg,
              borderRadius: BorderRadius.circular(12),
              border: Border.all(color: AppColors.colorBorderStrong, width: 1),
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
                      'Verification.DidntReceive'.tr(),
                      style: const TextStyle(
                        fontSize: 14,
                        fontWeight: FontWeight.w600,
                        color: AppColors.colorTextPrimary,
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 12),
                _buildCheckItem('Verification.CheckSpam'.tr()),
                _buildCheckItem('Verification.WaitMinutes'.tr()),
                _buildCheckItem('Verification.CheckEmailCorrect'.tr()),
              ],
            ),
          ),
          const SizedBox(height: 24),

          // Resend button
          SizedBox(
            width: double.infinity,
            height: 52,
            child: ElevatedButton(
              onPressed: (_canResend && !_isResending) ? _handleResend : null,
              style: ElevatedButton.styleFrom(
                backgroundColor: _canResend
                    ? AppColors.colorPrimary
                    : AppColors.colorInputBg,
                disabledBackgroundColor: AppColors.colorInputBg,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12),
                ),
                elevation: 0,
              ),
              child: _isResending
                  ? const SizedBox(
                      width: 22,
                      height: 22,
                      child: CircularProgressIndicator(
                        strokeWidth: 2.5,
                        color: Colors.white,
                      ),
                    )
                  : Text(
                      _canResend
                          ? 'Verification.ResendButton'.tr()
                          : 'Verification.ResendCountdown'.tr(
                              args: ['$_countdown'],
                            ),
                      style: TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.w600,
                        color: _canResend
                            ? Colors.black
                            : AppColors.colorTextSecondary,
                      ),
                    ),
            ),
          ),
          const SizedBox(height: 12),

          // Close button
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
                'Verification.CloseButton'.tr(),
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
