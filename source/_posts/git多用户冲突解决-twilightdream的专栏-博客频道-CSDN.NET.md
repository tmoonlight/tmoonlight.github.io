---
title: git多用户冲突解决-twilightdream的专栏-博客频道-CSDN.NET
date: 2016/8/3 5:09:45
tags:
---


#  [ git 多用户冲突解决 ](http://blog.csdn.net/twilightdream/article/details/72953313)

.

标签： [git](http://www.csdn.net/tag/git)[合并](http://www.csdn.net/tag/%e5%90%88%e5%b9%b6)

2017-06-09 08:43 44人阅读 [评论](http://blog.csdn.net/twilightdream/article/details/72953313#comments)(0) 收藏 [举报](http://blog.csdn.net/twilightdream/article/details/72953313#report "举报")

.

分类：

git _（4）_

.

版权声明：本文为博主原创文章，未经博主允许不得转载。

目录[(?)](http://blog.csdn.net/twilightdream/article/details/72953313# "系统根据文章中H1到H6标签自动生成文章目录")[[+]](http://blog.csdn.net/twilightdream/article/details/72953313# "展开")

#### git多用户冲突一

**场景：dev 分支修改了 File1，master 分支更新了远端代码后也修改了 File1 的同一个代码块，dev 变基到 master 时会产生冲突**   
**解决方法：**   
case 1 ：如果希望 master 分支完全覆盖 dev 分支的修改，使用如下命令回退后变基：
    
    
    git reset --hard
    git rebase master
    
      * 1
      * 2
    
    
      * 1
      * 2
    
    

case 2 ：希望保留 dev 分支的改动。如果 dev 分支的改动没有commit，需要暂存变更，变基后再手动合并冲突，使用命令如下：
    
    
    git stash
    git rebase master
    git stash pop         // 会有冲突
    git diff ***          // 手动合并冲突，***代表文件名
    git add ***           // 修改完成后提交
    git rebase --continue // 完成变基
    
      * 1
      * 2
      * 3
      * 4
      * 5
      * 6
    
    
      * 1
      * 2
      * 3
      * 4
      * 5
      * 6
    
    

其中 [Git](http://lib.csdn.net/base/git "Git知识库") stash 表示备份当前工作区内容到 [git](http://lib.csdn.net/base/git "Git知识库") 栈，并使当前工作区内容与上次提交时一致，更新最新代码，git stash pop 表示从 Git 栈中读取最近一次保存的内容，恢复工作区的相关内容，git diff 表示手动 merge 冲突的文件

case 3 ：如果本地已经 commit，使用如下命令撤销最近一个 commit，再执行步骤2
    
    
    git reset HEAD^
    
      * 1
    
    
      * 1
    
    

  


#### git多用户冲突二

**场景：用户 UserA 提交了 change A，暂时没有 review，未被 merge 到远端主分支。之后用户 UserB 提交了change B，merge 成功。当 merge change A 时出错**

**解决方法：**   
1 . gerrit 上 Abandon change A

2 . 本地撤销此次提交
    
    
    git reset HEAD^
    
      * 1
    
    
      * 1
    
    

3 . 更新代码，重新提交

[](http://blog.csdn.net/twilightdream/article/details/72953313#) [](http://blog.csdn.net/twilightdream/article/details/72953313# "分享到QQ空间") [](http://blog.csdn.net/twilightdream/article/details/72953313# "分享到新浪微博") [](http://blog.csdn.net/twilightdream/article/details/72953313# "分享到腾讯微博") [](http://blog.csdn.net/twilightdream/article/details/72953313# "分享到人人网") [](http://blog.csdn.net/twilightdream/article/details/72953313# "分享到微信") .

顶
    0

踩
    0
.

 

 

  * 上一篇[Ubuntu 上 gerrit 服务器的搭建](http://blog.csdn.net/twilightdream/article/details/72953282)
  * 下一篇[git 推荐流程](http://blog.csdn.net/twilightdream/article/details/72953324)
.


#### 

  相关文章推荐 

  * _•_ [Git详解之五：分布式Git](http://xiebinghu.iteye.com/blog/1844967 "Git详解之五：分布式Git")
  * _•_ [Git使用之——冲突解决一（git merge conflict）](http://blog.csdn.net/u012150179/article/details/14047183 "Git使用之——冲突解决一（git merge conflict）")
  * _•_ [珊珊来迟的解决方法——VB中防止多用户登录](http://webdev2014.iteye.com/blog/2027807 "珊珊来迟的解决方法——VB中防止多用户登录")
  * _•_ [SVN多用户同时修改一个文件冲突过程分析及解决方法（非用锁方法）](http://blog.csdn.net/stephenzhu/article/details/6320955 "SVN多用户同时修改一个文件冲突过程分析及解决方法（非用锁方法）")
  * _•_ [node.js 初体验](http://sydica.iteye.com/blog/2064856 "node.js 初体验")


  * _•_ [多用户(windows)远程登录ubuntu 10.04 解决方案](http://sdhsdhsdhsdh.iteye.com/blog/741635 "多用户\(windows\)远程登录ubuntu 10.04 解决方案")
  * _•_ [git配置和常用命令](http://blog.csdn.net/happyer88/article/details/50722665 "git配置和常用命令")
  * _•_ [Git详解](http://blog.csdn.net/zmissm/article/details/39103147 "Git详解")
  * _•_ [网站多用户权限管理的一个解决方案](http://jjz.iteye.com/blog/105075 "网站多用户权限管理的一个解决方案")
  * _•_ [【SVN多用户开发】代码冲突&解决办法](http://blog.csdn.net/zhu1991_/article/details/54017247 "【SVN多用户开发】代码冲突&解决办法")


