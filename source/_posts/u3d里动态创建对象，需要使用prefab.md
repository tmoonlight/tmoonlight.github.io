---
title: u3d里动态创建对象，需要使用prefab
date: 2016/7/2 12:55:50
tags:
---


u3d里动态创建对象，需要使用prefab

而创建的时候 MonoBehaviour.Instantiate( GameObject orignal) 需要一个作为原型的对象。

本文提供三种方式获得prefab对象。

 

方式一：使用脚本的public字段

直接在Project视图里找到做好的prefab，将其拖拽到指定脚本的指定public GameObject 字段。

 

方式二：Resource类

1、在Assets目录下的任意位置创建一个名为resources的文件夹，将做好的prefab放到这个文件夹下，path形式如下:

Assets\\....\resources\prefabName.prefab

2、在代码里使用Resource.Load 或 LoadAll 函数，获得原型对象。

指定prefab时不需要指定扩展名（.prefab），形式如下：

GameObject prototype = Resource.Load("prefabName") as GameObject;

可以有任意数量的resources文件夹，怀疑是Resource类初始化的时候会搜集所有resources文件夹里的文件名。

 

方式三：加载到场景

一般我们制作Perfab的时候，都是在Hierarchy视图里创建GameObject，然后再搭建Prefab。

事后根据需要删除这个原始的GameObject。

因此我们可以保留这个GameObject，然后在场景加载后Find这个对象（代码方式），或者使用脚本public字段（编辑器方式）。

 

 

使用方式一更符合unity的风格吧。

我使用方式二，因为我是程序员~~想一切用代码来控制。

方式三比较罗嗦了。
