/*
 Navicat Premium Data Transfer

 Source Server         : mcbkr-dev
 Source Server Type    : MySQL
 Source Server Version : 80022
 Source Host           : 192.168.1.47:3306
 Source Schema         : BookStore

 Target Server Type    : MySQL
 Target Server Version : 80022
 File Encoding         : 65001

 Date: 26/04/2022 13:48:25
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for MC_User
-- ----------------------------
DROP TABLE IF EXISTS `MC_User`;
CREATE TABLE `MC_User`  (
  `UID` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Account` int(0) NOT NULL COMMENT '平台账号(MS+6位数字)',
  `Mobile` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '手机号',
  `Email` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '邮箱',
  `Pwd` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '密码',
  `Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '姓名',
  `DataStatus` tinyint(0) UNSIGNED NOT NULL DEFAULT 0,
  `Gender` tinyint(0) UNSIGNED NOT NULL DEFAULT 0 COMMENT '性别',
  `Birthdate` date NULL DEFAULT NULL COMMENT '出生日期',
  `CountryID` int(0) NOT NULL DEFAULT 0 COMMENT '国籍',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '备注',
  `IP` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '' COMMENT '注册ip',
  `AreaCode` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '86' COMMENT '手机区号(中国大陆 86,中国香港 852,中国澳门 853,中国台湾886)',
  `CreateTime` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0) COMMENT '创建时间',
  PRIMARY KEY (`UID`) USING BTREE,
  UNIQUE INDEX `Account`(`Account`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
