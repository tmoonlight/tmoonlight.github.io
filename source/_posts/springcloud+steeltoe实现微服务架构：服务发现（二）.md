---
title: springcloud+steeltoe实现微服务架构：服务发现（二）
date: 2018/11/16 12:13:25
tags:
---


# [spring cloud+.net core搭建微服务架构：服务发现（二）](https://www.cnblogs.com/longxianghui/p/7576736.html)

# 前言

[上篇文章](http://www.cnblogs.com/longxianghui/p/7561259.html)实际上只讲了服务治理中的服务注册，服务与服务之间如何调用呢？传统的方式，服务A调用服务B，那么服务A访问的是服务B的负载均衡地址，通过负载均衡来指向到服务B的真实地址，上篇文章已经说了这种方式的缺点。那么下面讲如何在spring cloud+dotnet core的应用下进行服务调用。

# 代码实现

假设一种场景，有一个订单服务，有一个产品服务，其中产品服务是由两个服务节点组成一个集群。需求是订单服务访问产品服务的一个API接口。根据上一章文章的内容创建3个应用程序ServiceOne（端口8010），ServiceTwo（端口8011），ServiceThree（8012）。其中ServiceOne设置应用程序名称为order。ServiceTwo和ServiceThree的应用程序名称为product，做成集群。

ServiceOne.appsettings.json

{

"Logging": {

"IncludeScopes": false,

"LogLevel": {

"Default": "Warning"

}

},

"spring": {

"application": {

"name": "order"

}

},

"eureka": {

"client": {

"serviceUrl": "<http://localhost:5000/eureka/>"

},

"instance": {

"port": 8010

}

}

}

ServiceOne.Controllers.ValuesController.CS

private readonly DiscoveryHttpClientHandler _handler;

private const string ProductUrl = "<http://product/api/values>";

  


public ValuesController(IDiscoveryClient client, ILoggerFactory logFactory)

{

_handler = new DiscoveryHttpClientHandler(client);

}

  


[HttpGet("product")]

public async Task<string> GoProductAsync()

{

var client = new HttpClient(_handler, false);

return await client.GetStringAsync(ProductUrl);

}

ServiceTwo.appsettings.json

{

"Logging": {

"IncludeScopes": false,

"LogLevel": {

"Default": "Warning"

}

},

"spring": {

"application": {

"name": "product"

}

},

"eureka": {

"client": {

"serviceUrl": "<http://localhost:5000/eureka/>"

},

"instance": {

"port": 8011

}

}

}

ServiceTwo.appsettings.json

{

"Logging": {

"IncludeScopes": false,

"LogLevel": {

"Default": "Warning"

}

},

"spring": {

"application": {

"name": "product"

}

},

"eureka": {

"client": {

"serviceUrl": "<http://localhost:5000/eureka/>"

},

"instance": {

"port": 8012

}

}

}

为了展现访问的差异，设置不同的返回值。

ServiceTwo.Controllers.ValuesController.cs

[HttpGet]

public string Get(){

return "ServiceTwo";

}

ServiceThree.Controllers.ValuesController.cs

[HttpGet]

public string Get(){

return "ServiceThree";

}

同时启动这3个项目，先看看服务中心<http://localhost:5000/>

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1085116/o_1.png)

这个3个应用程序都已经注册到了服务中心。ServiceOne被注册到ORDER，ServiceTwo和ServiceThree注册到了PRODUCT。

分别访问

<http://localhost:8011/api/values> 返回ServiceTwo

<http://localhost:8012/api/values> 返回ServiceThree

证明这两个服务是没有问题的。

再访问<http://localhost:8010/api/values/product>，

如图所示，分别返回了“ServiceTwo”和“ServiceThree”，多刷新几次，发现结果是来回变动的，这说明服务中心帮我们实现了负载均衡。

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1085116/o_2.png)

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1085116/o_3.png)

我们再做一个测试，断开ServiceTwo这个应该程序。我们继续访问<http://localhost:8010/api/values/product>，发现一次错误，一次正常返回ServiceThree。30秒以后（可配置）再访问正常返回ServiceThree，同时发现服务中心已经踢掉了端口为8011的应用程序（ServiceTwo）。

![](https://images.cnblogs.com/cnblogs_com/longxianghui/1085116/o_4.png)

# 后记

通过上面3个实例我们模拟了分布式的调用场景，其中Order访问Product集群的时候，并没有指定具体的地址，而是指定了服务名称（product），服务中心自动分配了地址，并实现了负载均衡。联系实际应用场景，配合docker，我们可以快速的对某个服务进行添加，不再需要维护服务节点。同时某个服务节点挂掉以后，服务中心也会踢出这个服务节点（会有短暂的不可用）。结合CAP理论来说，服务中心满足了AP。

这篇文章讲解了服务之间的调用，我们实际的应用场景，还有各种客户端（IOS，Andriod，Web...）来访问，而服务一般是内网不对外暴露的，所以客户端访问服务的时候就需要有一个专门对外暴露的入口，那么就引入了下篇文章的API网关。

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




  

