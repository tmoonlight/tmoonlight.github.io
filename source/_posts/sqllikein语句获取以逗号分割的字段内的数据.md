---
title: sqllikein语句获取以逗号分割的字段内的数据
date: 2018/1/29 1:54:32
tags:
---


# [sql like in 语句获取以逗号分割的字段内的数据](http://www.cnblogs.com/goody9807/archive/2011/07/27/2118107.html)

sql中的某个字段用“，”分隔数据，

需要获取数据的时候直接把“，”拆分成数据，获得一个数据的list。

  


例如：需要查询某字段是否包含一个值，

111是否存在于1111,2111,1112,1121,1113这个字段中 。

因为根据“，”逗号分开，要求的答案是：不在字段中。

用传统的like '%111%',显然不合适，这样虽然111不存在但是依然能查到该条记录。

所以应该用以下语句实现：

select * from Table where ','+columA+',' like '%,111,%'。

实际就是把字段填上一个逗号然后在比较。如果你的字段是用别的分隔符，同理可得。

  


 

假设我们有一字段名为name,其值是用逗号分隔的。

值为：'111,111xu2,1112'。

现在，我们需要编写语句搜索该name值 like '11'的。

按理说，这个name中没有11，我们要的结果就是返回空。

但是如果我们 select * from student where name like '%11%'的话，依然可以正常的查询出结果。

－－－

此时，我们应该采用如下的语句来实现：

 

select * from student where name like '%11%' \--按照我的想法是不能查到的。但结果是查到了

\--解决办法是：将sql字段名前后加上,号，并且比较值前后也加上。

\--特别注意的是：字段名加逗号时，要用字符串连接的形式，不能直接 ',name,'

select * from student where ','+name+',' like '%,111,%'

 

   在与数据库交互的过程中，我们经常需要把一串ID组成的字符串当作参数传给存储过程获取数据。很多时候我们希望把这个字符串转成集合以方便用于in操作。 有两种方式可以方便地把这个以某种符号分隔的ID字符串转成临时表。

 

方式一：通过charindex和substring。 

![](https://images.cnblogs.com/OutliningIndicators/ContractedBlock.gif)代码

执行：select * from  dbo.func_splitstring('1,2,3,4,5,6', ',')

结果：

     ![](https://pic002.cnblogs.com/img/zhangjinming925/201008/2010081708430210.jpg)

 

方式二：通过XQuery（需要SQL Server 2005以上版本）。

![](https://images.cnblogs.com/OutliningIndicators/ContractedBlock.gif)代码

执行：select * from  dbo.func_splitid('1,2,3,4,5,6', ',')

结果：

     ![](https://pic002.cnblogs.com/img/zhangjinming925/201008/2010081708462936.jpg)

  


 select ID,cWhCode,cDepCode,cMemo from CY_WarehouseMap 

WHERE

 ','+(SELECT TOP 1 cMemo FROM CY_Hint WHERE CAST(Name as nvarchar) = '生产管理系统仓库选择')+',' LIKE '%,'+cWhCode+',%'

 

  

