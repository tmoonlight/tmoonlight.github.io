---
title: 苍穹BOS踩坑
date: 2017/9/26 7:16:26
tags:
---


1.idea断点总是调到class里，因为入口debugmservice没有complie这个项目，（切记编辑完了还要refresh一下gradle项目）

  


2.一直出现dbimpl的错误，很奇葩，一个个把包打进去就修复了

  


包已经看的到 但是build还有波浪线

类似这种https://ask.csdn.net/questions/760274  


如下图这种情况只是因为包文件没有了 但这里还存在

  


最后发现只是这里显示了路径而已？实际上文件并不存在？

解决步骤：1.gradle引用的bos-corelib改成bos，2.删除掉仓库之后重新拉包 ok

  


心得：拉新包之后先空跑一下 跑的起来再编译项目
