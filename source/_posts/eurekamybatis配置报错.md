---
title: eurekamybatis配置报错
date: 2019/7/16 0:11:02
tags:
---


Failed to instantiate [com.zaxxer.hikari.HikariDataSource]: Factory method 'dataSource' threw exception; nested exception is java.lang.IllegalStateException: Cannot load driver class: com.microsoft.sqlserver.jdbc.SQLServerDriver

  


org.springframework.beans.factory.BeanCreationException: Error creating bean with name 'dataSource' defined in class path resource [org/springframework/boot/autoconfigure/jdbc/DataSourceConfiguration$Hikari.class]: Bean instantiation via factory method failed; nested exception is org.springframework.beans.BeanInstantiationException: Failed to instantiate [com.zaxxer.hikari.HikariDataSource]: Factory method 'dataSource' threw exception; nested exception is java.lang.IllegalStateException: Cannot load driver class: com.microsoft.sqlserver.jdbc.SQLServerDriver

之前报这个错是因为mybatis注入的对象里有两个叫datasource的，这次不知道为啥 

\------------过段时间又好了？

  


  


  


java.lang.IllegalArgumentException: Result Maps collection does not contain value for tmoonlight.srvone.dao.CustomerMapper.BaseResultMap

没有配置mapper扫描路径导致的

  


 又一次出现：Result Maps collection does not contain value for CustomerMapper.BaseResultMap

这是因为：

对:

@Select({

"select",

"id, Name, CardNo, Descriot, CtfTp, CtfId, Gender, Birthday, Address, Zip, Dirty, ",

"District1, District2, District3, District4, District5, District6, FirstNm, LastNm, ",

"Duty, Mobile, Tel, Fax, EMail, Nation, Taste, Education, Company, CTel, CAddress, ",

"CZip, Family, Version",

"from crm_customer",

"where id = #{id,jdbcType=INTEGER}"

})

@ResultMap("tmoonlight.srvone.dao.CustomerMapper.BaseResultMap")

Customer selectByPrimaryKey(Integer id);

错:

@Select({

"select",

"id, Name, CardNo, Descriot, CtfTp, CtfId, Gender, Birthday, Address, Zip, Dirty, ",

"District1, District2, District3, District4, District5, District6, FirstNm, LastNm, ",

"Duty, Mobile, Tel, Fax, EMail, Nation, Taste, Education, Company, CTel, CAddress, ",

"CZip, Family, Version",

"from crm_customer",

"where id = #{id,jdbcType=INTEGER}"

})

@ResultMap("CustomerMapper.BaseResultMap")

Customer selectByPrimaryKey(Integer id);

为毛这个前缀会被搞没就不得而知了！坑！java的项目复制修改需要谨慎！

  


  


Cause: java.lang.IllegalArgumentException: Result Maps collection already contains value for tmoonlight.rchat.dao.CustomerMapper.BaseResultMap

mapper同时生成两坨导致，因为使用代码生成器增量加到了xml后面

  


如下图 eurekaserver红色，引用不进来（按官网的pom文件可以搞定）

  


<?xml version="1.0" encoding="UTF-8"?><project xmlns="<http://maven.apache.org/POM/4.0.0>" xmlns:xsi="<http://www.w3.org/2001/XMLSchema-instance>"

xsi:schemaLocation="<http://maven.apache.org/POM/4.0.0><http://maven.apache.org/xsd/maven-4.0.0.xsd>">

<modelVersion>4.0.0</modelVersion>

  


<groupId>com.example</groupId>

<artifactId>eureka-service</artifactId>

<version>0.0.1-SNAPSHOT</version>

<packaging>jar</packaging>

  


<parent>

<groupId>org.springframework.boot</groupId>

<artifactId>spring-boot-starter-parent</artifactId>

<version>2.0.5.RELEASE</version>

<relativePath/> <!-- lookup parent from repository -->

</parent>

  


<properties>

<project.build.sourceEncoding>UTF-8</project.build.sourceEncoding>

<java.version>1.8</java.version>

</properties>

  


<dependencies>

<dependency>

<groupId>org.springframework.cloud</groupId>

<artifactId>spring-cloud-starter-netflix-eureka-server</artifactId>

</dependency>

  


<dependency>

<groupId>org.springframework.boot</groupId>

<artifactId>spring-boot-starter-test</artifactId>

<scope>test</scope>

</dependency>

</dependencies>

  


<dependencyManagement>

<dependencies>

<dependency>

<groupId>org.springframework.cloud</groupId>

<artifactId>spring-cloud-dependencies</artifactId>

<version>Finchley.SR1</version>

<type>pom</type>

<scope>import</scope>

</dependency>

</dependencies>

</dependencyManagement>

  


<build>

<plugins>

<plugin>

<groupId>org.springframework.boot</groupId>

<artifactId>spring-boot-maven-plugin</artifactId>

</plugin>

</plugins>

</build>

  


<repositories>

<repository>

<id>spring-milestones</id>

<name>Spring Milestones</name>

<url><https://repo.spring.io/libs-milestone></url>

<snapshots>

<enabled>false</enabled>

</snapshots>

</repository>

</repositories>

  


</project>

eureka-client/pom.xml

<?xml version="1.0" encoding="UTF-8"?><project xmlns="<http://maven.apache.org/POM/4.0.0>" xmlns:xsi="<http://www.w3.org/2001/XMLSchema-instance>"

xsi:schemaLocation="<http://maven.apache.org/POM/4.0.0><http://maven.apache.org/xsd/maven-4.0.0.xsd>">

<modelVersion>4.0.0</modelVersion>

  


<groupId>com.example</groupId>

<artifactId>eureka-client</artifactId>

<version>0.0.1-SNAPSHOT</version>

<packaging>jar</packaging>

  


<parent>

<groupId>org.springframework.boot</groupId>

<artifactId>spring-boot-starter-parent</artifactId>

<version>2.0.5.RELEASE</version>

<relativePath/> <!-- lookup parent from repository -->

</parent>

  


<properties>

<project.build.sourceEncoding>UTF-8</project.build.sourceEncoding>

<java.version>1.8</java.version>

</properties>

  


<dependencies>

<dependency>

<groupId>org.springframework.cloud</groupId>

<artifactId>spring-cloud-starter-netflix-eureka-client</artifactId>

</dependency>

  


<dependency>

<groupId>org.springframework.boot</groupId>

<artifactId>spring-boot-starter-test</artifactId>

<scope>test</scope>

</dependency>

<dependency>

<groupId>org.springframework.cloud</groupId>

<artifactId>spring-cloud-starter-netflix-eureka-server</artifactId>

<scope>test</scope>

</dependency>

</dependencies>

  


<dependencyManagement>

<dependencies>

<dependency>

<groupId>org.springframework.cloud</groupId>

<artifactId>spring-cloud-dependencies</artifactId>

<version>Finchley.SR1</version>

<type>pom</type>

<scope>import</scope>

</dependency>

</dependencies>

</dependencyManagement>

  


<build>

<plugins>

<plugin>

<groupId>org.springframework.boot</groupId>

<artifactId>spring-boot-maven-plugin</artifactId>

</plugin>

</plugins>

</build>

  


<repositories>

<repository>

<id>spring-milestones</id>

<name>Spring Milestones</name>

<url><https://repo.spring.io/libs-milestone></url>

<snapshots>

<enabled>false</enabled>

</snapshots>

</repository>

</repositories>

  


</project>

com.sun.jersey.api.client.ClientHandlerException: [java.net.ConnectException](http://java.net.connectexception/): Connection refused: connect 

1.直接建立server 出错

eureka:

client:

register-with-eureka: false

fetch-registry: false 

可解决

2.配了eurekaclient，但没加Eurekaclient导致无法运行

  


  


  


eurekaserver org.apache.catalina.LifecycleException: Failed to start component 

没看到后面的错误，实际上是端口被占用

  


feign 无法注入

2018-10-21 11:55:16.423  INFO 10144 --- [  restartedMain] ConditionEvaluationReportLoggingListener :

  


Error starting ApplicationContext. To display the conditions report re-run your application with 'debug' enabled.

2018-10-21 11:55:16.645 ERROR 10144 --- [  restartedMain] o.s.b.d.LoggingFailureAnalysisReporter   :

  


***************************

APPLICATION FAILED TO START

***************************

  


Description:

  


Field remoteService in tmoonlight.srvapi.service.DemoServiceImpl required a bean of type 'tmoonlight.srvapi.rpc.RemoteService' that could not be found.

  


  


  


有时候有彩色有时候没了？不知道，先加一个配置可以变彩色

多项目启动，使用dashboard，多项目启动时ideaj会提示你是否打开这个多启动管理的界面

  


为什么有时候spring启动时树叶 有时候没了？

已解决，在配置里手动配就行了，之前没有树叶可能时没有在pom里加入springboot相关组件

  


  


feign本来可以调用的，突然又出个这玩意？

[com.netflix.client.ClientException](http://com.netflix.client.clientexception/): Load balancer does not have available server for client: ShaoSrvOne

ribbon.eager-load.enabled=true 解决了

  


  


  


  

