-- create tables

/*
 *@ by oracle
 ******************************/

drop table "tb_Auth" cascade constraints;

drop table "tb_CaseClasses" cascade constraints;

drop table "tb_CaseClassesStatistics" cascade constraints;

drop table "tb_Menu" cascade constraints;

drop table "tb_Orgnization" cascade constraints;

drop table "tb_Role" cascade constraints;

drop table "tb_User" cascade constraints;

/*==============================================================*/
/* Table: "tb_Auth"                                             */
/*==============================================================*/
create table "tb_Auth" 
(
   "Id"                 varchar(32)          not null,
   "IsDel"              smallint             not null,
   "RoleId"             varchar(32)          not null,
   "MenuId"             varchar(32)          not null,
   constraint PK_TB_AUTH primary key ("Id")
);

comment on column "tb_Auth"."Id" is
'主键';

comment on column "tb_Auth"."IsDel" is
'0：未删除；1：已删除';

comment on column "tb_Auth"."RoleId" is
'角色ID';

comment on column "tb_Auth"."MenuId" is
'菜单ID';

/*==============================================================*/
/* Table: "tb_CaseClasses"                                      */
/*==============================================================*/
create table "tb_CaseClasses" 
(
   "Id"                 varchar(32)          not null,
   "IsDel"              smallint             not null,
   "Name"               varchar(128)         not null,
   "Code"               varchar(128)         not null,
   "ParentId"           varchar(32),
   "ThdVal"             int                  default -1 not null,
   constraint PK_TB_CASECLASSES primary key ("Id")
);

comment on table "tb_CaseClasses" is
'案件类型';

comment on column "tb_CaseClasses"."Id" is
'主键';

comment on column "tb_CaseClasses"."IsDel" is
'0：未删除；1：已删除';

comment on column "tb_CaseClasses"."Name" is
'名称';

comment on column "tb_CaseClasses"."Code" is
'代码';

comment on column "tb_CaseClasses"."ParentId" is
'父级ID';

comment on column "tb_CaseClasses"."ThdVal" is
'-1：标识不预警';

/*==============================================================*/
/* Table: "tb_CaseClassesStatistics"                            */
/*==============================================================*/
create table "tb_CaseClassesStatistics" 
(
   "Id"                 varchar(32)          not null,
   "ClassesId"          varchar(32)          not null,
   "OrgId"              varchar(32)          not null,
   "CaseCount"          int                  not null,
   "TotalDate"          number(18,0)         not null,
   constraint PK_TB_CASECLASSESSTATISTICS primary key ("Id")
);

comment on table "tb_CaseClassesStatistics" is
'区域分类案件统计';

comment on column "tb_CaseClassesStatistics"."Id" is
'主键';

comment on column "tb_CaseClassesStatistics"."ClassesId" is
'案件类型ID';

comment on column "tb_CaseClassesStatistics"."OrgId" is
'行政区划ID';

comment on column "tb_CaseClassesStatistics"."CaseCount" is
'案发数量';

comment on column "tb_CaseClassesStatistics"."TotalDate" is
'统计时间';

/*==============================================================*/
/* Table: "tb_Menu"                                             */
/*==============================================================*/
create table "tb_Menu" 
(
   "Id"                 varchar(32)          not null,
   "IsDel"              smallint             not null,
   "Name"               varchar(128)         not null,
   "Path"               varchar(128)         not null,
   "ParentId"           varchar(32),
   constraint PK_TB_MENU primary key ("Id")
);

comment on table "tb_Menu" is
'菜单';

comment on column "tb_Menu"."Id" is
'主键';

comment on column "tb_Menu"."IsDel" is
'0：未删除；1：已删除';

comment on column "tb_Menu"."Name" is
'名称';

comment on column "tb_Menu"."Path" is
'路径';

comment on column "tb_Menu"."ParentId" is
'父级ID';

/*==============================================================*/
/* Table: "tb_Orgnization"                                      */
/*==============================================================*/
create table "tb_Orgnization" 
(
   "Id"                 varchar(32)          not null,
   "IsDel"              smallint             not null,
   "Name"               varchar(128)         not null,
   "Code"               varchar(128)         not null,
   "ParentId"           varchar(32),
   "Glv"                smallint             default 0 not null,
   "Ylv"                smallint             default 0 not null,
   "Olv"                smallint             default 0 not null,
   constraint PK_TB_ORGNIZATION primary key ("Id")
);

comment on table "tb_Orgnization" is
'行政区划';

comment on column "tb_Orgnization"."Id" is
'主键';

comment on column "tb_Orgnization"."IsDel" is
'0：未删除；1：已删除';

comment on column "tb_Orgnization"."Name" is
'名称';

comment on column "tb_Orgnization"."Code" is
'代码';

comment on column "tb_Orgnization"."ParentId" is
'父级ID';

comment on column "tb_Orgnization"."Glv" is
'绿色等级最大值';

comment on column "tb_Orgnization"."Ylv" is
'黄色等级最大值';

comment on column "tb_Orgnization"."Olv" is
'橙色等级最大值';

/*==============================================================*/
/* Table: "tb_Role"                                             */
/*==============================================================*/
create table "tb_Role" 
(
   "Id"                 varchar(32)          not null,
   "IsDel"              smallint             not null,
   "Name"               varchar(128)         not null,
   "Remarks"            varchar(512),
   constraint PK_TB_ROLE primary key ("Id")
);

comment on table "tb_Role" is
'角色';

comment on column "tb_Role"."Id" is
'主键';

comment on column "tb_Role"."IsDel" is
'0：未删除；1：已删除';

comment on column "tb_Role"."Name" is
'名称';

comment on column "tb_Role"."Remarks" is
'备注';

/*==============================================================*/
/* Table: "tb_User"                                             */
/*==============================================================*/
create table "tb_User" 
(
   "Id"                 varchar(32)          not null,
   "IsDel"              smallint             not null,
   "Name"               varchar(32)          not null,
   "Sex"                smallint             not null,
   "Account"            varchar(16)          not null,
   "Passwd"             varchar(32)          not null,
   "SignupDate"         number(18,0)         not null,
   "RoleId"             varchar(16)          not null,
   "Status"             smallint             not null,
   "Avatar"             varchar(256)         not null,
   constraint PK_TB_USER primary key ("Id")
);

comment on table "tb_User" is
'用户';

comment on column "tb_User"."Id" is
'主键';

comment on column "tb_User"."IsDel" is
'0：未删除；1：已删除';

comment on column "tb_User"."Name" is
'名称';

comment on column "tb_User"."Sex" is
'0：未知；1：女性；2：男性；3：其它；';

comment on column "tb_User"."Account" is
'账户';

comment on column "tb_User"."Passwd" is
'密码';

comment on column "tb_User"."SignupDate" is
'创建时间';

comment on column "tb_User"."RoleId" is
'角色ID';

comment on column "tb_User"."Status" is
'0：正常；-1：异常；-2：冻结；';

comment on column "tb_User"."Avatar" is
'头像地址';