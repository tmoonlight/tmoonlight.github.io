---
title: dotnetcore安装swagger并部署到docker中
date: 2018/2/24 6:15:41
tags:
---


1.dotnet core安装

2.dotnet 运行

dotnet build

dotnet new

dotnet new --help

dotnet run

dotnet add package

3.

引用log4net

引用swagger

dotnet add Swashbuckle.AspNetCore(注意这里有个坑，引用的名字一定要带core的，否则会引用上一个4.6版本的swagger，但是无法使用)

 vscode中使用如下插件userdefinedsnippets自动生成注释

加入这一条就能生成xml文件

<GenerateDocumentationFile>true</GenerateDocumentationFile>

有时会需要做一些额外的工作<http://wmpratt.com/dotnet-publish-where-are-my-xml-docs/>

configureService中加入public void Configure(IApplicationBuilder app)

public void ConfigureServices(IServiceCollection services)

{

    services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

    services.AddMvc();

  


    // Register the Swagger generator, defining one or more Swagger documents

    services.AddSwaggerGen(c =>

    {

        c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

    });

}

startup的configure中加入

using Swashbuckle.AspNetCore.Swagger;

public void Configure(IApplicationBuilder app)

{

    // Enable middleware to serve generated Swagger as a JSON endpoint.

    app.UseSwagger();

  


    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.

    app.UseSwaggerUI(c =>

    {

        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

    });

  


    app.UseMvc();

}

vs所有都是通过配置来实现

  


dockerfile

部署[asp.net](http://asp.net/) core 到 docker

<https://github.com/dotnet/dotnet-docker>

  


docker版本升级是一个坑，直接安装的版本很旧

注意，官方需要升级到17.06

yum update

  


# vim /etc/yum.repos.d/docker.repo

//添加以下内容

[dockerrepo]

name=Docker Repository

baseurl=<https://yum.dockerproject.org/repo/main/centos/7/>

enabled=1

gpgcheck=1

gpgkey=<https://yum.dockerproject.org/gpg>

# yum install docker-engine -y

安装完之后出错，查日志提示

3月 10 23:07:04 centos1 systemd[1]: Unit docker.service entered failed state.

3月 10 23:07:04 centos1 systemd[1]: docker.service failed.

3月 10 23:07:04 centos1 systemd[1]: docker.service holdoff time over, scheduling restart.

3月 10 23:07:04 centos1 systemd[1]: start request repeated too quickly for docker.service

3月 10 23:07:04 centos1 systemd[1]: Failed to start Docker Application Container Engine.

====================================

这个可以通过如下方式解决:

查看文件系统 /etc/docker/daemon.json 有没有这个文件，没有测创建它包括二级目录 docker

在daemon.json文件中输入以下内容:

{ "storage-driver": "devicemapper" }

如果daemon.json文件包含格式不正确的JSON，Docker将无法启动。

======================================

docker run -it --rm -p 5000:80 --name aspnetcore_sample aspnetapp

（对外端口：容器中服务的端口）

docker ps查看所有运行的镜像

docker stat进行docker监控

  


 
