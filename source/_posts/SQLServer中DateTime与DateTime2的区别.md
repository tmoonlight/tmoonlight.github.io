---
title: SQLServer中DateTime与DateTime2的区别
date: 2018/8/2 8:03:25
tags:
---


随笔-1675  文章-338  评论-30206 

#   


DateTime字段类型对应的时间格式是 yyyy-MM-dd HH:mm:ss.fff ，3个f，精确到1毫秒(ms)，示例 2014-12-03 17:06:15.433 。

DateTime2字段类型对应的时间格式是 yyyy-MM-dd HH:mm:ss.fffffff ，7个f，精确到0.1微秒(μs)，示例 2014-12-03 17:23:19.2880929 。

如果用SQL的日期函数进行赋值，DateTime字段类型要用 GETDATE() ，DateTime2字段类型要用 SYSDATETIME() 。

  

