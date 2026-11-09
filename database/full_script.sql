-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema oxide_db
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema oxide_db
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `oxide_db` DEFAULT CHARACTER SET utf8 ;
USE `oxide_db` ;

-- -----------------------------------------------------
-- Table `oxide_db`.`user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `oxide_db`.`user` (
  `user_id` CHAR(36) NOT NULL,
  `login` VARCHAR(45) NOT NULL UNIQUE,
  `password` CHAR(60) NOT NULL,
  `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_active_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `email` VARCHAR(30) NULL UNIQUE,
  `is_verify` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`user_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `oxide_db`.`role`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `oxide_db`.`role` (
  `role_id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`role_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `oxide_db`.`category`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `oxide_db`.`category` (
  `category_id` CHAR(36) NOT NULL,
  `name` VARCHAR(50) NOT NULL,
  `icon_name` VARCHAR(30) NOT NULL DEFAULT 'default',
  PRIMARY KEY (`category_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `oxide_db`.`note`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `oxide_db`.`note` (
  `note_id` CHAR(36) NOT NULL,
  `title` VARCHAR(50) NOT NULL,
  `content` JSON NOT NULL,
  `category_id` CHAR(36) NOT NULL,
  `is_commented` TINYINT(1) NOT NULL DEFAULT 0,
  `creation_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_change_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_arhived` TINYINT(1) NOT NULL DEFAULT 0,
  `is_deleted` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`note_id`),
  INDEX `fk_note_category_idx` (`category_id` ASC) VISIBLE,
  CONSTRAINT `fk_note_category`
    FOREIGN KEY (`category_id`)
    REFERENCES `oxide_db`.`category` (`category_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `oxide_db`.`comment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `oxide_db`.`comment` (
  `comment_id` CHAR(36) NOT NULL,
  `user_id` CHAR(36) NOT NULL,
  `note_id` CHAR(36) NOT NULL,
  `content` VARCHAR(300) NOT NULL,
  PRIMARY KEY (`comment_id`),
  INDEX `fk_comment_users_idx` (`user_id` ASC) VISIBLE,
  INDEX `fk_comment_notes_idx` (`note_id` ASC) VISIBLE,
  CONSTRAINT `fk_comment_users`
    FOREIGN KEY (`user_id`)
    REFERENCES `oxide_db`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `fk_comment_notes`
    FOREIGN KEY (`note_id`)
    REFERENCES `oxide_db`.`note` (`note_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `oxide_db`.`user_notes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `oxide_db`.`user_notes` (
  `user_id` CHAR(36) NOT NULL,
  `note_id` CHAR(36) NOT NULL,
  `role_id` INT NOT NULL DEFAULT 1,
  PRIMARY KEY (`user_id`, `note_id`),
  INDEX `fk_user_notes_notes_idx` (`note_id` ASC) INVISIBLE,
  INDEX `fk_user_notes_roleid_idx` (`role_id` ASC) VISIBLE,
  CONSTRAINT `fk_user_notes_userid`
    FOREIGN KEY (`user_id`)
    REFERENCES `oxide_db`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `fk_user_notes_noteid`
    FOREIGN KEY (`note_id`)
    REFERENCES `oxide_db`.`note` (`note_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `fk_user_notes_roleid`
    FOREIGN KEY (`role_id`)
    REFERENCES `oxide_db`.`role` (`role_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `oxide_db`.`user_categories`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `oxide_db`.`user_categories` (
  `user_id` CHAR(36) NOT NULL,
  `category_id` CHAR(36) NOT NULL,
  PRIMARY KEY (`user_id`, `category_id`),
  INDEX `fk_user_categories_categories_idx` (`category_id` ASC) VISIBLE,
  CONSTRAINT `fk_user_categories_users`
    FOREIGN KEY (`user_id`)
    REFERENCES `oxide_db`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `fk_user_categories_categories`
    FOREIGN KEY (`category_id`)
    REFERENCES `oxide_db`.`category` (`category_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `oxide_db`.`draft`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `oxide_db`.`draft` (
  `draft_id` CHAR(36) NOT NULL,
  `title` VARCHAR(50) NOT NULL,
  `category_id` CHAR(36) NOT NULL,
  `last_change_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `creation_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_arhived` TINYINT(1) NOT NULL DEFAULT 0,
  `is_deleted` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`draft_id`),
  INDEX `fk_draft_categories_idx` (`category_id` ASC) VISIBLE,
  CONSTRAINT `fk_draft_categories`
    FOREIGN KEY (`category_id`)
    REFERENCES `oxide_db`.`category` (`category_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `oxide_db`.`user_drafts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `oxide_db`.`user_drafts` (
  `user_id` CHAR(36) NOT NULL,
  `draft_id` CHAR(36) NOT NULL,
  PRIMARY KEY (`user_id`, `draft_id`),
  INDEX `fk_user_drafts_drafts_idx` (`draft_id` ASC) VISIBLE,
  CONSTRAINT `fk_user_drafts_users`
    FOREIGN KEY (`user_id`)
    REFERENCES `oxide_db`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `fk_user_drafts_drafts`
    FOREIGN KEY (`draft_id`)
    REFERENCES `oxide_db`.`draft` (`draft_id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `oxide_db`.`role`
-- -----------------------------------------------------
START TRANSACTION;
USE `oxide_db`;
INSERT INTO `oxide_db`.`role` (`role_id`, `name`) VALUES (1, 'Владелец');
INSERT INTO `oxide_db`.`role` (`role_id`, `name`) VALUES (2, 'Администратор');
INSERT INTO `oxide_db`.`role` (`role_id`, `name`) VALUES (3, 'Редактор');
INSERT INTO `oxide_db`.`role` (`role_id`, `name`) VALUES (4, 'Зритель');

COMMIT;

