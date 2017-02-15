

drop table "tb_Config" cascade constraints;

/*==============================================================*/
/* Table: "tb_Config"                                      */
/*==============================================================*/
create table "tb_Config" 
(
   "Id"                 varchar(32)          not null,
   "IsDel"              smallint             not null,
   "Name"               varchar(128)         not null,
   "Type"               smallint			 not null,
   "Data"               NCLOB				 not null,
   constraint PK_TB_CONFIG primary key ("Id")
);
comment on table "tb_Config" is
'系统配置';

comment on column "tb_Config"."Id" is
'主键';

comment on column "tb_Config"."IsDel" is
'0：未删除；1：已删除';

comment on column "tb_Config"."Name" is
'名称';

comment on column "tb_Config"."Type" is
'类型。1：说明文本；2：';

comment on column "tb_Config"."Data" is
'内容';

INSERT INTO "tb_Config"
  (
    "Id",
    "IsDel",
    "Name",
    "Type",
    "Data"
  )
  VALUES
  (
    '5af390dc3b3128f', -- "Id"
    0, -- "IsDel"
    '首页表格提示文本', -- "Name"
    1, -- "Type"
    '首页表格提示文本' -- "Data"
  );