---
title: springcloud+steeltoe实现微服务架构：配置中心（四）
date: 2018/10/14 8:11:54
tags:
---


# [spring cloud+.net core搭建微服务架构：配置中心（四）](https://www.cnblogs.com/longxianghui/p/7660752.html)

# 前言

我们项目中有很多需要配置的地方，最常见的就是各种服务URL地址，这些地址针对不同的运行环境还不一样，不管和打包还是部署都麻烦，需要非常的小心。一般配置都是存储到配置文件里面，不管多小的配置变动，都需要对应用程序进行重启，对于分布式系统来说，这是非常不可取的。所以配置中心就在这种场景孕育出来，能够适配不同的环境，正在运行的程序不用重启直接生效。

# 介绍

现在开始介绍我们今天的主角spring cloud config，我觉得它最大的优点就是可以和git做集成，使用起来非常方便。spring cloud config包含服务端和客户端，服务端提供配置的读取和配置仓库，客户端来获取配置。

> 也可以使用svn或者文件来存储配置文件，我们这里只讲Git的方式

# 业务场景

我们模拟一个业务场景，有一个远程配置文件我们通过应用程序获取它。

# 代码实现

我们需要创建2个应用程序:配置服务服务端（Java），配置服务客户端（.Net Core）和一个Github仓库。

使用IntelliJ IDEA创建一个spring boot项目，创建配置中心服务端，端口设置为5100

##### pom.xml

<dependency>

<groupId>org.springframework.cloud</groupId>

<artifactId>spring-cloud-config-server</artifactId></dependency>

##### ConfigServerApplication.java

@SpringBootApplication@EnableConfigServerpublic class ConfigServerApplication {

  


public static void main(String[] args) {

SpringApplication.run(ConfigServerApplication.class, args);

}

}

##### application.properties

server.port=5100

spring.application.name=config-server

#git仓库地址

spring.cloud.config.server.git.uri=https://[github.com/longxianghui/configs.git](http://github.com/longxianghui/configs.git)#git用户名和密码#spring.cloud.config.server.git.username=xxx#spring.cloud.config.server.git.password=xxx#git目录下的文件夹，多个用逗号分割#spring.cloud.config.server.git.search-paths=xxx,xxx,xxx

使用Github创建一个仓库，并提交3个文件，文件内容如下（注意yml格式）

##### demo-dev.yml

name: mickey

age: 3env: test

##### demo-test.yml

name: fiona

age: 28env: test

##### demo-prod.yml

name: leo

age: 30env: prod

> 配置文件命名规则{application}-{profile}.yml
> 
> 支持yml和properties格式

运行配置中心服务端

在浏览器输入<http://localhost:5001/demo/dev>

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1095508/o_1.png)

再访问<http://localhost:5001/demo/test>

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1095508/o_2.png)

再访问<http://localhost:5001/demo/prod>

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1095508/o_3.png)

通过上面3个URL我们发现配置中心通过REST的方式将配置信息返回。

配置服务REST规则如下：

> /{application}/{profile}[/{label}]
> 
> /{application}-{profile}.yml
> 
> /{label}/{application}-{profile}.yml
> 
> /{application}-{profile}.properties
> 
> /{label}/{application}-{profile}.properties

下面我们再看看.NET程序如何读取配置信息呢？

创建一个 .net core web api程序，端口5101

##### nuget引用

<PackageReference Include="Steeltoe.Extensions.Configuration.ConfigServer" Version="1.1.0" />

##### appsettings.json

{

"spring": {

"application": {

"name": "demo"//与配置文件的名称对应

},

"cloud": {

"config": {

"uri": "[http://localhost:5100](http://localhost:5100/)",

"env": "dev" //与环境名称对应

}

}

},

"Logging": {

"IncludeScopes": false,

"LogLevel": {

"Default": "Warning"

}

}

}

##### Startup.cs

public Startup(IHostingEnvironment env)

{

var builder = new ConfigurationBuilder()

.SetBasePath(env.ContentRootPath)

.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)

.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)

.AddEnvironmentVariables()

.AddConfigServer(env);

Configuration = builder.Build();

}

public void ConfigureServices(IServiceCollection services)

{

services.AddConfigServer(Configuration);

// Add framework services.

services.AddMvc();

services.Configure<Demo>(Configuration);

}

##### Demo.cs

public class Demo

{

public string Name { get; set; }

public int Age { get; set; }

public string Env { get; set; }

}

##### ValuesController.cs

[Route("/")]

public class ValuesController : Controller

{

private readonly IConfigurationRoot _config;

private readonly IOptionsSnapshot<Demo> _configDemo;

  


public ValuesController(IConfigurationRoot config, IOptionsSnapshot<Demo> configDemo)

{

_config = config;

_configDemo = configDemo;

}

[HttpGet]

public Demo Get()

{

//两种方式获取配置文件的数据

//var demo = new Demo

//{

// Name = _config["name"],

// Age = int.Parse(_config["age"]),

// Env = _config["env"]

//};

var demo = _configDemo.Value;

return demo;

}

}

运行程序 浏览器访问<http://localhost:5101/>,

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1095508/o_5.png)

更改配置文件appsettings.json，"env": "test",重新启动程序，刷新页面

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1095508/o_6.png)

再更改配置文件appsettings.json，"env": "prod",程序启动程序，刷新页面

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1095508/o_7.png)

这样通过修改本地的配置文件，就能获取远程上的各种配置了。

我们再试试修改远程的配置文件，修改demo-prod.yml的配置name: leo1,提交到github。

再访问<http://localhost:5011/>，发现配置并没有变化，这是因为配置服务并不知道git有更新，我们重启配置服务，再次访问，问题依旧，那么再重启客户端,发现我们得到了刚才更新的配置name= leo1，配置生效了。

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1095508/o_8.png)

# 后记

通过上面的例子，我们能够通过应用程序获取到配置信息，但是这有明显的问题，总不能一有配置更新就去重启配置服务和客户端吧？如何做到自动通知到客户端呢？留下这些问题，敬请期待下一章。

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




  

