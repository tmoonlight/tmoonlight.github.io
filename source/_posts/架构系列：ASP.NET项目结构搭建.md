---
title: 架构系列：ASP.NET项目结构搭建
date: 2017/2/15 0:22:56
tags:
---


# [架构系列：ASP.NET 项目结构搭建](http://www.cnblogs.com/easygame/p/5150949.html)

我们头开始，从简单的单项目解决方案，逐步添加业务逻辑的约束，从应用逻辑和领域逻辑两方面考虑，从简单的单个项目逐步搭建一个多项目的解决方案。主要内容：

（1）搭建应用逻辑和领域逻辑都简单的单项目

（2）为应用逻辑复杂的单项目添加应用服务

（3）为领域逻辑复杂的单项目添加领域行为

（4）Application膨胀时，分离Application项目

（5）分离Infrastructure项目

（6）添加Web服务支持

（7）Web服务器负载均衡的支持

（8）其他方面的扩展支持

### 1.搭建应用逻辑和领域逻辑都简单的单项目

业务逻辑简单，主要的用例和CURD几乎一一对应，没有区分应用逻辑和领域逻辑的必要。

（1）搭建单项目解决方案：Example，项目类型为[ASP.NET](http://asp.net/) MVC

（2）添加Application文件夹，添加IRepository<T>接口。

（3）在Application文件夹中添加Domain文件夹，使用POCO作为实体。

（3）添加Infrastructure文件夹，添加Dependency文件夹，添加IContainer和IoCContainer实现，添加Repository文件夹和EfRepository<T>实现。

（4）添加Web文件夹，添加IoCControllerFactory实现，在Controller中通过构造注入IRespository<T>。

### 2.为应用逻辑复杂的单项目添加应用服务

业务逻辑复杂的原因更多体现在流程控制上而非领域逻辑上，因此我们对上文的项目进行改造。

（1）Application文件夹中添加Service文件夹，通过ApplicationService接口来抽象应用逻辑，在实现ApplicationService接口时通过构造注入IRepository<T>。

（2）在Controller不在直接依赖IRepository，在Controller中通过构造注入IApplicationService。

### 3.为领域逻辑复杂的单项目添加领域行为

领域逻辑复杂表现在过多的直接通过属性进行实体状态判断并多次赋值，一般情况下这些代码可以通过重构添加到实体。

（1）从ApplicationService中分离出与流程控制无关的代码。

（2）对实体类添加行为，实体类的public方法的定义分离到实体接口中，其他方法为私有方法。

此时的项目结构如图所示：

### 4.Application膨胀时，分离Application项目

Application是项目的核心，本身都是业务逻辑相关的代码，即使对其他类库有依赖也可以通过接口隔离方式消除，因此在Application代码膨胀时，无论是应用逻辑和领域逻辑哪种原因，都应该分离Application项目，更重要的意义在于我们需要对Application项目进行单元测试。事实上复杂一些的项目，我们一开始构建的就是Application项目及其单元测试。

（1）在解决方案中添加Example.Application项目。

（2）将Example项目中的Application文件夹下的全部文件迁移到Example.Application项目中，这样无需修改命名空间。

（3）修改Example项目添加Example.Application项目的引用。

此时解决方案的结构如图所示：

### 5.分离Infrastructure项目

分离Application项目后，由于Infrastructure只单向依赖Application中的接口，因此分离Infrastructure项目顺理成章。如果是多客户端项目，在分离Infrastructure后可以考虑再从Web项目中分离出单独表现逻辑层Example.WebBase。

（1）在解决方案中添加Example.Infrastructure项目。

（2）将Example项目中的Infrastructure文件夹下的全部文件迁移到Example.Infrastructure项目中，同样无需修改命名空间。

（3）修改项目的引用，添加Example对Example.Infrastructure项目的引用，添加Example.Infrastructure对Example.Application的引用。

此时解决方案结构如图所示：

### 6.添加Web服务支持

由于Web项目只依赖ApplicationService的接口，这是应有之意。我们添加服务层时只需要提供Web服务类型的IApplicationService接口实现即可。

（1）添加Example.Application.WebApi项目，引用Example.Application项目，封装ApplicationService应用服务。

（2）添加Example.WebApiApplicationService项目，引用Example.Application和Example.Application.WebApi项目，实现IApplicationService接口的WebApi版本WebApiApplicationService。

（3）修改Example项目的依赖注入配置，将IApplicationService的实现配置为WebApiApplicationService。

（4）还要记得将Web项目中配置的ApplicationService的第三方依赖接口的依赖注入配置转移到Web服务项目中。

此时解决方案如图所示：

### 7.Web服务器负载均衡的支持

添加Web服务器的负载均衡主要解决认证token的问题和Session的问题。

（1）[ASP.NET](http://asp.net/)的Forms认证可以通过修改Web.config支持生成同样的用户token。

（2）[ASP.NET](http://asp.net/)的Session可以通过自定义SessionStateStoreProviderBase实现分离Session到Session状态服务器或集群。

### 8.其他方面的扩展支持

无论是邮件服务、缓存还是数据库，Application都是通过接口隔离了具体的实现，因此我们可以按需添加ApplicationService中定义的IEmail、ICache、ILogger等的其他实现，再修改依赖注入的配置即可。如果没有采用Web服务，修改Web项目，否则修改Web服务项目的依赖注入配置。

您的推荐，我的动力。

  

