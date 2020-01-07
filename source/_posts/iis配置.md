---
title: iis配置
date: 2018/7/20 15:45:06
tags:
---


# 有人问到了类似问题，这里给出一个标准模板，方便以后从0开始配iis大型工作环境时参考

=================================================

  


  


　　找到Web站点对应的应用程序池，“应用程序池” → 找到对应的“应用程序池” → 右键“高级设置...”

　　![](https://images2015.cnblogs.com/blog/665662/201610/665662-20161010142245633-1929353268.png)

## 一、一般优化方案

　　1、基本设置

　　[1] 队列长度： 默认值1000，将原来的队列长度改为 65535。

　　[2] 启动32位应用程序：默认值False，改为True， 否则安装一些32的组建或32位的php都会出错。

　　[3] 托管管道模式：Integrated 或 Classsic。 

　　[![](http://files.jb51.net/file_images/article/201606/IIS75/IIS753.png)](http://files.jb51.net/file_images/article/201606/IIS75/IIS753.png)

　　2、高级设置

　　[1] 闲置超时（分钟）：默认20分钟，修改设长。

　　[2] 快速故障防护 → 已启用 ：默认True，改为False。 

　　[![](http://files.jb51.net/file_images/article/201606/IIS75/IIS7512.png)](http://files.jb51.net/file_images/article/201606/IIS75/IIS7512.png)

　　3、解决PEP第一次打开PEP速度慢

　　回收间隔时间

　　[![](http://files.jb51.net/file_images/article/201606/IIS75/IIS759.png)](http://files.jb51.net/file_images/article/201606/IIS75/IIS759.png)

　　使用windows server 2008 r2解决回收假死的问题

　　打开应用程序池 -> 高级设置 ->在“禁止重叠回收”里选择“true”，这样就有效避免了应用程序池回收假死问题。

　　![](http://files.jb51.net/file_images/article/201510/20151017230541.png)

## 二、支持同时10万个请求

　　通过对IIS7的配置进行优化，调整IIS7应用池的队列长度，请求数限制，TCPIP连接数等方面，从而使WEB服务器的性能得以提升，保证WEB访问的访问流畅。

　　站点碰到如下问题：

　　Error Summary:

　　HTTP Error 503.2 - Service Unavailable

　　The serverRuntime@appConcurrentRequestLimit setting is being exceeded.

　　Detailed Error Information:

　　Module IIS Web Core 

　　Notification BeginRequest 

　　Handler StaticFile

　　Error Code 0x00000000

　　由于之前使用的是默认配置，服务器最多只能处理5000个同时请求，今天下午由于某种情况造成同时请求超过5000，从而出现了上面的错误。

　　为了避免这样的错误，我们根据相关文档调整了设置，让服务器从设置上支持10万个并发请求。

　　具体设置如下：

　　1. 调整IIS 7应用程序池队列长度

　　将原来的队列长度由默认值 1000 改为 65535。当然这里的队列长度你可以根据自己的 访问用户*1.5 来设置，例如：有2000用户，此处就可以设置为3000(3000=2000用户数*1.5）。

　　2.  调整IIS 7的appConcurrentRequestLimit设置

　　由原来的默认5000改为100000。

　　[1] 在cmd中执行：

　　c:\windows\system32\inetsrv\appcmd.exe set config /section:serverRuntime /appConcurrentRequestLimit:100000

　　[2] 在%systemroot%\System32\inetsrv\config\applicationHost.config中可以查看到该设置：

　　<serverRuntime appConcurrentRequestLimit="100000" />

　　![](http://files.jb51.net/file_images/article/201606/20160608000103.png)

　　[![](http://files.jb51.net/file_images/article/201606/20160608000104.png)](http://files.jb51.net/file_images/article/201606/20160608000104.png)

　　3. 调整machine.config中的processModel>requestQueueLimit的设置

　　[1] 单击“开始”，然后单击“运行”，或者 windows + R。

　　[2] 在“运行”对话框中，键入 notepad %systemroot%\[Microsoft.Net](http://microsoft.net/)\Framework64\v4.0.30319\CONFIG\machine.config，然后单击“确定”。(不同的.NET版本路径不一样，可以选择你自己当前想设置的.NET版本的config)

　　[3] 找到如下所示的 processModel 元素：<processModel autoConfig="true" />

　　[4] 将 processModel 元素替换为以下值：<processModel enable="true" requestQueueLimit="15000" />

　　![](http://files.jb51.net/file_images/article/201606/20160608000105.png)

　　[5] 保存并关闭 Machine.config 文件。

　　由原来的默认5000改为100000。

<configuration>

<system.web>

<processModel enable="true" requestQueueLimit="100000"/>

　　参考文章：<http://technet.microsoft.com/en-us/library/dd425294(office.13).aspx>

　　4. 修改注册表，调整IIS 7支持的同时TCPIP连接数

　　由原来的默认5000改为100000。在cmd中执行：

　　reg add HKLM\System\CurrentControlSet\Services\HTTP\Parameters /v MaxConnections /t REG_DWORD /d 100000

　　![](https://images2015.cnblogs.com/blog/665662/201610/665662-20161010144453524-461950581.png)

　　可在注册表中查看

　　[![](http://files.jb51.net/file_images/article/201606/20160608000107.png)](http://files.jb51.net/file_images/article/201606/20160608000107.png)

　　5. 运行命令使用设置生效

　　net stop http  & net start  http & iisreset

　　完成上述5个设置，就可以支持10万个并发请求，博客园博客服务器已经启用上述设置。

　　为了方法大家与自己使用，我把上面能用bat操作简单放到一个bat文件里面了。将下面的内容保存为do.bat文件运行就可以了，需要手工的自己操作

## 三、支持高并发的IIS Web服务器常用设置 　　

　　适用的IIS版本：IIS 7.0, IIS 7.5, IIS 8.0

　　适用的Windows Server版本：Windows Server 2008, Windows Server 2008 R2, Windows Server 2012

　　1、应用程序池（Application Pool）的设置：

　　[1] General->Queue Length设置为65535（队列长度所支持的最大值）

　　[2] Process Model->Idle Time-out设置为0（不让应用程序池因为没有请求而回收）

　　[3] Recycling->Regular Time Interval设置为0（禁用应用程序池定期自动回收）

　　2、.Net Framework相关设置

　　[1] 在machine.config中将

　　< processModel autoConfig="true" />

　　改为

　　<processModel enable="true" requestQueueLimit="100000"/>

　　（保存后该设置立即生效）

　　[2] 打开C:\Windows\[Microsoft.NET](http://microsoft.net/)\Framework64\v4.0.30319\Config\Browsers\Default.browser，找到<defaultBrowser id="Wml" parentID="Default" >，注释<capabilities>部分，然后在命令行中运行aspnet_regbrowsers -i。以解决text/vnd.wap.wml问题。

　　设置命令：

　　c:\windows\system32\inetsrv\appcmd.exe set config /section:serverRuntime /appConcurrentRequestLimit:100000

　　设置结果：

　　< serverRuntime appConcurrentRequestLimit="100000" />

　　（保存后该设置立即生效）

　　4、http.sys的设置

　　注册表设置命令1（将最大连接数设置为10万）：

　　reg add HKLM\System\CurrentControlSet\Services\HTTP\Parameters /v MaxConnections /t REG_DWORD /d 100000

　　注册表设置命令2（解决Bad Request - Request Too Long问题）：

　　reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\HTTP\Parameters /v MaxFieldLength /t REG_DWORD /d 32768

　　reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\HTTP\Parameters /v MaxRequestBytes /t REG_DWORD /d 32768

　　(需要在命令行运行 net stop http  & net start http & iisreset 使设置生效)

　　5、针对负载均衡场景的设置

　　在Url Rewrite Module中增加如下的规则：

　　注意事项：添加该URL重写规则会造成IIS内核模式缓存不工作，详见微软的坑：Url重写竟然会引起IIS内核模式缓存不工作。

　　6、 设置Cache-Control为public

　　在web.config中添加如下配置：

![](https://common.cnblogs.com/images/copycode.gif)

<configuration>

<system.webServer>

<staticContent>

<clientCache cacheControlCustom="public" />

</staticContent>

</system.webServer></configuration>

![](https://common.cnblogs.com/images/copycode.gif)

　　在machine.config的<processModel>中添加如下设置：

< processModel enable="true" maxWorkerThreads="100" maxIoThreads="100" minWorkerThreads="50" minIoThreads="50"/>

  


  

