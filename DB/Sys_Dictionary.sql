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

 Date: 26/04/2022 13:48:34
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for Sys_Dictionary
-- ----------------------------
DROP TABLE IF EXISTS `Sys_Dictionary`;
CREATE TABLE `Sys_Dictionary`  (
  `Dic_ID` int(0) NOT NULL AUTO_INCREMENT,
  `Config` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreateDate` datetime(0) NULL DEFAULT NULL,
  `CreateID` int(0) NULL DEFAULT NULL,
  `Creator` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `DBServer` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `DbSql` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `DicName` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `DicNo` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Enable` tinyint(0) NOT NULL,
  `Modifier` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ModifyDate` datetime(0) NULL DEFAULT NULL,
  `ModifyID` int(0) NULL DEFAULT NULL,
  `OrderNo` int(0) NULL DEFAULT NULL,
  `ParentId` int(0) NOT NULL,
  `Remark` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Dic_ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 76 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
