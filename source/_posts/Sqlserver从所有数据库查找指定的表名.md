---
title: Sqlserver从所有数据库查找指定的表名
date: 2017/6/1 14:33:48
tags:
---


这个在oracle里是十分容易实现的,sqlserver找了半天终于找到了这个sql: sp_msforeachdb 'select "?" AS db, * from [?].sys.tables where [name] like ''%file%'''
