/*
 Navicat Premium Data Transfer

 Source Server         : localhostMYSQL
 Source Server Type    : MySQL
 Source Server Version : 50717
 Source Host           : localhost
 Source Database       : QuakeViewer

 Target Server Type    : MySQL
 Target Server Version : 50717
 File Encoding         : utf-8

 Date: 02/12/2017 17:38:43 PM
*/

SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
--  Table structure for `Accounts`
-- ----------------------------
DROP TABLE IF EXISTS `Accounts`;
CREATE TABLE `Accounts` (
  `Id` varchar(20) COLLATE utf8_bin NOT NULL,
  `UserName` varchar(50) COLLATE utf8_bin NOT NULL,
  `Password` varchar(50) COLLATE utf8_bin NOT NULL,
  `CreateDate` datetime NOT NULL,
  `UserType` int(11) DEFAULT NULL,
  `LastLoginDate` datetime DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `AccountType` int(11) DEFAULT '1' COMMENT '1:用户，2:管理员',
  `Mobile` varchar(11) COLLATE utf8_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
--  Records of `Accounts`
-- ----------------------------
BEGIN;
INSERT INTO `Accounts` VALUES ('4750713371772938697', 'araces', '7C4A8D09CA3762AF61E59520943DC26494F8941B', '2017-02-11 20:12:43', '1', '2017-02-11 20:12:43', '1', '2', '18612556662');
COMMIT;

-- ----------------------------
--  Table structure for `AreaParams`
-- ----------------------------
DROP TABLE IF EXISTS `AreaParams`;
CREATE TABLE `AreaParams` (
  `Id` varchar(20) COLLATE utf8_bin NOT NULL,
  `Name` varchar(50) COLLATE utf8_bin NOT NULL,
  `ParentId` varchar(20) COLLATE utf8_bin NOT NULL,
  `GroupNo` int(11) NOT NULL,
  `SiteType` int(11) NOT NULL,
  `IntensityDegree` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
--  Table structure for `Choices`
-- ----------------------------
DROP TABLE IF EXISTS `Choices`;
CREATE TABLE `Choices` (
  `Id` varchar(20) COLLATE utf8_bin NOT NULL,
  `UserId` varchar(20) COLLATE utf8_bin NOT NULL,
  `UserName` varchar(50) COLLATE utf8_bin DEFAULT NULL,
  `FirstChoice` varchar(20) COLLATE utf8_bin DEFAULT NULL,
  `SecondChoice` int(10) DEFAULT NULL,
  `ThirdChoice` int(10) DEFAULT NULL,
  `ForthChoice` int(10) DEFAULT NULL,
  `FifthChoice` int(10) DEFAULT NULL,
  `Sixth` int(11) DEFAULT NULL,
  `FromType` int(11) DEFAULT NULL,
  `MinorResult` int(11) DEFAULT NULL,
  `MajorResult` int(11) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

SET FOREIGN_KEY_CHECKS = 1;
