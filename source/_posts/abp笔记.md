---
title: abp笔记
date: 2016/8/15 19:09:44
tags:
---


部署 

  


后端

1.模块

2.领域层 （核心层）core

对于业务概念的表达和业务状态的信息以及业务逻辑的响应

实体 并非由属性定义 而是由行为和id定义

  


3.应用层

dto  automapfrom和automapto注解，用来关联数据流的输入输出

  


  


4.基础设施层

  


前端

异常处理

  


.angular.cli.json

模块

路由

组件

  


automapfrom和automapto 写反导致应用不上，已解决  


{{today | date}} 无法显示的问题，待解决，已解决，因为区域设置未处理

提示注入失败相关错误，已解决，因为没有在service.module.ts中加入相应的provider

  


*ngfor 

ng

.Observable的subscribe  


observable代表可观察对象 subscribe代表订阅事件（事件回调）

  


组件

属性：标签内使用[] 

标签内使用()

如果要直接传递字符串，直接用不带括号的即可

  


UBB 提交使用UBB，转码所有html标签，显示使用ubb转化为html

  


未解决问题：多语言，dependson的顺序影响加载

automapper对应异常，已解决（删除了如下module中的红字导致automapper对应失效），

  


var thisAssembly = typeof(ContentYardShaoApplicationModule).GetAssembly();  
//配置automapper  
Configuration.Modules.AbpAutoMapper().Configurators.Add(  
// Scan the assembly for classes which inherit from AutoMapper.Profile  
cfg => cfg.AddProfiles(thisAssembly)  
);  
//与自动注册与以上相似  
IocManager.RegisterAssemblyByConvention(thisAssembly);  
  
  


  


 心得：

1.依赖注入可以解决的一些问题：A.数据库频繁切换 B.接口频繁切换 C.单元测试-模拟数据， 局部可替换，总体来说就是组件依赖解耦，互相之间可以随意替换

  


  


分页问题，又有人问到这个查询总数和查询单页导致两次查询的问题

  


  


  

