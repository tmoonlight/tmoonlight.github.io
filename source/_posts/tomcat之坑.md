---
title: tomcat之坑
date: 2019/11/20 22:05:05
tags:
---


yum 安装tomcat，

  


q:tomcat为啥这么多不同的文件夹都有？ A：因为用了链接文件

  


sudo yum install tomcat

sudo yum install tomcat-webapps tomcat-admin-webapps

  


iptable

无法连接

  


常用conf

  


配置权限

tomcat.conf

tomcat-users.xml 用户配置

server.xml 初始端口配置

  


<tomcat-users>

    <user username="admin" password="admin" roles="manager-gui,admin-gui"/>

</tomcat-users>

  


  

