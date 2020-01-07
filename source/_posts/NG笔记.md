---
title: NG笔记
date: 2018/4/10 8:20:01
tags:
---


NPM发布：  


发布前准备：

NG中发布npm时至少有一个module对应你的组件

假想一个墙纸包，里面包含n种墙纸，则做成n中组件，将组件组织到ngModule中即可

如果需要使用组件，必须在#ngmodule中使用

declarations: [THelloComponent],  
exports: [THelloComponent]

将该组件发布出去

需要有一个入口js 如index.js  


文件中导入组件，例如

  


  


  


rem 先登录上login，不登录直接发布会提示(root/administrator)无权之类的提示  


  


npm login 

  


rem 直接执行npm发布命令即可

  


npm publish

注意：发布时npm不能以1.0.0这种来编号版本

  


  


  

