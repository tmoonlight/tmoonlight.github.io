---
title: OutputCache缓存优化asp.net代码提高网页性能作者
date: 2016/5/3 0:12:13
tags:
---


## 对于asp.net编写的网页来说，使用缓存是一种非常重要也是很常用的优化技术，它可以大大减轻服务器的负载压力，优化这些网页的性能，在网与使用 .NET Framework 的任何其他功能相比，适当地使用缓存可以更好地提高站点的性能。同时，在网页加速显示上也起了很大的作用。

  *  **OutputCache**  
以声明的方式控制 ASP.NET 页或页中包含的用户控件的输出缓存策略。

 **语法** ：

<%@ OutputCache Duration="#ofseconds"  
   Location="Any | Client | Downstream | Server | None | ServerAndClient "  
   Shared="True | False"  
   VaryByControl="controlname"  
   VaryByCustom="browser | customstring"  
   VaryByHeader="headers"  
   VaryByParam="parametername"  
   VaryByContentEncoding="encodings"  
   CacheProfile="cache profile name | ''"  
   NoStore="true | false"  
   SqlDependency="database/table name pair | CommandNotification"  
   ProviderName="Provider Name"   
%>

 **参数解释** ：

 **参数**|  **说明**|  **备注**  
---|---|---  
Duration| 

页或用户控件进行缓存的时间（以秒计）。 在页或用户控件上设置该特性为来自对象的 HTTP 响应建立了一个过期策略，并将自动缓存页或用户控件输出。

| 

此特性必选。 如果未包含该属性，将出现分析器错误。

除非你的Location=None，可以不添加此属性，其余时候都是必须的。  
  
 Location| 

OutputCacheLocation 枚举值之一。 默认值为 Any。  
Location当被设置为None时，其余的任何设置将不起作用；  
Any——页面被缓存在浏览器、代理服务器端和web服务器端；  
Client——缓存在浏览器；  
DownStream——页面被缓存在浏览器和任何的代理服务器端；  
None——页面不缓存；  
ServerAndClient——页面被缓存在浏览器和web服务器端。

| 包含在用户控件（.ascx 文件）中的 @ OutputCache 指令不支持此特性。  
 CacheProfile| 与该页关联的缓存设置的名称。 这是可选特性，默认值为空字符串 ("")。| 

 包含在用户控件（.ascx 文件）中的 @ OutputCache 指令不支持此特性。 在页中指定此属性时，属性值必须与 outputCacheSettings 节下面的 outputCacheProfiles 元素中的一个可用项的名称匹配。 如果此名称与配置文件项不匹配，将引发异常。

 **参考** ：[ _CacheProfile实例_](http://www.webkaka.com/tutorial/asp.net/2012/111913/)  
  
 NoStore| 一个布尔值，它决定了是否阻止敏感信息的二级存储。| 包含在用户控件（.ascx 文件）中的 @ OutputCache 指令不支持此特性。 将此特性设置为 true 等效于在请求期间执行以下代码：  
Response.Cache.SetNoStore();  
 ProviderName| 一个字符串值，标识要使用的自定义输出缓存提供程序。| 此属性仅在用户控件（.ascx 文件）中受到支持。 它不受包含在 ASP.NET 页（.aspx 文件）中的 @ OutputCache 指令的支持。  
 Shared| 一个布尔值，确定用户控件输出是否可以由多个页共享。 默认值为 false。| 包含在 ASP.NET 页（.aspx 文件）中的 @ OutputCache 指令不支持此特性。  
 SqlDependency| 标识一组数据库/表名称对的字符串值，页或控件的输出缓存依赖于这些名称对。 请注意，SqlCacheDependency 类监视输出缓存所依赖的数据库中的表，因此当更新表中的项时，使用基于表的轮询时将从缓存中移除这些项。 如果以值 CommandNotification 使用通知（在 Microsoft SQL Server 2005 中），则最终会使用 SqlDependency 类向 SQL Server 2005 服务器注册查询通知。| 

SqlDependency 特性的 CommandNotification 值仅在网页 (.aspx) 中有效。 用户控件只能将基于表的轮询用于 @ OutputCache 指令。

 **参考** ：[ _SqlDependency和SqlCacheDependency缓存的用法及具体步骤_](http://www.webkaka.com/tutorial/asp.net/2012/111912/)  
  
 VaryByCustom| 任何表示自定义输出缓存要求的文本。 如果特性的赋值为 browser，缓存将随浏览器名称和主要版本信息的不同而异。 如果输入自定义字符串，则必须在应用程序的 Global.asax 文件中重写 GetVaryByCustomString 方法。|    
 VaryByHeader| 

分号分隔的 HTTP 标头列表，用于使输出缓存发生变化。 将该特性设为多标头时，对于每个指定标头组合，输出缓存都包含一个不同版本的请求文档。  
Accept-Language——代表请求页面中用户最希望的有优先级顺序的人类语言列表；  
User-Agent——代表请求页面设备的类型；  
Cookie——代表当前域名下创建的浏览器的cookie项。

| 

设置 VaryByHeader 特性将启用在所有 HTTP 1.1 版缓存中缓存项，而不仅仅在 ASP.NET 缓存中进行缓存。 用户控件中的 @ OutputCache 指令不支持此特性。  
例如：设置VaryByHeader="Accept-Language"。  
当网站有多种语言版本时，可以为每种语言都进行缓存。  
  
 VaryByParam| 分号分隔的字符串列表，用于使输出缓存发生变化。 默认情况下，这些字符串对应于使用 GET 方法特性发送的查询字符串值，或者使用 POST 方法发送的参数。 将该特性设置为多个参数时，对于每个指定参数组合，输出缓存都包含一个不同版本的请求文档。 可能的值包括 none、星号 (*) 以及任何有效的查询字符串或 POST 参数名称。| 

在 ASP.NET 页和用户控件上使用 @ OutputCache 指令时，需要此特性或 VaryByControl 特性。 如果没有包含它，则发生分析器错误。 如果不希望通过指定参数来改变缓存内容，请将值设置为 none。 如果希望通过所有的参数值改变输出缓存，请将特性设置为星号 (*))。  
例如：  
如果命令设置为：  
<%@ OutputCache Duration="60"  VaryByParam="ProductType" %>  
当请求路径 bitauto.com/test.aspx?ProductType=1发生时，会建立缓存当ProductType的值变为2时，系统会建立新的缓存,但是原来的缓存在有效期内并不会失效。相当于为不同的ProductType值建立了不同版本的缓存。如果熟悉.Net数据缓存的话，很类似于下面的用法。  
Cache.Add(“productType1”,cachedObject)  
Cache.Add(“productType2”,cachedObject)  
  
 VaryByControl| 分号分隔的字符串列表，用于改变用户控件的输出缓存。 这些字符串代表用户控件中声明的 ASP.NET 服务器控件的 ID 属性值。| 在 ASP.NET 页和用户控件上使用 @ OutputCache 指令时，需要此特性或 VaryByParam 特性。  
例如下面：根据页面上下拉列表控件的选择的值不同进行不同的缓存输出  
<%@ OutputCache Duration="100" VaryByControl="dropTest"%>  
 VaryByContentEncodings| 以分号分隔的字符串列表，用于更改输出缓存。 将 VaryByContentEncodings 特性用于 Accept-Encoding 标头，可确定不同内容编码获得缓存响应的方式。|    
  
 **OutputCache示例** ：

下面的代码示例演示如何设置页或用户控件进行输出缓存的持续时间。

<%@ OutputCache Duration="100" VaryByParam="none" %>

下一个代码示例演示如何指示输出缓存按页或用户控件的位置对它们进行缓存，并根据窗体的 POST 方法或查询字符串对窗体参数进行计数。每个收到的具有不同位置或计数参数（或两者）的 HTTP 请求都进行 10 秒的缓存处理。带有相同参数值的任何后继请求都将从缓存中得到满足，直至超过输入的缓存期。

<%@ OutputCache Duration="10" VaryByParam="location;count" %>

 **应用实例** ：

 

▲图1 停止缓存的效果图

 

▲图2 执行缓存的效果图

如图1所示，应用程序初始显示的是停止执行缓存的时间。当用户刷新页面时，时间值将随时变化，以便显示当前的最新时间。

如图2所示，单击“缓存时间”超链接后，页面重定向到另一个页面。这时，页面显示的时间被缓存，数据过期时间为5秒。如果不断地刷新该页，那么每隔5秒钟时间值才变化一次。

本节示例存在两个关键点。一是在运行时实现停止缓存，二是配置@OutputCache指令。这两点都已经在应用程序Default.aspx文件中予以实现，下面列举了该文件源代码。

Default.aspx文件源代码：

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>  
<%@ OutputCache Duration="5" VaryByParam="location" %>  
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">  
<html xmlns="http://www.w3.org/1999/xhtml">  
<script language="C# " runat="server">  
void Page_Load(object sender, EventArgs e)  
{   //设置仅将缓存数据存储在服务器上    
    Response.Cache.SetCacheability(HttpCacheability.Server);    
    string temp_location = Request.QueryString["location"];    
    //如果location为空，则不缓存，否则根据@ OutputCache指令声明执行缓存    
    if (temp_location == null)   {    
        //停止当前响应的所有服务器缓存    
        Response.Cache.SetNoServerCaching();    
        Label1.Text = "停止缓存的时间：" \+ DateTime.Now.ToString();    
    }    
    else  
    {    
        Label1.Text = "设置了缓存的时间：" \+ DateTime.Now.ToString();    
    }    
}    
</script>    
<head id="Head1" runat="server">    
<title>示例12-1</title>    
<link id="InstanceStyle" href="StyleSheet.css" type="text/css" rel="stylesheet" />    
</head>    
<body>    
<form id="form1" runat="server">    
<div>    
<fieldset style="width: 240px">    
<legend class="mainTitle">设置页面输出缓存</legend><br />  
<center><asp:Label ID="Label1" runat="server" CssClass="commonText"></asp:Label></center><br />    
<a href="Default.aspx?location=beijing" class="littleMainTitle" >缓存时间</a><br />  
</fieldset>  
</div>    
</form>    
</body>    
</html>  
 

如上代码所示，代码头部的@OutputCache指令设置了Duration和VaryByParam属性，其指示数据过期时间为5秒。同时，缓存根据参数location发生变化。另外，代码还实现了Page_Load事件处理程序。在该程序中，首先，使用SetCacheability方法设置数据缓存必须存储在服务器上，然后，获取QueryString的location参数值，最后，根据location参数值进行判断。如果location参数值为空，则调用SetNoServerCaching方法停止当前响应的所有服务器缓存，并显示当前时间值。虽然@ OutputCache指令配置了页面输出缓存，但是，不会执行页面输出缓存功能。如果location参数值不为空，则直接显示当前时间值。在这种情况下，将执行@ OutputCache指令的配置内容。

< %@ OutputCache NoStore="True" Duration="15" Location="Any" VaryByControl="OC" VaryByCustom="browser" VaryByHeader="headers" VaryByParam="none" %>

 **使用OutputCache注意事项**

在使用 ASP.NET 缓存时，应注意以下事项。首先，不要缓存太多项。缓存每个项都有内存开销。不要缓存容易重新计算和很少使用的项。其次，给缓存项分配的有效期不要太短。很快到期的项会导致缓存中不必要的周转，并且会导致额外的代码清除和垃圾回收工作。使用与“ASP.NET Applications”性能对象关联的“Cache Total Turnover Rate”（缓存总流通率）性能计数器，您可以监视缓存中由于项到期而导致的周转。高周转率可能说明存在问题，特别是当项在到期前被移除时。（这种情况有时称作内存压力。）

可以考虑把静态的、变化不大的或者不经常变化需要动态加载的内容放入控件中，使用缓存技术。



