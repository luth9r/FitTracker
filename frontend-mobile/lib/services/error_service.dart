import 'package:dio/dio.dart';
import 'package:easy_localization/easy_localization.dart';

class ErrorService {
  static final ErrorService instance = ErrorService._internal();

  ErrorService._internal();

  /// Извлекает код ошибки из DioException
  String? extractErrorCode(dynamic error) {
    if (error is DioException && error.response?.data != null) {
      try {
        final responseData = error.response!.data;

        // Проверяем поле 'errors' (validation errors)
        if (responseData is Map && responseData['errors'] != null) {
          final errors = responseData['errors'] as Map;

          // Перебираем все поля ошибок (включая "General")
          for (var entry in errors.entries) {
            final fieldErrors = entry.value;

            if (fieldErrors is List && fieldErrors.isNotEmpty) {
              final errorCode = fieldErrors.first.toString();
              print('[DEBUG] Error from errors.${entry.key}: $errorCode');
              return errorCode;
            }
          }
        }

        // Проверяем поле 'detail'
        if (responseData is Map && responseData['detail'] != null) {
          final detail = responseData['detail'].toString();
          if (_isKnownErrorCode(detail)) {
            print('[DEBUG] Error from detail field: $detail');
            return detail;
          }
        }

        // Проверяем поле 'title'
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

  /// Проверяет, является ли текст известным кодом ошибки
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

  /// Получает локализованное сообщение об ошибке
  String getLocalizedError(dynamic error, String? errorCode) {
    // Если есть код ошибки, пытаемся найти перевод
    if (errorCode != null) {
      final translationKey = 'Errors.$errorCode';
      final translated = translationKey.tr();

      // Если перевод найден (ключ изменился)
      if (translated != translationKey) {
        return translated;
      }
    }

    // Проверяем сетевые ошибки
    final errorString = error.toString().toLowerCase();
    if (errorString.contains('socketexception') ||
        errorString.contains('network') ||
        errorString.contains('connection')) {
      return 'Errors.Network.Unreachable'.tr();
    }

    // Если код ошибки известен, но перевода нет - форматируем его
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

    // Дефолтная ошибка
    return 'Errors.Unknown'.tr();
  }

  /// Проверяет, является ли ошибка конкретным кодом
  bool isErrorCode(dynamic error, String code) {
    final errorCode = extractErrorCode(error);
    return errorCode == code;
  }

  /// Обрабатывает специфичные ошибки и возвращает локализованное сообщение
  String handleError(dynamic error) {
    final errorCode = extractErrorCode(error);
    print('[DEBUG] Error code: $errorCode');
    return getLocalizedError(error, errorCode);
  }
}
