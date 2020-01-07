---
title: windbg配置问题汇总
date: 2019/2/26 19:15:16
tags:
---


[windbg配置问题汇总](https://www.cnblogs.com/kissdodog/p/3922228.html)

.loadby sos.dll mscorwks

.symfix c:\windows\symbols

# windbg配置问题汇总

1、Failed to find runtime DLL (clr.dll), 0x80004005

　　必须加载正确的.net sos版本

0:000> !clrstack

Failed to find runtime DLL (clr.dll), 0x80004005

Extension commands need clr.dll in order to have something to do.

0:000> .load C:\Windows\[Microsoft.NET](http://microsoft.net/)\Framework\v2.0.50727\sos.dll

0:000> !clrstack

Failed to load data access DLL, 0x80004005

Verify that 1) you have a recent build of the debugger (6.2.14 or newer)

2) the file mscordacwks.dll that matches your version of mscorwks.dll is 

in the version directory

3) or, if you are debugging a dump file, verify that the file 

mscordacwks_<arch>_<arch>_<version>.dll is on your symbol path.

4) you are debugging on the same architecture as the dump file.

For example, an IA64 dump file must be debugged on an IA64

machine.

You can also run the debugger command .cordll to control the debugger's

load of mscordacwks.dll. .cordll -ve -u -l will do a verbose reload.

If that succeeds, the SOS command should work on retry.

If you are debugging a minidump, you need to make sure that your executable

path is pointing to mscorwks.dll as well.

  


2、Failed to load data access DLL, 0x80004005

mscordacwks.dll的版本不正确，必须加载正确的mscordacwks.dll版本

0:000> !DumpHeap -stat

Failed to load data access DLL, 0x80004005

Verify that 1) you have a recent build of the debugger (6.2.14 or newer)

2) the file mscordacwks.dll that matches your version of mscorwks.dll is 

in the version directory

3) or, if you are debugging a dump file, verify that the file 

mscordacwks_<arch>_<arch>_<version>.dll is on your symbol path.

4) you are debugging on the same architecture as the dump file.

For example, an IA64 dump file must be debugged on an IA64

machine.

You can also run the debugger command .cordll to control the debugger's

load of mscordacwks.dll. .cordll -ve -u -l will do a verbose reload.

If that succeeds, the SOS command should work on retry.

If you are debugging a minidump, you need to make sure that your executable

path is pointing to mscorwks.dll as well.

  


3、mscordacwks.dll版本不匹配

0:000> .cordll -ve -u -l 

CLR DLL status: No load attempts

0:000> .exepath+ C:\Windows\[Microsoft.NET](http://microsoft.net/)\Framework\v2.0.50727

Executable image search path is: C:\Windows\[Microsoft.NET](http://microsoft.net/)\Framework\v2.0.50727

Expanded Executable image search path is: c:\windows\[microsoft.net](http://microsoft.net/)\framework\v2.0.50727

0:000> !DumpHeap -stat

CLRDLL: C:\Windows\[Microsoft.NET](http://microsoft.net/)\Framework\v2.0.50727\mscordacwks.dll:2.0.50727.5420 f:0

doesn't match desired version 2.0.50727.5472 f:0

CLRDLL: Unable to find mscordacwks_x86_x86_2.0.50727.5472.dll by mscorwks search

CLRDLL: Unable to find 'mscordacwks_x86_x86_2.0.50727.5472.dll' on the path

CLRDLL: Unable to get version info for 'c:\windows\symbols\mscorwks.dll\5174DD695ad000\mscordacwks_x86_x86_2.0.50727.5472.dll', Win32 error 0n87

CLRDLL: ERROR: Unable to load DLL mscordacwks_x86_x86_2.0.50727.5472.dll, Win32 error 0n87

Failed to load data access DLL, 0x80004005

Verify that 1) you have a recent build of the debugger (6.2.14 or newer)

2) the file mscordacwks.dll that matches your version of mscorwks.dll is 

in the version directory

3) or, if you are debugging a dump file, verify that the file 

mscordacwks_<arch>_<arch>_<version>.dll is on your symbol path.

4) you are debugging on the same architecture as the dump file.

For example, an IA64 dump file must be debugged on an IA64

machine.

You can also run the debugger command .cordll to control the debugger's

load of mscordacwks.dll. .cordll -ve -u -l will do a verbose reload.

If that succeeds, the SOS command should work on retry.

If you are debugging a minidump, you need to make sure that your executable

path is pointing to mscorwks.dll as well.

  


看到下面的路径，将以下两个文件(长的那个为短的改了名字)复制到指定目录：mscordacwks.dll，mscordacwks_x86_x86_2.0.50727.5472.dlla

  

