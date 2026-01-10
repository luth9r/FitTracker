import 'package:dio/dio.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:cookie_jar/cookie_jar.dart';
import 'package:google_sign_in/google_sign_in.dart';
import 'package:path_provider/path_provider.dart';
import 'dart:io';

// Models
class LoginResponse {
  final String username;
  final String email;
  final String jwt;

  LoginResponse({
    required this.username,
    required this.email,
    required this.jwt,
  });

  factory LoginResponse.fromJson(Map<String, dynamic> json) {
    return LoginResponse(
      username: json['username'] ?? '',
      email: json['email'] ?? '',
      jwt: json['jwt'] ?? '',
    );
  }
}

class RegisterResponse {
  final String username;
  final String email;

  RegisterResponse({
    required this.username,
    required this.email,
  });

  factory RegisterResponse.fromJson(Map<String, dynamic> json) {
    return RegisterResponse(
      username: json['username'] ?? '',
      email: json['email'] ?? '',
    );
  }
}

class RegisterPayload {
  final String username;
  final String email;
  final String password;

  RegisterPayload({
    required this.username,
    required this.email,
    required this.password,
  });

  Map<String, dynamic> toJson() {
    return {
      'username': username,
      'email': email,
      'password': password,
    };
  }
}

class AuthService {
  // Singleton pattern
  static final AuthService _instance = AuthService._internal();
  factory AuthService() => _instance;
  AuthService._internal() {
    _initDio();
  }

  final GoogleSignIn _googleSignIn = GoogleSignIn(
    serverClientId: '719113265141-v8ckgmea9bob1nd65f4396n93o16dcqd.apps.googleusercontent.com',
    scopes: ['email', 'profile'],
  );

  late final Dio _dio;
  
  String get _baseUrl {
    if (Platform.isAndroid) {
      return 'http://192.168.1.111:5000';
    } else {
      return 'http://192.168.1.111:5000';
    }
  }

  String get _apiUrl => '$_baseUrl/api/auth';
  String get _userApiUrl => '$_baseUrl/api/user';

  void _initDio() {
    _dio = Dio(BaseOptions(
      connectTimeout: const Duration(seconds: 10),
      receiveTimeout: const Duration(seconds: 10),
      sendTimeout: const Duration(seconds: 10),
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      validateStatus: (status) {
        return status != null && status < 500;
      },
    ));
    
    _dio.interceptors.add(LogInterceptor(
      requestBody: true,
      responseBody: true,
      error: true,
      requestHeader: true,
      responseHeader: false,
      request: true,
    ));

    _initCookieManager();
  }

  Future<void> _initCookieManager() async {
    try {
      final appDocDir = await getApplicationDocumentsDirectory();
      final cookieJar = PersistCookieJar(
        storage: FileStorage('${appDocDir.path}/.cookies/'),
      );
      _dio.interceptors.add(CookieManager(cookieJar));
      print('[AUTH] Cookie manager initialized');
    } catch (e) {
      print('[AUTH] Error initializing cookies: $e');
    }
  }

  /// Regular Login
  Future<LoginResponse> login(String email, String password) async {
    try {
      print('[AUTH] Attempting login for: $email');
      final response = await _dio.post(
        '$_apiUrl/login',
        data: {
          'email': email,
          'password': password,
        },
      );

      if (response.statusCode == 200) {
        print('[AUTH] Login successful');
        return LoginResponse.fromJson(response.data);
      } else {
        throw Exception(response.data['message'] ?? 'Login failed');
      }
    } on DioException catch (e) {
      print('[AUTH] Login error: ${e.message}');
      throw _handleDioError(e);
    }
  }

  /// Regular Registration
  Future<RegisterResponse> register(RegisterPayload payload) async {
    try {
      print('[AUTH] Attempting registration for: ${payload.email}');
      final response = await _dio.post(
        '$_apiUrl/register',
        data: payload.toJson(),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        print('[AUTH] Registration successful');
        return RegisterResponse.fromJson(response.data);
      } else {
        throw Exception(response.data['message'] ?? 'Registration failed');
      }
    } on DioException catch (e) {
      print('[AUTH] Registration error: ${e.message}');
      throw _handleDioError(e);
    }
  }

  /// Verify Email
  Future<LoginResponse> verifyEmail(String token) async {
    try {
      print('[AUTH] Verifying email with token');
      final response = await _dio.post(
        '$_apiUrl/verify-email',
        queryParameters: {'token': token},
      );
      return LoginResponse.fromJson(response.data);
    } on DioException catch (e) {
      throw _handleDioError(e);
    }
  }

  Future<LoginResponse> signInWithGoogleNative() async {
    try {
      print('[AUTH] Starting Native Google Sign-In');

      await _googleSignIn.signOut();
      final GoogleSignInAccount? googleUser = await _googleSignIn.signIn();

      if (googleUser == null) {
        throw Exception('User cancelled login');
      }
      
      final String? authCode = googleUser.serverAuthCode;

      if (authCode == null) {
        throw Exception('Could not get Server Auth Code from Google');
      }

      print('[AUTH] Received Auth Code, sending to backend...');
      
      final response = await _dio.post(
        '$_apiUrl/mobile-google-auth',
        data: {
          'code': authCode,
        },
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        print('[AUTH] Native Google Auth successful');
        return LoginResponse.fromJson(response.data);
      } else {
        throw Exception(response.data['message'] ?? 'Backend auth failed');
      }
    } on DioException catch (e) {
      throw _handleDioError(e);
    } catch (e) {
      print('[AUTH] Native Google Error: $e');
      throw Exception(e.toString());
    }
  }

  /// Check Authentication
  Future<bool> checkAuth() async {
    try {
      final response = await _dio.get('$_userApiUrl/me');
      final userId = response.data['userId'];
      return userId != null;
    } catch (e) {
      print('[AUTH] Check auth failed: $e');
      return false;
    }
  }

  /// Resend Verification Email
  Future<void> resendVerificationEmail(String email) async {
    try {
      print('[AUTH] Resending verification email to: $email');
      await _dio.post(
        '$_apiUrl/resend-verification',
        data: {'email': email},
      );
      print('[AUTH] Verification email sent');
    } on DioException catch (e) {
      throw _handleDioError(e);
    }
  }

  /// Forgot Password
  Future<void> forgotPassword(String email) async {
    try {
      print('[AUTH] Sending password reset email to: $email');
      await _dio.post(
        '$_apiUrl/forgot-password',
        data: {'email': email},
      );
      print('[AUTH] Password reset email sent');
    } on DioException catch (e) {
      throw _handleDioError(e);
    }
  }

  /// Logout
  Future<void> logout() async {
    try {
      final appDocDir = await getApplicationDocumentsDirectory();
      final cookieJar = PersistCookieJar(
        storage: FileStorage('${appDocDir.path}/.cookies/'),
      );
      await cookieJar.deleteAll();
      print('[AUTH] Logged out - cookies cleared');
    } catch (e) {
      print('[AUTH] Logout error: $e');
    }
  }

  /// Error Handler
  Exception _handleDioError(DioException e) {
    switch (e.type) {
      case DioExceptionType.connectionTimeout:
      case DioExceptionType.sendTimeout:
      case DioExceptionType.receiveTimeout:
        return Exception('Connection timeout. Please check your internet connection.');

      case DioExceptionType.badResponse:
        final statusCode = e.response?.statusCode;
        final message = e.response?.data['message'];

        if (statusCode == 401) {
          return Exception('Invalid credentials');
        } else if (statusCode == 409) {
          return Exception('User already exists');
        } else if (statusCode == 400) {
          return Exception(message ?? 'Invalid request');
        } else {
          return Exception(message ?? 'Server error');
        }

      case DioExceptionType.cancel:
        return Exception('Request cancelled');

      case DioExceptionType.unknown:
        if (e.error.toString().contains('SocketException')) {
          return Exception('Cannot connect to server. Please check your connection.');
        }
        return Exception('Unexpected error: ${e.message}');

      default:
        return Exception('Network error');
    }
  }
}
