---
title: 使用NSSM将exe封装为服务
date: 2017/3/23 23:55:16
tags:
---


# [使用NSSM将exe封装为服务](http://www.cnblogs.com/TianFang/p/7912648.html)

NSSM是一个服务封装程序，它可以将普通exe程序封装成服务，使之像windows服务一样运行。同类型的工具还有微软自己的srvany，不过nssm更加简单易用，并且功能强大。它的特点如下：

  1. 支持普通exe程序（控制台程序或者带界面的Windows程序都可以）
  2. 安装简单，修改方便
  3. 可以重定向输出（并且支持Rotation）
  4. 可以自动守护封装了的服务，程序挂掉了后可以自动重启
  5. 可以自定义环境变量



这里面的每一个功能都非常实用，使用NSSM来封装服务可以大大简化我们的开发流程了。

开发的时候是一个普通程序，降低了开发难度，调试起来非常方便

安装简单，并且可以随时修改服务属性，更新也更加方便

可以利用控制台输出直接实现一个简单的日志系统

不用考虑再加一个服务实现服务守护功能

我觉得它还可以需要增加的一个功能是将输入输出重定向为一个tcp连接，这样可以通过telnet的方式实现程序的交互了，那样就更加好用了。

下面就简单的介绍一下如何使用这个工具。

首先去nssm的[官网](https://nssm.cc/)下载

 

服务安装：

服务安装可以使用如下命令： nssm install <servicename>

执行此命令后，会出现一个界面，基本上看着就知道怎么用了，大多数情况下，只需要填第一个界面的程序路径就可以了。

其它界面的是高级参数的配置，可以根据需要自行选择。

参数填完后执行"install service"按钮即可将服务安装到系统，可以使用系统的服务管理工具查看了。

当然，如果要自动化安装，可以直接带上程序路径： nssm install <servicename> <program> [<arguments>]

NSSM本身win7及以上的系统基本都是支持的，我测试过win7，2008,2016系统，都是没有问题的，如果安装失败，请首先检查是否装了某国产管家或国产杀毒软件。

安装完成后，服务还没有启动，需要通过下面的服务管理的命令启动服务。

 

服务管理：

服务管理主要有启动、停止和重启，其命令如下：

启动服务：　nssm start <servicename>

停止服务： nssm stop <servicename>

重启服务:    nssm restart <servicename>

当然，也可以使用系统自带的服务管理器操作和使用系统的命令。

 

修改参数：

NSSM安装的服务修改起来非常方便，命令如下：

nssm edit <servicename>

会自动启动操作界面，直接更改即可。

 

服务删除：

服务删除可以使用如下命令之一：

nssm remove <servicename>

nssm remove <servicename> confirm

功能没有大的区别，后面的命令是自动确认的，没有交互界面。

 

命令行：

服务自动化需要使用更多的命令行，具体参看官方文档： [Managing services from the command line](https://nssm.cc/commands)

如下是一个安装Jenkins服务的示例：

nssm install Jenkins %PROGRAMFILES%\Java\jre7\bin\java.exe

nssm set Jenkins AppParameters -jar slave.jar -jnlpUrl <https://jenkins/computer/%COMPUTERNAME%/slave-agent.jnlp> -secret redacted

nssm set Jenkins AppDirectory C:\Jenkins

nssm set Jenkins AppStdout C:\Jenkins\jenkins.log

nssm set Jenkins AppStderr C:\Jenkins\jenkins.log

nssm set Jenkins AppStopMethodSkip 6

nssm set Jenkins AppStopMethodConsole 1000

nssm set Jenkins AppThrottle 5000

nssm start Jenkins

 

其它教程：

这里找了网上一些关于nssm的使用教程，感兴趣的朋友可以参考一下：

<https://gogs.io/docs/installation/run_as_windows_service>

<http://www.huangwenchao.com.cn/2016/10/windows-service-wrapper.html>

<http://keenwon.com/1289.html>

  

