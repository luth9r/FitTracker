import 'dart:async';
import 'package:flutter/material.dart';
import 'package:easy_localization/easy_localization.dart';
import '../../../core/app_colors.dart';
import '../../../services/auth_service.dart';
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

  Timer? _timer;
  int _countdown = 60;
  bool _canResend = false;
  bool _isResending = false;

  @override
  void initState() {
    super.initState();
    _startTimer();
  }

  @override
  void dispose() {
    _timer?.cancel();
    super.dispose();
  }

  void _startTimer() {
    setState(() {
      _canResend = false;
      _countdown = 60;
    });
    _timer = Timer.periodic(const Duration(seconds: 1), (timer) {
      if (_countdown > 0) {
        setState(() => _countdown--);
      } else {
        setState(() => _canResend = true);
        _timer?.cancel();
      }
    });
  }

  Future<void> _handleResend() async {
    if (!_canResend || _isResending) return;

    setState(() => _isResending = true);

    try {
      await _authService.resendVerificationEmail(widget.email);
      _notificationService.showToast('VERIFICATION.RESEND_SUCCESS'.tr());
      _startTimer();
    } catch (e) {
      _notificationService.showToast(e.toString(), isError: true);
    } finally {
      if (mounted) setState(() => _isResending = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(24),
      decoration: const BoxDecoration(
        color: AppColors.colorCardBg,
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          _buildHeader(),
          const SizedBox(height: 32),
          _buildBody(),
          const SizedBox(height: 32),
          _buildFooter(),
        ],
      ),
    );
  }

  Widget _buildHeader() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Text(
          'VERIFICATION.TITLE'.tr(),
          style: const TextStyle(
            fontSize: 18,
            fontWeight: FontWeight.bold,
            color: AppColors.colorTextPrimary,
          ),
        ),
        IconButton(
          onPressed: () => Navigator.pop(context),
          icon: const Icon(Icons.close, color: AppColors.colorTextSecondary),
          visualDensity: VisualDensity.compact,
        ),
      ],
    );
  }

  Widget _buildBody() {
    return Column(
      children: [
        Container(
          padding: const EdgeInsets.all(16),
          decoration: BoxDecoration(
            color: AppColors.colorPrimary.withOpacity(0.1),
            borderRadius: BorderRadius.circular(12),
          ),
          child: const Icon(
            Icons.email_outlined,
            color: AppColors.colorPrimary,
            size: 40,
          ),
        ),
        const SizedBox(height: 24),
        Text(
          'VERIFICATION.MESSAGE'.tr(),
          style: const TextStyle(
            fontSize: 16,
            fontWeight: FontWeight.w600,
            color: AppColors.colorTextPrimary,
          ),
        ),
        const SizedBox(height: 8),
        Text(
          widget.email,
          style: const TextStyle(
            fontSize: 14,
            fontWeight: FontWeight.bold,
            color: AppColors.colorPrimary,
          ),
        ),
        const SizedBox(height: 16),
        Text(
          'VERIFICATION.INSTRUCTION'.tr(),
          textAlign: TextAlign.center,
          style: const TextStyle(
            fontSize: 14,
            color: AppColors.colorTextSecondary,
            height: 1.5,
          ),
        ),
        if (!_canResend) ...[
          const SizedBox(height: 24),
          Container(
            padding: const EdgeInsets.all(12),
            decoration: BoxDecoration(
              color: AppColors.colorPrimary.withOpacity(0.05),
              border: const Border(
                left: BorderSide(color: AppColors.colorPrimary, width: 3),
              ),
              borderRadius: BorderRadius.circular(8),
            ),
            child: Text(
              'VERIFICATION.WAIT_MESSAGE'.tr(args: [_countdown.toString()]),
              style: const TextStyle(
                fontSize: 14,
                color: AppColors.colorTextPrimary,
                fontWeight: FontWeight.w500,
              ),
            ),
          ),
        ],
      ],
    );
  }

  Widget _buildFooter() {
    return Column(
      children: [
        SizedBox(
          width: double.infinity,
          height: 48,
          child: ElevatedButton(
            onPressed: _canResend && !_isResending ? _handleResend : null,
            style: ElevatedButton.styleFrom(
              backgroundColor: AppColors.colorPrimary,
              disabledBackgroundColor: AppColors.colorInputBg,
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
              elevation: 0,
            ),
            child: _isResending
                ? const SizedBox(
              width: 20,
              height: 20,
              child: CircularProgressIndicator(strokeWidth: 2, color: Colors.white),
            )
                : Text(
              _isResending ? 'VERIFICATION.RESENDING'.tr() : 'VERIFICATION.RESEND_BUTTON'.tr(),
              style: TextStyle(
                color: _canResend ? Colors.black : AppColors.colorTextSecondary,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
        ),
        const SizedBox(height: 12),
        SizedBox(
          width: double.infinity,
          height: 48,
          child: OutlinedButton(
            onPressed: () => Navigator.pop(context),
            style: OutlinedButton.styleFrom(
              side: const BorderSide(color: AppColors.colorBorderStrong),
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
            ),
            child: Text(
              'VERIFICATION.CLOSE_BUTTON'.tr(),
              style: const TextStyle(color: AppColors.colorTextSecondary),
            ),
          ),
        ),
      ],
    );
  }
}