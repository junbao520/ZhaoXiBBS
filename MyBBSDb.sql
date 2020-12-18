/*
 Navicat Premium Data Transfer

 Source Server         : 192.168.3.203m
 Source Server Type    : MySQL
 Source Server Version : 50731
 Source Host           : 192.168.3.203:3306
 Source Schema         : MyBBSDb

 Target Server Type    : MySQL
 Target Server Version : 50731
 File Encoding         : 65001

 Date: 11/12/2020 17:27:57
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for PostReplys
-- ----------------------------
DROP TABLE IF EXISTS `PostReplys`;
CREATE TABLE `PostReplys`  (
  `Id` bigint(20) NOT NULL,
  `PostId` int(11) NOT NULL,
  `ReplyContent` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `CreateTime` datetime(0) NULL DEFAULT NULL,
  `CreateUserId` int(11) NOT NULL,
  `EditTime` datetime(0) NULL DEFAULT NULL,
  `EditUserId` int(11) NULL DEFAULT NULL,
  `Up` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Down` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  UNIQUE INDEX `index_PostReplys`(`Id`, `PostId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic PARTITION BY RANGE (PostId)
PARTITIONS 4
(PARTITION `p1` VALUES LESS THAN (20000) ENGINE = InnoDB MAX_ROWS = 0 MIN_ROWS = 0 ,
PARTITION `p2` VALUES LESS THAN (40000) ENGINE = InnoDB MAX_ROWS = 0 MIN_ROWS = 0 ,
PARTITION `p3` VALUES LESS THAN (60000) ENGINE = InnoDB MAX_ROWS = 0 MIN_ROWS = 0 ,
PARTITION `p4` VALUES LESS THAN (MAXVALUE) ENGINE = InnoDB MAX_ROWS = 0 MIN_ROWS = 0 )
;

-- ----------------------------
-- Records of PostReplys
-- ----------------------------
INSERT INTO `PostReplys` VALUES (13, 1, '<p>测试测试</p>', '2020-10-02 22:32:55', 1, NULL, NULL, '', '1');
INSERT INTO `PostReplys` VALUES (14, 1, '[quote]<p>测试测试</p>[/quote]<br/><p>回复测试</p><br/>', '2020-10-02 23:24:01', 1, NULL, NULL, '', NULL);
INSERT INTO `PostReplys` VALUES (15, 1, '[quote]<span style=\"border:1px solid #e5e5e5;background-color: #f5f5f5; font-size: 13px;  font-style: italic ;padding:5px;display: block;\"><p>测试测试</p></span><br/><p>回复测试</p><br/>[/quote]<br/>二次回复测试<br/>', '2020-10-02 23:24:13', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (16, 8, '<p>12345678</p>', '2020-12-01 10:04:10', 1, NULL, NULL, '1', '1');
INSERT INTO `PostReplys` VALUES (17, 9, '<p>sdfsdfs</p>', '2020-12-01 10:10:59', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (18, 9, '<p>123</p>', '2020-12-01 10:11:05', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (19, 8, '[quote]<p>12345678</p>[/quote]<br/>1234<br/>', '2020-12-01 10:13:15', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (20, 8, '[quote]<span style=\"border:1px solid #e5e5e5;background-color: #f5f5f5; font-size: 13px;  font-style: italic ;padding:5px;display: block;\"><p>12345678</p></span><br/>1234<br/>[/quote]<br/><br/>12344<br/>', '2020-12-01 10:14:09', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (21, 1, '[quote]<p>测试测试</p>[/quote]<br/><p>我的回复</p><br/>', '2020-12-01 10:17:25', 1, NULL, NULL, '1', '1');
INSERT INTO `PostReplys` VALUES (22, 6, '<p>werwe</p>', '2020-12-01 16:58:11', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (23, 6, '<p>sdfsd</p>', '2020-12-01 16:59:58', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (24, 6, '<p>sdf</p>', '2020-12-01 17:00:02', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (25, 6, '[quote]<p>sdf</p>[/quote]', '2020-12-01 17:00:04', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (26, 1, '[quote]<span style=\"border:1px solid #e5e5e5;background-color: #f5f5f5; font-size: 13px;  font-style: italic ;padding:5px;display: block;\"><p>测试测试</p></span><br/><p>我的回复</p><br/>[/quote]<br/><br/><br/>胜多负少的<br/>', '2020-12-02 14:34:19', 1, NULL, NULL, NULL, '1');
INSERT INTO `PostReplys` VALUES (27, 1, '<p>sdf</p>', '2020-12-02 14:34:26', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (28, 1, '<p>SDFSD</p>', '2020-12-03 13:54:40', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (29, 1, '[quote]<p>SDFSD</p>[/quote]SDFS', '2020-12-03 13:54:47', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (30, 17, '[quote]null[/quote]sdfds', '2020-12-03 14:24:27', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (31, 8, '<p>sdfsdf</p>', '2020-12-03 14:27:47', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (32, 21, '[quote]null[/quote]sfsdf', '2020-12-04 14:22:39', 1, NULL, NULL, NULL, NULL);
INSERT INTO `PostReplys` VALUES (33, 21, '[quote]<span style=\"border:1px solid #e5e5e5;background-color: #f5f5f5; font-size: 13px;  font-style: italic ;padding:5px;display: block;\">null</span>sfsdf[/quote]<br/>sdfsdfsd<br/>', '2020-12-04 14:22:45', 1, NULL, NULL, '', '1');

-- ----------------------------
-- Table structure for PostTypes
-- ----------------------------
DROP TABLE IF EXISTS `PostTypes`;
CREATE TABLE `PostTypes`  (
  `Id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `PostType` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreateTime` datetime(0) NOT NULL,
  `CreateUserId` int(11) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of PostTypes
-- ----------------------------
INSERT INTO `PostTypes` VALUES (1, 'MOD', '2020-10-02 00:00:00', 1);
INSERT INTO `PostTypes` VALUES (3, '资源', '2020-10-02 00:00:00', 1);
INSERT INTO `PostTypes` VALUES (4, '分享', '2020-10-02 00:00:00', 1);

-- ----------------------------
-- Table structure for Posts
-- ----------------------------
DROP TABLE IF EXISTS `Posts`;
CREATE TABLE `Posts`  (
  `Id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `PostTitle` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PostIcon` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `PostTypeId` int(11) NOT NULL,
  `PostType` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `PostContent1` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Clicks` int(11) NOT NULL,
  `Replys` int(11) NOT NULL,
  `CreateTime` datetime(0) NULL DEFAULT NULL,
  `CreateUserId` int(11) NOT NULL,
  `EditTime` datetime(0) NULL DEFAULT NULL,
  `EditUserId` int(11) NOT NULL,
  `LastReplyTime` datetime(0) NULL DEFAULT NULL,
  `LastReplyUserId` int(11) NOT NULL,
  `Up` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Down` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 24 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Posts
-- ----------------------------
INSERT INTO `Posts` VALUES (1, '【原创！3DM下载站/附件/网盘分流】《全面战争：三国》v1.0-v1.6.0 二十五项修改器 By 风灵月影 [2020.09.06更新]', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 2, '资源', NULL, 68868, 8866, '2020-09-12 13:07:37', 1, '2020-09-13 13:07:49', 1, '2020-09-13 13:07:49', 1, '1', '1');
INSERT INTO `Posts` VALUES (3, '1.6传说这个MOD是消灭敌方势力 可以招被消灭势力的将军', 'https://bbs.3dmgame.com/static/image/common/folder_common.gif', 3, '分享', NULL, 56898, 6689, '2020-09-12 13:10:21', 1, '2020-09-12 13:10:21', 1, '2020-09-12 13:10:21', 1, '1', '1');
INSERT INTO `Posts` VALUES (4, '1.6部队不会逃跑和都是强壮男人（不会疲劳）', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 3, '分享', NULL, 56698, 9685, '2020-09-16 19:31:50', 1, '2020-09-16 19:31:50', 1, '2020-09-16 19:31:50', 1, '1', '1');
INSERT INTO `Posts` VALUES (5, '【适配游戏1.6.0】《无双事件簿：黄巾之风》9月12日增加道具三昧真火并修复194吕布事件。收集神器，集结同伴，直面天命之战！', 'https://bbs.3dmgame.com/static/image/common/folder_common.gif', 1, 'MOD', NULL, 663698, 663698, '2020-09-16 19:33:20', 1, '2020-09-16 19:33:20', 1, '2020-09-16 19:33:20', 1, '1', '1');
INSERT INTO `Posts` VALUES (6, '测试', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', '<p>测试测试测试测试测试测试测试测试测试测试</p>', 1, 0, '2020-10-02 02:51:08', 1, '2020-10-02 02:51:08', 1, '2020-10-02 02:51:08', 1, '1', '1');
INSERT INTO `Posts` VALUES (7, 'test', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', '<p>test</p>', 1, 0, '2020-12-01 09:58:01', 1, '2020-12-01 09:58:01', 1, '2020-12-01 09:58:01', 1, NULL, NULL);
INSERT INTO `Posts` VALUES (8, 'test22', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', '<p>test22</p>', 1, 0, '2020-12-01 10:00:01', 1, '2020-12-01 10:00:01', 1, '2020-12-01 10:00:01', 1, '', '1');
INSERT INTO `Posts` VALUES (9, '243', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', '<p>2342343</p>', 1, 0, '2020-12-01 10:10:39', 1, '2020-12-01 10:10:39', 1, '2020-12-01 10:10:39', 1, '1', '1');
INSERT INTO `Posts` VALUES (11, '23', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', '<p>sdfsd</p>', 1, 0, '2020-12-01 16:56:55', 1, '2020-12-01 16:56:55', 1, '2020-12-01 16:56:55', 1, NULL, NULL);
INSERT INTO `Posts` VALUES (12, 'sdfs', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', '<p>sdfsdf</p>', 1, 0, '2020-12-01 16:58:00', 1, '2020-12-01 16:58:00', 1, '2020-12-01 16:58:00', 1, NULL, '1');
INSERT INTO `Posts` VALUES (13, '123', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 3, '资源', '<p>胜多负少胜多负少说的说的 说的分手<img src=\"https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1606879955969&di=b257b2cc7b23bcc4495b22193221ecc8&imgtype=0&src=http%3A%2F%2Fattach.bbs.miui.com%2Fforum%2F201408%2F07%2F213601f2xz7usscm2z1mjh.jpg\"/></p>', 1, 0, '2020-12-02 08:44:59', 1, '2020-12-02 08:44:59', 1, '2020-12-02 08:44:59', 1, NULL, NULL);
INSERT INTO `Posts` VALUES (14, '胜多负少的', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', '<p>胜多负少的说的三分毒&nbsp;</p>', 1, 0, '2020-12-02 14:34:34', 1, '2020-12-02 14:34:34', 1, '2020-12-02 14:34:34', 1, NULL, NULL);
INSERT INTO `Posts` VALUES (15, '分表标题', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', '<p>分表标题分表标题分表标题</p>', 1, 0, '2020-12-03 12:37:13', 1, '2020-12-03 12:37:13', 1, '2020-12-03 12:37:13', 1, NULL, NULL);
INSERT INTO `Posts` VALUES (17, 'sdfsfsd', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 3, '资源', NULL, 1, 0, '2020-12-03 14:23:13', 1, '2020-12-03 14:23:13', 1, '2020-12-03 14:23:13', 1, NULL, NULL);
INSERT INTO `Posts` VALUES (18, 'sfsdfd', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 3, '资源', NULL, 1, 0, '2020-12-03 14:28:05', 1, '2020-12-03 14:28:05', 1, '2020-12-03 14:28:05', 1, NULL, NULL);
INSERT INTO `Posts` VALUES (19, 'sdfsdfds', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 3, '资源', NULL, 1, 0, '2020-12-03 14:29:52', 1, '2020-12-03 14:29:52', 1, '2020-12-03 14:29:52', 1, NULL, NULL);
INSERT INTO `Posts` VALUES (20, '111111111', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', NULL, 1, 0, '2020-12-03 14:30:11', 1, '2020-12-03 14:30:11', 1, '2020-12-03 14:30:11', 1, NULL, NULL);
INSERT INTO `Posts` VALUES (21, 'sdfsdf', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', NULL, 1, 0, '2020-12-04 14:22:30', 1, '2020-12-04 14:22:30', 1, '2020-12-04 14:22:30', 1, '', '');
INSERT INTO `Posts` VALUES (22, '主从测试', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 3, '资源', NULL, 1, 0, '2020-12-04 21:16:58', 1, '2020-12-04 21:16:58', 1, '2020-12-04 21:16:58', 1, NULL, NULL);
INSERT INTO `Posts` VALUES (23, '水电费水电费', 'https://bbs.3dmgame.com/static/image/common/folder_new.gif', 1, 'MOD', NULL, 1, 0, '2020-12-04 21:20:24', 1, '2020-12-04 21:20:24', 1, '2020-12-04 21:20:24', 1, NULL, NULL);

-- ----------------------------
-- Table structure for PostsContent
-- ----------------------------
DROP TABLE IF EXISTS `PostsContent`;
CREATE TABLE `PostsContent`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PostContent` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of PostsContent
-- ----------------------------
INSERT INTO `PostsContent` VALUES (1, '<p>分表标题分表标题分表标题</p>');
INSERT INTO `PostsContent` VALUES (2, NULL);
INSERT INTO `PostsContent` VALUES (3, NULL);
INSERT INTO `PostsContent` VALUES (4, '<p>sdfsdfdasdfsdfsafsddsfafsdfsdfsdfsdfdssfddssdfsdfsdfsdfsdffsdfsdfsffdsfssfdffff</p>');
INSERT INTO `PostsContent` VALUES (5, '<p>111111111111<br/></p>');
INSERT INTO `PostsContent` VALUES (6, '<p>sdfsdfsdfsdfsdsd</p>');
INSERT INTO `PostsContent` VALUES (7, '<p>主从测试主从测试主从测试主从测试主从测试主从测试</p>');
INSERT INTO `PostsContent` VALUES (8, '<p>是发送到发是的</p>');

-- ----------------------------
-- Table structure for Users
-- ----------------------------
DROP TABLE IF EXISTS `Users`;
CREATE TABLE `Users`  (
  `Id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `UserNo` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UserName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UserLevel` int(11) NOT NULL,
  `Password` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of Users
-- ----------------------------
INSERT INTO `Users` VALUES (1, '1', 'Ace', 16, '111');
INSERT INTO `Users` VALUES (2, 'clay', 'clay', 0, '111');

SET FOREIGN_KEY_CHECKS = 1;
