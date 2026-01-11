import 'package:easy_localization/easy_localization.dart';
import 'package:flutter/material.dart';

import '../../../core/app_colors.dart';
import '../../../core/auth_input_styles.dart';
import '../../../services/notification_service.dart';
import '../../../shared/widgets/validation_checklist.dart';

class RegisterForm extends StatefulWidget {
  final bool isLoading;
  final Function(Map<String, String> data) onSubmit;
  final Function(String field, String value) onValidate;
  final Map<String, Map<String, bool>> validations;

  const RegisterForm({
    super.key,
    required this.isLoading,
    required this.onSubmit,
    required this.onValidate,
    required this.validations,
  });

  @override
  State<RegisterForm> createState() => _RegisterFormState();
}

class _RegisterFormState extends State<RegisterForm> {
  final _usernameController = TextEditingController();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _confirmController = TextEditingController();
  final _notificationService = NotificationService();

  final Map<String, FocusNode> _nodes = {
    'username': FocusNode(),
    'email': FocusNode(),
    'password': FocusNode(),
    'confirmPassword': FocusNode(),
  };

  final Map<String, bool> _touched = {
    'username': false,
    'email': false,
    'password': false,
    'confirmPassword': false,
  };

  bool _obscurePassword = true;
  bool _obscureConfirm = true;

  @override
  void dispose() {
    _usernameController.dispose();
    _emailController.dispose();
    _passwordController.dispose();
    _confirmController.dispose();
    for (var node in _nodes.values) {
      node.dispose();
    }
    super.dispose();
  }

  @override
  void initState() {
    super.initState();
    for (var entry in _nodes.entries) {
      entry.value.addListener(() {
        if (!entry.value.hasFocus) {
          setState(() {
            _touched[entry.key] = true;
          });
        }
        setState(() {});
      });
    }
  }

  bool _hasError(String field) {
    final state = widget.validations[field] ?? {};
    final isTouched = _touched[field] ?? false;

    final hasInvalidChecks = state.containsValue(false);

    return isTouched && hasInvalidChecks;
  }

  bool _isFieldValid(String field) {
    final state = widget.validations[field] ?? {};
    return !state.containsValue(false);
  }

  void _submit() {
    setState(() {
      _touched.updateAll((key, value) => true);
    });

    final username = _usernameController.text.trim();
    final email = _emailController.text.trim();
    final password = _passwordController.text.trim();
    final confirm = _confirmController.text.trim();

    if (username.isEmpty ||
        email.isEmpty ||
        password.isEmpty ||
        confirm.isEmpty) {
      _notificationService.showToast(
        'Errors.Auth.InvalidCredentials'.tr(),
        isError: true,
      );
      return;
    }

    final allValid =
        _isFieldValid('username') &&
        _isFieldValid('email') &&
        _isFieldValid('password') &&
        _isFieldValid('confirmPassword');

    if (!allValid) {
      _notificationService.showToast(
        'Errors.Auth.InvalidCredentials'.tr(),
        isError: true,
      );
      return;
    }

    widget.onSubmit({
      'username': username,
      'email': email,
      'password': password,
    });
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        _buildField(
          label: 'Register.Username'.tr(),
          placeholder: 'Register.UsernamePlaceholder'.tr(),
          field: 'username',
          controller: _usernameController,
          prefix: 'Register.Validation.Username',
          icon: Icons.person_outline,
        ),
        _buildField(
          label: 'Login.Email'.tr(),
          placeholder: 'Login.EmailPlaceholder'.tr(),
          field: 'email',
          controller: _emailController,
          prefix: 'Register.Validation.Email',
          icon: Icons.email_outlined,
          keyboardType: TextInputType.emailAddress,
        ),
        _buildField(
          label: 'Login.Password'.tr(),
          placeholder: 'Login.PasswordPlaceholder'.tr(),
          field: 'password',
          controller: _passwordController,
          prefix: 'Register.Validation.Password',
          icon: Icons.lock_outline,
          isPassword: true,
          obscureText: _obscurePassword,
          onToggleVisibility: () =>
              setState(() => _obscurePassword = !_obscurePassword),
        ),
        _buildField(
          label: 'Login.ConfirmPassword'.tr(),
          placeholder: 'Login.ConfirmPasswordPlaceholder'.tr(),
          field: 'confirmPassword',
          controller: _confirmController,
          prefix: 'Register.Validation.Confirm',
          icon: Icons.lock_outline,
          isPassword: true,
          obscureText: _obscureConfirm,
          onToggleVisibility: () =>
              setState(() => _obscureConfirm = !_obscureConfirm),
        ),
        const SizedBox(height: 24),
        _buildSubmitButton(),
      ],
    );
  }

  Widget _buildField({
    required String label,
    required String placeholder,
    required String field,
    required TextEditingController controller,
    required String prefix,
    required IconData icon,
    bool isPassword = false,
    bool obscureText = false,
    VoidCallback? onToggleVisibility,
    TextInputType keyboardType = TextInputType.text,
  }) {
    final state = widget.validations[field] ?? {};
    final focusNode = _nodes[field]!;
    final bool showChecklist = focusNode.hasFocus;
    final bool hasError = _hasError(field);

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        TextField(
          controller: controller,
          focusNode: focusNode,
          keyboardType: keyboardType,
          obscureText: isPassword && obscureText,
          onChanged: (val) {
            widget.onValidate(field, val);
            setState(() {});
          },
          style: const TextStyle(color: AppColors.colorTextPrimary),
          decoration: AuthInputStyles.authInputDecoration(
            labelText: label,
            hintText: placeholder,
            errorText: null,
            hasError: hasError,
            prefixIcon: Icon(
              icon,
              color: hasError
                  ? AppColors.colorAccentDanger
                  : AppColors.colorTextSecondary,
            ),
            suffixIcon: isPassword
                ? IconButton(
                    icon: Icon(
                      obscureText
                          ? Icons.visibility_outlined
                          : Icons.visibility_off_outlined,
                      color: AppColors.colorTextSecondary,
                    ),
                    onPressed: onToggleVisibility,
                  )
                : null,
          ),
        ),

        if (showChecklist) ...[
          const SizedBox(height: 8),
          Padding(
            padding: const EdgeInsets.only(left: 16),
            child: ValidationChecklist(
              validationState: state,
              translationPrefix: prefix,
              shouldShow: true,
            ),
          ),
          const SizedBox(height: 16),
        ] else
          const SizedBox(height: 16),
      ],
    );
  }

  Widget _buildSubmitButton() {
    return SizedBox(
      width: double.infinity,
      height: 44,
      child: ElevatedButton(
        onPressed: widget.isLoading ? null : _submit,
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
                width: 20,
                height: 20,
                child: CircularProgressIndicator(
                  strokeWidth: 2,
                  color: Colors.white,
                ),
              )
            : Text(
                'Login.SubmitButtonSignup'.tr(),
                style: const TextStyle(
                  fontSize: 14,
                  fontWeight: FontWeight.w500,
                ),
              ),
      ),
    );
  }
}
