---
title: ORA-12154：TNS：无法解析指定的连接标识符
date: 2016/5/25 15:49:53
tags:
---


  


[ORA-12154：TNS： **无法解析指定的连接标识符**](http://www.cnblogs.com/gis-dolphin/p/ORA-12154-TNS-%e6%97%a0%e6%b3%95%e8%a7%a3%e6%9e%90%e6%8c%87%e5%ae%9a%e7%9a%84%e8%bf%9e%e6%8e%a5%e6%a0%87%e8%af%86%e7%ac%a6.html)

 **ORA-12154：TNS： **无法解析指定的连接标识符****

 

 **1问题的描述**

Oracle11g server 64bit服务器端安装在Windows Server2008 Enterprise上，安装Oracle11g client 32bit，通过SQL Plus以sysdba身份可以连接数据库，并且创建表空间、用户、授权成功，如下图所示。

 

但是在连接数据库时出现了一些问题：

（1）在客户端配置服务，通过PLSQL连接数据库，出现错误“ora-12154: TNS： **无法解析指定的连接标识符** ”；

（2）通过PLSQL能够正确连接Oracle11g，但是同样的用户名密码在VS2010中却无法连接，报错“ORA-12154: TNS： **无法解析指定的连接标识符** ”；

（3）在VS2010中能够正确访问Oracle11g，但是网站发布之后数据无法访问，报错“ORA -12154: TNS： **无法解析指定的连接标识符** ”。

 **2问题的解决**

 **2.1在客户端配置服务，通过PLSQL连接数据库，出现错误“ORA-12154: TNS： **无法解析指定的连接标识符** ”**

 

（1）解决本问题需要给系统添加环境变量TNS_ADMIN并且将oracle服务器端和客户端“….NETWORK\ADMIN”路径复制给该环境变量，并且将oracle客户端的路径放在前面，如下图所示。

 

（2）然后在PLSQL中（点击“取消”，在没有成功登陆的情况下也可以设置连接属性），点击“tools”->“preferences”，在对话框中设置oralce home和OCI library，其中Oracle home选择所安装的oracle服务器端，oci library设置oracle 32位客户端的“****product\11.2.0\client_1\bin\oci.dll”路径。

 

 **2.2通过PLSQL能够正确连接Oracle11g，但是同样的用户名密码在VS2010中却无法连接，报错“ora-12154: TNS： **无法解析指定的连接标识符** ”**

 

这个问题的出现实在让人无语，正常情况下在VS2010中选择“Server Explorer”->Data Connection->Add Connection输入数据库连接的信息，是可以成功连接oracle数据库的，但是让人不解的是同样的用户名/密码在PLSQL中可以正常登陆，但是在这里却会显示上图所示的错误。这样在运行VS程序时，显示上述错误。

 

更莫名其妙的事情是，我们在客户端重新配置了一个新的服务，并用新的服务去连接的时候，居然成功了。至此，我们反思，只有一种原因，那就是之前我们用的服务是在设置环境变量“TNS_ADMIN”之前配置的，由于编辑了环境变量，导致Oracle无法获取之前的服务名称。

 **2.3在VS2010中能够正确访问Oracle11g，但是网站发布之后数据无法访问，报错“ORA-12154: TNS： **无法解析指定的连接标识符** ”**

当你解决了遇到的问题，觉得自己已经取得成功的时候，往往还会有新的问题出现，现实就是这样无情，它会迫使你将一个问题彻底搞清楚。

在VS程序中能正确访问的Oracle数据库，当网站在IIS中被发布以后，数据库仍旧不能访问，该死的“ **ORA-12154: TNS： **无法解析指定的连接标识符**** ”。

 

##  

## 　　出现本问题是因为Oracle的访问权限问题，第一种情况是Oracle目录缺乏Authenticated Users用户权限，第二种情况是缺乏interner来宾用户权限。  


 **Authenticated Users权限设置** 参考博客 **——**

 ******Asp.Net**  应用程序在IIS发布后无法连接oracle数据库问题的解决方法<http://www.cnblogs.com/zhangronghua/archive/2008/10/07/1305597.html>　　

　　具体方法是在oracle安装目录（d:"oracle"）上右键，属性->安全，选中Authenticated Users将权限的读取和运行项的勾去掉，再打上，然后点击应用，再点击高级，选中“用在此显示的可以应用到子对象的项目替代子对象的权限项目”，点击确定，然后重新启动机器 。

　　需要注意地方是设置完权限后，一定要重启电脑才会权限设置才会生效。

 

## 添加internet来宾用户

　　(1)在Oracle目录上右击选择“属性”，显示如下对话框。

 

　　（2）点击“编辑”进入权限编辑对话框，如下图所示。

　　（3）点击“添加”显示如下对话框。

　　（4）点击“高级”进入用户和组选择对话框。选择internet来宾用户“IIS_IUSRS”，然后一路确定即完成该用户权限的设置。操作完成后请牢记一定要重启电脑，设置才能生效。

　 **关于添加aspnet用户**

 **** 网上有博主说这个问题是由于Oracle目录没有aspnet用户访问权限所致，而有些情况下电脑上虽然安装了vs但是还是会没有aspnet用户，本人按照博客上的方法——下图所示添加aspnet用户，但是该操作虽然能够成功完成，但是系统用户和组中还是不会出现aspnet用户。奇怪是不去管它，只进行Authenticated Users权限设置和添加internet来宾用户也可以在发布后的网站内成功访问Oracle数据库。

 **3参考文献**

##  [1] [ORA-12514 TNS 监听程序当前无法识别连接描述符中请求服务 的解决方法](http://www.blogjava.net/freeman1984/archive/2011/04/15/348350.html)

##  <http://www.blogjava.net/freeman1984/archive/2011/04/15/348350.html>

[2] <http://bbs.csdn.net/topics/390312212>

[3] [Oracle 10g ORA-12154: TNS: could not resolve the connect identifier specified 问题解决!](http://www.blogjava.net/wahahacj/archive/2007/11/19/161689.html)

<http://www.blogjava.net/wahahacj/archive/2007/11/19/161689.html>
