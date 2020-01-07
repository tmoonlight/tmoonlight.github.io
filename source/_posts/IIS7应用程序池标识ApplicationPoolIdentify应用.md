---
title: IIS7应用程序池标识ApplicationPoolIdentify应用
date: 2017/4/12 16:08:39
tags:
---


[IIS7应用程序池标识ApplicationPoolIdentify应用](http://www.haiyun.me/archives/iis7-applicationpoolidentify.html)

发布时间：May 27, 2012 // 分类：[IIS](http://www.haiyun.me/category/iis/) // [No Comments](http://www.haiyun.me/archives/iis7-applicationpoolidentify.html#comments)

在IIS6中为防止跨站要为每个站设置不同的应用程序池、匿名浏览用户，IIS7就不用这么麻烦了。  
IIS7中应用程序池默认运行用户为ApplicationPoolIdentity，这是系统动态创建的虚拟帐号，在用户管理中查看无此账号，通过任务管理器查看IIS运行进程用户为应用程序池名称：  
[](http://www.haiyun.me/attachment/326/ "任务管理器查看IIS进程用户.png")  
网站验证用户传递应用池用户，应网站目录保留admini及system用户权限，添加用户"IIS AppPool应用程序池名"相应权限，如IIS AppPoolonovps 

[](http://www.haiyun.me/attachment/329/ "IIS7权限ApplicationPoolIdentify.png")
