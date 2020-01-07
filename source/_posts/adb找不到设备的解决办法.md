---
title: adb找不到设备的解决办法
date: 2014/9/8 0:09:35
tags:
---


1.设备第一次连接电脑，执行adb命令，出现以下内容

2.解决方法：将手机的VID添加到 **C:\Users\admin\\.android** 目录下的 **adb_usb.ini** 文件里

设备的VID在计算机右键属性->管理->设备管理器，如图

3.如果没有 **adb_usb.ini** 文件，自己可以新建一个，设备的VID前面要加 **0x**

adb_usb.ini文件格式如：  
0x22da  
0x119a

4.在黑窗口中输入

adb kill-server

adb start-server

adb devices

5.然后就能找到设备了

  

