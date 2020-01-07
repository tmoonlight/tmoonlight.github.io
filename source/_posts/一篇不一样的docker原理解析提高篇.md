---
title: 一篇不一样的docker原理解析提高篇
date: 2018/12/21 13:30:16
tags:
---


# 一篇不一样的docker原理解析 提高篇

[![](https://pic4.zhimg.com/da8e974dc_xs.jpg)](https://www.zhihu.com/people/creepyuncle)

[uncle creepy](https://www.zhihu.com/people/creepyuncle)

擅长分析分布式一致性系统，数据处理，系统负载均衡等问题

在上一篇 [一篇不一样的docker原理解析 - uncle creepy的文章 - 知乎专栏](https://zhuanlan.zhihu.com/p/22382728) 中，主要讨论了容器和虚拟机的区别，在实现细节上并没有深入，只是点到即止，在这篇提高篇中，将详细讨论容器的实现细节，当然我不希望把文章写成对于kernel man page的简单翻译，所以不熟悉内核和linux api的读者可以不急着点叉，这会是一篇不一样的讨论容器实现的提高篇。

在开始讨论前，先抛出一个问题：

  * [linux启动流程](https://link.zhihu.com/?target=http%3A//www.golinuxhub.com/2014/03/step-by-step-linux-boot-process.html) 中，容器需要使用其中的几步？ ![](https://pic3.zhimg.com/80/9881b7d20842e3a8efa097ef63291a1b_hd.jpg)




  


如图，linux的启动流程

## 0 从fork说起

在讲解容器之前，先来谈谈linux实现进程的原理，linux实现进程的方法为fork，实现的方式分为两个步骤：

  1. 在内存中复制一个父进程，得到“子进程”，此时子进程就是父进程上下文的简单克隆，内容完全一致

  2. 设置子进程的 pid，parent_pid，以及其他和父进程不一致的内容




## 1 namespace让进程隔离更灵活

从进程被制造的步骤可以看出，进程大部分资源和父进程共享，如果需要制造一个看起来像虚拟机的进程，我们需要比普通的进程多做几步。

  * 可以自定义rootfs，比如我们把整个ubuntu发行版的可执行文件以及其他文件系统都放在目录/home/admin/ubuntu/ 下，当我们重定义rootfs = /home/admin/ubuntu 后，则该文件地址被印射为 "/"

  * 把自身pid 印射为0，并看不到其他任何的pid，这样自身的pid成为系统内唯一存在pid，看起来就像新启动了系统

  * 用户名隔离，可以把用户名设置为“root”

  * hostname隔离，可以另取一个hostname，成为新启动进程的hostname

  * IPC隔离，隔离掉进程之间的互相通信

  * 网络隔离，隔离掉进程和主机之间的网络




如果做完这几步，至少在进程自身看来，和虚拟机执行环境上已经区别不大了，对应到linux系统中，这几个隔离需要的方法：[clone(2) - Linux manual page](https://link.zhihu.com/?target=http%3A//man7.org/linux/man-pages/man2/clone.2.html)

而clone方法和fork方法，在复制上下文的时候，调用的都是syscall_clone() 本质上它们是差不多的。

## 2 其实docker是一个内核的搬运工

所以虽然docker帮助我们准备好了rootfs地址，镜像里面的文件，以及各种资源隔离的配置，但是在启动一个容器的时候，它只是调用系统中早已内置的可以隔离资源的方法，而kernel支持这些方法，也是在创建进程的方法上做了一层资源隔离的扩展而已。

这就解释了docker两个特性：

  * 启动速度快，因为本质来说容器和进程差别没有想象中的大，共享了很多代码，流程也差的不多

  * linux内核版本有最低的要求，因为linux是在某个版本后开始支持隔离特性




## 3 容器内创建进程 --- Think a step further

这是我认为整个容器实现里面最优美的一点：

  * 内核开发者实现了容器的资源隔离一系列隔离后，内核开发者就不需要为容器内创建进程单独再做任何多余的工作了。

  * 在fork方法中，第一步就是继承父进程的一切，而这一切包含了父进程已有的资源隔离，所以容器进程创建的进程天然继承容器所有的一切资源隔离 ------ 就和虚拟机的pid = 0 的进程创建子进程所拥有的一样




  


## 4 One more thing

让我们再来看看开篇提出的问题：

[linux启动流程](https://link.zhihu.com/?target=http%3A//www.golinuxhub.com/2014/03/step-by-step-linux-boot-process.html) 中，容器需要使用其中的几步？

看完了fork，clone以及一大堆隔离后，相信读者很容易有答案了，这中间容器做完了隔离之后就算启动完毕，根本就不会来做kernel init之类的步骤，所以答案是一步都不用。

## 5 how to learn more

  * 比较除docker外其他的容器类产品，如coreOS，LXC

  * 了解linux如何做隔离，请参考：[namespaces(7)](https://link.zhihu.com/?target=http%3A//man7.org/linux/man-pages/man7/namespaces.7.html)

  * 了解freebsd如何做隔离，请参考：[freebsd jail](https://link.zhihu.com/?target=https%3A//www.freebsd.org/doc/handbook/jails.html)

  * docker 其实真正想做的事情是把资源隔离的接口标准化（最新的版本里windows的接口也被抽象到了docker自己的体系），严格说它是所有相似资源隔离的一层抽象和搬运工

  * docker 镜像很小的优势，主要是靠AUFS实现的，本篇不详细说明，因为AUFS在docker原理介绍的口水文里被用滥了，随便搜搜到处都是，而且我很不喜欢官方用的集装箱比喻




  

