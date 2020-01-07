---
title: log4文件被锁解决办法
date: 2014/3/9 15:04:53
tags:
---


Try to configure log4net with a minimal lock:

<appender name="FileAppender" type="log4net.Appender.FileAppender">

...

<lockingModel type="log4net.Appender.FileAppender+MinimalLock" />

...</appender>

[have a look here for better explanation.](http://logging.apache.org/log4net/release/config-examples.html)

Alternatively, try to open the log file with:

using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite))

{...}

or check this project:[Tailf](https://github.com/FelicePollano/tailf) In any case, remove the SetAttributes() part that could not work. Tailf Project Description Tailf is a C# implementation of the tail -f command available on unix/linux systems. Differently form other ports it does not lock the file in any way so it works even if other rename the file: this is expecially designed to works well with log4net rolling file appender.
