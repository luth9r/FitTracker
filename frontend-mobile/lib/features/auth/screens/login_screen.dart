import 'package:flutter/material.dart';
import 'package:easy_localization/easy_localization.dart';
import 'package:flutter_svg/flutter_svg.dart';
import '../../../services/auth_service.dart';
import '../../../services/notification_service.dart';
import '../../../services/validation_service.dart';
import '../../../core/app_colors.dart';
import '../widgets/login_form.dart';
import '../widgets/register_form.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _authService = AuthService();
  final _validator = ValidationService.instance;
  final _notificationService = NotificationService();

  bool _isGoogleLoading = false;
  bool _isFormLoading = false;
  String _authMode = 'login';
  String _currentPassword = '';
  String _currentConfirmPassword = '';

  Map<String, Map<String, bool>> registrationsValidations = {
    'username': {'minLength': false, 'noSpaces': false},
    'email': {'isValid': false},
    'password': {'minLength': false, 'oneLetter': false, 'oneNumber': false},
    'confirmPassword': {'matches': false},
  };

  @override
  void initState() {
    super.initState();
  }

  void _onValidateChange(String field, String value) {
    setState(() {
      switch (field) {
        case 'username':
          registrationsValidations['username'] = _validator.validateUsername(value);
          break;
        case 'email':
          registrationsValidations['email'] = _validator.validateEmail(value);
          break;
        case 'password':
          _currentPassword = value;
          registrationsValidations['password'] = _validator.validatePassword(value);
          registrationsValidations['confirmPassword'] =
              _validator.validateConfirm(_currentPassword, _currentConfirmPassword);
          break;
        case 'confirmPassword':
          _currentConfirmPassword = value;
          registrationsValidations['confirmPassword'] =
              _validator.validateConfirm(_currentPassword, value);
          break;
      }
    });
  }

  void _toggleMode(String mode) {
    setState(() {
      _authMode = mode;
      _currentPassword = '';
      _currentConfirmPassword = '';
      registrationsValidations.forEach((k, v) =>
      registrationsValidations[k] = v.map((key, value) => MapEntry(key, false))
      );
    });
  }

  Future<void> _onLoginSubmit(String email, String password) async {
    setState(() => _isFormLoading = true);
    try {
      final response = await _authService.login(email, password);
      _notificationService.showToast(
          'LOGIN.SUCCESS.LOGIN'.tr() + ' Welcome back, ${response.username}!'
      );

      Future.delayed(const Duration(milliseconds: 500), () {
        if (mounted) {
          Navigator.pushReplacementNamed(context, '/');
        }
      });
    } catch (e) {
      print('[ERROR] Login failed: $e');
      _notificationService.showToast(_getLocalizedError(e), isError: true);
    } finally {
      if (mounted) {
        setState(() => _isFormLoading = false);
      }
    }
  }

  Future<void> _onRegisterSubmit(Map<String, String> data) async {
    setState(() => _isFormLoading = true);
    try {
      final payload = RegisterPayload(
        username: data['username']!,
        email: data['email']!,
        password: data['password']!,
      );

      await _authService.register(payload);
      _showEmailVerificationModal(data['email']!);
      _notificationService.showToast('LOGIN.SUCCESS.REGISTER'.tr());
    } catch (e) {
      print('[ERROR] Registration failed: $e');
      _notificationService.showToast(_getLocalizedError(e), isError: true);
    } finally {
      if (mounted) {
        setState(() => _isFormLoading = false);
      }
    }
  }

  Future<void> _handleGoogleSignIn() async {
    if (_isGoogleLoading || _isFormLoading) return;

    setState(() => _isGoogleLoading = true);

    try {
      print('[INFO] Starting Native Google Sign-In...');

      final response = await _authService.signInWithGoogleNative();

      print('[SUCCESS] Google Auth successful. User: ${response.username}');

      _notificationService.showToast('Welcome, ${response.username}!');

      if (mounted) {
        Future.delayed(const Duration(milliseconds: 500), () {
          Navigator.pushReplacementNamed(context, '/');
        });
      }

    } catch (e) {
      print('[ERROR] Google Sign-In failed: $e');

      if (e.toString().contains('cancelled')) {
        print('[INFO] User cancelled Google Sign-In');
        return;
      }

      _notificationService.showToast(_getLocalizedError(e), isError: true);
    } finally {
      if (mounted) {
        setState(() => _isGoogleLoading = false);
      }
    }
  }

  String _getLocalizedError(dynamic error) {
    final errorString = error.toString().toLowerCase();

    if (errorString.contains('socketexception') ||
        errorString.contains('network') ||
        errorString.contains('connection')) {
      return 'LOGIN.ERRORS.API_UNREACHABLE'.tr();
    }

    if (errorString.contains('google')) {
      if (errorString.contains('config')) {
        return 'LOGIN.ERRORS.GOOGLE_CONFIG_ERROR'.tr();
      }
      return 'LOGIN.ERRORS.UNKNOWN_GOOGLE'.tr();
    }

    if (errorString.contains('password')) {
      if (errorString.contains('match')) {
        return 'LOGIN.ERRORS.PASSWORDS_DO_NOT_MATCH'.tr();
      }
      if (errorString.contains('weak') || errorString.contains('length')) {
        return 'LOGIN.ERRORS.PASSWORD_MIN_LENGTH'.tr();
      }
      if (errorString.contains('letter')) {
        return 'LOGIN.ERRORS.PASSWORD_ONE_LETTER'.tr();
      }
      if (errorString.contains('number')) {
        return 'LOGIN.ERRORS.PASSWORD_ONE_NUMBER'.tr();
      }
    }

    if (errorString.contains('username') && errorString.contains('short')) {
      return 'LOGIN.ERRORS.USERNAME_TOO_SHORT'.tr();
    }

    if (errorString.contains('email') && errorString.contains('invalid')) {
      return 'LOGIN.EMAIL_INVALID'.tr();
    }

    if (errorString.contains('validation')) {
      return 'LOGIN.ERRORS.VALIDATION_FAILED'.tr();
    }

    if (errorString.contains('empty') || errorString.contains('required')) {
      return 'LOGIN.ERRORS.EMPTY_FIELDS'.tr();
    }

    String cleanError = error.toString().replaceAll('Exception: ', '');

    if (cleanError.length < 100 && !cleanError.contains('Error:')) {
      return cleanError;
    }

    return 'LOGIN.ERRORS.UNKNOWN'.tr();
  }

  void _showEmailVerificationModal(String email) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: AppColors.colorCardBg,
        title: Text(
          'VERIFICATION.TITLE'.tr(),
          style: const TextStyle(color: AppColors.colorTextPrimary),
        ),
        content: Text(
          'VERIFICATION.SENT_TO'.tr(args: [email]),
          style: const TextStyle(color: AppColors.colorTextSecondary),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text(
              'OK',
              style: TextStyle(color: AppColors.colorPrimary),
            ),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final keyboardHeight = MediaQuery.of(context).viewInsets.bottom;
    final isKeyboardOpen = keyboardHeight > 0;

    return Scaffold(
      resizeToAvoidBottomInset: true,
      backgroundColor: AppColors.colorCardBg,
      body: SafeArea(
        child: LayoutBuilder(
          builder: (context, constraints) {
            final availableHeight = constraints.maxHeight;
            final useCenter = availableHeight > 700 && !isKeyboardOpen;

            return SingleChildScrollView(
              physics: const ClampingScrollPhysics(),
              child: ConstrainedBox(
                constraints: BoxConstraints(
                  minHeight: availableHeight,
                ),
                child: IntrinsicHeight(
                  child: Column(
                    children: [
                      if (useCenter) const Spacer(),

                      Padding(
                        padding: const EdgeInsets.symmetric(horizontal: 24),
                        child: Column(
                          children: [
                            SizedBox(height: useCenter ? 20 : 30),
                            _buildHeader(),
                            const SizedBox(height: 20),
                            _buildToggle(),
                            const SizedBox(height: 16),
                            _authMode == 'login'
                                ? LoginForm(
                              isLoading: _isFormLoading,
                              onSubmit: _onLoginSubmit,
                            )
                                : RegisterForm(
                              isLoading: _isFormLoading,
                              validations: registrationsValidations,
                              onValidate: _onValidateChange,
                              onSubmit: _onRegisterSubmit,
                            ),
                            const SizedBox(height: 16),
                            _buildDivider(),
                            const SizedBox(height: 16),
                            _buildSocialButton(),
                            const SizedBox(height: 24),
                            _buildFooter(),
                          ],
                        ),
                      ),
                      
                      const Spacer(),
                      
                      Padding(
                        padding: const EdgeInsets.symmetric(horizontal: 24),
                        child: Column(
                          children: [
                            _buildTermsText(),
                            const SizedBox(height: 20),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            );
          },
        ),
      ),
    );
  }

  Widget _buildHeader() => Column(
    children: [
      Container(
        width: 64,
        height: 64,
        decoration: BoxDecoration(
          color: AppColors.colorPrimary,
          borderRadius: BorderRadius.circular(16),
        ),
        child: const Icon(Icons.fitness_center, size: 32, color: Colors.black),
      ),
      const SizedBox(height: 16),
      Text(
        'LOGIN.TITLE'.tr(),
        style: const TextStyle(
          fontSize: 30,
          fontWeight: FontWeight.bold,
          color: AppColors.colorTextPrimary,
        ),
      ),
      Text(
        'LOGIN.SUBTITLE'.tr(),
        style: const TextStyle(
          color: AppColors.colorTextSecondary,
          fontSize: 14,
        ),
      ),
    ],
  );

  Widget _buildToggle() => Container(
    padding: const EdgeInsets.all(4),
    decoration: BoxDecoration(
      color: AppColors.colorInputBg,
      borderRadius: BorderRadius.circular(12),
    ),
    child: Row(
      children: [
        _toggleButton('login', 'LOGIN.SIGN_IN'.tr()),
        _toggleButton('register', 'LOGIN.SIGN_UP'.tr()),
      ],
    ),
  );

  Widget _toggleButton(String mode, String label) {
    final bool isActive = _authMode == mode;
    return Expanded(
      child: GestureDetector(
        onTap: () => _toggleMode(mode),
        child: AnimatedContainer(
          duration: const Duration(milliseconds: 200),
          padding: const EdgeInsets.symmetric(vertical: 10),
          decoration: BoxDecoration(
            color: isActive ? AppColors.colorPrimary : Colors.transparent,
            borderRadius: BorderRadius.circular(8),
          ),
          child: Center(
            child: Text(
              label,
              style: TextStyle(
                fontWeight: FontWeight.w500,
                color: isActive ? Colors.black : AppColors.colorTextSecondary,
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildDivider() => Row(
    children: [
      const Expanded(
        child: Divider(color: AppColors.colorBorderStrong, thickness: 1),
      ),
      Padding(
        padding: const EdgeInsets.symmetric(horizontal: 16),
        child: Text(
          'LOGIN.OR'.tr(),
          style: const TextStyle(
            color: AppColors.colorTextSecondary,
            fontSize: 13,
          ),
        ),
      ),
      const Expanded(
        child: Divider(color: AppColors.colorBorderStrong, thickness: 1),
      ),
    ],
  );

  Widget _buildSocialButton() {
    return SizedBox(
      width: double.infinity,
      child: OutlinedButton(
        onPressed: (_isFormLoading || _isGoogleLoading) ? null : _handleGoogleSignIn,
        style: OutlinedButton.styleFrom(
          padding: const EdgeInsets.all(12),
          side: const BorderSide(color: AppColors.colorBorderStrong),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12),
          ),
          backgroundColor: AppColors.colorInputBg,
        ),
        child: _isGoogleLoading
            ? const SizedBox(
          width: 20,
          height: 20,
          child: CircularProgressIndicator(
            strokeWidth: 2,
            color: AppColors.colorTextPrimary,
          ),
        )
            : Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            SvgPicture.asset(
              'assets/icons/google_logo.svg',
              width: 20,
              height: 20,
            ),
            const SizedBox(width: 12),
            Flexible(
              child: Text(
                (_authMode == 'login'
                    ? 'LOGIN.WITH_GOOGLE_LOGIN'
                    : 'LOGIN.WITH_GOOGLE_SIGNUP')
                    .tr(),
                style: const TextStyle(
                  color: AppColors.colorTextPrimary,
                  fontSize: 14,
                  fontWeight: FontWeight.w500,
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildFooter() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Text(
          _authMode == 'login'
              ? 'LOGIN.FOOTER_QUESTION_LOGIN'.tr()
              : 'LOGIN.FOOTER_QUESTION_SIGNUP'.tr(),
          style: const TextStyle(
            fontSize: 14,
            color: AppColors.colorTextSecondary,
          ),
        ),
        TextButton(
          onPressed: () {
            _toggleMode(_authMode == 'login' ? 'register' : 'login');
          },
          style: TextButton.styleFrom(
            padding: const EdgeInsets.symmetric(horizontal: 4),
            minimumSize: Size.zero,
            tapTargetSize: MaterialTapTargetSize.shrinkWrap,
          ),
          child: Text(
            _authMode == 'login'
                ? 'LOGIN.FOOTER_LINK_LOGIN'.tr()
                : 'LOGIN.FOOTER_LINK_SIGNUP'.tr(),
            style: const TextStyle(
              fontSize: 14,
              fontWeight: FontWeight.w500,
              color: AppColors.colorPrimary,
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildTermsText() => Padding(
    padding: const EdgeInsets.symmetric(horizontal: 20),
    child: RichText(
      textAlign: TextAlign.center,
      text: TextSpan(
        style: const TextStyle(
          color: AppColors.colorTextSecondary,
          fontSize: 12,
          height: 1.5,
        ),
        children: [
          TextSpan(text: '${'LOGIN.TERMS_TOP_1'.tr()} '),
          TextSpan(
            text: 'LOGIN.TERMS_LINK_1'.tr(),
            style: const TextStyle(
              color: AppColors.colorPrimary,
              fontWeight: FontWeight.w600,
              decoration: TextDecoration.underline,
            ),
          ),
          TextSpan(text: ' ${'LOGIN.TERMS_TOP_2'.tr()} '),
          TextSpan(
            text: 'LOGIN.TERMS_LINK_2'.tr(),
            style: const TextStyle(
              color: AppColors.colorPrimary,
              fontWeight: FontWeight.w600,
              decoration: TextDecoration.underline,
            ),
          ),
        ],
      ),
    ),
  );
}
