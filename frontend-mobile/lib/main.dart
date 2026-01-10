import 'package:fittracker_mobile/services/notification_service.dart';
import 'package:flutter/material.dart';
import 'package:easy_localization/easy_localization.dart';
import 'package:overlay_support/overlay_support.dart';
import 'features/auth/screens/login_screen.dart';
import 'core/app_colors.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await EasyLocalization.ensureInitialized();

  runApp(
    EasyLocalization(
      supportedLocales: const [Locale('uk'), Locale('en')],
      path: 'assets/translations',
      fallbackLocale: const Locale('en'),
      //startLocale: const Locale('uk'),
      child: const FitTrackerApp(),
    ),
  );
}

class FitTrackerApp extends StatelessWidget {
  const FitTrackerApp({super.key});

  @override
  Widget build(BuildContext context) {
    return OverlaySupport.global(
      child: MaterialApp(
        debugShowCheckedModeBanner: false,
        title: 'FitTracker',
        localizationsDelegates: context.localizationDelegates,
        supportedLocales: context.supportedLocales,
        locale: context.locale,

        theme: ThemeData(
          brightness: Brightness.dark,
          scaffoldBackgroundColor: AppColors.colorBgDark,
          primaryColor: AppColors.colorPrimary,
          fontFamily: 'Roboto',
          textTheme: const TextTheme(
            bodyMedium: TextStyle(color: Colors.white),
          ),
        ),

        home: const LoginScreen(),
      ),
    );
  }
}