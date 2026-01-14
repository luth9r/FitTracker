import 'dart:io';

import 'package:cookie_jar/cookie_jar.dart';
import 'package:dio/dio.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:google_sign_in/google_sign_in.dart';
import 'package:isar_community/isar.dart';
import 'package:path_provider/path_provider.dart';

import '../data/models/user_model.dart';

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

  RegisterResponse({required this.username, required this.email});

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
    return {'username': username, 'email': email, 'password': password};
  }
}

class AuthService {
  static final AuthService _instance = AuthService._internal();

  factory AuthService() => _instance;

  late final Dio _dio;
  late final PersistCookieJar _cookieJar;
  late final Isar _isar;
  bool _isInitialized = false;

  Isar get isar => _isar;

  AuthService._internal() {
    _dio = Dio(
      BaseOptions(
        baseUrl: _baseUrl,
        connectTimeout: const Duration(seconds: 10),
        receiveTimeout: const Duration(seconds: 10),
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        },
        validateStatus: (status) => status != null && status < 300,
      ),
    );

    _dio.interceptors.add(
      LogInterceptor(requestBody: true, responseBody: true),
    );
  }

  Future<void> initialize() async {
    if (_isInitialized) return;

    try {
      final appDocDir = await getApplicationDocumentsDirectory();

      _isar = await Isar.open([UserCacheSchema], directory: appDocDir.path);
      print('[AUTH] Isar DB ready');

      final cookiePath = '${appDocDir.path}/.cookies/';
      _cookieJar = PersistCookieJar(storage: FileStorage(cookiePath));

      _dio.interceptors.add(CookieManager(_cookieJar));

      var cookies = await _cookieJar.loadForRequest(Uri.parse(_baseUrl));
      print('[AUTH] Cookies loaded: ${cookies.length}');

      _isInitialized = true;
      print('[AUTH] Service fully initialized');
    } catch (e) {
      print('[AUTH] Initialization error: $e');
      rethrow;
    }
  }

  Future<void> persistUser(String email, String username) async {
    await _isar.writeTxn(() async {
      await _isar.userCaches.clear();
      await _isar.userCaches.put(
        UserCache(email: email, username: username, lastLogin: DateTime.now()),
      );
    });
  }

  final GoogleSignIn _googleSignIn = GoogleSignIn(
    serverClientId:
        '719113265141-v8ckgmea9bob1nd65f4396n93o16dcqd.apps.googleusercontent.com',
    scopes: ['email', 'profile'],
  );

  String get _baseUrl {
    if (Platform.isAndroid) {
      return 'http://192.168.1.111:5000';
    } else {
      return 'http://192.168.1.111:5000';
    }
  }

  String get _apiUrl => '$_baseUrl/api/auth';

  String get _userApiUrl => '$_baseUrl/api/user';

  /// Regular Login
  Future<LoginResponse> login(String email, String password) async {
    try {
      print('[AUTH] Attempting login for: $email');
      final response = await _dio.post(
        '$_apiUrl/login',
        data: {'email': email, 'password': password},
      );
      print('[AUTH] Login successful');
      return LoginResponse.fromJson(response.data);
    } on DioException catch (e) {
      print(
        '[AUTH] Login DioException: ${e.response?.statusCode} - ${e.response?.data}',
      );
      rethrow;
    } catch (e) {
      print('[AUTH] Login unexpected error: $e');
      rethrow;
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

      print('[AUTH] Registration successful');
      return RegisterResponse.fromJson(response.data);
    } on DioException catch (e) {
      print(
        '[AUTH] Registration DioException: ${e.response?.statusCode} - ${e.response?.data}',
      );
      rethrow;
    } catch (e) {
      print('[AUTH] Registration unexpected error: $e');
      rethrow;
    }
  }

  /// Google Sign In (Native)
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
        data: {'code': authCode},
      );

      print('[AUTH] Native Google Auth successful');
      return LoginResponse.fromJson(response.data);
    } on DioException catch (e) {
      print(
        '[AUTH] Google auth DioException: ${e.response?.statusCode} - ${e.response?.data}',
      );
      rethrow;
    } catch (e) {
      print('[AUTH] Native Google Error: $e');
      rethrow;
    }
  }

  /// Check Authentication
  Future<bool> checkAuth() async {
    try {
      final response = await _dio.get('$_userApiUrl/me');

      if (response.statusCode == 200) {
        final username = response.data['username'];
        final email = response.data['email'];

        await _isar.writeTxn(() async {
          await _isar.userCaches.clear();
          await _isar.userCaches.put(
            UserCache(
              username: username,
              email: email,
              lastLogin: DateTime.now(),
            ),
          );
        });

        print('[AUTH] Cache updated from server');
        return true;
      }
      return false;
    } on DioException catch (e) {
      if (e.response?.statusCode == 401) {
        print('[AUTH] Session expired. Clearing data...');
        await logout();
        return false;
      }

      if (e.type == DioExceptionType.connectionTimeout ||
          e.type == DioExceptionType.connectionError ||
          e.error is SocketException) {
        print('[AUTH] Offline mode. Checking local cache...');

        final cachedUser = await _isar.userCaches.where().findFirst();

        if (cachedUser != null) {
          print(
            '[AUTH] Found cached user: ${cachedUser.username}. Access granted (Offline).',
          );
          return true;
        }
      }

      return false;
    } catch (e) {
      print('[AUTH] Unexpected error during checkAuth: $e');
      return false;
    }
  }

  /// Resend Verification Email
  Future<void> resendVerificationEmail(String email) async {
    try {
      print('[AUTH] Resending verification email to: $email');
      await _dio.post('$_apiUrl/resend-verification', data: {'email': email});

      print('[AUTH] Verification email sent successfully');
    } on DioException catch (e) {
      print(
        '[AUTH] Resend verification DioException: ${e.response?.statusCode} - ${e.response?.data}',
      );
      rethrow;
    } catch (e) {
      print('[AUTH] Resend verification unexpected error: $e');
      rethrow;
    }
  }

  /// Forgot Password
  Future<void> forgotPassword(String email) async {
    try {
      print('[AUTH] Sending password reset email to: $email');
      await _dio.post('$_apiUrl/forgot-password', data: {'email': email});

      print('[AUTH] Password reset email sent successfully');
    } on DioException catch (e) {
      print(
        '[AUTH] Forgot password DioException: ${e.response?.statusCode} - ${e.response?.data}',
      );
      rethrow;
    } catch (e) {
      print('[AUTH] Forgot password unexpected error: $e');
      rethrow;
    }
  }

  /// Logout
  Future<void> logout() async {
    try {
      await _cookieJar.deleteAll();

      await _isar.writeTxn(() async {
        await _isar.userCaches.clear();
      });

      print('[AUTH] Logged out - all data cleared');
    } catch (e) {
      print('[AUTH] Logout error: $e');
    }
  }
}
