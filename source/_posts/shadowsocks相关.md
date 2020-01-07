---
title: shadowsocks相关
date: 2014/3/29 3:30:06
tags:
---


服务端使用

rpm -e --nodeps java-1.8.0-openjdk-1.8.0.191.b12-1.el7_6.x86_64

702945

# 

# nohup 

# ssserver -p 443 -k 8675309 -t 600 -m rc4-md5 

# > /dev/null 2>&1 &

#   


#   


#   


# CentOS命令行使用shadowsocks代理的方法

原创 2016年04月11日 14:32:57

标签：

[centos](http://so.csdn.net/so/search/s.do?q=centos&t=blog) /

[shadowsock](http://so.csdn.net/so/search/s.do?q=shadowsock&t=blog)

背景：[前文](http://www.codedream.top/?p=152)介绍了客户端为iOS，Android，Mac，Windows时，使用shadowsocks客户端的方法。本文介绍客户端为CentOS(一般linux环境)时，（尤其是命令行里的命令）使用shadowsocks的方法。 

### 安装客户端shadowsocks

其实shadowsocks安装时是不分客户端还是服务器端的，只不过安装后有两个脚本一个是sslocal代表以客户端模式工作，一个是ssserver代表以服务器端模式工作。

yum install python-pip

pip install shadowsocks

1

2

依次执行上述两个命令，先安装python的pip，然后安装shadowsocks。然后执行下述命令后台启动： 

nohup sslocal -s your_server_ip -p your_server_port -l 1080 -k your_server_passwd -t 600 -m rc4-md5 > /dev/null 2>&1 &

注意 

1,使用的是sslocal这个命令，表示shadowsocks以客户端模式工作 

2,将上述命令里的your_server_ip,your_server_port,your_server_passwd换成自己的，这三个分别代表服务器ip，服务器上shadowsocks的端口以及密码.后面的rc4-md5加密方式也要换成跟server端一致。 

3,前面的nohub表示后台执行，否则将会阻塞shell端口. 

为了更方便，建议新建一个.json的文件，将上述信息放里面,如新建/etc/shadowsocks.json文件，内容为：

{

"server":"your_server_ip", #ss服务器IP

"server_port":your_server_port, #端口

"local_address": "127.0.0.1", #本地ip

"local_port":1080, #本地端口

"password":"your_server_passwd",#连接ss密码

"timeout":300, #等待超时

"method":"rc4-md5", #加密方式

"fast_open": false, # true 或 false。如果你的服务器 Linux 内核在3.7+，可以开启 fast_open 以降低延迟。开启方法： echo 3 > /proc/sys/net/ipv4/tcp_fastopen 开启之后，将 fast_open 的配置设置为 true 即可

"workers": 1 # 工作线程数

}

1

2

3

4

5

6

7

8

9

10

11

然后运行nohup sslocal -c /etc/shadowsocks.json /dev/null 2>&1 &启动shadowsocks。 

如果想增加开启自动启动，执行：echo " nohup sslocal -c /etc/shadowsocks.json /dev/null 2>&1 &" /etc/rc.local 

执行ps aux |grep sslocal |grep -v "grep"查看后台sslocal是否运行。

### 安装Privoxy

上述安好了shadowsocks，但它是socks5代理，我门在shell里执行的命令，发起的网络请求现在还不支持socks5代理，只支持http／https代理。为了我门需要安装privoxy代理，它能把电脑上所有http请求转发给shadowsocks。 

访问官网<http://www.privoxy.org/>获得Privoxy的最新源码:privoxy-3.0.24-stable-src.tar.gz,执行tar -zxvf privoxy-3.0.24-stable-src.tar.gz解压，然后cd privoxy-3.0.24-stable进去。 

安装前需要执行useradd privoxy创建一个用户privoxy，然后依次执行如下三条命令:

autoheader && autoconf

./configure

make && make install

1

2

3

查看vim /usr/local/etc/privoxy/config文件，先搜索关键字:listen-address找到listen-address 127.0.0.1:8118这一句，保证这一句没有注释，8118就是将来http代理要输入的端口。然后搜索forward-socks5t,将forward-socks5t / 127.0.0.1:1080 .此句的注释去掉. 

执行如下命令启动privoxy，参考[官网,不同的平台对应不同的方法](https://www.privoxy.org/user-manual/startup.html): 

privoxy --user privoxy /usr/local/etc/privoxy/config

### 配置/etc/profile

执行vim /etc/profile,添加如下三句:

export http_proxy=http://127.0.0.1:8118

export https_proxy=http://127.0.0.1:8118

export ftp_proxy=http://127.0.0.1:8118

1

2

3

第三句ftp的代理根据需要，不需要的话可以不添加.然后source /etc/profile，执行curl [www.google.com](http://www.google.com/)或wget [www.google.com](http://www.google.com/)判断是否成功访问。

注意:此处不要用ping命令来检测

如果不能访问，请重启机器，依次打开shadowsocks和privoxy再测试.

nohup sslocal -c /etc/shadowsocks.json /dev/null 2>&1 &

privoxy \--user privoxy /usr/local/etc/privoxy/config

1

2

3

备注：如果不需要用代理了，记得把bash里的配置注释上，免得把流量跑完了。

  


  

