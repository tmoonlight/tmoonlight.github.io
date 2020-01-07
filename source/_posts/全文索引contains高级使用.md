---
title: 全文索引contains高级使用
date: 2016/2/27 13:51:36
tags:
---


. 查询住址在北京的学生  
SELECT student_id,student_name  
FROM students  
WHERE **CONTAINS** ( address, 'beijing' )  
remark: beijing是一个单词，要用单引号括起来。

2\. 查询住址在河北省的学生  
SELECT student_id,student_name  
FROM students  
WHERE **CONTAINS** ( address, '"HEIBEI province"' )  
remark: HEBEI province是一个词组，在单引号里还要用双引号括起来。

3\. 查询住址在河北省或北京的学生  
SELECT student_id,student_name  
FROM students  
WHERE **CONTAINS** ( address, '"HEIBEI province" OR beijing' )  
remark: 可以指定逻辑操作符(包括 AND ，AND NOT，OR )。

4\. 查询有 '南京路' 字样的地址  
SELECT student_id,student_name  
FROM students  
WHERE **CONTAINS** ( address, 'nanjing NEAR road' )  
remark: 上面的查询将返回包含 'nanjing road'，'nanjing east road'，'nanjing west road' 等字样的地址。  
          A NEAR B，就表示条件： A 靠近 B。

5\. 查询以 '湖' 开头的地址  
SELECT student_id,student_name  
FROM students  
WHERE **CONTAINS** ( address, '"hu*"' )  
remark: 上面的查询将返回包含 'hubei'，'hunan' 等字样的地址。  
          记住是 *，不是 %。

6\. 类似加权的查询  
SELECT student_id,student_name  
FROM students  
WHERE **CONTAINS** ( address, 'ISABOUT (city weight (.8), county wright (.4))' )  
remark: ISABOUT 是这种查询的关键字，weight 指定了一个介于 0~1之间的数，类似系数(我的理解)。表示不同条件有不同的侧重。

7\. 单词的多态查询  
SELECT student_id,student_name  
FROM students  
WHERE **CONTAINS** ( address, 'FORMSOF (INFLECTIONAL,street)' )  
remark: 查询将返回包含 'street'，'streets'等字样的地址。  
         对于动词将返回它的不同的时态，如：dry，将返回 dry，dried，drying 等等。
