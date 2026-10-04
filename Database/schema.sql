-- MySQL schema for the Space Invaders database (run in phpMyAdmin or the mysql client).

CREATE DATABASE IF NOT EXISTS spaceinvaders;
USE spaceinvaders;

CREATE TABLE `users` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `surname` VARCHAR(50) NOT NULL,
  `username` VARCHAR(50) NOT NULL,
  `password` VARCHAR(500) NOT NULL,
  `points` INT(50) NOT NULL,
  `salt` VARCHAR(500) NOT NULL,
  PRIMARY KEY (`id`)
);

CREATE TABLE `titles` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `title_name` varchar(50) NOT NULL,
  `price` int(100) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

INSERT INTO `titles` (`ID`, `title_name`, `price`) VALUES
(1, 'Staff', 10),
(2, 'Officer', 100),
(3, 'Lieutenant ', 1000),
(4, 'Captain', 10000),
(5, 'Commander', 100000);

CREATE TABLE `userstitles` (
  `ID` INT(11) NOT NULL AUTO_INCREMENT,
  `userID` INT(11) NOT NULL,
  `itemID` INT(11) NOT NULL,
  PRIMARY KEY (`ID`)
);
