---
title: socket经典错误
date: 2014/11/19 17:03:04
tags:
---


**1、经典错误之 无法访问已释放的对象。 对象名:“[System.Net.Sockets.Socket](http://system.net.sockets.socket/)”**
 
 
（1）、问题现场
 

[](http://blog.chinaunix.net/attachment/201208/22/25498312_1345603427G5dY.png)

 

（2）、问题叙述

> 程序中的某个地方调用到了socket.close后，这个socket还被调用，就出爆出上面错误！
> 
>  

（3）、解决方案

> 使用下面一句： 
> 
>   
> 
> 
> if (stsend != null && stsend.Connected)
> 
> stsend != null ：这一句在socket.close之后，会无效的，因为关闭socket连接时已经将其所有连接的资源都释放了；故要与Connected全用；
> 
> stsend.Connected：获取一个值，该值指示 Socket上次操作是Send还是Receive形式连接到远程主机，如果都不是，那就是断开了。

扩展说明：

> 所有非托管资源程序员必须能控制资源释放，诸如数据库连接，SOCKET连接等在使用后都应当显示关闭，如果是长连接，在程序退出时应当确保所有占用的连接都被关闭。

 

 

2、经典错误之 "远程主机强迫关闭了一个现有的连接。"

 

（1）问题现场

 

[](http://blog.chinaunix.net/attachment/201208/22/25498312_13456034423886.png)

 

（2）、问题叙述

>    如下图，对于网络的经典问题，由于是server和client中的任意一方主动断开连接，导致弹出 “远程主机强迫关闭了一个现在连接”的异常。这种情况，会导致程序提示异常，无法继续往下执行。
> 
>  

（3）、解决方案

> 可以通过异常处理的方式，通过异常来提示网络不正常，从而跳出该程序分支,不至于因为异常问题程序瘫痪!
> 
> 如下图，一Catch到这类[System.Net.Sockets.SocketException](http://system.net.sockets.socketexception/)异常（当然这里把它设为Exceptioin异常，抓取的范围更广），那么就用MessageBox给用户一个提示，然后从该分支退出就行了。

 

[](http://blog.chinaunix.net/attachment/201208/22/25498312_13456034565aBg.png)
