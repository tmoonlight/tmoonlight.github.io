---
title: Nginx子路径到端口映射
date: 2019/2/23 2:49:01
tags:
---


# Nginx子路径到端口映射

2014年11月18日 15:17:50 阅读数：4386更多

个人分类： [nginx](https://blog.csdn.net/yzhou86/article/category/2718065)

有时候需要使用nginx来将子路径映射到某个特殊端口上，例如下面的场景。

  


nginx所在服务器的域名为[www.service.com](http://www.service.com/)，监听在443端口，SSL已经打开。

此服务器上还有一个https服务名为test，运行在444端口上。这时候需要将发到<https://www.service.com/test> 的请求，全部转发到<https://www.service.com:444/> 上。

  


看下面的nginx配置

test.conf

  


server{

        listen 443 default_server;

        server_name [www.service.com](http://www.service.com/);

  


  


        ssl  on;

        ssl_certificate  /opt/nginx/conf.d/server.crt;

        ssl_certificate_key  /opt/nginx/conf.d/server.key.unsecure;

        ssl_client_certificate /opt/nginx/conf.d/ca.crt;

        ssl_verify_client off;

  


  


  


  


        client_max_body_size 100m;

  


  


        location / {

                rewrite / [https://www.service.com:443/test/](https://www.service.com/test/) permanent;

                proxy_set_header   Host             $host:443;

                proxy_set_header   X-Real-IP        $remote_addr;

                proxy_set_header   X-Forwarded-For  $proxy_add_x_forwarded_for;

                proxy_set_header Via    "nginx";

        }

  


  


        location ^~/test/ {

                proxy_pass  <https://www.service.com:444/>;

                proxy_set_header   X-Forwarded-For  $proxy_add_x_forwarded_for;

                proxy_set_header Via    "nginx";

        }

}

  

