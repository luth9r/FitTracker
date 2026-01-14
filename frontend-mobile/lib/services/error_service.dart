import 'package:dio/dio.dart';
import 'package:easy_localization/easy_localization.dart';

class ErrorService {
  static final ErrorService instance = ErrorService._internal();

  ErrorService._internal();

  String? extractErrorCode(dynamic error) {
    if (error is DioException && error.response?.data != null) {
      try {
        final responseData = error.response!.data;

        if (responseData is Map && responseData['errors'] != null) {
          final errors = responseData['errors'] as Map;

          for (var entry in errors.entries) {
            final fieldErrors = entry.value;

            if (fieldErrors is List && fieldErrors.isNotEmpty) {
              final errorCode = fieldErrors.first.toString();
              print('[DEBUG] Error from errors.${entry.key}: $errorCode');
              return errorCode;
            }
          }
        }

        if (responseData is Map && responseData['detail'] != null) {
          final detail = responseData['detail'].toString();
          if (_isKnownErrorCode(detail)) {
            print('[DEBUG] Error from detail field: $detail');
            return detail;
          }
        }

        if (responseData is Map && responseData['title'] != null) {
          final title = responseData['title'].toString();
          if (title.contains('.') || _isKnownErrorCode(title)) {
            print('[DEBUG] Error from title field: $title');
            return title;
          }
        }
      } catch (e) {
        print('[WARN] Error extracting error code: $e');
      }
    }
    return null;
  }

  bool _isKnownErrorCode(String text) {
    final knownPrefixes = [
      'Auth.',
      'User.',
      'Google.',
      'Validation.',
      'Resource.',
      'Server.',
    ];
    return knownPrefixes.any((prefix) => text.startsWith(prefix));
  }

  String getLocalizedError(dynamic error, String? errorCode) {
    if (errorCode != null) {
      final translationKey = 'Errors.$errorCode';
      final translated = translationKey.tr();

      if (translated != translationKey) {
        return translated;
      }
    }

    final errorString = error.toString().toLowerCase();
    if (errorString.contains('socketexception') ||
        errorString.contains('network') ||
        errorString.contains('connection')) {
      return 'Errors.Network.Unreachable'.tr();
    }

    if (errorCode != null) {
      return errorCode
          .replaceAll('Auth.', '')
          .replaceAll('User.', '')
          .replaceAll('Google.', '')
          .replaceAll('Validation.', '')
          .replaceAll('Resource.', '')
          .replaceAll('Server.', '')
          .replaceAll('.', ' ');
    }

    return 'Errors.Unknown'.tr();
  }

  bool isErrorCode(dynamic error, String code) {
    final errorCode = extractErrorCode(error);
    return errorCode == code;
  }
  
  String handleError(dynamic error) {
    final errorCode = extractErrorCode(error);
    print('[DEBUG] Error code: $errorCode');
    return getLocalizedError(error, errorCode);
  }
}
