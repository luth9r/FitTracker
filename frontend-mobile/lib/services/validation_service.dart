class ValidationService {
  static final ValidationService instance = ValidationService._internal();
  ValidationService._internal();

  Map<String, bool> validateUsername(String v) => {
    'minLength': v.length >= 3,
    'noSpaces': !v.contains(' '),
  };

  Map<String, bool> validateEmail(String v) => {
    'isValid': RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$').hasMatch(v),
  };

  Map<String, bool> validatePassword(String v) => {
    'minLength': v.length >= 8,
    'oneLetter': v.contains(RegExp(r'[a-zA-Z]')),
    'oneNumber': v.contains(RegExp(r'[0-9]')),
  };

  Map<String, bool> validateConfirm(String p, String c) => {
    'matches': p == c && c.isNotEmpty,
  };
}