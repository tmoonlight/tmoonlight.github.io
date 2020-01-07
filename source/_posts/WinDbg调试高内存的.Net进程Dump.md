---
title: WinDbg调试高内存的.Net进程Dump
date: 2019/8/9 4:57:23
tags:
---


# [WinDbg调试高内存的.Net进程Dump](https://www.cnblogs.com/wigis/p/6851918.html)  


WinDbg的学习路径，艰难曲折，多次研究进展不多，今日有所进展，记录下来。

微软官方帮助文档非常全面：<https://msdn.microsoft.com/zh-cn/library/windows/hardware/ff551063(v=vs.85).aspx>

问题发现在服务器上，服务器为WinServer2012 R2 x64。其中一个Windows服务，内存高达7G。但此服务，无什么操作，仅仅定时获取数据，更新数据。使用的EntityFramework。用任务管理器，抓包下来，查看。Dump包有7GB之大。

1、准备环境，加载sos.dll===========================

开始调试，首先WinDbg，分为x64和x86版本。由于Dump运行环境，为64位，故WinDbg也应64位，这就要主机调试环境也应64位。我主机环境为win8.1x64。

运行WinDbg，一定要管理员权限。

然后，确认是否已加载sos.dll。使用.chain命令，查看。若未加载，则加载64位的sos.dll。因为Dump是x64位的，故加载x32位不成，会提示

"The call to LoadLibrary(C:\Windows\[Microsoft.NET](http://microsoft.net/)\Framework\v4.0.30319\sos.dll) failed, Win32 error 0n193

"%1 不是有效的 Win32 应用程序。""

sos.dll微软官方帮助文档，<https://msdn.microsoft.com/en-us/library/bb190764(v=vs.110).aspx>

2、调试===================================

1）、.cls清屏。

2）、!eeheap -gc 查看托管堆，发现2代堆非常大，近2.5G。故确定问题在此。

每个Segment最大255M，begin为起始地址，allocated为结束地址，结束地址减去起始地址等于size,括号里为10进制大小。LOB并不大。

 ![](https://images2015.cnblogs.com/blog/347565/201705/347565-20170514100337722-171065805.png) 

3）、由于dump很大，全分析很慢，故只取一个二代堆的Segment分析。!DumpHeap 000000008f591000  000000009f58ffe0 

4)、分析几分钟后，stat统计节显示String类型最大。故再次分析String类型。!DumpHeap -type String  000000008f591000  000000009f58ffe0 

5）、发现String类型，有很多4KB字符串，不知什么。进一步分析大字符串，!DumpHeap -type String -min 1000  000000008f591000  000000009f58ffe0 

6）、!do 查看对象，发现String是错误信息。联系之前，此系统确实一直出现报错行为，但不影响使用也就没管。

![](https://images2015.cnblogs.com/blog/347565/201705/347565-20170514100647629-1672737371.png)

7）、!gcroot -all 查看引用 ，此操作也比较慢。发现String是一个对象属性，而对象是EF5的Context表记录。

8)、重复6）、7）步骤，发现都是如此。那么这个Context一直有效，其中内容也就一直被引用。

![](https://images2015.cnblogs.com/blog/347565/201705/347565-20170514100741254-2114539012.png)

8）、这时，用IL查看代码，发现确实有个静态的EF的ObjectContext被引用，此对象生命期与进程一致。由于长期运行，加入数据，ObjectContext会一直增加。

关于这个问题，我找了下EF相关文档，似乎没有清理DBContext的命令。

其实，这种操作非常常见，为此，我单独做了测试程序，证明了以上结论。 

[\+ View Code](https://www.cnblogs.com/wigis/p/6851918.html#)

　　

3、帮助信息================

1)、WinDbg可使用!help cmd.获取帮助，比如!help dumpheap

2)、可用搜索内存s -u 0x017a1000 0x017c8c78 "朝生暮死"

或搜索内存到底 .foreach (addr { s -[1]u 0x017a1000 0x017c8c78 "朝生暮死"}){du ${addr}}

3）、

  

