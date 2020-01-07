---
title: 集成模式经典模式kestrel和iis
date: 2018/6/28 12:59:39
tags:
---


集成模式 经典模式 kestrel和iis

  


kestrel+iis等同于 tomcat+nginx

  


集成模式下html等等都通过httpmodule来处理 经典模式只有aspx会通过httpmodule来处理

  


经典模式下html如果通过显示指定isapi 可以执行自定义操作 “但还是要多走一遍iis默认操作？”

  

