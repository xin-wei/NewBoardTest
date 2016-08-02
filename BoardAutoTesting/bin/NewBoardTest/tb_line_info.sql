/*
Navicat MySQL Data Transfer

Source Server         : boardtest
Source Server Version : 50130
Source Host           : localhost:3306
Source Database       : centercontrol

Target Server Type    : MYSQL
Target Server Version : 50130
File Encoding         : 65001

Date: 2016-08-02 14:46:46
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for tb_line_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_line_info`;
CREATE TABLE `tb_line_info` (
  `Craft_Idx` varchar(100) NOT NULL,
  `Route_Name` varchar(50) DEFAULT NULL,
  `Line_Idx` varchar(50) DEFAULT NULL,
  `Mcu_Ip` varchar(50) NOT NULL,
  `Ate_Ip` varchar(50) NOT NULL,
  `Is_Repair` tinyint(1) DEFAULT NULL,
  `Is_Out` tinyint(1) DEFAULT NULL,
  `Line_ESN` varchar(50) DEFAULT '',
  `Craft_ESN` varchar(50) DEFAULT '',
  `Port_Id` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`Craft_Idx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
