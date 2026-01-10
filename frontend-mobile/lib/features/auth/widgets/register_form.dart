import 'package:flutter/material.dart';
import 'package:easy_localization/easy_localization.dart';
import '../../../shared/widgets/validation_checklist.dart';
import '../../../shared/widgets/auth_input_field.dart';
import '../../../core/app_colors.dart';

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

  final Map<String, FocusNode> _nodes = {
    'username': FocusNode(),
    'email': FocusNode(),
    'password': FocusNode(),
    'confirmPassword': FocusNode(),
  };

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
      entry.value.addListener(() => setState(() {}));
    }
  }

  bool _hasError(String field, String text) {
    final state = widget.validations[field] ?? {};
    return text.isNotEmpty && state.containsValue(false);
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        _buildField(
          label: 'REGISTER.USERNAME'.tr(),
          placeholder: 'REGISTER.USERNAME_PLACEHOLDER'.tr(),
          field: 'username',
          controller: _usernameController,
          prefix: 'REGISTER.VALIDATION.USERNAME_',
        ),
        _buildField(
          label: 'LOGIN.EMAIL'.tr(),
          placeholder: 'LOGIN.EMAIL_PLACEHOLDER'.tr(),
          field: 'email',
          controller: _emailController,
          prefix: 'REGISTER.VALIDATION.EMAIL_',
          type: 'email',
        ),
        _buildField(
          label: 'LOGIN.PASSWORD'.tr(),
          placeholder: 'LOGIN.PASSWORD_PLACEHOLDER'.tr(),
          field: 'password',
          controller: _passwordController,
          prefix: 'REGISTER.VALIDATION.PASSWORD_',
          type: 'password',
        ),
        _buildField(
          label: 'LOGIN.CONFIRM_PASSWORD'.tr(),
          placeholder: 'LOGIN.CONFIRM_PASSWORD_PLACEHOLDER'.tr(),
          field: 'confirmPassword',
          controller: _confirmController,
          prefix: 'REGISTER.VALIDATION.CONFIRM_',
          type: 'password',
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
    String type = 'text',
  }) {
    final state = widget.validations[field] ?? {};
    final focusNode = _nodes[field]!;
    final bool showChecklist = focusNode.hasFocus;
    final bool hasError = controller.text.isNotEmpty && state.containsValue(false);

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        AuthInputField(
          label: label,
          placeholder: placeholder,
          fieldName: field,
          type: type,
          controller: controller,
          focusNode: focusNode,
          hasError: hasError,
          onChanged: (val) {
            widget.onValidate(field, val);
          },
        ),
        
        if (showChecklist) ...[
          const SizedBox(height: 8),
          Padding(
            padding: const EdgeInsets.only(left: 4),
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
          'LOGIN.SUBMIT_BUTTON_SIGNUP'.tr(),
          style: const TextStyle(
            fontSize: 14,
            fontWeight: FontWeight.w500,
          ),
        ),
      ),
    );
  }


  void _submit() {
    widget.onSubmit({
      'username': _usernameController.text,
      'email': _emailController.text,
      'password': _passwordController.text,
    });
  }
}
