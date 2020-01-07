---
title: android调试踩坑
date: 2014/7/14 13:32:52
tags:
---


ndk 

adb 调试桥 很重要的连接工具

  


adb device

出现adb server version (32) doesn't match this client (36); killing.. 

说明本地有多个版本的server，可以找下 5307端口被哪个目录adb占用了 对症下药，（什么？发现有10几个版本的adb了？没关系，同时起作用的只有一个，把那个找到搞定就ok了。）

  


  

