---
title: springcloud+steeltoe实现微服务架构：服务注册（一）
date: 2018/11/4 19:58:48
tags:
---


# [spring cloud+.net core搭建微服务架构：服务注册（一）](https://www.cnblogs.com/longxianghui/p/7561259.html)

# 背景

公司去年开始使用dotnet core开发项目。公司的总体架构采用的是微服务，那时候由于对微服务的理解并不是太深，加上各种组件的不成熟，只是把项目的各个功能通过业务层面拆分，然后通过nginx代理，项目最终上线。但是这远远没达到微服务的要求，其中服务治理，断路器都没有。我个人理解，我们谈微服务实际上更多的是谈服务治理这块东西，至于各个的服务只是微服务中的应用而已。一次偶然的机会发现了java的spring cloud这套框架，而且支持dotnet core集成（Steeltoe OSS）。所以目前我们的项目架构是spring cloud搭建底层微服务框架，dotnet core来编写业务逻辑。

# spring cloud

spring cloud是java平台提供的一套解决方案，目前市面上来说可能不是最好的微服务解决方案，但是一定是功能最齐全最全的解决方案。提供了一些微服务的基础功能，包括服务治理、负载均衡、断路器、配置中心、API网关等等。

# 服务治理

关于服务治理这块东西，网上太多太多的资料和原理。相信大家也看了很多，但是如何应用到实际的项目场景，为什么要这样做呢？传统的项目，服务与服务之间的调用都是通过URL来访问，如果是集群那么通过一个负载均衡地址来访问，增加或者减少机器都是通过维护负载均衡列表的IP地址来实现。微服务架构下，分散成了N个服务，每个服务又是一个集群，对于一个大项目来说，维护这些配置是非常头疼的。笔者曾经在某知名互联网公司工作过，公司最累最背锅的就是运维团队，基本24小时都在应付各个团队的部署上线工作以及各种配置的维护，而且还经常出错挨骂。那么服务治理就出现在这种应用场景之中，运维工程师不用再维护各个负载均衡节点，由服务中心去统一处理。举个简单例子，一个电商网站，分解成N个服务，其中有一个用户服务，有一个订单服务，用户服务需要调用订单服务，而订单服务是一个集群，对于用户中心来说他只需要知道访问订单中心即可，至于具体访问订单中心的哪台机器由服务中心来调配。

# 搭建服务治理平台

  1. java开发环境和IDE使用请自行百度，笔者java开发的ide使用的intellij idea。

  2. 创建一个spring boot项目，项目名称service-center，添加spring cloud的依赖




<dependency>

<groupId>org.springframework.cloud</groupId>

<artifactId>spring-cloud-starter-eureka-server</artifactId></dependency>

  1. 添加@EnableEurekaServer注解




@EnableEurekaServer@SpringBootApplicationpublic class ServiceCenterApplication {}

  1. 设置应用程序的端口和名称




spring.application.name=service-center

server.port=5000

  1. 启动项目，在浏览器输入<http://localhost:5000/>

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1083293/o_eureka.png)




# 注册服务

  1. 使用vs创建一个dotnet core web api程序

  2. 使用nuget添加Pivotal.Discovery.Client库




<PackageReference Include="Pivotal.Discovery.Client" Version="1.1.0" />

3.Program.cs 设置一个端口

public static void Main(string[] args)

{

var host = new WebHostBuilder()

.UseKestrel()

.UseContentRoot(Directory.GetCurrentDirectory())

.UseIISIntegration()

.UseStartup<Startup>()

.UseApplicationInsights()

.UseUrls("http://*:8010")

.Build();

  


host.Run();

}

  1. Startup.cs




public void ConfigureServices(IServiceCollection services)

{

services.AddDiscoveryClient(Configuration);

// Add framework services.

services.AddMvc();

}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)

{

loggerFactory.AddConsole(Configuration.GetSection("Logging"));

loggerFactory.AddDebug();

  


app.UseMvc();

app.UseDiscoveryClient();

}

  1. appsettings.json




{

"Logging": {

"IncludeScopes": false,

"LogLevel": {

"Default": "Warning"

}

},

"spring": {

"application": {

"name": "serviceone"

}

},

"eureka": {

"client": {

"serviceUrl": "<http://localhost:5000/eureka/>",

"shouldFetchRegistry": false,

"shouldRegisterWithEureka": true

},

"instance": {

"port": 8010

}

}

}

> 如果是团队开发，"shouldRegisterWithEureka"设置成false，防止本地环境注册到开发环境

  1. 启动程序，再次访问<http://localhost:5000/>发现已经注册到服务中心了

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1083293/o_eureka1.png)

  2. 同样的方式我们再创建一个ServiceTwo的项目，修改programe.cs和appsettings.json文件，其它不变




public static void Main(string[] args)

{

var host = new WebHostBuilder()

.UseKestrel()

.UseContentRoot(Directory.GetCurrentDirectory())

.UseIISIntegration()

.UseStartup<Startup>()

.UseApplicationInsights()

.UseUrls("http://*:8011")

.Build();

  


host.Run();

{

"Logging": {

"IncludeScopes": false,

"LogLevel": {

"Default": "Warning"

}

},

"spring": {

"application": {

"name": "servicetwo"

}

},

"eureka": {

"client": {

"serviceUrl": "<http://loclhost:5000/eureka/>",

"shouldFetchRegistry": false,

"shouldRegisterWithEureka": true

},

"instance": {

"port": 8011

}

}

}

  1. 再次访问<http://localhost:5000/>，发现ServiceOne和ServiceTwo都已经注册到服务中心了

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1083293/o_eureka2.png)




# 后记

这样一个简单的服务治理平台就搭建出来了,我们通过spring cloud来创建了一个服务中心,然后通过dotnet core创建了2个服务注册到了服务中心,但是这些离微服务还差的远.服务之间怎么相互调用呢?集群模式怎么处理呢?微服务的统一API网关呢？留下这些问题，且听下回分解。

第二篇文章已经发布。[spring cloud+dotnet core搭建微服务架构：服务发现（二）](http://www.cnblogs.com/longxianghui/p/7576736.html)

# 示例代码

所有代码均上传[github](https://github.com/longxianghui/microservice)。代码按照章节的顺序上传，例如第一章demo1，第二章demo2以此类推。

求推荐，你们的支持是我写作最大的动力，我的QQ群：328438252,交流微服务。

# 传送门

  * [spring cloud+dotnet core搭建微服务架构：服务注册（一）](http://www.cnblogs.com/longxianghui/p/7561259.html)

  * [spring cloud+dotnet core搭建微服务架构：服务发现（二）](http://www.cnblogs.com/longxianghui/p/7576736.html)

  * [spring cloud+dotnet core搭建微服务架构： Api网关（三）](http://www.cnblogs.com/longxianghui/p/7646870.html)

  * [spring cloud+dotnet core搭建微服务架构：配置中心（四）](http://www.cnblogs.com/longxianghui/p/7660752.html)




# 参考资料

java部分

  * [spring cloud文档](https://github.com/SpringCloud/spring-cloud-docs)

  * [纯洁大神spring cloud系列](http://www.cnblogs.com/ityouknow/category/994104.html)




.net部分

  * [SteeltoeOSS文档](http://steeltoe.io/docs/)

  * [SteeltoeOSS源码](https://github.com/SteeltoeOSS/)




  

