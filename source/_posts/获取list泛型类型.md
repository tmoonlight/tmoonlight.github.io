---
title: 获取list泛型类型
date: 2017/10/25 9:04:47
tags:
---


获取方法泛型列表的元素类型,比如List<string,int>  这个 source.GetType().GenericTypeArguments[0]返回的就是string ,[1] 是int 这个不仅仅用在list上 还可以用在诸多泛型类型中.

  

