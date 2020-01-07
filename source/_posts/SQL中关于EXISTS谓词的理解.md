---
title: SQL中关于EXISTS谓词的理解
date: 2019/2/2 9:21:35
tags:
---


# SQL中关于EXISTS谓词的理解

2017年12月06日 15:35:14 阅读数：1502更多

个人分类： [数据库](https://blog.csdn.net/qq_19782019/article/category/7330212)

版权声明：本文为博主原创文章，未经博主允许不得转载。 <https://blog.csdn.net/qq_19782019/article/details/78730882>

  在SQL语言中，EXISTS（存在）谓词是一个非常重要的查询关键词。

 

让我们先看看EXISTS的用法：EXISTS代表存在量词。带有EXISTS谓词的子查询不返回任何数据，只产生逻辑真值“true”或逻辑假值“false”。

例如，以下的SQL语句：

 

SELECT sname

FROM student

WHERE exists

(

SELECT *

FROM sc

WHERE sc.sno=student.sno AND cno='1;

)

 

 

本查询涉及表student和sc表。我们可以这样理解上面的SQL语句做的事情：在student表中从头到尾每次取一个元组出来，用这个元组的sno与sc表所有的元组做比较，如果比较条件成立（sc表中存在sno值等于student.sno中值并且其cno='1'的元组），即exists语句中的SQL语句有返回值过来，则EXISTS返回给上一级元组一个true值，则表示允许现在student表中的这个元组可以放入结果表中。

![](https://img-blog.csdn.net/20171206153207079?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMTk3ODIwMTk=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/Center)

 

使用exists后，若内层查询结果非空，则外层的WHERE子句返回真值，否则返回假值。

由exists引出的子查询，其目标列表达式通常都用*，因为EXISTS的子查询只返回真值或者假值，不返回选择出来的结果，因此，你给什么样的列名最后返回的都是true或者false，所以给出实际列名无意义。

 

对于如何写带有EXISTS查询的子句，我是这样理解的：

1.首先子查询中必须要有依赖父查询的条件，即我们单独把子查询的select语句提出来不能正常运行。

2.每次查询时父查询表中的一个元组对子查询所有的元组进行判定，如果为true则父查询中的这个元组允许放入结果表，否则进行父查询下一个元组的判定。

  

