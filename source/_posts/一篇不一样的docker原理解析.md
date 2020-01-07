---
title: 一篇不一样的docker原理解析
date: 2018/12/17 14:13:24
tags:
---


# 一篇不一样的docker原理解析

[![](https://pic4.zhimg.com/da8e974dc_xs.jpg)](https://www.zhihu.com/people/creepyuncle)

[uncle creepy](https://www.zhihu.com/people/creepyuncle)

擅长分析分布式一致性系统，数据处理，系统负载均衡等问题

## 0 引言

在学习docker的过程中，我发现目前docker学习最大的障碍，不是网上的资源太少，而是网上的资源太多，资源太多带来的噪声让学习效率降低不少。而在讲解docker原理上，所有的讲解都是关于cgroups，namespace，aufs以及deviceMapper，这对于一个初学者来说，就是用一堆名词替换另一堆名词，所以我打算写一篇不涉及太多api的原理解析，在这篇解析中，将不会讨论：

  * 一堆堆砌在一起的专有名词，让阅读者云里雾里

  * 一大堆写满了专有名词的图，但是不给太多解释




  


这篇解析将会涉及：

  * 虚拟机的实现原理

  * 虚拟机和容器的区别




在开始讨论前，先抛出一些问题，可先别急着查看答案，讨论的过程可以让答案更有趣，问题如下：

  * [Docker 容器有自己的kernel吗](https://link.zhihu.com/?target=http%3A//superuser.com/questions/889472/docker-containers-have-their-own-kernel-or-not)

  * [docker的kernel version由镜像确定还是由宿主机确定](https://link.zhihu.com/?target=https%3A//groups.google.com/forum/%23%21topic/docker-user/IDz4iQ15t0A)




  


## 1 虚拟机

先来理解一下虚拟机概念，广义来说，虚拟机是一种模拟系统，即在软件层面上通过模拟硬件的输入和输出，让虚拟机的操作系统得以运行在没有物理硬件的环境中（也就是宿主机的操作系统上），其中能够模拟出硬件输入输出，让虚拟机的操作系统可以启动起来的程序，被叫做hypervisor。用一张图来说明这个关系就是：

![](https://pic3.zhimg.com/80/307fc27ab1dbab631edd25496423f0fe_hd.jpg)

在这张图中：

  * 物理机被称为宿主机

  * 虚拟机也被称为guest OS

  * 而被hypervisor虚拟出来的硬件被称为虚拟硬件




比如，举一个大家都很熟悉的例子，在编写android程序时，调试和测试运行都可以在X86架构的台式机或笔记本进行，这就是一个典型的虚拟机例子，在这之中：

  * 宿主机就是台式机或笔记本

  * 虚拟机就是虚拟出来的android

  * 而模拟android的软件就是android box




  


当然android模拟机一个大问题就是：启动速度非常慢，最长可达10分钟或以上，这是因为单纯模拟硬件的输入输出，效率是很差的，所以这样的虚拟机如果真部署在服务器上，速度是感人的。

这个时候，就有计算机科学家提出了非常偷懒的想法：假如我们不模拟硬件输入输出，只是做下真实硬件输入输出的搬运工，那么虚拟机的指令执行速度，就可以和宿主机一致了。当然这前提是宿主机的硬件架构必须和虚拟硬件架构一致。比如，

  * 我们可以在linux的台式机上轻松模拟windows，而且这个windows的运行速度基本上和原生装一个windows速度差不多，因为windows也能被直接安装在这台台式机上。

  * 这个思路对于在windows系统中运行android系统不管用，因为android系统的运行硬件一般是手机（arm系统，可以理解为不同的硬件架构体系和cpu指令集），所以android模拟机还是一样的慢。




  


由于本篇并不是主要关于虚拟机的内容，所以这些点就点到而止，更多详细内容可以参阅：[Hypervisor](https://link.zhihu.com/?target=https%3A//en.wikipedia.org/wiki/Hypervisor)

## 2 容器的概念

一般来说，虚拟机都会有自己的kernel，自己的硬件，这样虚拟机启动的时候需要先做开机自检，启动kernel，启动用户进程等一系列行为，虽然现在电脑运行速度挺快，但是这一系列检查做下来，也要几十秒，也就是虚拟机需要几十秒来启动。

  * 重新来理解虚拟机的概念，计算机科学家发现其实我们创建虚拟机也不一定需要模拟硬件的输入和输出，假如宿主机和虚拟机他们的kernel是一致的，就不用做硬件输入输出的搬运工了，只需要做kernel输入输出的搬运工即可，为了有别于硬件层面的虚拟机，这种虚拟机被命名为 操作系统层虚拟化：[Operating-system-level virtualization](https://link.zhihu.com/?target=https%3A//en.wikipedia.org/wiki/Operating-system-level_virtualization) 也被叫做容器

  * 让我们来回顾虚拟机的概念，在虚拟机的系统中，虚拟机认为自己有独立的文件系统，进程系统，内存系统，等等一系列，所以为了让容器接近虚拟机，也需要有独立的文件系统，进程系统，内存系统，等等一系列，为了达成这一目的，主机系统采用的办法是：只要隔离容器不让它看到主机的文件系统，进程系统，内存系统，等等一系列，那么容器系统就是一个接近虚拟机的玩意了




更多关于容器的内容可以看这份课件：[https://](https://link.zhihu.com/?target=https%3A//courses.engr.illinois.edu/cs423/lectures/VirtOS.pdf)[courses.engr.illinois.edu](https://link.zhihu.com/?target=https%3A//courses.engr.illinois.edu/cs423/lectures/VirtOS.pdf)[/cs423/lectures/VirtOS.pdf](https://link.zhihu.com/?target=https%3A//courses.engr.illinois.edu/cs423/lectures/VirtOS.pdf)

至此就可以回答引言提到的两个问题：

  * [Docker 容器有自己的kernel吗](https://link.zhihu.com/?target=http%3A//superuser.com/questions/889472/docker-containers-have-their-own-kernel-or-not)

  * 没有，docker和宿主机共享kernel

  * [docker的kernel version由镜像确定还是由宿主机确定](https://link.zhihu.com/?target=https%3A//groups.google.com/forum/%23%21topic/docker-user/IDz4iQ15t0A)

  * 由宿主机决定

## 3 how to learn more

    * 关于操作系统层的虚拟化的概念：[Operating-system-level virtualization](https://link.zhihu.com/?target=https%3A//en.wikipedia.org/wiki/Operating-system-level_virtualization) 以及包括freebsd jail有关的一系列其他的操作系统上相似的实现

    * 想要了解更多，docker具体做了什么，可以参考：[一篇不一样的docker原理解析 提高篇 - uncle creepy的文章 - 知乎专栏](https://zhuanlan.zhihu.com/p/22403015)

    * 关于namespace，cgroups，aufs，deviceMapper 可以了解官方文档

    * 在mac os和windows上运行docker的秘密：[Boot2docker by boot2docker](https://link.zhihu.com/?target=http%3A//boot2docker.io/) 就是通过这个叫做boot2docker的玩意启动了一个虚拟linux kernel，所有的docker容器都跑在这个kernel上




  

