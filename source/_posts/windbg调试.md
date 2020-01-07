---
title: windbg调试
date: 2019/3/15 10:16:52
tags:
---


#   


故事是這様的，我們有一批網站搬到新主機出現詭異現象：每隔一段時間某些 IIS AppPool Process 會吃滿 25% CPU 使用量，在 4 核機器這象徵有一條 Thread 陷入無窮迴圈吃光一個 CPU Core 的時間。有時也會出現多個 AppPool 同時發難，每個 Process 吃 25%，把整體 CPU 使用率逼上 50％、75％，甚至 100％。出問題時，該 AppPool 網站仍能使用，但無法透過 IIS 管理回收 AppPool，只能用 TaskManager 砍掉 Process。砍掉 Process 後，系統會乖一陣子，但幾個小時或隔天又復發。

原本正常的程式移到新環境不穩定讓人心慌，很想知道吃光 CPU 的 AppPool 究竟在忙什麼？到底是哪支網頁是老鼠屎？我唯一想到的可用武器是強大但陌生的 WinDbg (Windows Debugger），爬文與一番摸索後居然真讓我成功抓出問題所在，興奮到想轉圈灑花。：P

以下就分享本次使用 WinDbg 找出 CPU 100％ 程式來源的經驗：

  1. 使用 TaskManager 產生 Memory Dump 檔

找出吃光 CPU 的 w3wp.exe Process，叫出右鍵選單執行「Create dump file」 產生記憶體傾印檔。

有件事要注意，要先釐清 AppPool 是 32 位元還是 64 位元，若為 32 位元需[改用 32 位元版 TaskManager](https://blogs.msdn.microsoft.com/tess/2010/09/29/capturing-memory-dumps-for-32-bit-processes-on-an-x64-machine/)，否則 Dump 檔解析出來的資訊會類似 wow64cpu!TurboDispatchJumpAddressEnd+0x598，看不到真實執行位置。32 位元版 TaskManager 位於 C:\Windows\SysWOW64\taskmgr.exe，執行後 Process 名稱應為「Task Manager (32bit)」。

若 CPU 是不定期飆高一陣子後恢復，SysInternals 有個超好用工具 [procdump](https://blogs.iis.net/webtopics/using-procdump-exe-to-monitor-w3wp-exe-for-cpu-spikes)，遇到 32 位元 Process 時會自動產生 32 位元 Dump 檔，還可以指定「當 CPU 高於 x% 並持續 y 秒時自動產生 Dump 檔」，可多加利用。

  2. 啟用 WinDbg 載入 Dump 檔案

Windows Debugger 有 32/64 位元之分，請視要分析對象的位元版本選擇對應版本的 WinDbg。

啟動 WinDbg 後，使用 File / Open Crash Dump 開啟剛才取得的 w3wp.DMP。

註：Windows Debugger 下載安裝資訊請自行參考 [MSDN](https://msdn.microsoft.com/en-us/library/windows/hardware/ff551063\(v=vs.85\).aspx)，此處不多贅述。

  3. WinDbg 的指令既多且雜，有興趣的朋友請參考[文件](http://windbg.info/doc/1-common-cmds.html)，此處記錄並說明我動用的指令：

.sympath srv*X:\Symbol*<https://msdl.microsoft.com/download/symbols>

分析過程需要 Symbol 檔，指定 WinDbg 自動由微軟網站下載，並 Cache 在 X:\Symbol 目錄避免重複下載

!sym noisy

指定顯示完整 Symbol 下載資訊

.cordll -ve -u  -l

自動載入 CLR 偵錯相關模組（分析來自其他台機器 Dump 檔時，這招好用）  

  4. 找出佔用 CPU 最多的 Thread，並將偵錯對象切換成該 Thread

!runaway

找出每個 Thread 消耗的 CPU 時間，在本案例中，Thread 27 耗用超過一個小時，無疑是吃光 CPU 的兇手

~27s

將偵錯對象切換成 Thread 27

  5. 使用 !clrstack 列出 Thread 27 的 Callstack




兇手現形！MasterPage 中某個共用元件使用 [ODP.NET](http://odp.net/) 讀取使用者基本資料，程式卡在 Oracle.DataAccess.Client.OpsSql.ExecuteReader()，有可能等待 Oracle 資料庫回應時陷入無窮迴圈並耗盡 CPU。

陸續分析了幾起案例的 Dump 檔，都能找到一條 Thread 明顯耗用大量 CPU，而 Callstack 問題都指向 Oracle.DataAccess.Client.OpsSql.ExecuteReader()，鎖定問題後推測是[網路設備干擾 Oracle 連線](http://blog.darkthread.net/post-2012-04-10-ora-12571-under-nlb.aspx)的問題（先前看過的症狀都是收到明確錯誤，像這樣 Hang 住的狀況是第一次遇到），避開後問題排除。

就這樣，靠著 WinDbg 漂亮偵破一個看似毫無頭緒的疑案。

呼口號時間：

#### WinDbg 好威呀！

  

