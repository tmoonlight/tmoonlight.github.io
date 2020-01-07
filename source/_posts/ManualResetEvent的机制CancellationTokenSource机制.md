---
title: ManualResetEvent的机制CancellationTokenSource机制
date: 2019/12/13 11:19:49
tags:
---


ManualResetEvent的机制很简单，就是利用set和 reset两个方法来手动设置线程的运行和终止，

初始化的构造函数true就是运行 否则就是终止。

reset则终止

set则开始跑

  


  


CancellationTokenSource 也是readstream的一种信号量  


====================

今天详细说一下ManualResetEvent

它可以通知一个或多个正在等待的线程已发生事件，允许线程通过发信号互相通信，来控制线程是否可心访问资源

当一个线程开始一个活动（此活动必须完成后，其他线程才能开始）时，它调用 [Reset](http://msdn.microsoft.com/zh-cn/library/system.threading.eventwaithandle.reset\(v=vs.100\).aspx) 以将 ManualResetEvent 置于非终止状态。此线程可被视为控制 ManualResetEvent。调用 ManualResetEvent 上的[WaitOne](http://msdn.microsoft.com/zh-cn/library/system.threading.waithandle.waitone\(v=vs.100\).aspx) 的线程将阻止，并等待信号。当控制线程完成活动时，它调用 [Set](http://msdn.microsoft.com/zh-cn/library/system.threading.eventwaithandle.set\(v=vs.100\).aspx) 以发出等待线程可以继续进行的信号。并释放所有等待线程。

一旦它被终止，ManualResetEvent 将保持终止状态，直到它被手动重置。即对 WaitOne 的调用将立即返回。

上面是它的功能描述，你可能会有点晕。我会用代码一点一点解释它，看完我写的这些内容，你自己运行一下代码你就会明白它的功能

源代码：[ManualResetEventDemo.rar](http://files.cnblogs.com/li-peng/ManualResetEventDemo.rar)

我们从初始化来开始讲

可以通过将布尔值传递给构造函数来控制 ManualResetEvent 的初始状态，如果初始状态处于终止状态，为 true；否则为 false。

我用代码 让大家看一下什么是终止状态和非终止状态

先看一下代码

  


class Program

   {

       static ManualResetEvent _mre = new ManualResetEvent(false);

       static void Main(string[] args)

       {

           Thread[] _threads = new Thread[3];

           for (int i = 0; i < _threads.Count(); i++)

           {

               _threads[i] = new Thread(ThreadRun);

               _threads[i].Start();

           }

           

       }

 

       static void ThreadRun()

       {

           int _threadID = 0;

           while (true)

           {

               _mre.WaitOne();

               _threadID = Thread.CurrentThread.ManagedThreadId;

               Console.WriteLine("current Tread is " \+ _threadID);

               Thread.Sleep(TimeSpan.FromSeconds(2));

                  

           }

       }

   }  
  
---  
  
  


当初始化为true时，为终止状态

  


static ManualResetEvent _mre = new ManualResetEvent(true);  
  
---  
  
  


执行结果

 ![](https://images0.cnblogs.com/blog/342595/201308/30131628-83f9c1d15f1c4b3a98f78c8295788139.png)

当初始化为false时，为非终止状态

  


static ManualResetEvent _mre = new ManualResetEvent(false);  
  
---  
  
  


执行结果为

![](https://images0.cnblogs.com/blog/342595/201308/30131805-b57b616b79814ae2ac8496feb032d7cc.png)

这样我们就能看出来

终止状态时WaitOne()允许线程访问下边的语句

非终止状态时WaitOne()阻塞线程，不允许线程访问下边的语句

我们也可以把WaitOne()放在方法最下边

  


static void ThreadRun()

        {

            int _threadID = 0;

            while (true)

            {

                 

                _threadID = Thread.CurrentThread.ManagedThreadId;

                Console.WriteLine("current Tread is " \+ _threadID);

                Thread.Sleep(TimeSpan.FromSeconds(2));

                _mre.WaitOne();

            }

        }  
  
---  
  
  


当初始化为true时执行结果和上边的一样会不停的执行

![](https://images0.cnblogs.com/blog/342595/201308/30131628-83f9c1d15f1c4b3a98f78c8295788139.png)

初始化为false时执行到waitOne()时就阻塞线程不会再往下执行了

![](https://images0.cnblogs.com/blog/342595/201308/30132424-bb9f452a11d943f2966445ceb88e4b0a.png)

接下来你可能就会想当在非终止状态时怎么让线程继续执行，怎么再让它停下来，这就要用了set()和Reset()方法了

把非终止状态改为终止状态用Set()方法

把终止状态改为非终止状态用Reset()方法

我用用代码来实现它们只要把我们上 边的代码做一下改动

  


class Program

    {

        static ManualResetEvent _mre = new ManualResetEvent(false);

        static void Main(string[] args)

        {

            Console.WriteLine("输入1为Set()   开始运行");

            Console.WriteLine("输入2为Reset() 暂停运行");

            Thread[] _threads = new Thread[3];

            for (int i = 0; i < _threads.Count(); i++)

            {

                _threads[i] = new Thread(ThreadRun);

                _threads[i].Start();

            }

            while (true)

            {

                switch (Console.ReadLine())

                {

                    case "1":

                        _mre.Set();

                        Console.WriteLine("开始运行");

                        break;

                    case "2":

                        _mre.Reset();

                        Console.WriteLine("暂停运行");

                        break;

                    default:

                        break;

                }

            }

            

        }

 

        static void ThreadRun()

        {

            int _threadID = 0;

            while (true)

            {

                 

                _threadID = Thread.CurrentThread.ManagedThreadId;

                Console.WriteLine("current Tread is " \+ _threadID);

                Thread.Sleep(TimeSpan.FromSeconds(2));

                _mre.WaitOne();

            }

        }

    }  
  
---  
  
  


 

 

当输入1 时会调用 Set()方法 ManualResetEvent 处于终止状态会WaitOne不会阻塞线程会一直运行下去

当输入2时会调用 Reser()方法ManualResetEvent处于非终止状态WaitOne会阻塞线程直到再调用 Set()方法

看一下执行结果吧

  


  

