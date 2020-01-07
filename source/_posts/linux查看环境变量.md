---
title: linux查看环境变量
date: 2019/4/4 4:59:02
tags:
---


linux也有环境变量

  


$PATH：决定了shell将到哪些目录中寻找命令或程序，PATH的值是一系列目录，当您运行一个程序时，Linux在这些目录下进行搜寻编译链接。

　　编辑你的 PATH 声明，其格式为：

　　PATH=$PATH:<PATH 1>:<PATH 2>:<PATH 3>:------:<PATH N>

　　你可以自己加上指定的路径，中间用冒号隔开。环境变量更改后，在用户下次登陆时生效，如果想立刻生效，则可执行下面的语句：$ source .bash_profile

　　需要注意的是，最好不要把当前路径 “./” 放到 PATH 里，这样可能会受到意想不到的攻击。完成后，可以通过 $ echo $PATH 查看当前的搜索路径。这样定制后，就可以避免频繁的启动位于 shell 搜索的路径之外的程序了。

 

1\. 可用 export 命令查看PATH值

[root@localhost u-boot-sh4]# export

declare -x CVS_RSH="ssh"

declare -x DISPLAY=":0.0"

declare -x G_BROKEN_FILENAMES="1"

declare -x HISTSIZE="1000"

declare -x HOME="/root"

declare -x HOSTNAME="localhost"

declare -x INPUTRC="/etc/inputrc"

declare -x LANG="zh_CN.UTF-8"

declare -x LESSOPEN="|/usr/bin/lesspipe.sh %s"

declare -x LOGNAME="root"

declare -x LS_COLORS="no=00:fi=00:di=00;34:ln=00;36:pi=40;33:so=00;35:bd=40;33;01:cd=40;33;01:or=01;05;37;41:mi=01;05;37;41:ex=00;32:*.cmd=00;32:*.exe=00;32:*.com=00;32:*.btm=00;32:*.bat=00;32:*.sh=00;32:*.csh=00;32:*.tar=00;31:*.tgz=00;31:*.arj=00;31:*.taz=00;31:*.lzh=00;31:*.zip=00;31:*.z=00;31:*.Z=00;31:*.gz=00;31:*.bz2=00;31:*.bz=00;31:*.tz=00;31:*.rpm=00;31:*.cpio=00;31:*.jpg=00;35:*.gif=00;35:*.bmp=00;35:*.xbm=00;35:*.xpm=00;35:*.png=00;35:*.tif=00;35:"

declare -x MAIL="/var/spool/mail/root"

declare -x OLDPWD="/root"

declare -x PATH="/usr/kerberos/sbin:/usr/kerberos/bin:/usr/local/sbin:/usr/local/bin:/sbin:/bin:/usr/sbin:/usr/bin:/root/bin"

declare -x PWD="/opt/STM/STLinux-2.3/devkit/sources/u-boot/u-boot-sh4"

declare -x SHELL="/bin/bash"

declare -x SHLVL="1"

declare -x SSH_ASKPASS="/usr/libexec/openssh/gnome-ssh-askpass"

declare -x TERM="xterm"

declare -x USER="root"

declare -x XAUTHORITY="/root/.xauthkSzH7b"

2\. 单独查看PATH环境变量，可用：

[root@localhost u-boot-sh4]#echo $PATH

/usr/kerberos/sbin:/usr/kerberos/bin:/usr/local/sbin:/usr/local/bin:/sbin:/bin:/usr/sbin:/usr/bin:/root/bin

3\. 添加PATH环境变量(临时)，可用：

[root@localhost u-boot-sh4]#export PATH=/opt/STM/STLinux-2.3/devkit/sh4/bin:$PATH

再次查看：

[root@localhost u-boot-sh4]# echo $PATH

/opt/STM/STLinux-2.3/devkit/sh4/bin:/usr/kerberos/sbin:/usr/kerberos/bin:/usr/local/sbin:/usr/local/bin:/sbin:/bin:/usr/sbin:/usr/bin:/root/bin

说明添加PATH成功。

上述方法的PATH 在终端关闭 后就会消失。

4\. 永久添加环境变量(影响当前用户)

#vim ~/.bashrc

export PATH="/opt/STM/STLinux-2.3/devkit/sh4/bin:$PATH"

 

5.永久添加环境变量(影响所有用户)

# vim /etc/profile

在文档最后，添加:

export PATH="/opt/STM/STLinux-2.3/devkit/sh4/bin:$PATH"

保存，退出，然后运行：

#source /etc/profile

不报错则成功。

问题： 

1\. 做了各实验，在/etc/profile, ~/.profile, ~/.bashrc中加入新PATH，重启都没有效果，只有使用source才可以，ubunt12.04

 找到原因，~/.zshrc导致的，因为在zshrc中直接对PATH重新赋值，而没有继承之前的$PATH，导致启动加载完/etc/profile后，PATH又被重新赋值。
