---
title: EntityFramework6withOracle(可实现codefirst)-wlf-博客园
date: 2016/3/15 1:33:29
tags:
---


**[Entity Framework6 with Oracle(可实现code first)](http://www.cnblogs.com/wlflovenet/p/4187455.html)**

  


  Oracle 与2个月前刚提供对EF6的支持。以前只支持到EF5。EF6有很多有用的功能 值得升级。这里介绍下如何支持Oracle

  


 **  一.Oracle 对.net支持的一些基础知识了解介绍。**

  


1.早年的时候,微软自己做的有 System.Data.OracleClient。 现在已经成了过期类了。性能等都不是很好。

  


2.Oracle 官方出的odp.net  Oracle.DataAccess.dll(非托管版本) 还要分32/64位。而且很麻烦的是 部署的时候 需要装客户端环境。非常繁琐。

  


3.Oracle 官方近年新出的  Oracle.ManagedDataAccess.dll  这个非常给力 不再区分32/64位了。 而且不需要客户端再安装东西了。性能也得到了提高。不管是用ado.net或者其他ORM框架

  


   都建议使用此版本dll。这次我们的EF6 也会依据此dll进行开发。

  


官方下载地址是这个 一定要去官方下载最新的 才能支持EF6  我从nuget上下的版本较低 不支持。。

  


<http://www.oracle.com/technetwork/database/windows/downloads/index-090165.html>  (当时下载的版本为4.121.2.0)

  


下载好后  从这个目录下 odp.net\managed\common  拿出2最关键的dll

  


Oracle.ManagedDataAccess.dll 和 Oracle.ManagedDataAccess.EntityFramework.dll

  


 ** ** **二.修改webconfig配置文件**

  


加入如下代码：

  


<configSections>  
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />  
    <!--<section name="oracle.manageddataaccess.client" type="OracleInternal.Common.ODPMSectionHandler, Oracle.ManagedDataAccess, Version=4.121.2.0, Culture=neutral, PublicKeyToken=89b483f429c47342" />\-->   
  </configSections>  
  
  <entityFramework>  
    <defaultConnectionFactory type="Oracle.ManagedDataAccess.EntityFramework.OracleConnectionFactory,  
Oracle.ManagedDataAccess.EntityFramework,  
Version=6.121.2.0,  
Culture=neutral,  
PublicKeyToken=89b483f429c47342" />  
    <providers>  
      <provider invariantName="Oracle.ManagedDataAccess.Client" type="Oracle.ManagedDataAccess.EntityFramework.EFOracleProviderServices, Oracle.ManagedDataAccess.EntityFramework, Version=6.121.2.0, Culture=neutral, PublicKeyToken=89b483f429c47342" />  
     
    </providers>  
  </entityFramework>  
  
  <system.data>  
    <DbProviderFactories>  
      <remove invariant="Oracle.ManagedDataAccess.Client" />  
      <add name="ODP.NET, Managed Driver" invariant="Oracle.ManagedDataAccess.Client" description="Oracle Data Provider for .NET, Managed Driver"  
          type="Oracle.ManagedDataAccess.Client.OracleClientFactory, Oracle.ManagedDataAccess, Version=4.121.2.0, Culture=neutral, PublicKeyToken=89b483f429c47342" />  
    </DbProviderFactories>  
  </system.data>

连接字符串如下 记得提供下 providerName 为 Oracle.ManagedDataAccess.Client

  


<connectionStrings>  
  
    <add name="OraString" connectionString="Data Source= (DESCRIPTION =  
    (ADDRESS = (PROTOCOL = TCP)(HOST = *****)(PORT = 1521))  
    (CONNECT_DATA =  
      (SERVER = DEDICATED)  
      (SERVICE_NAME = ORCL)  
    )  
  );User ID=*****;Password=*****;Persist Security Info=True" providerName="Oracle.ManagedDataAccess.Client" />  
  
  </connectionStrings>

这里还有个注意事项,因为有的人以及装了oracle的客户端。这可能会导致一些错误。这里 请注意检查下  C:\Windows\Microsoft.NET\Framework\v4.0.30319\Config

  


下的  machine.config （64位的话 路径是 Framework64,最好都检测下)

  


machine.config 可以理解为webconfig的父类 所以我们需要检查下里面的内容  是否有

  


<section name="oracle.manageddataaccess.client" type="OracleInternal.Common.ODPMSectionHandler, Oracle.ManagedDataAccess, Version=4.121.1.0, Culture=neutral, PublicKeyToken=89b483f429c47342" />

  


 这样的内容   如果有  修改版本号Version 为当前Oracle.ManagedDataAccess.Client版本。

  


 **三.注意事项**

  


1.访问时提示表不存在，有可能权限不够。需要设置默认的Schema 需要在 OnModelCreating设置

  


 modelBuilder.HasDefaultSchema(“Schema名”);

  


2.oracle 的配置是否配置了使用ora方式来访问。 建议先不使用ora方式,有可能是这个方式访问导致的问题。

  


3.oracle 访问的问题 一定多去官网看下。

  


  推荐几篇

  


  <http://docs.oracle.com/cd/E56485_01/win.121/e55744/toc.htm>

  


   <http://docs.oracle.com/cd/E56485_01/win.121/e55744/entityMigrate.htm#BABEHEFE>

  


<http://www.cnblogs.com/wlflovenet/p/4187455.html>       转载请注明出处

  

