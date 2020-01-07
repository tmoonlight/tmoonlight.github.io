---
title: unity3d的四元数Quaternion
date: 2015/1/27 2:53:06
tags:
---


**unity3d的四元数 Quaternion**

  


四元数在电脑图形学中用于表示物体的旋转，在unity中由x,y,z,w 表示四个值。

  


四元数是最简单的超复数。 复数是由实数加上元素 i 组成，其中i^2 = -1 ,。 相似地，四元数都是由实数加上三个元素 i、j、k 组成，而且它们有如下的关系： i^2 = j^2 = k^2 = ijk = -1 , 每个四元数都是 1、i、j 和 k 的线性组合，即是四元数一般可表示为a + bi + cj + dk ,。

  


具体的四元数知识可从百度、维基等网站了解。

  


http://baike.baidu.com/view/319754.htm

  


现在只说说在unity3D中如何使用Quaternion来表达物体的旋转。

  


基本的旋转我们可以用脚本内置旋转函数transform.Rotate()来实现。

  


function Rotate (eulerAngles : Vector3, relativeTo : Space = Space.Self) : void

  


但是当我们希望对旋转角度进行一些计算的时候，就要用到四元数Quaternion了。我对高等数学来说就菜鸟一个，只能用最朴素的方法看效果了。

  


Quaternion的变量比较少也没什么可说的，大家一看都明白。唯一要说的就是xyzw的取值范围是[-1,1],物体并不是旋转一周就所有数值回归初始值，而是两周。

  


初始值： (0,0,0,1)

  


沿着y轴旋转：180°(0,1,0,0) 360°（0,0,0,-1）540°(0,-1,0,0) 720°(0,0,0,1)

  


沿着x轴旋转：180°(-1,0,0,0) 360°（0,0,0,-1）540°(1,0,0,0) 720°(0,0,0,1)

  


无旋转的写法是Quaternion.identify

  


现在开始研究Quaternion的函数都有什么用。

  


函数

  


1）function ToAngleAxis (out angle : float, out axis : Vector3) : void

  


Description

  


Converts a rotation to angle-axis representation

  


这个函数的作用就是返回物体的旋转角度（物体的z轴和世界坐标z轴的夹角）和三维旋转轴的向量到变量out angle 和out axis

  


脚本：

  


var a=0.0;

  


var b=Vector3.zero;

  


transform.rotation.ToAngleAxis(a,b);

  


输入：transform.localEularAngles=(0,0,0);

  


输出： a=0, b=(1,0,0);

  


输入：transform.localEularAngles=(0,90,0);

  


输出：a=90, b=(0,1,0);

  


输入：transform.localEularAngles=(270,0,0);

  


输出：a=90, b=(-1,0,0)

  


2）function SetFromToRotation (fromDirection : Vector3, toDirection : Vector3) : void

  


Description

  


Creates a rotation which rotates from fromDirection to toDirection.

  


这个函数的作用是把物体的fromDirection旋转到toDirection

  


脚本：

  


var a:Vector3;

  


var b:Vector3;

  


var q:Quaternion;

  


var headUpDir:Vector3;

  


q.SetFromToRotation(a,b);

  


transform.rotation=q;

  


headUpDir=transform.TransformDirection(Vector3.Forward);

  


输入：a=Vector3(0,0,1); b=Vector3(0,1,0)//把z轴朝向y轴

  


输出： q=(-0.7,0,0,0.7); headUpDir=(0,1,0)

  


输入：a=Vector3(0,0,1); b=Vector3(1,0,0)//把z轴朝向x轴

  


输出： q=(0,0.7,0,0.7); headUpDir=(1,0,0)

  


输入：a=Vector3(0,1,0); b=Vector3(1,0,0)//把y轴朝向x轴

  


输出： q=(0,0,-0.7,0.7); headUpDir=(0,0,1)

  


3）function SetLookRotation (view : Vector3, up : Vector3 = Vector3.up) : void

  


Description

  


Creates a rotation that looks along forward with the the head upwards along upwards

  


Logs an error if the forward direction is zero.

  


这个函数建立一个旋转使z轴朝向view y轴朝向up。这个功能让我想起了Maya里的一种摄像机lol，大家自己玩好了，很有趣。

  


脚本：

  


var obj1: Transform;

  


var obj2: Transform;

  


var q:Quaternion;

  


q.SetLookRotation(obj1.position, obj2.position);

  


transform.rotation=q;

  


然后大家拖动obj1和obj2就可以看到物体永远保持z轴朝向obj1， 并且以obj2的位置来保持y轴的倾斜度。

  


傻逗我玩了半天 哈哈^^ 这个功能挺实用的。

  


4）function ToString () : string

  


Description

  


Returns a nicely formatted string of the Quaternion

  


这个一般用不着吧？看不懂的一边查字典去~

  


Class Functions

  


1）四元数乘法 *

  


建议非特别了解的人群就不要用了。

  


作用很简单，c=a*b (c,a,b∈Quaternion)可以理解为 ∠c=∠a+∠b

  


但是a*b 和b*a效果不一样的。

  


2) == 和 !=

  


不解释了

  


3）static function Dot (a : Quaternion, b : Quaternion) : float

  


Description

  


The dot product between two rotations

  


点积，返回一个float. 感觉用处不大。Vector3.Angle()比较常用。

  


4）static function AngleAxis (angle : float, axis : Vector3) : Quaternion

  


Description

  


Creates a rotation which rotates angle degrees around axis.

  


物体沿指定轴向axis旋转角度angle, 很实用的一个函数也是。

  


脚本：

  


var obj1: Transform;

  


var obj2: Transform;

  


var q:Quaternion;

  


//物体沿obj2的z轴旋转，角度等于obj1的z轴。

  


q=Quaternion.AngleAxis(obj1.localEularAngle.z, obj2.TransformDirection(Vector3.forward));

  


transform.rotation=q;

  


5）static function FromToRotation (fromDirection : Vector3, toDirection : Vector3) : Quaternion

  


Description

  


Creates a rotation which rotates from fromDirection to toDirection.

  


Usually you use this to rotate a transform so that one of its axes eg. the y-axis – follows a target direction toDirection in world space.

  


跟SetFromToRotation差不多，区别是可以返回一个Quaternion。通常用来让transform的一个轴向(例如 y轴)与toDirection在世界坐标中同步。

  


6）static function LookRotation (forward : Vector3, upwards : Vector3 = Vector3.up) : Quaternion

  


Description

  


Creates a rotation that looks along forward with the the head upwards along upwards

  


Logs an error if the forward direction is zero.

  


跟SetLootRotation差不多，区别是可以返回一个Quaternion。

  


7）static function Slerp (from : Quaternion, to : Quaternion, t : float) : Quaternion

  


Description

  


Spherically interpolates from towards to by t.

  


从from 转换到to，移动距离为t。 也是很常用的一个函数，用法比较多，个人感觉比较难控制。当两个quaternion接近时，转换的速度会比较慢。

  


脚本：

  


var obj1: Transform;

  


var t=0.1;

  


var q:Quaternion;

  


//让物体旋转到与obj1相同的方向

  


q=Quaternion.Slerp(transform.rotation, obj1.rotation,t);

  


transform.rotation=q;

  


根据我个人推测，可能t 代表的是from 和to 之间距离的比例。 为此我做了实验并证明了这一点即：

  


q=Quaternion.Slerp(a,b,t);

  


q,a,b∈Quaternion

  


t[0,1]

  


q=a+(b-a)*t

  


并且t最大有效范围为0~1

  


脚本：

  


var obj1: Transform;

  


var obj2：Transform；

  


var t=0.1;

  


var q:Quaternion;

  


//让物体obj1和obj2 朝向不同的方向，然后改变t

  


q=Quaternion.Slerp(obj1.rotation, obj2.rotation,t);

  


transform.rotation=q;

  


t+=Input.GetAxis(“horizontal”)*0.1*Time.deltaTime；

  


7）static function Lerp (a : Quaternion, b : Quaternion, t : float) : Quaternion

  


Description

  


Interpolates from towards to by t and normalizes the result afterwards.

  


This is faster than Slerp but looks worse if the rotations are far apart

  


跟Slerp相似，且比Slerp快，.但是如果旋转角度相距很远则会看起来很差。

  


8）static function Inverse (rotation : Quaternion) : Quaternion

  


Description

  


Returns the Inverse of rotation.

  


返回与rotation相反的方向

  


9）static function Angle (a : Quaternion, b : Quaternion) : float

  


Description

  


Returns the angle in degrees between two rotations a and b.

  


计算两个旋转之间的夹角。跟Vector3.Angle() 作用一样。

  


10）static function Euler (x : float, y : float, z : float) : Quaternion

  


Description

  


Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).

  


把旋转角度变成对应的Quaternion

  


以上就是Quaternion的所有函数了。

  


关于应用，就说一个，其他的有需要再补充。

  


Slerp 函数是非常常用的一个函数，用来产生旋转。

  


static function Slerp (from : Quaternion, to : Quaternion, t : float) : Quaternion

  


对于新手来说，最难的莫过于如何用它产生一个匀速的旋转。如果想用它产生匀速转动，最简单的办法就是把form和to固定，然后匀速增加t

  


脚本：

  


var obj1: Transform;

  


var obj2：Transform；

  


var speed:float;

  


var t=0.1;

  


var q:Quaternion;

  


q=Quaternion.Slerp(obj1.rotation, obj2.rotation,t);

  


transform.rotation=q;

  


t+=Time.deltaTime;

  


但是这并不能解决所有情况。 很多时候from 和to都不是固定的，而且上一个脚本也不能保证所有角度下的旋转速度一致。所以我写了这个脚本来保证可以应付大多数情况。

  


脚本：

  


var target: Transform;

  


var rotateSpeed=30.0;

  


var t=float;

  


var q:Quaternion;

  


var wantedRotation=Quaternion.FromToRotation(transform.position,target.position);

  


t=rotateSpeed/Quaternion.Angle(transform.rotation,wantedRotation)*Time.deltaTime;

  


q=Quaternion.Slerp(transform.rotation, target.rotation,t);

  


transform.rotation=q;

  


这个脚本可以保证物体的旋转速度永远是rotateSpeed。

  


第七行用旋转速度除以两者之间的夹角得到一个比例。

  


如果自身坐标和目标之间的夹角是X度，我们想以s=30度每秒的速度旋转到目标的方向,则每秒旋转的角度的比例为s/X。 再乘以每次旋转的时间Time.deltaTime我们就得到了用来匀速旋转的t值
