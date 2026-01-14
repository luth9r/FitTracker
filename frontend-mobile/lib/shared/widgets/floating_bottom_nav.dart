import 'package:flutter/material.dart';

import '../../core/app_colors.dart';

class FloatingBottomNav extends StatelessWidget {
  final String active;

  const FloatingBottomNav({super.key, required this.active});

  @override
  Widget build(BuildContext context) {
    return Container(
      height: 64,
      decoration: BoxDecoration(
        color: const Color(0xFF1E293B),
        borderRadius: BorderRadius.circular(50),
        border: Border.all(color: Colors.white.withOpacity(0.1)),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withOpacity(0.4),
            blurRadius: 32,
            offset: const Offset(0, 8),
          ),
        ],
      ),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: [
          _navItem(Icons.home, 'Home', active == 'home'),
          _navItem(Icons.fitness_center, 'Exercises', active == 'exercises'),
          _navItem(Icons.bar_chart, 'Progress', active == 'progress'),
          _navItem(Icons.person, 'Profile', active == 'profile'),
        ],
      ),
    );
  }

  Widget _navItem(IconData icon, String label, bool isActive) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Icon(
          icon,
          color: isActive
              ? AppColors.colorPrimary
              : AppColors.colorTextSecondary,
          size: 22,
        ),
        const SizedBox(height: 4),
        Text(
          label,
          style: TextStyle(
            fontSize: 10,
            color: isActive
                ? AppColors.colorPrimary
                : AppColors.colorTextSecondary,
          ),
        ),
      ],
    );
  }
}
