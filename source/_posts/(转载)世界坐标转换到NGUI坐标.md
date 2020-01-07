---
title: (转载)世界坐标转换到NGUI坐标
date: 2015/5/8 23:10:22
tags:
---


世界坐标转换到NGUI坐标:   
场景中有一个照3D物体的透视摄像机,NGUI使用自己独立的正交摄像机,转换步骤如下:      
1\. 使用透视摄像机把世界坐标转换到屏幕坐标

1

2

| 

`Vector3 pos = Camera.main.WorldToScreenPoint(worldPos);`

`pos.z = 0f;  ` `//z一定要为0.`  
  
---|---  
  
2\. 使用UI摄像机转换到NGUI的世界坐标  


1

| 

`Vector3 pos2 = UICamera.currentCamera.ScreenToWorldPoint(pos);`  
  
---|---  
  
3\. 赋值给NGUI控件  


1

2

| 

`//temp为NGUI控件.`

`temp.transform.position = pos2;`  
  
---|---
