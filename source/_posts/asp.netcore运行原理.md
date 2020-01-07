---
title: asp.netcore运行原理
date: 2018/9/27 17:47:02
tags:
---


## 1.1. 概述

在[ASP.NET](http://asp.net/) Core之前，[ASP.NET](http://asp.net/) Framework应用程序由IIS加载。Web应用程序的入口点由InetMgr.exe创建并调用托管。以初始化过程中触发HttpApplication.Application_Start()事件。开发人员第一次执行代码的机会是处理Application_StartGlobal.asax中的事件。在[ASP.NET](http://asp.net/) Core中，Global.asax文件不再可用，已被新的初始化过程替代。

![](https://ss.csdn.net/p?http://mmbiz.qpic.cn/mmbiz_png/gak2lhVxV6LibJia5SrhPY3kiaeLAr6QQ3Z1zLFy9puPTTvDxFuzWbeUcYbya2mCgdbQWW5xhvuer0SrQbsfDiavMA/640?wx_fmt=png&wxfrom=5&wx_lazy=1)

[ASP.NET](http://asp.net/) Core 应用程序是在.NET Core 控制台程序下调用特定的库，这是[ASP.NET](http://asp.net/) Core应用程序开发的根本变化。所有的[ASP.NET](http://asp.net/)托管库都是从Program开始执行，而不是由IIS托管。也就是说.NET工具链可以同时用于.NET Core控制台应用程序和[ASP.NET](http://asp.net/) Core应用程序。

using System;

using Microsoft.AspNetCore.Hosting;

  


namespace aspnetcoreapp{  

  


 public class Program

   {      

  public static void Main(string[] args)        {            

           var host = new WebHostBuilder()

               .UseKestrel() //指定宿主程序为Kestrel

               .UseStartup<Startup>()// 调用Startup.cs类下的Configure 和 ConfigureServices

               .Build();

           host.Run();

       }

   }

}

以上是Program类中Main方法的示例代码，Main方法负责初始化Web主机，调用Startup和执行应用程序。主机将调用Startup类下面的Configure和ConfigureServices方法。

## 1.2. 文件配置

### 1.2.1. Starup文件配置

对于一个[ASP.NET](http://asp.net/) Core 程序而言，Startup 类是必须的。[ASP.NET](http://asp.net/) Core在程序启动时会从Program类中开始执行，然后再找到UseStartup<Startup>中找到配置的Startup的类，如果不指定Startup类会导致启动失败。

在Startup中必须定义Configure方法，而ConfigureServices方法则是可选的，方法会在程序第一次启动时被调用，类似传统的[ASP.NET](http://asp.net/) MVC的路由和应用程序状态均可在Startup中配置，也可以在此初始化所需中间件。

#### Configure

在[ASP.NET](http://asp.net/) Core 应用程序中Configure方法用于指定中间件以什么样的形式响应HTTP请求。

using System;

using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Http;

  


namespace aspnetcoreapp{  

  


 public class Startup

   {      

 

  public Startup(IConfiguration configuration)    

   {

           Configuration = configuration;

       }    

  

     public IConfiguration Configuration { get; }        

   

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)        {                        

           if (env.IsDevelopment())

           {

               app.UseDeveloperExceptionPage();

               app.UseBrowserLink();

           }            else

           {

               app.UseExceptionHandler("/Home/Error");

           }

           

           app.UseStaticFiles();

                                   

           app.UseMvc(routes =>

           {

               routes.MapRoute(

                   name: "default",

                   template: "{controller=Home}/{action=Index}/{id?}");

           });

       }

   }

}

[ASP.NET](http://asp.net/) Core是通过对IApplicationBuilder进行扩展来构建中间件的， 上面代码中每个use扩展方法都是将中间件添加到请求管道。也可以给Configure方法附加服务（如：IHostingEnvironment）这些服务在ConfigureServices方法中被初始化。

用[ASP.NET](http://asp.net/) Core项目模板添加的应用程序，默认添加的几个中间件：

UseStaticFiles 允许应用程序提供静态资源。

UseMvc 将MVC添加到管道并允许配置路由。

#### ConfigureServices

ConfigureServices方法是应用程序运行时将服务添加到容器中，用[ASP.NET](http://asp.net/) Core项目模板的时候默认会将MVC的服务添加到容器中：

public void ConfigureServices(IServiceCollection services){

   services.AddMvc();

}

接下来举一个例子，在实际应用中ConfigureServices方法和Configure方法配合使用，在[ASP.NET](http://asp.net/) Core中有一个UI开发框架Telerik UI for [ASP.NET](http://asp.net/) Core，它有60多个UI组件，不仅支持[ASP.NET](http://asp.net/) Core的跨平台布署模式，而且还支持前端自适应渲染。

当在项目中应用Telerik UI的时候，首先在项目中引用相关的包，然后再在ConfigureServices方法中将Kendo UI服务添加到容器中：

public void ConfigureServices(IServiceCollection services){

   services.AddKendo();

}

接下来，在Configure中设置Kendo UI

public void Configure(IApplicationBuilder app, IHostingEnvironment env){    //...

   app.UseKendo(env);

}

### 1.2.2. appsetting.json配置

Configuration API 提供了一个基于键-值对来配置应用程序的方法，在运行时可以从多个来源来读取配置。键-值对可以分组形成多层结构。键-值对可以配置在不同的地方，如：文件、内存等，其中放在内存中不能持久化，这里笔者选择将其配置在appsetting.json文件里面。

配置appsetting文件

{

 "key1": "字符串",

 "key2": 2,

 "key3":true,

 "parentObj": {

   "key1": "sub-key1"

 },

 "members": [

   {

     "name": "Lily",

     "age": "18"

   },

   {

     "name": "Lucy",

     "age": "17"

   }

 ]}

一个分层结构的JSON文件，键（如：key1）作为索引器，值作为参数，类型可以为：字符串、数字、布尔、对象、数组。下面具体来看下在应用中怎样使用。

在应用程序加加载和应用配置文件

public static IConfigurationRoot Configuration { get; set; }

  


public static void Main(string[] args = null){  

  


 var builder = new ConfigurationBuilder()

       .SetBasePath(Directory.GetCurrentDirectory())

       .AddJsonFile("appsettings.json");

   Configuration = builder.Build();

   Console.WriteLine($"key1 = {Configuration["key1"]}");

   Console.WriteLine($"key2 = {Configuration["key2"]}");

   Console.WriteLine(

       $"subkey1 = {Configuration["parentObj:key1"]}");

   Console.WriteLine();

   Console.WriteLine("members:");

   Console.Write($"{Configuration["members:0:name"]}, ");

   Console.WriteLine($"age {Configuration["members:0:age"]}");

   Console.Write($"{Configuration["members:1:name"]}, ");

   Console.WriteLine($"age {Configuration["members:1:age"]}");

   Console.WriteLine();

   Console.WriteLine("Press a key...");

   Console.ReadKey();

}

由于加载的是一个JSON文件,所以文件加载进来以后程序可以直接将它当作一个JSON对象来使用。如果有过动态语言使用经验的同学来说这种方式就比较熟悉了。只在这里访问属性的时候将平时常见的.变成了:，这和写的JSON对象更接近。

## 1.3. 处理管道（中间件）

在[ASP.NET](http://asp.net/) Core应用程序中使用中间件，应用程序所做的任何事情（包括服务器中的静态文件）都是由中间件来完成的。没有任何中间件的应用程序在请求的出错时候简单返回404 Not Found。中间件可以让您完全控制请求的处理方式，并且让您的应用程序更加精简。

当接收到一个请求时，请求会交给中间件构成的中间件管道进行处理，管道就是多个中间件构成，请求从一个中间件的一端进入，从中间件的另一端出来，每个中间件都可以对HttpContext请求开始和结束进行处理：

在[ASP.NET](http://asp.net/) Core中可以用Run、Map和Use三种方式来配置HTTP管道。Run 方法称为短路管道（因为它不会调用 next 请求委托）。因此，Run方法一般在管道尾部被调用。Run 是一种惯例，有些中间件组件可能会暴露他们自己的 Run方法，而这些方法只能在管道末尾处运行。下面两段代码是等效的，因为Use没有调用next方法

Run方法示例代码

public void Configure(IApplicationBuilder app, IHostingEnvironment env){

   app.Run(async context =>

       {            await context.Response.WriteAsync("environment " \+ env);

       });

}

Use方法不执行next时示例代码

public void Configure(IApplicationBuilder app, IHostingEnvironment env){

   app.Use(async (context, next) =>

       {            await context.Response.WriteAsync("environment " \+ env);

       });

}

在.NET Core 中约定了Map*扩展被用于分支管道，当前的实现已支持基于请求路径或使用谓词来进入分支。Map扩展方法用于匹配基于请求路径的请求委托。Map只接受路径，并配置单独的中间件管道的功能。

private static void HandleMapUrl(IApplicationBuilder app){

   app.Run(async context =>

   {      

 await context.Response.WriteAsync("Map Url Test Successful");

   });

}public void ConfigureMapping(IApplicationBuilder app, IHostingEnvironment env){

   app.Map("/mapurl", HandleMapUrl);

}

上例中是一个用Map方法来接受路径进入分支管道，也就是说所有基于/mapurl路径请求都会被管道中的HandleMapUrl方法所处理；如果想用谓词来进入中间件分支，则要使用MapThen方法。MapThen方法允许以一种非常灵活的方式构建中间管道。比如可以检测查询字符串是否具有'branch'来进入分支：

private static void HandleBranch(IApplicationBuilder app){

   app.Run(async context =>

   {      

 await context.Response.WriteAsync("Branch used.");

   });

}

 public void Configure(IApplicationBuilder app){

   app.MapWhen(context => {        return context.Request.Query.ContainsKey("branch");

   }, HandleBranch);

}

## 1.4 总结

这节讲解了[ASP.NET](http://asp.net/) Core在运行时首先加载Program类下面的Main方法，在Main方法中指定托管服务器，并调用Startup类中的Configure和ConfigureServices方法等完成初始化；

在[ASP.NET](http://asp.net/) Core中 HTTP请求是以中间件管道的形式进行处理，每个中间件都可以在HTTP请求开始和结束处理对它进行处理；

[ASP.NET](http://asp.net/) Core可以构建跨平台应用，服务运行在Http.Sys（仅适用于Windows平台）和Kestrel上，不需要用IIS进行托管，所以相比传统[ASP.NET](http://asp.net/)来说性能更高效也更加灵活。

  

