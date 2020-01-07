---
title: ThreadLocal对象
date: 2018/1/5 4:49:42
tags:
---


.net java里都有ThreadLocal对象，作用是隔离不同线程的变量，好比本地局部变量。

本地局部变量和ThreadLocal对象的区别是什么？线程池，本地变量在线程池无法复用，即使是同一个id的线程销毁了就没有了，但是ThreadLocal可以复用 （大概？）

  

