---
title: Unity3D如何有效地组织代码？
date: 2015/3/20 9:24:46
tags:
---


**Unity3D如何有效地组织代码？**

Unity3D可以说是高度的Component-Based Architecture，

同时它的库提供了大量的全局变量。

  


这些都和我曾接触到的cocos2d-x，和非游戏框架有很大出入，

请问各位前辈有没有什么好的方法、模式、框架来组织代码呢？

谢谢！

 **  
**

 **[梁伟国](http://www.zhihu.com/people/waigo)** **，** **北京一零八科技。iOS, Unity3D, 108km原…**

  


准确地说，代码作为Unity项目里的一种资源，此问题应该扩展到如何组织Unity资源。简单说说我们的经验：

\- Unity有一些自身的约定，譬如项目里的Editor,Plugins等目录作为编辑器，插件目录等等。知名的插件会自己存放一个目录，譬如NGUI等。

所以我们自己的代码，一般目录名会以下划线开头，譬如 "_Scripts", "_Prefabs"等。

对于场景，文档等目录，用两条下划线，以便他们能排在最顶部。

\- 代码用C#，别用JS。必要的话用namespace将自己的代码括起来。我们是用namespace把自己积攒的公用库包住。

\- C#的注释要认真写，打///就能帮你补全了，没理由偷懒。

\- 每个程序文件开头要用一段注释写修改Log，谁改过什么简单留一条说明。就算用了Unity的版本管理或者Git，那些log终究会丢失，只有认真把log写在代码里，才会有意识去认真优化它。

\- Unity的脚本逻辑，就功能而言大体分为两种，一种是比较独立的，譬如爆炸之后1秒钟消失，这种单独写个脚本绑定到目标上即可。

更多的是脚本里与其它的脚本进行交互。Unity里提供了一种万金油的方法是SendMessage, 这种方法性能略差，如果你调用的频率不高，随便用也无妨。另一种方法是直接通过对象的实例去调用。

  


我们的做法是写几个公用的控制器，让它们各司其职，负责各自的事情：

\- 写一个一个GlobalManager.cs来控制游戏的全局变量及全局方法。静态类模式。譬如当前玩到第几大关第几小关，玩家的金币数量等。

\- 写一个GameController.cs来控制当前关的游戏进程。单实例模式。游戏的主循环也是用它控制。初始化，胜利、失败判定等等。

\- 写一个InputController.cs来控制所有的用户输入。单实例模式。鼠标、键盘、触摸屏，我们做游戏是保证同时支持这三种输入的，因为大部分时间是在PC上测试。

关于GameController与InputController的联系，有点让人纠结。一般来讲是在InputContoller里调用GameController.Instance.Foo()执行方法。或者直接对Input写成Listener的模式，让GameController去监听。

\- 其它的类似菜单控制器，声音控制器，成就控制器，IAP虚拟道具控制器等等，也是采用类似的方法管理。

\- 关于PlayerPref的操作，统一写成静态类的get/set模式，程序中哪里要用则直接读写。

\- 如果你的项目里场景的数量少(<5)，那么拖入场景的资源可以很随意。如果场景数量很多（几十个，有的解谜游戏每个关卡就是一个场景），那么拖入场景的prefab数量一定要少。

\- 设计你的prefab资源里，你要想像当其他人拿到这些资源，是否直接拖入一个空场景里就能run，顶多再简单设置几下。如果你设计的资源不能做到这些，那么得好好重新想想。

  


写了这些，感觉写不下去了。

想吃透Unity，起码得真做出几款产品放上线才行。真正做产品的过程中会碰到各种各样意想不到的问题，代码不断地被重构和妥协，不存在什么最佳的方案。

暂时就写这些吧，希望能抛砖引玉。

  


[2013-05-17](http://www.zhihu.com/question/21070379/answer/17078045) [11 条评论](http://www.zhihu.com/question/21070379#)

  


 **  
**

 **[feicheng wu](http://www.zhihu.com/people/feicheng-wu)**

[伟伟](http://www.zhihu.com/people/wei-wei-67-50)、[刘展祺](http://www.zhihu.com/people/liu-zhan-qi)、[蒋中博](http://www.zhihu.com/people/jiangzhongbo) 等人赞同

我的代码大概遵循这么几条原则吧（想到哪儿写哪儿不分先后）：

1.逻辑脚本基于场景划分

2.抽离静态配置数据、全局管理数据以及对局临时数据的管理

3.使用单例模式创建不依赖于场景的游戏对象及其上的全局管理器

4.避免使用GameObject.Find以及SendMessage，声明对象引用以显示标明脚本之间的依赖性，活用delegate解耦合

5.多用组合少用继承（Component的架构真的是太棒了）

6.数据行为与逻辑表现分离，即V与MC的分离，换句话说多写class少写MonoBehavior。（通常初期在快速开发原型时会把一个功能全部实现写在一个继承于MonoBehavior的脚本中，尽早进行重构，抽离出负责数据管理与控制的类，这对于后期功能的增加与修改时很有必要的）

7.善用Coroutine(Coroutine真是太方便了）

8.尽量能够使用自定义的配置文件辅助Prefab上脚本参数的配置。

  


总的来说记得知乎上看到谁说过cocos2d是程序员友好的，而Unity3D是设计师友好的，写了这么多年Unity3D代码我真是觉得我的思考方式越来越像策划而不是程序员了，使用Unity3D开发，写代码应该只占了大概50%的工作，另外50%都在编辑器上，如果你用过相信你懂得。

  


[2014-04-07](http://www.zhihu.com/question/21070379/answer/24182335) [4 条评论](http://www.zhihu.com/question/21070379#)

  


 **  
**

 **[周华](http://www.zhihu.com/people/zhou-hua-78)** **，** **我的Unity QQ群号：291633884**

[Graphic](http://www.zhihu.com/people/graphic)、[赵振国](http://www.zhihu.com/people/zhao-zhen-guo-27)、[伍一峰](http://www.zhihu.com/people/iSamurai) 赞同

Unity除了挂在Game Object上的代码比较难找，这一点跟其他工具做项目不同外，其他就是每个公司自己的管理风格了。在实际操作中，无非就是文件夹/文件夹/.../文件，名字要起的好。这样就在源代码级别管理好了自己的代码。

  


至于楼主所提的Component-based architecture，个人在管理过程中会习惯去适应unity的脚本概念，虽然与C++等代码做的项目一样，一样有源代码级的很多manager，但是这些manager的启动不再需要自己做一个类似main一样的入口手动启动，只要挂在某个Game Object上，在Start()中启动就可以。配合Unity的script execution order，慢慢脚本的概念就会扎入你的心里。用Unity，因为逃不开在Game Object上挂脚本，所以，个人觉得还是习惯这种方式，的确在概念上也很简单。唯一要注意的是，一定要对Game Object的名字有一定的约束，养成很好的命名习惯。然后自己再写一些查找工具用来查找用到某个名字的脚本的Game Object，配合起来就会理解Unity这样工具的设计理念：基于脚本的对象化。

  


举个例子，单机版的一些存储，如果配合Unity的playerprefs和Unity的回调机制（OnApplicationPause()，OnApplicationQuit())，你会发现你不用为save/load去设计存储格式，去设计一个基于虚函数的OnSave()、OnLoad()，更不用需要去考虑各个模块的存储先后问题。Unity都给你做好了。

  


我不知道有哪些模式，只知道也曾经用C++等熟悉的语言来组织自己的逻辑（因为当初刚开始untiy），但是当我做项目多了，发现其实unity的方式挺简单，也很好用。如果所谓的脚本能做好一件事情，你会发现小即是美。

  


[2013-09-09](http://www.zhihu.com/question/21070379/answer/18799464) [1 条评论](http://www.zhihu.com/question/21070379#)

  


 **  
**

 **[承天一](http://www.zhihu.com/people/cty41)**

[千鬼神](http://www.zhihu.com/people/qian-gui-shen)、[伍一峰](http://www.zhihu.com/people/iSamurai) 赞同

我的建议是：不要在开头去想如何组织代码，而是先让代码跑起来。

  


不过既然lz问了，还是大概说下吧。。。

与unity相比，cocos2d顶多只能算是一个跨平台图形库，他基本没有游戏引擎相关的代码，而unity里component-based design就是为了能更灵活地去处理游戏内的逻辑。[梁伟国](http://www.zhihu.com/people/waigo) 说的controller，inputManager的概念(其实这些在游戏编程里很普遍)都狠详细，但这些东西在你没有实际详尽的认识之前，其实并不好理解。我建议lz去大致系统的看一下unity方面的书籍，然后实际照着去写，把设计先放在后头，等做出形了再重构。

无止境的重构是做不出东西的。

  


[2013-05-23](http://www.zhihu.com/question/21070379/answer/17173363) [4 条评论](http://www.zhihu.com/question/21070379#)

  


 **  
**

 **[杨利华](http://www.zhihu.com/people/www.6djoy.com)** **，** **Unity3D老兵 东方乐动创始人 6年U3D体会**

[千鬼神](http://www.zhihu.com/people/qian-gui-shen) 赞同

能谈到代码布局可能就不是初级水平了，可以去看看一些热门的框架 比如MVCVM 有点学习成本 但很灵活 框架没有万能的 每个游戏结构可能不一样 一般都是根据项目自行设计 以易用易维护为准
