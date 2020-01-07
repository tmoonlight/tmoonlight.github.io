---
title: dll放入单独文件夹
date: 2013/11/7 2:56:13
tags:
---


软件引用的DLL比较多的时候，全部的DLL都放在exe同目录下，显得比较乱而且不利于管理。为了更好的管理软件中的各种文件应该分门别类的放入相应的文件夹中。

  


下面是解决该问题的一种方法：

  


右键点击项目：属性-》设置，项目会生成一个app.config文件，在<configuration>节点后面添加<runtime>节点再添加下面的节点，重新生成一下就可以达到预期效果了。指定的目录即为生成exe所在路径的次级dll文件目录。

  


<?xml version="1.0" encoding="utf-8" ?>

<configuration>

  <runtime>

    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
    
      <probing privatePath="bin/dll;" />
    
    </assemblyBinding>

  </runtime>

</configuration>

\---------------------

作者：代码养家

来源：CSDN

原文：<https://blog.csdn.net/wangzl1163/article/details/77993432>

版权声明：本文为博主原创文章，转载请附上博文链接！
