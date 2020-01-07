---
title: 处理asyncvoid方法的异常问题
date: 2013/8/24 8:18:55
tags:
---


2\. 9 　处理 async void方法的异常问题需要处理从 async void方法传递出来的异常。解决方案没有什么好的办法。如果可能的话，方法的返回类型不要用 void，把它改为 Task。某些情况下这是不可能的，例如，需要对一个 ICommand的实现（必须返回 void）做单元测试。这种情况下，可以为 Execute方法提供一个返回 Task类型的重载，就像这样： 

sealed class MyAsyncCommand : ICommand { async void ICommand. Execute( object parameter) { await Execute( parameter); } public async Task Execute( object parameter) { ... //这里实现异步操作。 } ... //其他成员（ CanExecute等）。 }

最好不要从 async void方法传递出异常。如果必须使用 async void方法，可考虑把所有代码放在 try块中，直接处理异常。处理 async void方法的异常还有一个办法。一个异常从 async void方法中传递出来时，会在 SynchronizationContext中引发出来。当 async void方法启动时， SynchronizationContext就处于激活状态。如果系统运行环境支持 SynchronizationContext，通常就可以在全局范围内处理这些顶层的异常。例如， WPF有 Application. DispatcherUnhandledException， WinRT有 Application. UnhandledException， ASP. NET有 Application_ Error。通过控制 SynchronizationContext，也可以处理从 async void方法传出的异常。自己编写 SynchronizationContext的工作量太大，可以使用免费的 NuGet库 Nito. AsyncEx，里面有 AsyncContext类。 AsyncContext可以在没有自带 SynchronizationContext的场合发挥作用，例如控制台程序、 Win32服务程序。下面的例子是在控制台程序中使用 AsyncContext，其中 async方法不返回 Task，但 AsyncContext仍能在 async void方法中起作用： static class Program { static int Main( string[] args) { try { return AsyncContext. Run(() = > MainAsync( args)); } catch (Exception ex) { Console. Error. WriteLine( ex); return -1; } } static async Task < int > MainAsync( string[] args) { ... } }讨论推荐使用 async Task而不是 async void，原因之一就是返回 Task的方法更容易测试。至少要用 Task方法重载 void方法，那样可以提供便于测试的 API外壳。如果你确实需要自己编写 SynchronizationContext类（例如 AsyncContext），千万不要把这个 SynchronizationContext类放到不属于你的线程上。作为通用的准则，不要在已经有 SynchronizationContext的线程上（比如 UI或 ASP. NET request线程）安装这个类，也不要在线程池线程上放 SynchronizationContext。属于你的线程有控制台程序的主线程，还有你自己创建的所有线程。 ￼ AsyncContext类在 NuGet包 Nito. AsyncEx中。参阅 2. 8节介绍 async Task方法的异常处理。 6. 3节介绍 async void方法的单元测试。
