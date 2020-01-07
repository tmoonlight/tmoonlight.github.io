---
title: git心得
date: 2014/4/27 9:14:32
tags:
---


git clone只会拉远程master分支

其他分支需要使用

git checkout -b dev origin/daily/1.4.1

  


远端git回滚  git reset  然后git push -f 有风险时必须加f参数才能push上去

  


git部分代码合并->git摘取

  


git rebase操作，等同于merge，但是merge相当于服务端进行了合并，rebase是把其他分支的改动提取出来在当前分支再提交一次

  


git保存的是文件的副本，而svn保存的是提交的历史，所以不管是回滚还是切换分之，git都特别快，

1\. git clone **(项目地址) 

2\. git status 

3\. git add * 

4\. git commit -m “备注” 

5\. git push origin 分支名  到新分支

6\. git pull 

7\. git checkout 分支名 切换分支 （新分支）

8\. git merge 分支名 

 git commit -m “”, git push origin 。 

  


git pull出错 本地有多余文件

  


git clean -n

git clean -df

git clean -f
