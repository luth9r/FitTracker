import 'package:easy_localization/easy_localization.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';

import '../../../core/app_colors.dart';
import '../../../services/auth_service.dart';
import '../../../services/error_service.dart';
import '../../../services/notification_service.dart';
import '../../../services/validation_service.dart';
import '../../home/screens/home_screen.dart';
import '../widgets/email_not_verified_modal.dart';
import '../widgets/email_verification_modal.dart';
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
  final _errorService = ErrorService.instance;

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
          registrationsValidations['username'] = _validator.validateUsername(
            value,
          );
          break;
        case 'email':
          registrationsValidations['email'] = _validator.validateEmail(value);
          break;
        case 'password':
          _currentPassword = value;
          registrationsValidations['password'] = _validator.validatePassword(
            value,
          );
          registrationsValidations['confirmPassword'] = _validator
              .validateConfirm(_currentPassword, _currentConfirmPassword);
          break;
        case 'confirmPassword':
          _currentConfirmPassword = value;
          registrationsValidations['confirmPassword'] = _validator
              .validateConfirm(_currentPassword, value);
          break;
      }
    });
  }

  void _toggleMode(String mode) {
    setState(() {
      _authMode = mode;
      _currentPassword = '';
      _currentConfirmPassword = '';
      registrationsValidations.forEach(
        (k, v) => registrationsValidations[k] = v.map(
          (key, value) => MapEntry(key, false),
        ),
      );
    });
  }

  Future<void> _onLoginSubmit(String email, String password) async {
    setState(() => _isFormLoading = true);
    try {
      final response = await _authService.login(email, password);
      _notificationService.showToast(
        'Login.Success.Login'.tr(args: [response.username]),
      );

      Future.delayed(const Duration(milliseconds: 500), () {
        if (mounted) {
          Navigator.of(context).pushAndRemoveUntil(
            MaterialPageRoute(builder: (context) => const HomeScreen()),
            (route) => false,
          );
        }
      });
    } catch (e) {
      print('[ERROR] Login failed: $e');
      _handleLoginError(e, email);
    } finally {
      if (mounted) {
        setState(() => _isFormLoading = false);
      }
    }
  }

  void _handleLoginError(dynamic error, String email) {
    if (_errorService.isErrorCode(error, 'User.EmailNotVerified')) {
      _showEmailNotVerifiedPrompt(email);
      return;
    }

    if (_errorService.isErrorCode(error, 'User.RateLimitExceeded')) {
      _notificationService.showToast(
        'Errors.User.RateLimitExceeded'.tr(),
        isError: true,
      );
      return;
    }

    _notificationService.showToast(
      _errorService.handleError(error),
      isError: true,
    );
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
      _notificationService.showToast('Login.Success.Register'.tr());
    } catch (e) {
      print('[ERROR] Registration failed: $e');

      _notificationService.showToast(
        _errorService.handleError(e),
        isError: true,
      );
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

      _notificationService.showToast(
        _errorService.handleError(e),
        isError: true,
      );
    } finally {
      if (mounted) {
        setState(() => _isGoogleLoading = false);
      }
    }
  }

  void _showEmailNotVerifiedPrompt(String email) {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      isDismissible: true,
      builder: (context) => EmailNotVerifiedPromptModal(email: email),
    );
  }

  void _showEmailVerificationModal(String email) {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (context) => EmailVerificationModal(email: email),
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
                constraints: BoxConstraints(minHeight: availableHeight),
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
                            const SizedBox(height: 24),
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
                            const SizedBox(height: 20),
                            _buildDivider(),
                            const SizedBox(height: 20),
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
        'Login.Title'.tr(),
        style: const TextStyle(
          fontSize: 30,
          fontWeight: FontWeight.bold,
          color: AppColors.colorTextPrimary,
        ),
      ),
      Text(
        'Login.Subtitle'.tr(),
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
        _toggleButton('login', 'Login.SignIn'.tr()),
        _toggleButton('register', 'Login.SignUp'.tr()),
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
          'Login.Or'.tr(),
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
        onPressed: (_isFormLoading || _isGoogleLoading)
            ? null
            : _handleGoogleSignIn,
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
                              ? 'Login.WithGoogleLogin'
                              : 'Login.WithGoogleSignup')
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
              ? 'Login.FooterQuestionLogin'.tr()
              : 'Login.FooterQuestionSignup'.tr(),
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
                ? 'Login.FooterLinkLogin'.tr()
                : 'Login.FooterLinkSignup'.tr(),
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
          TextSpan(text: '${'Login.TermsTop1'.tr()} '),
          TextSpan(
            text: 'Login.TermsLink1'.tr(),
            style: const TextStyle(
              color: AppColors.colorPrimary,
              fontWeight: FontWeight.w600,
              decoration: TextDecoration.underline,
            ),
          ),
          TextSpan(text: ' ${'Login.TermsTop2'.tr()} '),
          TextSpan(
            text: 'Login.TermsLink2'.tr(),
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
