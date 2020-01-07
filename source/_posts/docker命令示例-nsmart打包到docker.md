---
title: docker命令示例-nsmart打包到docker
date: 2014/6/28 2:02:42
tags:
---


docker build . -t 名字 -f docker文件 

docker run --name //容器的名字 -d 镜像的名字 最好用一下容器名，方便attach或者exec

docker run -dit nsp nsprun -d -P --net=host nsp1 tail -f /dev/null //host模式出不去

docker exec -it a4b7d07ec51b /bin/bash

docker rm $(docker ps -a -q)//删container

docker rmi $(docker images -q)//删镜像

  


docker run --name nsp5 -dit -p 12308:12308 -p 12309:12309 -p 19974:19974 -p 20000-20050 nsp

 //端口太多会导致死机哦

  


* * *

docker发布：三步：1登陆 2打标签 3push

docker login 

docker tag <existing-image>  <hub-user>/<repo-name>[:<tag>]  #tag不指定默认为latest

docker push <hub-user>/<repo-name>:<tag>

  


* * *

azurepipeline采坑

  


workingdirectory不能设置很深的路径，不知道为啥 不是不能设置 是根本没这个路径

 **始终提示无法打包docker，原来是因为是个zip包！！！**

  


dockerfile 

docker build . -t d:/test -f d:/test/Dockerfile

  


为什么azdemo不会zip呢？

  


出现docker权限不足，原因有二：

1.login，push顺序错乱

2.没有打tag，屁都没有，生传
