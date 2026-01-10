import 'package:flutter/material.dart';
import 'package:easy_localization/easy_localization.dart';
import '../../../core/app_colors.dart';

class ValidationChecklist extends StatelessWidget {
  final Map<String, bool> validationState;
  final String translationPrefix;
  final bool shouldShow;

  const ValidationChecklist({
    super.key,
    required this.validationState,
    required this.translationPrefix,
    required this.shouldShow,
  });

  @override
  Widget build(BuildContext context) {
    if (!shouldShow) return const SizedBox.shrink();

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: validationState.entries.map((entry) {
        final bool isValid = entry.value;
        final Color statusColor = isValid
            ? AppColors.colorAccentSuccess
            : AppColors.colorAccentDanger;

        return Padding(
          padding: const EdgeInsets.only(top: 0, bottom: 2),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(
                isValid ? Icons.check_circle : Icons.cancel,
                size: 14,
                color: statusColor,
              ),
              const SizedBox(width: 8),
              Text(
                '$translationPrefix${entry.key}'.tr(),
                style: TextStyle(
                  fontSize: 12,
                  color: statusColor,
                  fontWeight: isValid ? FontWeight.w500 : FontWeight.normal,
                ),
              ),
            ],
          ),
        );
      }).toList(),
    );
  }
}