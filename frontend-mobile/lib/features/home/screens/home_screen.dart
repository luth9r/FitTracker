import 'package:easy_localization/easy_localization.dart';
import 'package:flutter/material.dart';
import 'package:isar_community/isar.dart';
import 'package:percent_indicator/linear_percent_indicator.dart';
import 'package:shimmer/shimmer.dart';

import '../../../core/app_colors.dart';
import '../../../data/models/user_model.dart';
import '../../../services/auth_service.dart';
import '../../../shared/widgets/floating_bottom_nav.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  bool _isLoading = true;

  UserCache? _user;

  Future<void> _loadUserData() async {
    final authService = AuthService();
    final user = await authService.isar.userCaches.where().findFirst();

    if (mounted) {
      setState(() {
        _user = user;
      });
    }
  }

  @override
  void initState() {
    super.initState();
    Future.delayed(const Duration(seconds: 2), () {
      if (mounted) setState(() => _isLoading = false);
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.colorBgDark,
      body: SafeArea(
        child: Stack(
          children: [
            RefreshIndicator(
              onRefresh: () async =>
                  await Future.delayed(const Duration(seconds: 1)),
              child: SingleChildScrollView(
                padding: const EdgeInsets.fromLTRB(16, 20, 16, 100),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    _buildHeader(_user),
                    const SizedBox(height: 24),
                    _isLoading ? _buildSkeletonStats() : _buildStatsGrid(),
                    const SizedBox(height: 24),
                    _isLoading ? _buildSkeletonGoal() : _buildGoalCard(),
                    const SizedBox(height: 24),
                    _buildRecentWorkoutsHeader(),
                    const SizedBox(height: 12),
                    _isLoading ? _buildSkeletonRecent() : _buildRecentList(),
                  ],
                ),
              ),
            ),
            const Positioned(
              bottom: 12,
              left: 16,
              right: 16,
              child: FloatingBottomNav(active: 'home'),
            ),
          ],
        ),
      ),
      floatingActionButton: Padding(
        padding: const EdgeInsets.only(bottom: 80),
        child: FloatingActionButton(
          backgroundColor: AppColors.colorPrimary,
          onPressed: () {},
          shape: const CircleBorder(),
          child: const Icon(Icons.add, color: Colors.black),
        ),
      ),
    );
  }

  Widget _buildHeader(UserCache? user) => Row(
    mainAxisAlignment: MainAxisAlignment.spaceBetween,
    children: [
      Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            user != null ? 'Hello, ${user.username}!' : 'HOME.TITLE'.tr(),
            style: const TextStyle(
              fontSize: 26,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
          Text(
            'HOME.SUBTITLE'.tr(),
            style: const TextStyle(
              fontSize: 14,
              color: AppColors.colorTextSecondary,
            ),
          ),
        ],
      ),
      const CircleAvatar(
        radius: 22,
        backgroundColor: AppColors.colorCardBg,
        child: Icon(Icons.person_outline, color: AppColors.colorPrimary),
      ),
    ],
  );

  Widget _buildStatsGrid() {
    return GridView.count(
      shrinkWrap: true,
      physics: const NeverScrollableScrollPhysics(),
      crossAxisCount: 2,
      crossAxisSpacing: 16,
      mainAxisSpacing: 16,
      childAspectRatio: 1.3,
      children: [
        _statCard(
          'Total Workouts',
          '12',
          Icons.fitness_center,
          const Color(0xFF5B4B8A),
        ),
        _statCard(
          'Training Days',
          '8',
          Icons.calendar_month,
          const Color(0xFF10B981),
        ),
        _statCard(
          'Streak',
          '4 Days',
          Icons.local_fire_department,
          const Color(0xFFEF4444),
        ),
        _statCard(
          'Total Weight',
          '12.4 T',
          Icons.scale,
          const Color(0xFFF59E0B),
        ),
      ],
    );
  }

  Widget _statCard(String label, String value, IconData icon, Color color) {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.colorCardBg,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: AppColors.colorBorderStrong.withOpacity(0.1)),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            padding: const EdgeInsets.all(8),
            decoration: BoxDecoration(
              color: color.withOpacity(0.2),
              borderRadius: BorderRadius.circular(10),
            ),
            child: Icon(icon, size: 18, color: color),
          ),
          const Spacer(),
          Text(
            label,
            style: const TextStyle(
              fontSize: 12,
              color: AppColors.colorTextSecondary,
            ),
          ),
          Text(
            value,
            style: const TextStyle(
              fontSize: 18,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildGoalCard() {
    return Container(
      padding: const EdgeInsets.all(20),
      decoration: BoxDecoration(
        color: AppColors.colorCardBg,
        borderRadius: BorderRadius.circular(20),
        border: Border.all(color: AppColors.colorBorderStrong.withOpacity(0.1)),
      ),
      child: Column(
        children: [
          Row(
            children: [
              Container(
                padding: const EdgeInsets.all(12),
                decoration: BoxDecoration(
                  color: const Color(0xFF5B4B8A).withOpacity(0.2),
                  borderRadius: BorderRadius.circular(12),
                ),
                child: const Icon(Icons.speed, color: Color(0xFF5B4B8A)),
              ),
              const SizedBox(width: 16),
              const Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Average Volume',
                    style: TextStyle(fontWeight: FontWeight.bold, fontSize: 16),
                  ),
                  Text(
                    'Target: 10,000 kg',
                    style: TextStyle(
                      color: AppColors.colorTextSecondary,
                      fontSize: 12,
                    ),
                  ),
                ],
              ),
            ],
          ),
          const SizedBox(height: 20),
          LinearPercentIndicator(
            lineHeight: 8.0,
            percent: 0.75,
            barRadius: const Radius.circular(10),
            progressColor: AppColors.colorPrimary,
            backgroundColor: AppColors.colorBgDark,
            padding: EdgeInsets.zero,
          ),
          const SizedBox(height: 12),
          const Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text('7,500 kg', style: TextStyle(fontWeight: FontWeight.bold)),
              Text(
                '75% of target',
                style: TextStyle(color: Color(0xFF10B981), fontSize: 12),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildRecentList() {
    return ListView.separated(
      shrinkWrap: true,
      physics: const NeverScrollableScrollPhysics(),
      itemCount: 3,
      separatorBuilder: (_, __) => const SizedBox(height: 12),
      itemBuilder: (context, index) => _recentWorkoutItem(),
    );
  }

  Widget _recentWorkoutItem() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.colorCardBg,
        borderRadius: BorderRadius.circular(16),
      ),
      child: Row(
        children: [
          Container(
            padding: const EdgeInsets.all(10),
            decoration: BoxDecoration(
              color: AppColors.colorPrimary,
              borderRadius: BorderRadius.circular(10),
            ),
            child: const Icon(
              Icons.fitness_center,
              color: Colors.black,
              size: 20,
            ),
          ),
          const SizedBox(width: 16),
          const Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Full Body Power',
                  style: TextStyle(fontWeight: FontWeight.bold),
                ),
                Text(
                  'Yesterday',
                  style: TextStyle(
                    color: AppColors.colorTextSecondary,
                    fontSize: 12,
                  ),
                ),
              ],
            ),
          ),
          const Column(
            crossAxisAlignment: CrossAxisAlignment.end,
            children: [
              Text('65 min', style: TextStyle(fontWeight: FontWeight.w500)),
              Text(
                '4,200 kg',
                style: TextStyle(color: AppColors.colorPrimary, fontSize: 12),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildRecentWorkoutsHeader() => Row(
    mainAxisAlignment: MainAxisAlignment.spaceBetween,
    children: [
      const Text(
        'Recent Workouts',
        style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
      ),
      TextButton(
        onPressed: () {},
        child: const Text(
          'View All',
          style: TextStyle(color: AppColors.colorPrimary),
        ),
      ),
    ],
  );

  // --- SKELETONS ---
  Widget _buildSkeletonStats() => Shimmer.fromColors(
    baseColor: AppColors.colorCardBg,
    highlightColor: AppColors.colorBorderStrong.withOpacity(0.1),
    child: _buildStatsGrid(),
  );

  Widget _buildSkeletonGoal() => Shimmer.fromColors(
    baseColor: AppColors.colorCardBg,
    highlightColor: AppColors.colorBorderStrong.withOpacity(0.1),
    child: Container(
      height: 140,
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(20),
      ),
    ),
  );

  Widget _buildSkeletonRecent() => Shimmer.fromColors(
    baseColor: AppColors.colorCardBg,
    highlightColor: AppColors.colorBorderStrong.withOpacity(0.1),
    child: Column(
      children: List.generate(
        3,
        (i) => Container(
          margin: const EdgeInsets.only(bottom: 12),
          height: 70,
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(16),
          ),
        ),
      ),
    ),
  );
}
