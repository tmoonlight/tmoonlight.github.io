---
title: winformwindowsserver混合程序
date: 2014/4/23 17:56:19
tags:
---


1.需要两个程序，一个做为监控程序，一个作为服务程序

  


2.入口点

 

  


//windows service

            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[]

            {

                new Service1()

            };

//winform

            ServiceBase.Run(ServicesToRun);

            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());
