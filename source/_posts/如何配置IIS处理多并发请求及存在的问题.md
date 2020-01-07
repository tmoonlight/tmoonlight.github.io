---
title: 如何配置IIS处理多并发请求及存在的问题
date: 2017/4/2 23:37:27
tags:
---


# 如何配置IIS处理多并发请求及存在的问题

时间：2015-08-18 18:06:56      阅读：5340      评论：0      收藏：0      [点我收藏+]

  


标签：

很多时候多线程能快速高效独立的计算数据，应用比较多。

但今天遇到的多进程下的问题更是让人觉得复杂

多进程下static变量都要失效，就目前的平台和产品static使用是很多的，各种session、cache等，完全不适合多进程。

分布式系统之间不能相互使用进程内的变量，必须使用分布式缓存之类的远程容器，否则无法做到跨进程。

同样的Application变量也没法使用，必须做进程间通信。

分布式系统比普通系统复杂得多的，支持几千人在线的系统和支持数十万人在线的系统的架构是不同的。

so，面对如此多的问题，在来谈iis现在支持的多进程和多线程配置，至少能让更多人晓得不同的应用场景了。

 

 

以下内容来自网路：

　　一个[ASP.NET](http://asp.net/)项目在部署到生产环境时，当用户并发量达到200左右时，IIS出现了明显的请求排队现象，发送的请求都进入等待，无法及时响应，系统基本处于不可用状态。因经验不足，花了很多时间精力解决这个问题，本文记录了我查找问题的过程和最后解决方案，供大家参考。

硬件环境：

　　IBM刀片服务器，Intel至强处理器，4[物理](http://cpro.baidu.com/cpro/ui/uijs.php?adclass=0&app_id=0&c=news&cf=1001&ch=0&di=128&fv=17&is_app=0&jk=2d5831cd39f60919&k=%CE%EF%C0%ED&k0=%CE%EF%C0%ED&kdi0=0&luki=1&n=10&p=baidu&q=smileking_cpr&rb=0&rs=1&seller_id=1&sid=1909f639cd31582d&ssp2=1&stid=0&t=tpclicked3_hc&td=1682280&tu=u1682280&u=http%3A%2F%2Fwww%2Eth7%2Ecn%2FProgram%2Fnet%2F201308%2F148023%2Eshtml&urlid=0)核，16个逻辑核心，内存32G

　　Windows Server2008 Enterprise R2， [ASP.NET](http://asp.net/) 4.0 Webform  IIS7.5  集成模式

当发现请求明显延迟，没有被即时处理的现象，首先就要查看Windows自带的性能日志Performance Monitor。

 

　　由于我注意到只有对于.aspx或.ashx的请求才会延迟，而.htm或.jpg文件都是即时响应的，所以很明显问题出在[ASP.NET](http://asp.net/)上，于是我选择了性能监视器中的[ASP.NET](http://asp.net/) 4.0中的2个主要[计数器](http://cpro.baidu.com/cpro/ui/uijs.php?adclass=0&app_id=0&c=news&cf=1001&ch=0&di=128&fv=17&is_app=0&jk=2d5831cd39f60919&k=%BC%C6%CA%FD%C6%F7&k0=%BC%C6%CA%FD%C6%F7&kdi0=0&luki=3&n=10&p=baidu&q=smileking_cpr&rb=0&rs=1&seller_id=1&sid=1909f639cd31582d&ssp2=1&stid=0&t=tpclicked3_hc&td=1682280&tu=u1682280&u=http%3A%2F%2Fwww%2Eth7%2Ecn%2FProgram%2Fnet%2F201308%2F148023%2Eshtml&urlid=0)：Requests Current（当前请求数）, Requests Queued（被排队的请求数）进行观察。通过观察发现，当前请求数达到200左右时，被排队的请求数就从0开始上升，一直到50左右，如果请求数继续上升，则被排队数也随之上升。当被排队的请求数>0时，就意味着这个时候去访问任何.aspx页面，页面都会处于长时间等待中，没有任何响应，直到IIS处理完了其他请求，才会开始处理队列中的请求。也就是说，当排队数长期>0时，系统基本处于不可用的状态。

　　由于这个系统的页面布局比较复杂，采用了大量的Ajax+.ashx的方式，将内容分批展示在页面上，所以对服务器的请求总数会比传统aspx模式来的多一些，一个页面全部加载完毕可能需要5-10秒，但我想这不应该是造成问题的主要原因，就算系统性能较差，IIS也应该足以承受这么小的并发量的。

　　为探究到底是系统写的有问题，还是IIS本身的问题，我抛开我们的系统，写了一个简单的页面，就一个aspx文件，page_load里sleep 10秒。假设这就是一个性能比较差的网站，每个页面都要10秒才能展现，我将其部署在IIS上测试其性能，我使用的是Microsoft Web Application Stress Tool，模拟发起80个线程，每个连接有4个Socket，总共相当于320个并发请求。

　　测试开始后，可以从下图中看到，当前请求数立刻攀升到300左右（图中红线），然后队列中的请求数也上升到300左右（图中绿线），就是说在300个并发请求下，几乎所有的请求都被排队了，系统基本不可用，通过简单的测试，这个问题已经得以重现了。

 

　　随着时间推移，发现绿线慢慢减少，从300下降到100多，就是系统可用性渐渐提高，有一部分用户可以正常使用，但大部分还在排队。

 

 

　　过了6，7分钟，队列中的请求数下降到0左右，并有一些小幅波动。这个时候大部分请求可以被正常处理了。 按照这个现象分析的话，应该是IIS发现有大量请求在队列中，就会试图增加处理线程数以满足要求，但是增长速度有些缓慢。

 

 

　　那是不是系统经过了6，7分钟的适应期之后，以后就一直可以在这个并发量下稳定运作了呢？事实并非如此。我将[压力测试](http://cpro.baidu.com/cpro/ui/uijs.php?adclass=0&app_id=0&c=news&cf=1001&ch=0&di=128&fv=17&is_app=0&jk=2d5831cd39f60919&k=%D1%B9%C1%A6%B2%E2%CA%D4&k0=%D1%B9%C1%A6%B2%E2%CA%D4&kdi0=0&luki=6&n=10&p=baidu&q=smileking_cpr&rb=0&rs=1&seller_id=1&sid=1909f639cd31582d&ssp2=1&stid=0&t=tpclicked3_hc&td=1682280&tu=u1682280&u=http%3A%2F%2Fwww%2Eth7%2Ecn%2FProgram%2Fnet%2F201308%2F148023%2Eshtml&urlid=0)停了几秒，当服务器的请求数降为0以后，再重新开启320个请求的测试，IIS如何表现？从下图可以看到，只要请求数有明显上升，则等待队列又开始达到最高值，然后缓慢下降，重复上面的过程。总结下来就是，当出现较大并发时，IIS的处理请求能力完全跟不上，需要很长时间才能开出足够的线程。

 

 

　　然后我做了一个测试，看看IIS默认情况到底能承受多少请求而不排队？似乎是在100个并发左右，表现尚可，未出现排队。

 

 

当200个左右就不行了。

 

　　

 

　　然后我将测试程序从sleep10秒改成3秒，对于一个应用系统来说，页面平均3秒处理时间的性能该还算比较正常了。但可惜的是，排队现象与处理时间并无太大关系，排队仍然很严重。

 

 

针对以上问题，查阅了相关资料，是否出现排队是和应用程序池的可用线程有关，通过2个方法可以查看系统总线程数和当前可用线程数。

ThreadPool.GetAvailableThreads( out availableWorker, out availableIO);

ThreadPool.GetMaxThreads(out maxWorker, out maxIO);

在队列请求数达到120左右时，通过此方法，得到maxWorker=1600，而availableWorker=1472

因为[服务器](http://cpro.baidu.com/cpro/ui/uijs.php?adclass=0&app_id=0&c=news&cf=1001&ch=0&di=128&fv=17&is_app=0&jk=2d5831cd39f60919&k=%B7%FE%CE%F1%C6%F7&k0=%B7%FE%CE%F1%C6%F7&kdi0=0&luki=7&n=10&p=baidu&q=smileking_cpr&rb=0&rs=1&seller_id=1&sid=1909f639cd31582d&ssp2=1&stid=0&t=tpclicked3_hc&td=1682280&tu=u1682280&u=http%3A%2F%2Fwww%2Eth7%2Ecn%2FProgram%2Fnet%2F201308%2F148023%2Eshtml&urlid=0)是16核的，ASP.NET4.0默认每核可以使用100个线程，所以maxWorker是1600，1600-120=1480，大致相等。

就是说当前有120个线程被用来处理请求，还有1400多个处于空闲。关键问题就是为什么这些空闲线程没有被及时启用？

 

[ASP.NET](http://asp.net/)提供的线程配置参数中，有一个参数是非常重要，但是可能被大家忽略的，就是minWorkerThreads。

意指最小工作线程，根据我们以上的测试结果，IIS托管线程启动非常慢，微软也认识到了这个问题，所以提供此参数用于设置正常情况下的最小工作线程数。比如我们系统白天的并发在200-300之间，则可以设置最小线程为300，这样系统响应速度可以大幅提高。

据此，我对配置文件（machine.config）进行了如下修改。注意都是针对单个CPU的，系统会自动乘以逻辑CPU的数量。

<system.web>

<processModel autoConfig="false" maxWorkerThreads="200" minWorkerThreads="50" />

 相当于最小工作线程设置成了50*16=800。

 

重启IIS后进行测试，我们得到了以下结果：

 

可以看到，由于设置了合理的最小工作线程数，使得IIS无需不断创建新线程来处理请求，系统的响应能力已可以满足并发要求。

 

除此之外，在IIS6之后引入了一个新功能叫Web Garden，其[设计](http://cpro.baidu.com/cpro/ui/uijs.php?adclass=0&app_id=0&c=news&cf=1001&ch=0&di=128&fv=17&is_app=0&jk=2d5831cd39f60919&k=%C9%E8%BC%C6&k0=%C9%E8%BC%C6&kdi0=0&luki=8&n=10&p=baidu&q=smileking_cpr&rb=0&rs=1&seller_id=1&sid=1909f639cd31582d&ssp2=1&stid=0&t=tpclicked3_hc&td=1682280&tu=u1682280&u=http%3A%2F%2Fwww%2Eth7%2Ecn%2FProgram%2Fnet%2F201308%2F148023%2Eshtml&urlid=0)目的是为了在CPU占用较低，但是并发请求数比较多的情况下，提升服务器性能。这正符合我当前的情况，于是我启用了Web Garden，将工作进程数从1调整到5，在任务管理器中可以看到w3wp进程从原来的1增加到了5，然后重新测试。

 

 

同样的320个请求下，可以看到除了一开始的几秒出现了一些排队，后面基本上表现良好，没有请求进入队列。

 

 

　　通过以上两种方式，都可以有效解决本文开头提出的问题。但Web Garden是工作在多进程模式下，如果应用中用到了依赖进程的Session和Cache等对象都必须另想办法，不能保存在[服务器](http://cpro.baidu.com/cpro/ui/uijs.php?adclass=0&app_id=0&c=news&cf=1001&ch=0&di=128&fv=17&is_app=0&jk=2d5831cd39f60919&k=%B7%FE%CE%F1%C6%F7&k0=%B7%FE%CE%F1%C6%F7&kdi0=0&luki=7&n=10&p=baidu&q=smileking_cpr&rb=0&rs=1&seller_id=1&sid=1909f639cd31582d&ssp2=1&stid=0&t=tpclicked3_hc&td=1682280&tu=u1682280&u=http%3A%2F%2Fwww%2Eth7%2Ecn%2FProgram%2Fnet%2F201308%2F148023%2Eshtml&urlid=0)内存中，而且Web Garden的多个进程切换时会有上下文复制，其资源消耗相对单进程要大，这些是需要考虑的因素。

  

