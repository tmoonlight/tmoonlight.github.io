---
title: docker17.06.2-ce安装指南
date: 2013/10/22 14:39:12
tags:
---



[docker](http://so.csdn.net/so/search/s.do?q=docker&t=blog) /

[linux](http://so.csdn.net/so/search/s.do?q=linux&t=blog) /

[清华docker](http://so.csdn.net/so/search/s.do?q=%E6%B8%85%E5%8D%8Edocker&t=blog) /

[安装docker](http://so.csdn.net/so/search/s.do?q=%E5%AE%89%E8%A3%85docker&t=blog)

docker 17.03.1安装指南

2017 04 05

美国原版 默认源 没有问题

sudo apt install curl 

 sudo curl -sSL <https://get.docker.com/> | sudo sh 

清华 centos yum 镜像

<https://mirrors.tuna.tsinghua.edu.cn/help/centos/>

清华 docker-ce

<https://mirrors.tuna.tsinghua.edu.cn/help/docker-ce/>

  


  


如果你之前安装过 docker，请先删掉

sudo yum remove docker docker-common docker-selinux docker-engine

安装一些依赖

sudo yum install -y yum-utils device-mapper-persistent-data lvm2

根据你的发行版下载repo文件: 

wget -O /etc/yum.repos.d/docker-ce.repo <https://download.docker.com/linux/centos/docker-ce.repo>

把软件仓库地址替换为 TUNA:

sudo sed -i 's+[download.docker.com](http://download.docker.com/)+[mirrors.tuna.tsinghua.edu](http://mirrors.tuna.tsinghua.edu/).cn/docker-ce+' /etc/yum.repos.d/docker-ce.repo

最后安装:

sudo yum makecache fast

sudo yum install docker-ce

  


  


17.06.2-ce

  


  


sudo usermod -aG docker your-user

centos 7.3 

2017-9-4

 su root 

curl -sSL <http://get.docker.com/>  | sh

usermod -aG docker root

service docker start

腾讯云 加速地址

curl -sSL <https://get.daocloud.io/daotools/set_mirror.sh> | sh -s [http://mirror.ccs.tencentyun.com](http://mirror.ccs.tencentyun.com/)

  


service docker restart

chkconfig docker on

  


  


* * *

debian+raspberrypi的按抓国内方法

# 查看内核版本及架构

uname -ar

  


# 查看 debian 版本

lsb_release -cs

  


# deb包地址 ：<https://download.docker.com/linux/debian/dists/># 然后选对应 debian 版本的　/pool/stable 文件下的 对应 架构下下载 deb 包# 我的 debian 版本是 jessie ，架构是 armv71

  


所以选择 jessie/ -> /pool/stable/ -> armhf

  


# 下载 (版本随便挑)

  


wget https://[download.docker.com/linux/debian/dists/jessie/pool/stable/armhf/docker-ce_18.06.1~ce~3-0~debian_armhf.deb](http://download.docker.com/linux/debian/dists/jessie/pool/stable/armhf/docker-ce_18.06.1~ce~3-0~debian_armhf.deb)

  


  


# dpkg 安装

$ sudo dpkg -i /包地址/包.deb

  


# 验证安装

$ sudo docker run hello-world  


  


手动安装sdk  把可执行文件放到任意位置 使用 /usr/bin创建连接
