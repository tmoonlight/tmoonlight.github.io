---
title: c#中单例模式和双重检查锁
date: 2019/9/14 16:18:09
tags:
---


# c#中单例模式和双重检查锁

2018年08月10日 15:51:38 [zhongliangtang](https://me.csdn.net/zhongliangtang) 阅读数：310

版权声明：本文为博主原创文章，未经博主允许不得转载。 <https://blog.csdn.net/zhongliangtang/article/details/81564749>

单例模式是软件工程中最着名的模式之一。从本质上讲，单例是一个只允许创建自身的单个实例的类，并且通常可以简单地访问该实例。所有这些实现共享四个共同特征：

单个构造函数，它是私有且无参数的。这可以防止其他类实例化它（这将违反模式）。请注意，它还可以防止子类化 - 如果单例可以被子类化一次，它可以被子类化两次，并且如果每个子类都可以创建一个实例，则违反该模式。如果您需要基本类型的单个实例，则可以使用工厂模式，但直到运行时才能知道确切类型。

### 版本1，线程不安全

// Bad code! Do not use!public sealed class Singleton

{

private static Singleton instance=null;

  


private Singleton()

{

}

  


public static Singleton Instance

{

get

{

if (instance==null)

{

instance = new Singleton();

}

return instance;

}

}

}

1

2

3

4

5

6

7

8

9

10

11

12

13

14

15

16

17

18

19

20

21

这种方式称为延迟初始化，但是在多线程的情况下会失效，于是使用同步锁，给Instance 方法加锁：

### 版本2：线程安全

public sealed class Singleton

{

private static Singleton instance = null;

private static readonly object padlock = new object();

  


Singleton()

{

}

  


public static Singleton Instance

{

get

{

lock (padlock)

{

if (instance == null)

{

instance = new Singleton();

}

return instance;

}

}

}

}

1

2

3

4

5

6

7

8

9

10

11

12

13

14

15

16

17

18

19

20

21

22

23

24

可以满足并行化的需求，但是同步是需要开销的，我们只需要在初始化的时候同步，而正常的代码执行路径不需要同步

### 版本3，使用双重锁检查实现线程安全

// Bad code! Do not use!public sealed class Singleton

{

private static Singleton instance = null;

private static readonly object padlock = new object();

  


Singleton()

{

}

  


public static Singleton Instance

{

get

{

if (instance == null)

{

lock (padlock)

{

if (instance == null)

{

instance = new Singleton();

}

}

}

return instance;

}

}

}

1

2

3

4

5

6

7

8

9

10

11

12

13

14

15

16

17

18

19

20

21

22

23

24

25

26

27

28

这样一种设计可以保证只产生一个实例，并且只会在初始化的时候加同步锁，看似看似没有问题，但却会引发另一个问题，这个问题由指令重排序引起。 

指令重排序是为了优化指令，提高程序运行效率。指令重排序包括编译器重排序和运行时重排序。JVM规范规定，指令重排序可以在不影响单线程程序执行结果前提下进行。例如 instance = new Singleton() 可分解为如下伪代码：

排序前

memory = allocate(); //1：分配对象的内存空间

ctorInstance(memory); //2：初始化对象instance = memory; //3：设置instance指向刚分配的内存地址

1

2

3

排序后

memory = allocate(); //1：分配对象的内存空间instance = memory; //3：设置instance指向刚分配的内存地址

//注意，此时对象还没有被初始化！

ctorInstance(memory); //2：初始化对象

1

2

3

4

由于并行执行，则线程A执行了instance = memory然后线程B检查到instance不为null，则会使用未经实例化完全的对象进行操作，应发错误

###### 双重检查优化1

// Bad code! Do not use!

  


public sealed class Singleton

{

private volatile static Singleton instance = null;//volatile 可以禁止

private static readonly object padlock = new object();

  


Singleton()

{

}

  


public static Singleton Instance

{

get

{

if (instance == null)

{

lock (padlock)

{

if (instance == null)

{

instance = new Singleton();

}

}

}

return instance;

}

}

}

1

2

3

4

5

6

7

8

9

10

11

12

13

14

15

16

17

18

19

20

21

22

23

24

25

26

27

28

29

30

volatile表明属性将被多个线程同时访问，告知编译器不要按照单线程访问的方式去优化该字段，线程会监听字段变更，但是不保证字段访问总是顺序执行

###### 双重检查优化2，线程安全，避免实例创建和引用赋值混乱

public sealed class Singleton

{

private static Singleton instance = null;//volatile 可以禁止

private static readonly object padlock = new object();

  


Singleton()

{

}

  


public static Singleton Instance

{

get

{

if (instance == null)

{

lock (padlock)

{

if (instance == null)

{

var tmp= new Singleton();

// ensures that the instance is well initialized,

// and only then, it assigns the static variable.

System.Threading.Thread.MemoryBarrier();

instance = tmp;

}

}

}

return instance;

}

}

}

  


官方文档是说Thread.MemoryBarrier()，保证之前的数据存取优先于MemoryBarrier执行，只有在多CPU下才需要使用

### 版本4，不用锁实现线程安全，不全是延时初始化

public sealed class Singleton

{

private static readonly Singleton instance = new Singleton();

  


// Explicit static constructor to tell C# compiler

// not to mark type as beforefieldinit

static Singleton()

{

}

  


private Singleton()

{

}

  


public static Singleton Instance

{

get

{

return instance;

}

}

}

  


C＃中的静态构造函数被指定仅在创建类的实例或引用静态成员时执行，并且每个AppDomain只执行一次，所以是延时初始化，但是当Singleton有多个静态属性，且其他的属性被访问时，实例也会被初始化，所以不是完全的延时初始化方式

### 版本5，线程安全，延时初始化

public sealed class Singleton

{

private Singleton()

{

}

  


public static Singleton Instance { get { return Nested.instance; } }

  


private class Nested

{

// Explicit static constructor to tell C# compiler

// not to mark type as beforefieldinit

static Nested()

{

}

  


internal static readonly Singleton instance = new Singleton();

}

}

  


只有当Nested类的第一个引用调用时，Singleton才会被实例化，也就是调用Instance，是完全的延时初始化

### 版本6，线程安全， .NET 4之后支持

public sealed class Singleton

{

private static readonly Lazy<Singleton> lazy =

new Lazy<Singleton>(() => new Singleton());

  


public static Singleton Instance { get { return lazy.Value; } }

  


private Singleton()

{

}

}

  


  

