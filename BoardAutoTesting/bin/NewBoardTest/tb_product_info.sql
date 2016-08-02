/*
Navicat MySQL Data Transfer

Source Server         : boardtest
Source Server Version : 50130
Source Host           : localhost:3306
Source Database       : centercontrol

Target Server Type    : MYSQL
Target Server Version : 50130
File Encoding         : 65001

Date: 2016-08-02 14:46:58
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for tb_product_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_product_info`;
CREATE TABLE `tb_product_info` (
  `RFID` varchar(50) NOT NULL,
  `ESN` varchar(50) NOT NULL,
  `Is_Pass` varchar(50) DEFAULT NULL,
  `Route_Name` varchar(50) DEFAULT NULL,
  `Craft_Idx` varchar(100) DEFAULT NULL,
  `Current_Ip` varchar(20) DEFAULT NULL,
  `Old_Ip` varchar(20) DEFAULT NULL,
  `Action_Name` varchar(20) DEFAULT NULL,
  `ATE_IP` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`RFID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
