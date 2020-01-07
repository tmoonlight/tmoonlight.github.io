---
title: Activiti速查表
date: 2019/3/27 5:31:22
tags:
---


Activiti数据库介绍：

  


Activiti的后台所有的表都以ACT_开头。第二部分是表

示表的用途的两个字母标识。用途也和服努的API对应。

ACT_RE_*:，RE’表示repository这个前缀的表包含了流程定义和流程静

态资源（图片，规则，等等）•

ACT_RU_*: ’RIT表示runtime。这些运行时的表，包含流程实例，任务，

变重，异步任务，等运行中的数据。Activiti只在流程实例执行过程中保

存这些数据，在流程结束时就会删除这些记录。这样运行时表可以一直

很小速度很快。

ACT_ID_*: ’ID’表示identity。这些表包含身份信息，比如用户，组等等。

ACT_HI_*: 表示history。这些表包含历史数据，比如历史流程实例，变量，任务等等。

ACT_GE_*:通用数据，用于不同场景下，如存放资源文件。

  


taskService

repositoryService

  


  


分支实现

  


会签实现

  


服务任务（Service Task）
