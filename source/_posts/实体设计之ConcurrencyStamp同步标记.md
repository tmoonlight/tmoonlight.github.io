---
title: 实体设计之ConcurrencyStamp同步标记
date: 2018/2/2 19:20:00
tags:
---


concurrencystamp 

  


As the name state, it's used to prevent concurrency update conflict.

For example, there's a UserA named Peter in the database 2 admins open the editor page of UserA, want to update this user.

  1. Admin_1 opened the page, and saw user called Peter.

  2. Admin_2 opened the page, and saw user called Peter (obviously).

  3. Admin_1 updated user name to Tom, and save data. Now UserA in the db named Tom.

  4. Admin_2 updated user name to Thomas, and try to save it.




What would happen if there's no ConcurrencyStamp is Admin_1's update will be overwritten by Admin_2's update. But since we have ConcurrencyStamp, when Admin_1/Admin_2 loads the page, the stamp is loaded. When updating data this stamp will be changed too. So now step 5 would be system throw exception telling Admin_2 that this user has already been updated, since he ConcurrencyStamp is different from the one he loaded.

  


这是一种防止两个用户同时编辑一个数据的好方法,用户A打开界面时,A的同步标签会被附加到页面上,编辑完后保存时,系统会首先校验该同步标签是否已被更改,若被更改,则提示"内容已被修改"点击"确认"打开同步界面"(当然也可以更粗暴一点,比如直接抛出异常).
