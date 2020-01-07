---
title: sqlserver全表查找
date: 2017/8/17 5:18:28
tags:
---


sp_msforeachdb 'select "?" AS db, * from [?].sys.tables where LOWER([name]) like ''%stampfieldprice%'''

sp_msforeachdb 'select "?" AS db, * from [?].sys.objects where LOWER([name]) like ''%stampfieldprice%'''
