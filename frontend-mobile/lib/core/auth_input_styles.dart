import 'package:flutter/material.dart';
import 'app_colors.dart';

class AuthInputStyles {
  static InputDecoration authInputDecoration({
    required String labelText,
    required String hintText,
    String? errorText,
    bool hasError = false,
    Widget? prefixIcon,
    Widget? suffixIcon,
  }) {
    const Color errorRed = AppColors.colorAccentDanger;

    return InputDecoration(
      labelText: labelText,
      labelStyle: TextStyle(
        color: hasError ? errorRed : AppColors.colorTextSecondary,
      ),
      hintText: hintText,
      hintStyle: const TextStyle(color: AppColors.colorTextMuted),
      errorText: errorText,
      errorStyle: const TextStyle(color: errorRed, fontWeight: FontWeight.w500),
      filled: true,
      fillColor: AppColors.colorInputBg,
      prefixIcon: prefixIcon,
      suffixIcon: suffixIcon,
      contentPadding: const EdgeInsets.symmetric(vertical: 16, horizontal: 16),
      
      suffixIconConstraints: const BoxConstraints(
        minWidth: 44,
        minHeight: 44,
      ),

      // Default Border
      enabledBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(12),
        borderSide: BorderSide(
          color: hasError ? errorRed : AppColors.colorBorderStrong,
          width: hasError ? 2 : 1,
        ),
      ),
      // Focus Border
      focusedBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(12),
        borderSide: const BorderSide(color: AppColors.colorPrimary, width: 2),
      ),
      // Error Border
      errorBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(12),
        borderSide: const BorderSide(color: errorRed, width: 2),
      ),
      // Focus Error Border
      focusedErrorBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(12),
        borderSide: const BorderSide(color: errorRed, width: 2),
      ),
    );
  }
}
