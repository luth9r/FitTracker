import 'package:isar_community/isar.dart';

part 'user_model.g.dart';

@collection
class UserCache {
  Id id = Isar.autoIncrement;

  @Index(unique: true)
  late String email;
  late String username;
  late DateTime lastLogin;

  UserCache({
    required this.email,
    required this.username,
    required this.lastLogin,
  });
}
