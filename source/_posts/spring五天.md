---
title: spring五天
date: 2019/4/28 10:22:29
tags:
---


摘要：======================&nbsp;第一天================&nbsp;&nbsp;1.Spring框架作用和优点&nbsp;&nbsp;Spring提供了一个整合应用平台。&nbsp;&nbsp;该框架具有IOC和AOP机制的实现,&nbsp;&nbsp;基于这些特性开发系统,&nbsp;&nbsp;可以提高系统结构的灵活性,&nbsp;&nbsp;降低组件之间的耦合度。&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;在整合应用

======================

  


 第一天

  


================

 

  


  1.Spring框架作用和优点

  


   Spring提供了一个整合应用平台。

   该框架具有IOC和AOP机制的实现,

   基于这些特性开发系统,

   可以提高系统结构的灵活性,

   降低组件之间的耦合度。

   

    在整合应用,

    我们会将应用程序的Action,DAO,Service组件交给Spring框架负责管理,

    使用Spring框架的IOC和AOP机制以低耦合方式建立关联。

 

 *2.Spring框架容器

  


    Spring框架提供了一个容器,主要在该容器中管理应用程序的各个组件,建立组件关联

  


   1)容器特性,如何实例化

  


    Spring容器首先具有工厂特性,除此之外还具备了IOC和AOP机制的实现。

   BeanFactory<-继承-ApplicationContext

   两个实现类:

   ClassPathApplicationContext

   FileSystemApplicationContext

  


   2)容器如何对Bean组件管理

  


    当将一个Bean组件交给Spring容器后,Spring容器可以负责创建、销毁该对象。

  


    *a.Bean对象创建模式

  


     Spring容器支持singleton和prototype两种模式创建对象。

     默认为singleton,如果想改变可以使用scope属性定义。

     (如果在Web整合应用,还支持request,session等值)

  


    b.Bean对象的创建时机

  


    singleton 模式对象在容器实例化时创建,

    通过lazy=true属性可以将创建推迟到getBean方法。

    prototype 模式对象在getBean方法时创建

  


   c.Bean对象初始化设置

  


   通过init-method属性可以指定一个方法当做初始化方法,

   在对象创建后自动执行。

   

   通过destroy-method属性可以指定一个销毁方法,

   在对象被垃圾回收前自动执行。

   (仅对singleton对象有效)

 

 

 

 *3.Spring框架的IOC机制

 

  


   1)IOC概念

  


    Inverse of Controller 被称为反向控制、控制反转。

    更确切的讲应该是控制的转移。

   意思是当两个组件之间具有使用关系时,

   原有将对象创建和关系指定逻辑交给了使用一方负责。

   最终导致了两个组件耦合度过高,

   为维护和组件替换带来的不便。

   采用了IOC机制后,

   会将对象创建和关系指定这些逻辑交给第三方框架或容器负责,

   将这些控制逻辑转移给了第三方负责,

   这样发生变更后,只需要修改第三方配置就可以了。

  


   2)DI概念(Dependency Injection)

  


    DI被称为依赖注入。Spring框架中IOC机制是通过DI技术实现的。

    DI注入技术有以下几种实现:

  


   *a.setter方式注入(推荐)

  


     依靠setter方法接收注入的对象实例

     \--添加setter方法

     \--采用<property>描述

  


   b.constructor方式注入

  


     依靠构造方法接收注入的对象实例

     \--添加带参数构造方法

     \--采用<constructor-arg>描述

  


 4.DI注入的使用

  


   可以通过注入技术注入各种不同类型的数据

   1)注入一个Bean对象

     采用ref="Bean的ID名称"

     <property name="costDao"

       ref="hibernateCostDao">

     </property>

   2)注入一个基本类型数据

     采用value="值"

     <property name="password"

         value="123456">

     </property>

   3)注入集合类型数据

      List,Set,Map,Properties

======================

  


 第二天

  


================

  


 1.Spring框架的AOP机制

 

  


   1)什么是AOP和优点

  


   Apsect Oriented Programming面向方面编程。

 面向方面编程是以OOP面向对象编程为基础。

 AOP关注的是共通处理问题,可以将共通处理封装成一个组件(方面组件),

 然后采用AOP机制可以以低耦合方式作用到指定的目标组件上。

  


   2)AOP使用步骤

  


    a.引入Spring的IOC和AOP开发包

    b.添加Spring容器配置文件

    c.将共通处理封装成一个独立Bean组件

    d.采用AOP配置将Bean组件作用到其它目标组件及其方法上

  


   3)AOP相关概念

  


     *a.方面(Aspect)

      方面指的封装共通处理的组件。可以灵活的切入到目标对象及方法上。

     *b.切入点(Pointcut)

      切入点用于指定目标对象及方法,利用特定表式指定目标对象及方法

     c.连接点(JoinPoint)

     连接点指的是方面和某一个目标方法的关联点。切入点是连接点的集合

     *d.通知(Advice)

      通知用于指定方面功能在目标对象方法上执行的时机。

      例如方法前、方法后、异常发生后等。

     e.目标组件(Target)

      使用方面功能的Bean组件,或者切入点指定的Bean组件

     f.动态代理(AutoProxy)

     动态代理机制是AOP机制的实现原理.

     Spring框架在使用AOP配置后,返回的Bean对象,

     是采用动态代理机制生成的一个新类型。

     该类型的方法负责去执行方面组件和目标组件的处理。

 Spring框架采用了两种方式生成动态代理类。

  \--采用CGLIB工具生成(目标对象没有接口)

  public class 代理类 extends 原目标组件{

    // 重写原目标组件的方法

  }

  \--采用JDK Proxy API生成(目标对象有接口)

  public class 代理类 implements 原目标组件接口{

    // 重写原目标组件的方法

 }

 

  *4)切入点表达式

  


    利用表达式指定目标组件及方法。

  *a.方法限定表达式

   execution(修饰符? 

    返回类型 方法名(参数列表) throws异常?)

 示例1:匹配容器中所有组件以add开始的方法

    execution(* add*(..))

 示例2:匹配CostService组件的所有方法

    execution(* 

      org.tarena.service.CostService.*(..))

 示例3:匹配service包下所有类的所有方法

    execution(* org.tarena.service.*.*(..))

 示例4:匹配service包下及其子包中所有类的所有方法

    execution(* org.tarena.service..*.*(..))

   

   *b.类型限定表达式

    within(类型)

   示例1:匹配CostService类中所有方法

    within(org.tarena.service.CostService)

   示例2:匹配service包下所有类的所有方法

    within(org.tarena.service.*)

   示例3:匹配service包及其子包所有类所有方法

    within(org.tarena.service..*)

   

   c.Bean名称限定表达式

     bean(Bean的id或name属性值)

  示例1:匹配id=costAction的Bean组件方法

     bean(costAction)

  示例2:匹配名称以DAO结尾的Bean组件方法

     bean(*DAO)

   d.参数限定表达式

     args(参数列表)

  示例1:匹配只有一个参数,并且符合Serializable类型

    args(java.io.Serializable)  

 (注意:上述表达式可以采用&&,||连接在一起)

 

  5)通知类型

  


   通知主要用于指定方面组件在目标组件方法上作用的时机。

    a.前置通知<aop:before>

     方面组件在目标方法之前调用

    b.后置通知<aop:after-returnning>

     方面组件在目标方法之后调用,如果目标方法抛出异常,将不再执行方面组件

    c.最终通知<aop:after>

     方面组件在目标方法之后调用,目标方法有无异常都会执行

    d.异常通知<aop:after-throwing>

     方面组件在目标方法抛出异常后执行。

    e.环绕通知<aop:around>

     方面组件在目标方法之前和之后都要执行

   try{

     // 前置通知--执行方面组件

     // 执行目标方法

     // 后置通知--执行方面组件

   }catch(Exception ex){

     // 异常通知--执行方面组件

   }finally{

     // 最终通知--执行方面组件

   }

 

  6)采用AOP记录异常日志信息

  


   a.编写记录异常信息的方面组件ExceptionBean

   b.将方面组件定义到Spring容器

   c.添加AOP配置,定义切入点,方面和通知元素

  


  7)Log4j工具简介

  


 Log4j是一款日志工具。

 优点:该日志器可以灵活的控制输出信息的级别和输出的方式。

 Log4j主要由以下3部分构成:

    a.日志器组件

 (Logger)

 提供了消息输出的方法,可以按消息级别输出

    b.输出器组件(Appender)

 用于指定消息采用哪种方式输出。例如以文件形式输出,以控制台形式输出

    c.布局器组件(Layout)

 用于指定消息输出的格式。

   一个日志器可以指定多个不同的输出器,每个输出器只能对应一个布局器。

==============

  


 第三天

  


===========

  


 1.Spring注解配置的使用

  


   注解技术源于JDK 5.0,从Spring2.5版本开始支持注解配置形式,可以替代原有的XML配置。

   注解可以在类定义、方法定义、成员定义前使用。

  


   1)<bean>元素和注入的配置

  


     可以采用组件扫描技术替代原有的<bean>定义和注入配置。

     使用方法如下:

     a.在applicationContext.xml中开启组件扫描功能,指定要扫描的package路径   

     b.如果需要将组件扫描到Spring容器,需要在组件类定义前使用以下注解标记。

      \--@Controller

      \--@Service

      \--@Repository

      \--@Component

     默认扫描到容器采用类名首字母小写当id值,

     如果需要指定可采用@Service("id值")格式指定。

     如果需要修改scope创建对象的模式,

     可使用@Scope("prototype")格式指定

    c.如果两个组件之间有注入关系,可以在变量定义前或setter方法前使用下面注解标记

     \--@Resource

     \--@Autowired

     如果不指定注入的id名,会采用类型匹配注入。

     @Resource(name="costDao")将指定的costDao对象注入。

 

   2)<aop>元素配置

  


     AOP注解配置的使用方法如下:

    a.在applicationContext.xml中开启AOP注解配置。

    b.在方面组件中使用以下注解标记。

    \--@Aspect // 将Bean组件定义成方面,类定义前使用

    \--@Pointcut// 定义切入点表达式,方法定义前使用。因此需要编写个空方法,才能使用该标记。

    \--通知定义,@Around,@Before,

      @After,@AfterReturning,

      @AfterThrowing,在方法前使用

 

 2.利用Spring整合JDBC和Hibernate 

  1)Spring框架对数据库访问技术提供了以下支持

  


    a.提供了一致的异常处理层次.提供了一套异常类型,例如DataAccessException   

    b.提供了编写DAO的一套工具类,主要有DaoSupport和Template两种封装类型。

 JDBC技术:JdbcDaoSupport,JdbcTemplate

 Hibernate技术:HibernateDaoSupport,

                 HibernateTemplate

    c.提供了事务管理的支持。只需要添加AOP配置即可。

 

   2)Spring和Jdbc技术的整合

  


   \--新建工程,引入jar包

       (spring开发包,ojdbc.jar,dbcp开发包)

   \--在src下添加applicationContext.xml

   \--针对COST表编写实体类

   \--编写CostDAO接口,定义要实现的方法

   \--编写JdbcCostDAO实现类

    (继承JdbcDaoSupport,在方法体中使用JdbcTemplate类完成增删改查操作)

      update:用于增、删、改操作

      queryForObject:用于查询一条记录

      query:用于查询多条记录

      queryForInt:用于查询一个数值的

   \--将JdbcCostDAO在Spring容器中定义

   \--追加一个连接池,在Spring容器中定义一个dataSource组件对象Bean,

   并将该Bean组件对象给JdbcCostDAO注入。

 (JdbcDaoSupport里有一个setDataSource方法,

   接收容器注入的DataSource对象,

   利用DataSource对象实例化JdbcTemplate)

 

   3)Spring和Hibernate技术的整合

  


    \--创建工程,引入开发包

    (spring开发包,ojdbc.jar,dbcp连接池,hibernate框架开发包)

    \--在src下添加applicationContext.xml

    \--针对数据表添加实体类和映射描述文件

      (Cost类,Cost.hbm.xml)

    \--编写CostDAO接口

    \--编写实现类HibernateCostDAO

     (继承HibernateDaoSupport,采用HibernateTemplate的方法完成增删改查)

      save():添加

      update():更新

      delete():删除

      load(),get():按主键查询

      find():执行HQL语句

    \--在Spring容器配置HibernateCostDAO

  需要事先定义DataSource,SessionFactory

 组件对象,按DataSource-注入->

 SessionFactory-注入->

 HibernateCostDAO顺序建立关联

   提示:如果需要在DAO中使用Session,

   可以采用DaoSupport的getSession()获取,

   也可以通过HibernateTemplate.execute方法以回调方式使用。

   (参考spring_03_2中HibernateCostDAO.java的useSession()方法)

=====================

  


 第四天

  


================

  


 1.Spring框架整合Struts2步骤

  


   a.创建工程,引入开发包

     Struts开发包,Spring开发包

   b.添加Struts控制器配置和struts.xml配置文件

  *c.引入struts2-spring-plugin.jar整合包

     然后将<action>配置的class指定为Spring容器中Action组件定义的id值。

 (plugin.jar整合包提供一个StrutsSpringObjectFactory,采用该组件获取Action对象。

   该组件可以访问Spring容器,获取容器中定义的Bean对象)

  *d.在web.xml中添ContextLoaderListener组件,用于在启动服务器时实例化Spring容器

  


 2.struts-spring-plugin.jar作用

  


   该plugin.jar提供了一个StrutsSpringObjectFactory类,

   当引入该jar后,Struts2会采用该组件获取Action对象

  ObjectFactory在获取Action对象时,有以下两种途径:

  

 a.利用<action>元素的class属性值去Spring容器中寻找Bean对象,寻找规则是id=class值 

  参考(ssh2-2.jpg结构图)

 b.如果利用<action>的class属性值去Spring容器获取不到Bean对象,

  ObjectFactory会利用反射机制创建一个Action对象,

  然后访问Spring容器,

  将容器中id名与Action属性一致的Bean对象注入给Action。

  参考(ssh2-1.jpg结构图)

 try{

  // 第一种利用class值去Spring获取Bean对象

 }catch(){

  // 第二种自己创建一个Action,之后将Spring中的Bean对象给Action属性注入。

  //注入规则是属性名=id值

 }

 

 3.SSH学习建议

  


   XML和注解配置,XML配置方式了解,学会使用注解配置形式。

  Struts2+Spring整合,建议采用ssh2-1.jpg结构整合。

   掌握:ssh2-1.jpg采用注解方法整合应用。

 (参考spring_04_4的demo2案例)

======================

  


 第五天

  


===================

  


 *1.重构netctoss系统的资费管理模块处理

  


  1)重构资费列表显示功能

   a.梳理处理流程

 /cost/list.action-->ListCostAction

 \-->CostDAO.findAll/getTotalPages

 \-->cost_list.jsp

   b.重构CostDAO的findAll/getTotalPages

    (参考原来总结过的Spring+Hibernate步骤)

   c.测试Spring容器中的DAO

   d.改造ListCostAction

    \--引入struts-spring-plugin.jar

    \--将Spring容器中的DAO给Action注入

   e.在web.xml中添加ContextLoaderListener配置,启动服务器时实例化Spring容器对象。

  


    

  2)重构资费修改操作

  


   a.梳理处理流程

 /cost/detail.action-->DetailCostAction

 \-->CostDAO.findById-->cost_detail.jsp

   b.重构CostDAO.findById方法

     由于前面重构已经将CostDAO交给Spring容器管理,因此只要将findById实现就可以了

   c.重构DetailCostAction

     \--将Spring容器中的DAO给Action注入

   d.测试功能

 *3)Spring对Hibernate延迟加载操作的支持

   为了支持Hibernate延迟加载操作,

   Spring提供了一个Filter组件,

   该组件为OpenSessionInViewFilter。

   可以在web.xml中定义该Filter。

   这样可以将Template方法关闭Session时机推迟到JSP解析之后。

   注意:定义在StrutsFilter之前才有效

 

 *2.Spring的事务管理

  


    Spring提供了以下两种事务管理方法

   *a.声明式事务管理

     以AOP配置的形式实现事务管理

  JDBC事务管理的方面组件:

  (DataSourceTransactionManager)

  Hibernate事务管理的方面组件:

   (HibernateTransactionManager)

   事务管理通知:<tx:advice>

   切入点:根据实际情况编写表达式

  \-----如果采用注解方式配置事务------

   首先在applicationContext中开启事务注解

 <bean id="txManager" 

  class="org.springframework.orm.hibernate3.HibernateTransactionManager">

  <property name="sessionFactory" 

     ref="sessionFactory">

  </property>

 </bean>

 <tx:annotation-driven 

  transaction-manager="txManager"/>

   然后在目标组件中,使用@Transactional.

   该标记可用在类定义和方法定前。类定义前指定全局,方法定以前指定当前方法。

   b.编程式事务管理(简单了解)

     在业务组件中添加事务管理代码.

    代码中使用TransactionTemplate控制事务

 

 3.Spring MVC框架

  


   1)了解Spring MVC主要实现

    Spring提供的MVC框架主要有以下实现组件:

    控制器DispatcherServlet,Controller

    映射处理器HandlerMapping

    视图解析器ViewResolver

    模型和视图ModelAndView

     

   2)了解Spring MVC的处理流程

    a.客户端发送请求,请求交给DispatcherServlet控制器

    b.DispatcherServlet控制器调用HandlerMapping映射器组件,

    根据请求找到对应的Controller组件

 (HandlerMapping组件负责维护请求和Controller组件的对应关系)

    c.控制器调用请求对应的Controller处理请求,也可以调用DAO实现对数据库的操作

    d.Controller处理完毕后,会返回一个ModelAndView对象。

 (ModelAndView负责封装响应数据和视图名)

    e.控制器会调用ViewResolver组件,

    根据ModelAndView信息,

    定位到指定的JSP,生成响应的HTML结果

    f.将响应结果输出,为客户浏览器显示

   3)入门示例

     \--基于XML配置的示例

       a.引入spring ioc和springmvc开发包

       b.在src下添加applicationContext.xml

       c.在web.xml中添加DispatcherServlet配置,

       指定classpath:applicationContext.xml

       d.编写Controller组件,实现Controller接口,

       实现handleRequest方法。

       e.在Spring配置文件中定义Controller组件,

       定义handlerMapping和viewResolver组件

       

     \--基于注解配置的示例(推荐)

       a.引入spring ioc和springmvc开发包

       b.在src下添加applicationContext.xml

       c.在web.xml中添加DispatcherServlet配置,

       指定classpath:applicationContext.xml

       d.在applicationContext.xml中定义

 AnnotationMethodHandlerAdapter(支持注解的handlerMapping组件)和viewResolver组件。

 开启组件扫描配置。

       e.编写Action组件。

       Action类定义前使用@Controller将Action扫描到容器。

 业务方法前使用@RequestMapping,指定该方法处理哪个请求

  

