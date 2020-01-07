---
title: git排除文件不生效的方案
date: 2017/11/18 9:08:13
tags:
---


git rm -r \--cached .

git add .

git commit -m 'update .gitignore'

  


用以上的方法就把仓库的文件删了!

但是无法现"远程仓库有文件而本地不想编辑"这个功能
