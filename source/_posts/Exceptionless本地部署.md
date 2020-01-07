---
title: Exceptionless本地部署
date: 2018/5/21 18:17:30
tags:
---


# [Exceptionless 本地部署](https://www.cnblogs.com/uptothesky/p/5864863.html)

# [免费开源分布式系统日志收集框架 Exceptionless](http://www.cnblogs.com/savorboard/p/exceptionless.html)

前两天看到了这篇文章，亲身体会了下，确实不错，按照官方的文档试了试本地部署，折腾一番后终于成功，记下心得在此，不敢独享。

[本地部署官方wiki](https://github.com/exceptionless/Exceptionless/wiki/Self-Hosting)

  * [.NET 4.6.1](https://msdn.microsoft.com/en-us/library/8z6watww\(v=vs.110\).aspx) 这个因为我装了VS2015，就没有单独再装了

  * [Java JDK 1.8+](http://www.oracle.com/technetwork/java/javase/downloads/index.html) 安装完后还需配置下Java环境，系统变量添加：JAVA_HOME  对应 C:\Program Files\Java\jdk1.8.0_102 是安装jdk的目录，用户变量Path 中添加 %JAVA_HOME%\bin; 配置完成后打开cmd，运行

java -version 如果报错的话有很多种可能，搜索一下会有解决方案，我的就是在C:\Windows\System32 目录下把java.exe改名成javaa.exe，再次cmd运行就成功了

  * IIS 8+ 这个感觉不是强制的，我win7的IIS 7.5也是可以的

  * [ElasticSearch 1.7.5 (](https://www.elastic.co/guide/en/elasticsearch/reference/1.7/setup.html)[Elasticsearch 2.x is not yet supported](https://github.com/exceptionless/Exceptionless/issues/145)) 到连接地址去下载1.7.5版本，人家已经说明2.x的版本不支持，找这个历史版本得翻好几页，大概在第7页左右，直接给个下载连接：[elasticsearch-1.7.5](https://download.elastic.co/elasticsearch/elasticsearch/elasticsearch-1.7.5.zip) ，下载完后解压

  * 下载最新的[latest Exceptionless release artifact ZIP](https://github.com/exceptionless/Exceptionless/releases) ，下载后解压，将目录中的elasticsearch.yml 复制到到elasticsearch的解压目录的bin目录中，执行elasticsearch目录中的elasticsearch.bat，看到最后一行有

started 就说明成功了，打开<http://localhost:9200/>就能看到相关信息，如果es是部署服务的话复制到config目录中，执行

service.bat install|remove|start|stop|manager

  * 在IIS中新建一个网站，路径选择Exceptionless解压目录中的wwwroot目录，端口这里可以自定义，比如用8004，应用程序池选4.0集成

  * 修改web.config中的ElasticSearchConnectionString为es的站点[http://localhost:9200](http://localhost:9200/)，修改<add key="BaseURL" value="<http://localhost:8004/#>" />，注意后面的"#"

  * 修改app.config.*.js中的.constant('BASE_URL', '[http://localhost:8004](http://localhost:8004/)')

  * 打开[http://localhost:8004](http://localhost:8004/) 就能看到登录页面了，然后创建账户-->创建项目，比如创建一个控制台项目，这里就会提示怎么使用Exceptionless

  * ExceptionlessClient.Default.Startup("oXX5BJqhS30ni045BqthqJtiSnpB0naMactfmYmI")，这里的oXX5BJqhS30ni045BqthqJtiSnpB0naMactfmYmI就是api-key

  * 新建控制台项目，使用 [NuGet](http://nuget.org/)  安装

[Install-Package Exceptionless](http://nuget.org/packages/Exceptionless)




在Main中写测试代码：

![](https://common.cnblogs.com/images/copycode.gif)

//ExceptionlessClient.Default.Startup("qnN5lVebQ7LA94Erkthtkq5z57xX5Wg7ZzafiMdZ");

var client = new ExceptionlessClient(c => {

c.ApiKey = "oXX5BJqhS30ni045BqthqJtiSnpB0naMactfmYmI";

c.ServerUrl = "[http://localhost:8004](http://localhost:8004/)";

});

try

{

throw new Exception("test exception "+DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));

}

catch (Exception ex)

{

client.SubmitException(ex);

//ex.ToExceptionless().Submit();

Console.WriteLine("error send");

}

Console.ReadKey();

![](https://common.cnblogs.com/images/copycode.gif)

这里需要注意注释掉的部分，或者是如下调用：

![](https://common.cnblogs.com/images/copycode.gif)

using Exceptionless.Configuration;

[assembly: Exceptionless("oXX5BJqhS30ni045BqthqJtiSnpB0naMactfmYmI", ServerUrl = "[http://localhost:8004](http://localhost:8004/)")]

namespace ExceptionTest

{

class Program

{

static void Main(string[] args)

{

ExceptionlessClient.Default.Startup("oXX5BJqhS30ni045BqthqJtiSnpB0naMactfmYmI");

//var client = new ExceptionlessClient(c => {

// c.ApiKey = "oXX5BJqhS30ni045BqthqJtiSnpB0naMactfmYmI";

// c.ServerUrl = "[http://localhost](http://localhost/):8004";

//});

try

{

throw new Exception("test exception "+DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));

}

catch (Exception ex)

{

//client.SubmitException(ex); ex.ToExceptionless().Submit();

Console.WriteLine("error send");

}

Console.ReadKey();

}

}

}

![](https://common.cnblogs.com/images/copycode.gif)

api-key指定两次，也可以通过。

使用config配置：

![](https://common.cnblogs.com/images/copycode.gif)

<configuration>

<configSections>

<section name="exceptionless" type="Exceptionless.ExceptionlessSection, Exceptionless" />

</configSections>

<exceptionless apiKey="oXX5BJqhS30ni045BqthqJtiSnpB0naMactfmYmI" serverUrl="[http://localhost:8004](http://localhost:8004/)" />

</configuration>

![](https://common.cnblogs.com/images/copycode.gif)

![](https://common.cnblogs.com/images/copycode.gif)

using Exceptionless.Configuration;

//[assembly: Exceptionless("oXX5BJqhS30ni045BqthqJtiSnpB0naMactfmYmI", ServerUrl = "[http://localhost](http://localhost/):8004")]namespace ExceptionTest

{

class Program

{

static void Main(string[] args)

{

ExceptionlessClient.Default.Startup("oXX5BJqhS30ni045BqthqJtiSnpB0naMactfmYmI");

//var client = new ExceptionlessClient(c => {

// c.ApiKey = "oXX5BJqhS30ni045BqthqJtiSnpB0naMactfmYmI";

// c.ServerUrl = "[http://localhost](http://localhost/):8004";

//});

try

{

throw new Exception("test exception "+DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));

}

catch (Exception ex)

{

//client.SubmitException(ex); ex.ToExceptionless().Submit();

Console.WriteLine("error send");

}

Console.ReadKey();

}

}

}

![](https://common.cnblogs.com/images/copycode.gif)

这样就不需要在using那指定了。

接下来就可以在<http://localhost:8004/#/type/error/dashboard>中看到异常信息了。

  


* * *

  


踩坑1：无法外网使用，原因 web.config文件和appconfig.js需要修改端口配置（此外js文件还存在缓存问题，每次修改需要更新js文件！）

踩坑2：无法外网使用，应用程序池调整成4.0，原来是2.0的（坑爹！操作时不要跳跃性太大，不然很容易踩坑）

  

