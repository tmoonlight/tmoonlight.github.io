---
title: [EntityFramework分层]EntityFramework-利用T4分离Entity和DbContext
date: 2016/4/22 1:57:52
tags:
---


**[Entity Framework - 利用T4 分离 Entity 和 DbContext](http://www.cnblogs.com/fangrobert/archive/2011/08/22/2150048.html)**

  


通常情况，我们会在项目中新建一个ClassLibrary的EF.Data层，然后在该层中添加一个ADO.NET Entity Data Model的edmx文件。IDE就会利用ADO.NET Entity Data Model生成基于当前数据库对应的实体类以及实体类对应的数据访问代码。如果你的项目是简单的三层架构（应用层，业务层，数据访问层），你会发现我们再应用层需要用到实体类的话，必须引用EF.Data层，这样一来也就把数据访问的代码暴露给应用层了。这样有悖于我们三层架构的一个初衷。

  


让我们一起来Step by Step的来用T4 分离Entity 和 DbContext

  


 **Step 1 新建Solution**

  


EF.App应用层 - Console Application

EF.Data实体层 - ClassLibrary

EF.DataAccess数据访问层 - ClassLibary

EF.Services业务层 - ClassLibary

  


 **Step 2 添加edmx**

  


打开EFModel.edmx文件，右键菜单选择“Add Code generation Item”,在弹出的窗口选择“ADO.NET DbContext Generator”，EF.DataAccess project中将产生2个T4文件：EFModel.tt, EFModel.Context.tt

  


我想此时你应该有种预感：EFModel.tt是为产生实体而生产的T4 模板文件，而EFModel.Context.tt正是为产生DbContext而生产的T4模板文件。

  


 **Step 3 转移实体**

  


将EFModel.edmx和EFModel.tt 复制到EF.Data,然后再EF.DataAccess中删除EFModel.tt，然后设置EFModel.edmx的Code Generation Strategy设为None[确保EFModel.edmx不自动产生代码]

  


 **Step  4 正确引用**

  


 App 引用 Data 和 Service

 Service 引用 Data

 DataAccess 引用 Data

  


 这样一来App 跟DataAccess之间就用Service隔离开了了，这也就是我们的目的。

  


 **因为T4模板文件我们是可以修改的，所以我们可以对新生成的2个T4文件做一些修改来生产我们想要的代码。例如我们可以修改EFModel.tt文件把生产的实体类加上DataContract，满足在WCF中的应用。**

  


 ** **
