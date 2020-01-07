---
title: VSTO项目
date: 2019/10/19 3:20:55
tags:
---


SmartTag ID只能存于内存当中 name可以使用，但因为前端占用，不能使用

  


浮动改造

浮动对象 ole对象

https://stackoverflow.com/questions/27922034/drawing-directly-to-excel-cell-canvas  


  


VSTOexcel常规操作

comobj导致代码调试起来比较麻烦

  


  


单元格

 **Sheet.UsedRange                            表示工作表中已用区域，即用户已经使用过的单元格区域**

  


UBound(arr)                                    表示数组第一维的上界

       UBound(arr, 2)                                表示数组第二维的上界

       [a65536].End(3).Row                      表示A列最后一个非空单元格所在的行号

      Cells(r, "xfd").End(xlToLeft).Column 表示R行最后一列非空单元格所在的列号

  


  


  


  


  


  


  

