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

 Date: 02/09/2017 14:58:10 PM
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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

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
  `FirstChoice` varchar(10) COLLATE utf8_bin DEFAULT NULL,
  `SecondChoice` varchar(10) COLLATE utf8_bin DEFAULT NULL,
  `ThirdChoice` varchar(10) COLLATE utf8_bin DEFAULT NULL,
  `ForthChoice` varchar(10) COLLATE utf8_bin DEFAULT NULL,
  `FifthChoice` varchar(10) COLLATE utf8_bin DEFAULT NULL,
  `FromType` int(11) DEFAULT NULL,
  `MinorResult` int(11) DEFAULT NULL,
  `MajorResult` int(11) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

SET FOREIGN_KEY_CHECKS = 1;
