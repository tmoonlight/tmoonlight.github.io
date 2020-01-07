---
title: JavaSpringMVC入门（六）依赖注入之实践部分-scofi-博客园
date: 2017/11/20 5:55:32
tags:
---


# [Java Spring MVC入门（六）依赖注入之实践部分](http://www.cnblogs.com/scofi/articles/5980258.html)

#   依赖注入的种类                                      


　　上次说到依赖注入的理论部分，那么具体代码该怎么写呢？下面我们将一一介绍，依赖注入有好几种实现方法，分别是Set注入、构造函数注入和注解注入。

      先简单的介绍下三种注入方式：

　　　　Set注入：Spring XML文件里声明依赖关系，代码里必须有set方法设置依赖注入

　　　　构造函数注入：Spring XML文件里声明依赖关系，代码里必须有构造方法设置依赖注入

　　　　注解注入：无须申明依赖关系，代码里使用注解即可自动注入。

  

#   Set注入                                            

1、首先Spring的XML文件中声明依赖关系

<bean id = "IocDemoOne" class="com.game.demo.scofi.IocDemoOne">

<property name="axe" ref="axe"></property>

</bean>

<bean id ="axe" class="com.game.demo.scofi.Axe"></bean>

其中name表示代码中set方法的名称，Spring会取出name的值这里是axe，之后把首字母大写，再加上字符串“set” , 最后字符串为"setAxe"，以此为方法名，系统会调用代码中的这个方法，当然代码里必须要有此方法。

ref表示需要注入的bean对象ID ，会把此对象作为setAxe的参数，传入到调用者。

2、编写java代码

IocDemoOne

package com.game.demo.scofi;

  


import java.io.IOException;

import javax.servlet.http.HttpServletRequest;

import javax.servlet.http.HttpServletResponse;

import org.springframework.stereotype.Controller;

import org.springframework.web.bind.annotation.RequestMapping;

  


  


/**

* 通过XML文件配置依赖注入

* SET方法注入demo

* @author wangyongguo

*

*/

@Controller

public class IocDemoOne {

private Axe myAxe;

//通过@RequestMapping为控制器指定哪些需要的请求

@RequestMapping("/iocDemoOne")

public void index(HttpServletRequest request, HttpServletResponse response) throws IOException {

response.setHeader("content-type", "text/html;charset=UTF-8");

String c = myAxe.useAxe();

response.getWriter().append("I am Demo one!");

response.getWriter().append(c);

}

public void setAxe(Axe axe){

this.myAxe = axe;

}

}

 Axe.java

package com.game.demo.scofi;

public class Axe{

public String useAxe(){

return "I am use AXE";

}

}

setAxe方法中把系统注入进来axe对象 赋值给myAxe这样调用者就获得了被依赖对象axe ，之后就可以使用他的方法了。

最后执行结果：

I am Demo one!I am use AXE

#   构造函数注入                                          

1、首先Spring的XML文件中声明依赖关系

<bean id ="axe" class="com.game.demo.scofi.Axe"></bean>

<bean id = "IocDemoTwo" class="com.game.demo.scofi.IocDemoTwo">

<constructor-arg ref="axe" />

</bean>

constructor-arg 标签表示构造函数注入，ref值的含义和set注入一样，会把axe对象作为构造函数的参数注入到调用者对象内。

2、编写代码

IocDemoTwo.java

package com.game.demo.scofi;

  


import java.io.IOException;

import javax.servlet.http.HttpServletRequest;

import javax.servlet.http.HttpServletResponse;

import org.springframework.stereotype.Controller;

import org.springframework.web.bind.annotation.RequestMapping;

  


  


/**

* 通过XML文件配置依赖注入

* 构造器注入DEMO

* @author wangyongguo

*

*/

@Controller

public class IocDemoTwo {

private Axe myAxe;

//构造函数注入

public IocDemoTwo(Axe axe){

this.myAxe = axe;

}

//通过@RequestMapping为控制器指定哪些需要的请求

@RequestMapping("/iocDemoTwo")

public void index(HttpServletRequest request, HttpServletResponse response) throws IOException {

response.setHeader("content-type", "text/html;charset=UTF-8");

String c = myAxe.useAxe();

response.getWriter().append("I am Demo two!");

response.getWriter().append(c);

}

  


}

Axe.java 同上

运行结果：

I am Demo two!I am use AXE

#   注解注入                                         

上面两种方法都要在java代码里写方法来实现依赖注入，这样非常麻烦，为了解决这个问题，注解注入应用而生，不再需要写方法来实现，而是通过注解即可。

具体看代码 IocDemoThree.java

代码位置：

package com.game.demo.iocTwo;

  


import java.io.IOException;

import javax.servlet.http.HttpServletRequest;

import javax.servlet.http.HttpServletResponse;

  


import org.springframework.beans.factory.annotation.Autowired;

import org.springframework.stereotype.Controller;

import org.springframework.web.bind.annotation.RequestMapping;

  


/**

* 使用@Autowired注解实现依赖注入

* spring配置文件里必须包含 <bean class="org.springframework.beans.factory.annotation.AutowiredAnnotationBeanPostProcessor" />

* spring才能理解@Autowired,否则将不会生效，或者使用 <context:component-scan base-package="xxx" /> 会自动扫描所有注解

*

* @author wangyongguo

*

*/

@Controller

public class IocDemoThree {

//如果Spring找不到Axe类型的bean，会报出异常。

//如果不想报出异常就需要加上(required = false)

//@Autowired(required = false)

@Autowired

private Axe myAxe;

@RequestMapping("/iocDemoThree")

public void index(HttpServletRequest request, HttpServletResponse response) throws IOException {

response.setHeader("content-type", "text/html;charset=UTF-8");

String c = myAxe.useAxe();

response.getWriter().append("I am Demo Three 11!");

response.getWriter().append(c);

}

}

 对应的配置文件代码：

<context:component-scan base-package="com.game.demo.iocTwo" >

</context:component-scan>

接口注入： 

上面介绍的是注入简单的Java类，如果有一个接口，有多个实现类，这样就会存在有同一个类型多个Bean的情况存在，那么这种情况该如何处理？

假设我们有一个Car接口 ，有两个类Benz和BMW 实现这个接口

Car.java

package com.game.demo.iocThree;

  


public interface Car {

public String getCarName();

}

Benz.java

package com.game.demo.iocThree;

  


import org.springframework.stereotype.Service;

  


@Service

public class Benz implements Car {

public String getCarName(){

return "I am Benz GLK";

}

}

BMW.java

package com.game.demo.iocThree;

  


import org.springframework.stereotype.Service;

  


@Service

public class Bmw implements Car{

public String getCarName(){

return "I am BMW 740";

}

}

IocDemoFour.java

package com.game.demo.iocThree;

  


import java.io.IOException;

import javax.servlet.http.HttpServletRequest;

import javax.servlet.http.HttpServletResponse;

  


import org.springframework.beans.factory.annotation.Autowired;

import org.springframework.beans.factory.annotation.Qualifier;

import org.springframework.stereotype.Controller;

import org.springframework.web.bind.annotation.RequestMapping;

  


/**

* @Autowired 接口注入

* 如果@Autowired 属性的类型是接口，这个接口被多个类实现，就会有一个问题，到底改注入哪个类呢？

* @author wangyongguo

*

*/

@Controller

public class IocDemoFour {

//必须加上import org.springframework.beans.factory.annotation.Qualifier;

//@Qualifier注解括号里面的应当是Car接口实现类的类名，但必须小写，否则出错

@Autowired

@Qualifier("bmw")

private Car myCar;

@RequestMapping("/iocDemoFour")

public void index(HttpServletRequest request, HttpServletResponse response) throws IOException {

response.setHeader("content-type", "text/html;charset=UTF-8");

String c = myCar.getCarName();

response.getWriter().append("I am Demo Four!");

response.getWriter().append(c);

}

}

根据上面代码我们可以看到，当存在同一类型有多个bean情况存在时，我们使用 @Qualifier 注解，qualifier的意思是合格者，通过这个标示，表明了哪个实现类才是我们所需要的，我们修改调用代码，添加@Qualifier注解，括号里面的内容是需要注入小写类名。

 

分类: [java spring mvc](http://www.cnblogs.com/scofi/category/883870.html)

好文要顶 关注我 收藏该文

[](http://home.cnblogs.com/u/scofi/)

[scofi](http://home.cnblogs.com/u/scofi/)

[关注 - 1](http://home.cnblogs.com/u/scofi/followees)

[粉丝 - 16](http://home.cnblogs.com/u/scofi/followers)

+加关注

0

0

[«](http://www.cnblogs.com/scofi/articles/5939501.html) 上一篇：[Java Spring MVC入门（五）依赖注入之理论部分](http://www.cnblogs.com/scofi/articles/5939501.html "发布于2016-10-09 11:26")

posted @ 2017-09-14 18:30 [scofi](http://www.cnblogs.com/scofi/) 阅读(521) 评论(2)  [编辑](https://i.cnblogs.com/EditArticles.aspx?postid=5980258) [收藏](http://www.cnblogs.com/scofi/articles/5980258.html#)

  

