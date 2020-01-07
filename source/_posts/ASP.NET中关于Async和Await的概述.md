---
title: ASP.NET中关于Async和Await的概述
date: 2017/2/3 4:18:39
tags:
---


# [ASP.NET](http://asp.net/) 中关于 Async 和 Await 的概述

  


原文地址 : [【A Simple Explanation of Async and Await in ASP.NET】](https://www.exceptionnotfound.net/using-async-and-await-in-asp-net-what-do-these-keywords-mean/)

同步和异步.png

有时，我在日常工作中的技术分享迫使我去学习一些新的东西，有些甚至是以前从未接触过的。上周就出现了这种情况，我的同事投票赞成下一次的技术分享主题为.NET 中的 Async 和 Await 异步编程。这两个关键字使得异步编程在 .NET 4.5 之后变得更简单了。

老实说，直到上一周，对于异步编程我知之甚少。但在我做了一些研究并创建了我自己的简单项目之后，我开始理解了为什么 Stephen Cleary 会说 ：“异步编程将从根本上改变大多数代码的编写方式”。

本着不关心我的结巴，只要我在学习（[参见](https://www.exceptionnotfound.net/i-dont-care-if-i-suck-as-long-as-im-learning/)）,我决定直接学习 async , await 和 异步编程。 但是我遇到一个问题：我发现很少有资源能够用简单的术语向我解释异步编程的概念。

我试图用这篇文章来弥补这个空缺。

我并不打算去详细介绍 .NET 中关于异步编程的技术实现细节，相反，我会专注于将这些不透明的概念分解成简单的概念，使得它更好的被理解。这帮助我更好的理解当我使用这些关键字时我正在做什么，同时，希望它也能帮助到你。开始吧！

## 什么是异步编程？

异步编程是在编写代码，这些代码允许在没有“阻塞”的情况下同时发生几件事情，或者等待其它的事情完成。它不同于同步编程，在同步编程中，所有的事情都按照它所写的顺序发生（如果你为一个活人编写代码，那么可能是同步代码）。

让我们来看一个 C# 中同步编程的方法：

public string GetNameAndContent()

{

var name = GetLongRunningName(); //调用另一个webservice，需要 1 分钟。

var content = GetContent(); //需要 30 秒

return name + ": " \+ content;

}

每一次调用这个方法，调用者必须等待一分钟才能恢复处理。这一分钟的时间被浪费了，它可以用来做其他的事情。

而 .NET 的异步编程，我们可以像这样改变这个方法：

public async Task<string> GetNameAndContent()

{

var nameTask = GetLongRunningName(); //这个方法是异步的

var content = GetContent(); //这个方法是同步的

var name = await nameTask;

return name + ": " \+ content;

}

我们改变了这个方法的三个地方：

  * 1.我们把方法改为了异步 async。它会告诉编译器这个方法可以异步执行。
  * 2.我们使用 await 关键字修饰 nameTask 变量，它告诉编译器我们最终需要GetLongRunningName()方法的结果，但是我们不需要阻塞这个调用。
  * 3.我们将方法的返回类型改为Task<string>。它通知调用者返回的最终类型为字符串，但不是立即获得，我们不可以做任何其他的事情直到GetLongRunningName()方法调用结束。



但是即使是这样，它任然很模糊。当 “等待” GetLongRunningName()完成时，我们实际在做什么？

这很难用简单的术语来表达，但我还是会尝试的。本质上，系统希望执行GetLongRunningName()，因为它首先被调用，但它是一个异步任务，所以我们需要等待它，控制器被抛出去执行GetContent()，这意味着我们现在有两种方法在同时运行。这不是转移到另一个线程，使用 async和 await不会导致线程的创建。

（如果您想要更深入地解释在异步调用过程中发生了什么，[参见MSDN](https://msdn.microsoft.com/en-us/library/hh191443.aspx#BKMK_WhatHappensUnderstandinganAsyncMethod)。这是我能找到的最好的例子，有一个图。）

## 等待多个调用

让我来看另一个简单是例子。有一个叫 ContentManagement 的类，它既包含了同步方法也包含了异步方法：

public class ContentManagement

{

public string GetContent()

{

Thread.Sleep(2000);

return "content";

}

  


public int GetCount()

{

Thread.Sleep(5000);

return 4;

}

  


public string GetName()

{

Thread.Sleep(3000);

return "Matthew";

}

public async Task<string> GetContentAsync()

{

await Task.Delay(2000);

return "content";

}

  


public async Task<int> GetCountAsync()

{

await Task.Delay(5000);

return 4;

}

  


public async Task<string> GetNameAsync()

{

await Task.Delay(3000);

return "Matthew";

}

}

ContentManagement 是一个简单的类，它模拟了一些需要长时间运行的执行方法。注意，其中有三个方法被标注了 async，并且(通过约定)将Async这个关键字添加到方法名中。我们将在下面的潜在问题部分中解释为什么我们需要同步和异步方法。

现在，然给我们写一个如下的 MVC 控制器：

public class HomeController : Controller

{

[HttpGet]

public ActionResult Index()

{

Stopwatch watch = new Stopwatch();

watch.Start();

ContentManagement service = new ContentManagement();

var content = service.GetContent();

var count = service.GetCount();

var name = service.GetName();

  


watch.Stop();

ViewBag.WatchMilliseconds = watch.ElapsedMilliseconds;

return View();

}

  


[HttpGet]

public async Task<ActionResult> IndexAsync()

{

Stopwatch watch = new Stopwatch();

watch.Start();

ContentManagement service = new ContentManagement();

var contentTask = service.GetContentAsync();

var countTask = service.GetCountAsync();

var nameTask = service.GetNameAsync();

  


var content = await contentTask;

var count = await countTask;

var name = await nameTask;

watch.Stop();

ViewBag.WatchMilliseconds = watch.ElapsedMilliseconds;

return View("Index");

}

}

(顺便说一下，Stopwatch类在尝试记录执行时间时非常有用。)

注意这两个动作的作用。在这两种情况下，操作都调用 ContentManagement 服务中的方法，但在其中一个操作中，它们是异步执行的。

在我们的 Index 视图中，我们有一个显示 WatchMilliseconds 值的输出。让我们来看看Index中结果是什么:

直观地说,这是有道理的。我们称为三种方法;他们分别花了 2 秒，5 秒和 3 秒来执行;所以总执行时间应该是 2 + 5 + 3 = 10 秒。

现在看看如果我们调用 IndexAsync 操作会发生什么:

我们从哪里能得到这个数字？ 这三个任务中的最长任务需要的时间为5秒。 通过设计这个来使用async，我们能减少总花费时长的一半！通过编写一些额外的代码就可以获得很好的加速效果!

## 返回类型们

如你所见，我们为什么要编写异步编程代码，现在看来至少对我来说是有意义的。但是，到底我们在 IndexAsync 操作中使用的Task<ActionResult>是什么东西？

含有async关键字的方法中有三种返回类型可以选择：

  * [Task](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task\(v=vs.110\).aspx)：这个类表示一个异步操作，并且可以被等待；
  * [Task<T>](https://msdn.microsoft.com/en-us/library/dd321424\(v=vs.110\).aspx)：这个类表示一个有返回值的异步操作，并且可以被等待；
  * void：如果一个异步方法返回void，它不能被等待。这实际上把方法变成了“fire and forget(阅后即焚)”方法，这样的情况很少出现。进一步，返回void的异步方法的错误处理有点不同，比如[shown by Stephen Cleary](https://msdn.microsoft.com/en-us/magazine/jj991977.aspx)。没有理由使用void作为异步调用的返回类型，除非完全不关心调用是否实际完成。



简而言之，几乎所有的异步方法都会使用 Task 或 Task<T> 作为它们的返回类型。Task 类表示异步操作本身，而不是action的结果。在一个 Task 中调用 await 意味着我们需要等待这个 Task 执行完成，而在 Task<T> 的情况下，需要检索任务返回的值。

## 潜在问题

让我们注意一些事情：大多数应用程序在实现异步编程时可能不会看到这样的戏剧性的改进(比如50%加速!)，同样，我们不会对单个的方法进行压力测试，只是同时执行它们。事实上，如果我们不正确地设计我们的异步方法，实际上可能会损害整体性能。

当我们将一个方法标记为 async 时，编译器在后台生成一个状态机；这是额外的代码。如果我们编写好的、稳定的异步代码，创建这种额外结构所需的时间不会对我们造成任何影响，因为运行异步的好处超过了构建状态机的成本。然而，如果我们的 async 方法没有 await，方法将会被同步运行，我们将花费额外的时间来创建我们没有使用的状态机。

还有一个潜在的问题需要注意。我们不能从同步方法调用异步方法。因为在所有情况下， async 和 await应该是一起的，我们需要在所有的方法上都有异步。这就是为什么我们在前面的 ContentManagement 类中需要单独的 async 方法。最终，这导致了更多的代码，这意味着更多的东西理论上可以中断。然而，由于对我们想要完成的事情有了良好的设计和坚实的理解，拥有额外的代码也会带来更大的性能，所以在我看来，这是一个公平的交易。

## 摘要

在实现异步的过程中付出一点额外的努力，对于提高我们的应用程序的性能和响应能力有很长的一段路要走。.NET使 async 和await关键字变得容易，我们可以简单、简洁地、快速地实现异步设计。

我在这个练习之前完全不懂异步编程；现在我至少可以说我不是一无所知。希望你也一样！

如果你想学更多的内容，可以看一下 Alex Davies 写的 [《Async in C# 5.0》](http://www.amazon.com/gp/product/1449337163/ref=as_li_tl?ie=UTF8&camp=1789&creative=390957&creativeASIN=1449337163&linkCode=as2&tag=excnotfou-20&linkId=62WBH65BFYIRJIOM) ,我在试图理解MVC web应用程序中异步/等待的情况时，我读过，它帮了我很多。

我使用 Visual Studio 2015 和 [ASP.NET](http://asp.net/) MVC(包括我们之前使用的ContentManagement类和HomeController) 创建了一个简单项目。我把它放到了[Github](https://github.com/exceptionnotfound/AsyncAwaitDemo)，你可以check out下来进行练习。

  

