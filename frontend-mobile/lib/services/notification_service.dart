import 'package:flutter/material.dart';
import 'package:overlay_support/overlay_support.dart';
import '../core/app_colors.dart';

class NotificationService {
  // Singleton pattern
  static final NotificationService _instance = NotificationService._internal();
  factory NotificationService() => _instance;
  NotificationService._internal();

  void showToast(String message, {bool isError = false}) {
    showSimpleNotification(
      Text(
        message,
        style: const TextStyle(
          color: Colors.white,
          fontSize: 16,
          fontWeight: FontWeight.w500,
        ),
      ),
      leading: Icon(
        isError ? Icons.error_outline : Icons.check_circle_outline,
        color: Colors.white,
      ),
      background: isError ? AppColors.toastBgError : AppColors.toastBgSuccess,
      position: NotificationPosition.top,
      duration: const Duration(seconds: 3),
      slideDismissDirection: DismissDirection.up,
      elevation: 4,
    );
  }
}