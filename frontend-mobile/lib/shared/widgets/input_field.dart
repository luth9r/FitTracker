import 'package:flutter/material.dart';
import '../../core/app_colors.dart';

class CustomInputField extends StatelessWidget {
  final String? label;
  final String? placeholder;
  final String name;
  final String type;
  final bool hasError;
  final String value;
  final Widget? prefix;
  final Widget? suffix;
  final TextEditingController? controller;
  final ValueChanged<String>? onChanged;
  final VoidCallback? onFocus;
  final VoidCallback? onBlur;
  final FocusNode? focusNode;

  const CustomInputField({
    super.key,
    this.label,
    this.placeholder,
    required this.name,
    this.type = 'text',
    this.hasError = false,
    this.value = '',
    this.prefix,
    this.suffix,
    this.controller,
    this.onChanged,
    this.onFocus,
    this.onBlur,
    this.focusNode,
  });

  @override
  Widget build(BuildContext context) {
    final effectiveFocusNode = focusNode ?? FocusNode();
    
    if (focusNode == null) {
      effectiveFocusNode.addListener(() {
        if (effectiveFocusNode.hasFocus) {
          onFocus?.call();
        } else {
          onBlur?.call();
        }
      });
    }

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        if (label != null)
          Padding(
            padding: const EdgeInsets.only(bottom: 8),
            child: Text(
              label!,
              style: TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.w500,
                color: hasError
                    ? AppColors.colorAccentDanger
                    : AppColors.colorTextSecondary,
              ),
            ),
          ),
        TextFormField(
          controller: controller,
          focusNode: effectiveFocusNode,
          obscureText: type == 'password',
          keyboardType: type == 'email'
              ? TextInputType.emailAddress
              : TextInputType.text,
          onChanged: onChanged,
          style: const TextStyle(
            color: AppColors.colorTextPrimary,
            fontSize: 16,
          ),
          decoration: InputDecoration(
            isDense: true,
            hintText: placeholder,
            hintStyle: const TextStyle(color: AppColors.colorTextMuted),

            filled: true,
            fillColor: hasError
                ? AppColors.colorAccentDanger.withOpacity(0.05)
                : AppColors.colorInputBg,

            prefixIcon: prefix != null
                ? Padding(
              padding: const EdgeInsets.only(left: 12.0, right: 12.0),
              child: prefix,
            )
                : null,
            prefixIconConstraints: const BoxConstraints(minWidth: 44),

            suffixIcon: suffix != null
                ? Padding(
              padding: const EdgeInsets.only(right: 12.0),
              child: suffix,
            )
                : null,
            suffixIconConstraints: const BoxConstraints(minWidth: 44),

            contentPadding: const EdgeInsets.symmetric(
                vertical: 12,
                horizontal: 16
            ),
            enabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(12),
              borderSide: BorderSide(
                color: hasError
                    ? AppColors.colorAccentDanger
                    : AppColors.colorBorderStrong,
                width: hasError ? 2 : 1,
              ),
            ),
            focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(12),
              borderSide: BorderSide(
                color: hasError
                    ? AppColors.colorAccentDanger
                    : AppColors.colorPrimary,
                width: 2,
              ),
            ),
          ),
        ),
      ],
    );
  }
}
