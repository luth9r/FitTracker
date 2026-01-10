import 'package:flutter/material.dart';
import '../../../core/app_colors.dart';
import '../../../shared/widgets/input_field.dart';

class AuthInputField extends StatefulWidget {
  final String label;
  final String placeholder;
  final String fieldName;
  final String type;
  final TextEditingController controller;
  final bool hasError;
  final VoidCallback? onFocus;
  final VoidCallback? onBlur;
  final ValueChanged<String>? onChanged;
  final FocusNode? focusNode;

  const AuthInputField({
    super.key,
    required this.label,
    required this.placeholder,
    required this.fieldName,
    required this.controller,
    this.type = 'text',
    this.hasError = false,
    this.onFocus,
    this.onBlur,
    this.onChanged,
    this.focusNode,
  });

  @override
  State<AuthInputField> createState() => _AuthInputFieldState();
}

class _AuthInputFieldState extends State<AuthInputField> {
  bool _showPassword = false;
  late FocusNode _internalFocusNode;

  @override
  void initState() {
    super.initState();
    _internalFocusNode = widget.focusNode ?? FocusNode();
    
    _internalFocusNode.addListener(_handleFocusChange);
  }

  @override
  void dispose() {
    _internalFocusNode.removeListener(_handleFocusChange);
    if (widget.focusNode == null) {
      _internalFocusNode.dispose();
    }
    super.dispose();
  }

  void _handleFocusChange() {
    if (_internalFocusNode.hasFocus) {
      widget.onFocus?.call();
    } else {
      widget.onBlur?.call();
    }
  }

  Widget _buildPrefixIcon() {
    IconData iconData;
    switch (widget.fieldName) {
      case 'username':
        iconData = Icons.person_outline;
        break;
      case 'email':
        iconData = Icons.mail_outline;
        break;
      case 'password':
      case 'confirmPassword':
      default:
        iconData = Icons.lock_outline;
        break;
    }
    return Icon(iconData, color: AppColors.colorTextSecondary, size: 24);
  }

  @override
  Widget build(BuildContext context) {
    return CustomInputField(
      label: widget.label,
      placeholder: widget.placeholder,
      name: widget.fieldName,
      controller: widget.controller,
      hasError: widget.hasError,
      focusNode: _internalFocusNode,

      onChanged: widget.onChanged,
      type: (_showPassword && widget.type == 'password') ? 'text' : widget.type,

      prefix: _buildPrefixIcon(),

      suffix: widget.type == 'password'
          ? GestureDetector(
        onTap: () {
          setState(() {
            _showPassword = !_showPassword;
          });
        },
        behavior: HitTestBehavior.opaque,
        child: Padding(
          padding: const EdgeInsets.all(6),
          child: Icon(
            _showPassword ? Icons.visibility_off_outlined : Icons.visibility_outlined,
            color: AppColors.colorTextSecondary,
            size: 24,
          ),
        ),
      )
          : null,
    );
  }
}
