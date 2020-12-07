/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2020-12-7 10:30:08                           */
/*==============================================================*/


drop table if exists PostReplys;

drop table if exists PostTypes;

drop table if exists Posts;

drop table if exists PostsContent;

drop table if exists Users;

/*==============================================================*/
/* Table: PostReplys                                            */
/*==============================================================*/
create table PostReplys
(
   Id                   int(11) unsigned not null auto_increment,
   PostId               int(11) not null,
   ReplyContent         longtext character set utf8,
   CreateTime           datetime,
   CreateUserId         int(11) not null,
   EditTime             datetime,
   EditUserId           int(11) default NULL,
   Up                   longtext character set utf8,
   Down                 longtext character set utf8,
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

/*==============================================================*/
/* Table: PostTypes                                             */
/*==============================================================*/
create table PostTypes
(
   Id                   int(11) unsigned not null auto_increment,
   PostType             varchar(50) character set utf8,
   CreateTime           datetime,
   CreateUserId         int(11) not null,
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

/*==============================================================*/
/* Table: Posts                                                 */
/*==============================================================*/
create table Posts
(
   Id                   int(11) unsigned not null auto_increment,
   PostTitle            varchar(100) character set utf8,
   PostIcon             text character set utf8,
   PostTypeId           int(11) not null,
   PostType             varchar(50) character set utf8,
   PostContent          longtext character set utf8,
   Clicks               int(11) not null,
   Replys               int(11) not null,
   CreateTime           datetime,
   CreateUserId         int(11) not null,
   EditTime             datetime,
   EditUserId           int(11) not null,
   LastReplyTime        datetime,
   LastReplyUserId      int(11) not null,
   Up                   longtext character set utf8,
   Down                 longtext character set utf8,
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

/*==============================================================*/
/* Table: PostsContent                                          */
/*==============================================================*/
create table PostsContent
(
   Id                   int(11) not null auto_increment,
   PostContent          longtext character set utf8,
   primary key (Id)
)
ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

/*==============================================================*/
/* Table: Users                                                 */
/*==============================================================*/
create table Users
(
   Id                   int(11) unsigned not null auto_increment,
   UserNo               varchar(50) character set utf8,
   UserName             varchar(50) character set utf8,
   UserLevel            int(11) not null,
   Password             varchar(50) character set utf8,
   primary key (Id)
)
ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

