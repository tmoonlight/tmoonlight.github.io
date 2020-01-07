---
title: asp.net根据Cookie变化控制缓存-zhdd1234的专栏-博客频道-CSDN.NET
date: 2016/5/5 20:11:59
tags:
---


[](http://www.csdn.net/?ref=toolbar)

[](http://blog.csdn.net/?ref=toolbar_logo)

  *   




遇到一个实际应用：做的网站静态内容居多，但是多语言支持，网站的开发已经结束，是将语言存入客户端cookie中，然后通过cookie值获取对应资源文件。

现在需要对网站增加缓存。在asp.net 框架下,当然是首选 OutputCache。OutputCache是将客户端缓存以及服务端缓存进行统一管理，通过不同的策略设置。由于语言的更换不会修改URL，所以是无法使用浏览器缓存了（这里想起google以及很多网站在做多语言时都会把语言放入到URL中的好处了，可以充分利用浏览器缓存）。

网上查了些资料，比较零碎，这里进行一个整理。

首先：在web.config中定义缓存策略，

在web.config中的system.web下增加如下：

  


  


<caching>

     <outputCacheSettings>

       <outputCacheProfiles>

         <add name="pageCache" enabled="true" duration="600" varyByParam="none" varyByCustom="LANG" location="Server" />

         <add name="productCache" enabled="true" duration="600" varyByParam="*" varyByCustom="LANG" location="Server" />

       </outputCacheProfiles>

     </outputCacheSettings>

     <outputCache enableOutputCache="true"/>

   </caching>

这里定义了两条策略，一条是不会根据参数更新缓存， 因为所有页面不需要传递参数，然后使用 varyByCustom="LANG" 自定义缓存失效，location设置缓存存储位置，这里仅选择Server.另外一条，需要根据传递的参数更新缓存，所以设置 varyByParam="*"。

然后关键的一步实现自定义缓存失效：需要在 Global.asax 中重写GetVaryByCustomString

  


public override string GetVaryByCustomString(HttpContext context, string custom)

    {

        return "LANG=" \+ context.Request.Cookies["PureWirelessEnt.Language"]["OverrideLanguage"].ToString();

    }

 这里的custom就是 varyByCustom 值，框架会调用此方法判断是否有更新。如果定义了多个 varyByCustom的话，这里代码可以使用

  


switch(custom)

{

case "Lang"

...

}

分别管理缓存更新策略。我这里是获取cookie中的语言，如果语言改变时cookie改变，返回值也就改变，就会更新缓存。

 使用缓存策略，在aspx文件头部加入：

<%@ OutputCache CacheProfile="pageCache"%>

即可。

方法很简单，但是很实用，另外，我判断是否缓存起效的方法是，启用调试，然后在页面的page_load中设置断点，然后浏览器中刷新页面，如果能获取断点，说明缓存失效，如果无法获取断点，说明浏览器为缓存中的内容。

本文技术含量不高，圈子里有位朋友自定义实现通过文件的方式进行缓存， 如有未说到之处，或者更好的解决方案，欢迎指正。

  


[](http://blog.csdn.net/zhdd1234/article/details/7214408#)

[](http://blog.csdn.net/zhdd1234/article/details/7214408#)

[](http://blog.csdn.net/zhdd1234/article/details/7214408#)

[](http://blog.csdn.net/zhdd1234/article/details/7214408#)

[](http://blog.csdn.net/zhdd1234/article/details/7214408#)

[](http://blog.csdn.net/zhdd1234/article/details/7214408#)

  


[](http://www.csdn.net/app/)

[](http://www.csdn.net/app/)

[](http://blog.csdn.net/zhdd1234/article/details/)
