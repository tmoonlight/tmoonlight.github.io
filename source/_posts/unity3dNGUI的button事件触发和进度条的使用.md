---
title: unity3dNGUI的button事件触发和进度条的使用
date: 2014/11/26 12:27:49
tags:
---




       你自己先建的NGUI里面的camera里面如果没有audio listener和audio source的话要自己添加，否则button音效播不了的
    
       控制ProgressBar的大小时，用sliderValue,因为inirialValue不行

  



1.在panel中添加一个Button和一个ProgressBar。

  


2.

  1. using UnityEngine;
  2. using System.Collections;
  3.  **public**   **class**  click2 : MonoBehaviour {
  4.  **public**  UISlider progressbar;
  5.  **void**  OnClick(){
  6. progressbar.sliderValue-=0.1f;
  7. }
  8. }



把这个脚本绑定在Button上面就行，再把进度条添加进去就OK了。

  


3.如果你不想把脚本绑定在Button上面的话，你还可以用另一种方法。给Button添加UIButtonMessage脚本。

  





FunctionName:当你的button Onclick时触发的函数，直接写函数名就ok了。

  


Target:你绑定FunctionName的脚本的对像。

  


  1. using UnityEngine;
  2. using System.Collections;
  3.  **public**   **class**  click2 : MonoBehaviour {
  4.  **public**  UISlider progressbar;
  5.  **void**  haha(){
  6. progressbar.sliderValue-=0.1f;
  7. print("heh");
  8. }
  9. }



注：我的这个"haha"函数是绑定在物体"hehe"上的.

  


4.

  





这个进度条的一些参数和上面的差不多，FunctionName是当你的进度条的SliderValue值改变以后触发的函数，这里面默认是OnSliderChange。EventReceiver是绑定FunctionName的对象。

  


注：我这里的OnSliderChange函数就是绑定在进度条上的。

  


[](http://blog.csdn.net/nateyang/article/details/#)

[](http://blog.csdn.net/nateyang/article/details/#)

[](http://blog.csdn.net/nateyang/article/details/#)

[](http://blog.csdn.net/nateyang/article/details/#)

[](http://blog.csdn.net/nateyang/article/details/#)

  

