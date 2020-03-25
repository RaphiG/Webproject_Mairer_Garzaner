create database if not exists collate utf8mb4_general_ci;
use db_webproject;

create table users(
	id int not null auto_increment,
    firstname varchar(100) null,
    lastname varchar(100) not null,
    gender int not null,
    birthdate date null,
    username varchar(100) not null unique,
    password varchar(128) not null,
    
    constraint id_PK primary Key(id)


)engine=InnoDB;

Insert INTO users VALUES(null, "Davd", "Holzi", 0, "2001-04-15", "david2", sha2("irgendwas", 256));
select * from users;
