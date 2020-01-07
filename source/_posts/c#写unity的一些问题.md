---
title: c#写unity的一些问题
date: 2015/2/17 12:57:01
tags:
---


1. 继承之MonoBehaviour类

所有的行为脚本代码必须继承之MonoBehaviour类（直接或间接）。如果使用的是javascript的话会自动（隐性）的继承，如果使用的是 C#或Boo就必须明确地指定其继承于MonoBehaviour。如果你是在u3d中通过“Asset->Create->C Sharp Script/Boo Script”来创建了脚本代码文件的话，u3d的脚本创建模板将会提前将相关继承语句定义在脚本代码文件中。

public class NewBehaviourScript : MonoBehaviour {...} // C#

class NewBehaviourScript (MonoBehaviour): ... # Boo

 

 

2. 使用Awake或Start方法进行初始化。

你需要在C#或Boo在使用Awake或Start方法。

Awake和Start之间的区别在于：Awake是当一个场景调入过程完成后会自动运行，而Start则是会在Update或FixedUpdate方法被第一次调用之前被运行。所有的Awake方法运行的优先级会高于任意的Start方法。

 

3.（文件中）主类名必须与文件名相同。

在javascript脚本文件中，u3d虽然没有明确地定义主类，但事实上，u3d已经隐性地自动定义了主类，并将类名设置为等于脚本文件名（不包括扩展名）。

如果使用的是C#a或Boo脚本，那就必须得手动的将主类名设置为与文件同名。

 

4.使用C#实现协同，在语法上会有一处不同。

(U3D中的)协同会（同时）用一个属于IEnumerator接口类型（枚举）的返回值和你使用的yield 返回值...;来替代yield......;

如下面代码：

using System.Collections;

using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

// C# coroutine

IEnumerator SomeCoroutine () {

// Wait for one frame

yield return 0;

// Wait for two seconds

yield return new WaitForSeconds (2);

}

}

 

5. 不要使用命名空间。

U3D目前不支持你在脚本中使用命名空间，这个需求会在未来的版本中实现。

 

6. 只有（public公有的）成员变量是可以在U3D程序的Inspector栏中会被以序列形式显示出来

私有类型（private）和成员类型（protected）变量只能在专家模式（Expert Mode）下可见，（而且）属性（Properties）

 

7. 避免使用构造函数

不要通过构造函数来初始化变量。这些工作可以使用第2条中的Awake方法和Start方法来替代（换句话来说就是在u3d中，Awake方法和 Start方法是每个脚本文件类中默认的构造函数）。U3D甚至可以在标准编辑模式下就调用它们。它们通常是直接汇编在脚本中,因为构造函数需要检索默认脚本变量用于引用。（u3d）在任意的时候不光可以调用构造函数，还可能会调用预设（物体）或未被唤醒的游戏物体。

实例化（C#脚本文件）时，单脚本文件状态下使用（自定义的）构造函数（可能）会导致严重的后果，并且会产生引用为空的异常。

所以，如果你实例化C#脚本文件（即运行C#脚本文件。这是c＃程序运行的基本方式，详细内容可以从C#专门的教材中了解），单脚文件不要使用（自定义的）的构造函数，直接使用Awake方法替代即可，实在没有理由为一个继承之MonoBehaviour的（文件）类写任何的（构造函数）代码。
