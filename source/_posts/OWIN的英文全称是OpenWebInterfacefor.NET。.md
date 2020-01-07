---
title: OWIN的英文全称是OpenWebInterfacefor.NET。
date: 2018/5/23 12:12:48
tags:
---


# OWIN的英文全称是Open Web Interface for .NET。

如果仅从名称上解析，可以得出这样的信息：OWIN是针对.NET平台的开放Web接口。

那Web接口是谁和谁之间的接口呢？是Web应用程序与Web服务器之间的接口，OWIN就是.NET Web应用程序与Web服务器之间的接口。

为什么需要这样一个接口呢？因为.NET Web应用程序是运行于Web服务器之中的，.NET Web应用程序需要通过Web服务器接收用户的请求，并且通过Web服务器将响应内容发送用户。如果没有这样一个接口，.NET Web应用程序就要依赖于所运行的具体Web服务器，比如[ASP.NET](http://asp.net/)应用程序要依赖于IIS。有了这个接口，[ASP.NET](http://asp.net/)应用程序只需依赖这个抽象接口，不用关心所运行的Web服务器。

所以，OWIN的作用就是通过引入一组抽象接口，解耦了.NET Web应用程序与Web服务器，再次体现了接口的重要性。在软件开发中，每次解耦都是一次很大的进步。

【进一步的理解】

OWIN是对[ASP.NET](http://asp.net/) Runtime的抽象。

[ASP.NET](http://asp.net/) 5.0是OWIN的一种实现

通过下面几张图可以更直观地理解：

![](https://images0.cnblogs.com/blog/110119/201504/012233035141016.jpg)

 

![](https://images0.cnblogs.com/blog/110119/201504/012233122018495.jpg)

 

![](https://images0.cnblogs.com/blog/110119/201504/012233143894393.jpg)

 

# ![](https://images0.cnblogs.com/blog/110119/201504/012233163571293.jpg)   


  

