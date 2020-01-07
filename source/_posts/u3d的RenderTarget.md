---
title: u3d的RenderTarget
date: 2014/12/18 15:31:59
tags:
---


u3d的rendertarget实现很简单，一句话：assert区域新建rendertarget的texture，将此texture贴进某个材质，再将此材质应用到某个物体上。这个和xna在graphic里修改rendertarget进行draw是一样的。 
