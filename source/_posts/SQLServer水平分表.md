---
title: SQLServer水平分表
date: 2018/8/14 0:45:38
tags:
---


SQLServer水平分表

  


最近随着业务量增大，需要对主业务订单的单表进行划分，划分的大概步骤如下

  


  


0.对库增加新的文件组，指定好文件

alter database Test add file (name='test01',filename=N'D:\tmp\data\test01.ndf',size=5Mb,filegrowth=5mb) to filegroup s1

alter database Test add file (name='test02',filename=N'D:\tmp\data\test02.ndf',size=5Mb,filegrowth=5mb) to filegroup s2

alter database Test add file (name='test03',filename=N'D:\tmp\data\test03.ndf',size=5Mb,filegrowth=5mb) to filegroup s3

alter database Test add file (name='test04',filename=N'D:\tmp\data\test04.ndf',size=5Mb,filegrowth=5mb) to filegroup s4

1.新建分区函数，指定划分凭据字段 ,datetime是一个数据类型

CREATE PARTITION FUNCTION pf_card2 (datetime) AS RANGE RIGHT FOR VALUES(

'2016-10-01',

'2017-10-01',

'2018-10-01'

)

2.新建分区架构（parition schema）指定文件组分区路径

CREATE PARTITION SCHEME ps_card AS PARTITION pf_card TO(s1,s2,s3,s4)

3.新建一张表，on字段指定到之前的分区schema中

create TABLE [dbo].[StampOrder3]

(

  


[cTeamName] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NULL,

[CreateDate] [datetime] NULL,

[StampType] [nvarchar] (30) COLLATE Chinese_PRC_CI_AS NULL,

[Status] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL,

[GoodsName] [nvarchar] (100) COLLATE Chinese_PRC_CI_AS NOT NULL,

[DigitalNum] [nvarchar] (30) COLLATE Chinese_PRC_CI_AS NULL,

[cInvDefine1] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NULL,

[cInvDefine2] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NULL,

[cInvDefine3] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NULL,

[cInvDefine4] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NULL,

[cInvDefine5] [nvarchar] (60) COLLATE Chinese_PRC_CI_AS NULL,

  


) ON ps_card([CreateDate])

GO

stamporder3即是分区好的表

  


4.注意事项，建立索引时，涉及到跨表查询的字段需要建立好分区凭据字段
