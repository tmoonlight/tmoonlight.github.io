---
title: Brainstorming-Creatingasmallsingleself-containedexecutableoutofa.NETCoreapplication-ScottHanselman
date: 2013/9/10 19:25:14
tags:
---


# [Scott Hanselman](https://www.hanselman.com/)

  * [about](https://hanselman.com/about)
  * [blog](https://hanselman.com/blog)
  * [speaking](https://hanselman.com/speaking)
  * [podcasts](https://hanselman.com/podcasts)
  * [books](https://hanselman.com/books)



browse by [category](https://www.hanselman.com/blog/archives.aspx "The complete archive of Scott's Blog") or [date](https://www.hanselman.com/blog/monthview.aspx "Scott's blog posts in a friendly calendar format")

[](https://www.hanselman.com/blog/)

## [Brainstorming - Creating a small single self-contained executable out of a .NET Core application](https://www.hanselman.com/blog/BrainstormingCreatingASmallSingleSelfcontainedExecutableOutOfANETCoreApplication.aspx)

二月 1, '19 [Comments [33]](https://www.hanselman.com/blog/CommentView.aspx?guid=0b5da3d4-99cb-4b30-88af-cc1aee40adcb#commentstart) Posted in [DotNetCore](https://www.hanselman.com/blog/CategoryView.aspx?category=DotNetCore)

Sponsored By  


I've been using ILMerge and various hacks to merge/squish executables together for well over 12 years. The .NET community has long toyed with the idea of a single self-contained EXE that would "just work." No need to copy a folder, no need to install anything. Just a single EXE.

While work and thought continues on a CoreCLR Single File EXE solution, there's a nice [Rust tool called Warp](https://github.com/dgiagio/warp?WT.mc_id=-blog-scottha) that creates self-contained single executables. Warp is cross-platform, works on any tech, and is very clever 

The Warp Packer app has a slightly complex command line, like this:
    
    
    .\warp-packer --arch windows-x64 --input_dir bin/Release/netcoreapp2.1/win10-x64/publish --exec myapp.exe --output myapp.exe

Fortunately [Hubert Rybak](https://github.com/Hubert-Rybak/DotnetPack?WT.mc_id=-blog-scottha) has created a very nice "[dotnet-warp](https://github.com/Hubert-Rybak/dotnet-warp?WT.mc_id=-blog-scottha)" global tool that wraps this all up into a single command, dotnet-warp. 

All you have to do is this:
    
    
    C:\supertestweb> dotnet tool install -g dotnet-warp  
    C:\supertestweb> dotnet-warp  
    O Running Publish...  
    O Running Pack...

In this example, I just took a Razor web app with "dotnet new razor" and then packed it up with this tool using Warp packer. Now I've got a 40 meg self-contained app. I don't need to install anything, it just works.
    
    
    C:\supertestweb> dir  
        Directory: C:\supertestweb  
      
    Mode                LastWriteTime         Length Name  
    ----                -------------         ------ ----  
    d-----         2/6/2019   9:14 AM                bin  
    d-----         2/6/2019   9:14 AM                obj  
    d-----         2/6/2019   9:13 AM                Pages  
    d-----         2/6/2019   9:13 AM                Properties  
    d-----         2/6/2019   9:13 AM                wwwroot  
    -a----         2/6/2019   9:13 AM            146 appsettings.Development.json  
    -a----         2/6/2019   9:13 AM            157 appsettings.json  
    -a----         2/6/2019   9:13 AM            767 Program.cs  
    -a----         2/6/2019   9:13 AM           2115 Startup.cs  
    -a----         2/6/2019   9:13 AM            294 supertestweb.csproj  
    -a----         2/6/2019   9:15 AM       40982879 supertestweb.exe

Now here's what it gets interesting. Let's say I have a console app. Hello World, packed with Warp, ends up being about 35 megs. But if I use the "dotnet-warp -l aggressive" the tool will add the [Mono ILLinker (tree shaker/trimmer)](https://www.hanselman.com/blog/ACompleteContainerizedNETCoreApplicationMicroserviceThatIsAsSmallAsPossible.aspx) and shake off all the methods that aren't needed. The resulting single executable? **Just 9 megs compressed (20 uncompressed).**
    
    
    C:\squishedapp> dotnet-warp -l aggressive  
    O Running AddLinkerPackage...  
    O Running Publish...  
    O Running Pack...  
    O Running RemoveLinkerPackage...  
    C:\squishedapp> dir  
    Mode                LastWriteTime         Length Name  
    ----                -------------         ------ ----  
    d-----         2/6/2019   9:32 AM                bin  
    d-----         2/6/2019   9:32 AM                obj  
    -a----         2/6/2019   9:31 AM             47 global.json  
    -a----         2/6/2019   9:31 AM            193 Program.cs  
    -a----         2/6/2019   9:32 AM            178 squishedapp.csproj  
    -a----         2/6/2019   9:32 AM        9116643 squishedapp.exe

**Here is where you come in!**

**NOTE:** The .NET team has _planned_ to have a "single EXE" supported packing solution built into .NET 3.0. There's a lot of ways to do this. Do you zip it all up with a header/unzipper? Well, that would hit the disk a lot and be messy. Do you "unzip" into memory? Do you merge into a single assembly? Or do you try to AoT (Ahead of Time) compile and do as much work as possible before you merge things? Is a small size more important than speed? 

What do you think? How should a built-in feature like this work and what would YOU focus on?

* * *

**Sponsor:** [Check out Seq 5](https://hnsl.mn/2MKfZjU) for real-time diagnostics from ASP.NET Core and Serilog, now with faster queries, [support for Docker on Linux](https://hnsl.mn/2FZOvpO), and beautiful new dark and light themes.

[« Visiting The National Museum of Computin...](https://www.hanselman.com/blog/VisitingTheNationalMuseumOfComputingInsideBletchleyParkCanWeCrackEnigmaWithRaspberryPis.aspx) | [Blog Home](https://www.hanselman.com/blog/default.aspx) | [Teaching Kids to Code with Minecraft Mod... »](https://www.hanselman.com/blog/TeachingKidsToCodeWithMinecraftModsMadeEasyUsingMakeCodeAndCodeConnection.aspx)

#### About Scott

Scott Hanselman is a former professor, former Chief Architect in finance, now speaker, consultant, father, diabetic, and Microsoft employee. He is a failed stand-up comic, a cornrower, and a book author.

[](https://facebook.com/scott.hanselman) [](https://twitter.com/shanselman) [](https://plus.google.com/108573066018819777334?rel=author) [](http://feeds.hanselman.com/ScottHanselman)  
[About](http://hanselman.com/about)   [Newsletter](http://www.hanselman.com/newsletter)

**Sponsored By**  


**Hosting By**  
[](https://sherweb.com/)

[Comments [33]](https://www.hanselman.com/blog/CommentView.aspx?guid=0b5da3d4-99cb-4b30-88af-cc1aee40adcb#commentstart)

Share on: [Twitter](https://twitter.com/intent/tweet?url=https://www.hanselman.com/blog/BrainstormingCreatingASmallSingleSelfcontainedExecutableOutOfANETCoreApplication.aspx&text=Brainstorming%20-%20Creating%20a%20small%20single%20self-contained%20executable%20out%20of%20a%20.NET%20Core%20application%20-%20Scott%20Hanselman&via=shanselman), [Facebook](https://facebook.com/sharer.php?u=https://www.hanselman.com/blog/BrainstormingCreatingASmallSingleSelfcontainedExecutableOutOfANETCoreApplication.aspx), [Google+](https://plus.google.com/share?url=https://www.hanselman.com/blog/BrainstormingCreatingASmallSingleSelfcontainedExecutableOutOfANETCoreApplication.aspx) or use the [Permalink](https://www.hanselman.com/blog/BrainstormingCreatingASmallSingleSelfcontainedExecutableOutOfANETCoreApplication.aspx)

[](https://www.hanselman.com/blog/)

[](https://www.hanselman.com/blog/)2019年2月6日 18:04:13 UTC

In what way does this method differ from using **Fody** with **Costura** ? See [Link: Fody/Costura](https://github.com/Fody/Costura?WT.mc_id=-blog-scottha).

Stef

[](https://www.hanselman.com/blog/)2019年2月6日 18:07:58 UTC

Dotnet core would be damn near perfect if they had a way to package into a single executable. I want to be able to deliver command line tools as a single, small .exe. Just download and run.

Scott

[](https://www.hanselman.com/blog/)2019年2月6日 18:12:09 UTC

I'd say speed first, then small size by techniques like trimming, and then optionally compression.  
.net apps today are actually quite long to start, maybe because of many dlls to load. I'd expect a single exe to load faster.  
  
That being said, I'd like to be able to do scenarios like having app A and B in the same folder that share the same library L as a dll (this dependency might be a combination of L1 and L2 too). In other words : not only self-contained apps, but also self-contained libraries and self-contained + some externals (could be for licensing reasons).  
  
The key thing is : make it easy to configure complex link scenarii

cube45

[](https://www.hanselman.com/blog/)2019年2月6日 18:20:39 UTC

I would love to have _options_ \-- perhaps a command line switch that allows me to choose between file size and speed of execution, because I can see cases for both.

Stacy

[](https://www.hanselman.com/blog/)2019年2月6日 18:50:13 UTC

Can you connect to the ".NET Native" Team? they built single-exe native apps out of .NET Executables since years, for impressive Performance gains. And, well, They're part of Microsoft, so..  
  
https://blogs.windows.com/buildingapps/2015/08/20/net-native-what-it-means-for-universal-windows-platform-uwp-developers/#lVhv2aTdHxTRkqsZ.97

[David Spörri](https://davepermen.net/)

[](https://www.hanselman.com/blog/)2019年2月6日 20:05:59 UTC

I got excited and looked it up. Warp actually just uncompresses all the files to a temporary folder to run, it's almost like one click unzip or silent installer that's it. I guess it's still nice, but it's it a single exe for real and will make a target system messy. 

Ivan G

[](https://www.hanselman.com/blog/)2019年2月6日 20:14:10 UTC

> Do you zip it all up with a header/unzipper? Well, that would hit the disk a lot and be messy

  
  
Warp is interesting, but isn't this exactly how it works?  
  


> Do you "unzip" into memory?

  
  
If we can't rely on a shared CLR being installed, presumably this would require a native 'packer' executable that contained the compressed binaries?  
  


> Do you merge into a single assembly?

  
  
Can't ILRepack and/or ILMerge do this today?  
  


> Or do you try to AoT (Ahead of Time) compile and do as much work as possible before you merge things? 

  
  
Is that what IL Linker and Mono Linker do? I think this is a really interesting avenue to explore, but would it play nicely with reflection?

cocowalla

[](https://www.hanselman.com/blog/)2019年2月6日 20:19:04 UTC

Greg Szorc looked into this problem in depth for Python, I suspect that a lot of his research would be extremely useful here as well. Specifically, he considered the zip-unpacking strategy and its downsides extensively, as well as how to handle loading dependencies and executing them from areas of memory: <https://gregoryszorc.com/blog/2018/12/18/distributing-standalone-python-applications/>

Brandon Siegel

[](https://www.hanselman.com/blog/)2019年2月7日 5:31:40 UTC

@cocowalla - see the section titled CoreRT Deployments.  
https://msdn.microsoft.com/en-us/magazine/mt830370.aspx  
  
It's been working well for me.  


[Kijana Woodard](https://kijanawoodard.com/)

[](https://www.hanselman.com/blog/)2019年2月7日 7:53:20 UTC

For me this functionality is a bit of a holy grail, having a consistent way of publishing my applications. It's certainly cool that dotnet core can publish into a directory, but the end-user experience has room for improvement! Having a directory where one needs to find the executable is not something I find convenient. This would already improve if the executable is in the root of the publish directory and the needed dependencies are separated into directories which are named to the functionality. Something like "runtime-dependencies", and "application-dependencies". Having a single executable is like cream on top, especially if this only contains the code which is used and not all libraries with everything they provide but I don't use. I already follow some issues on GitHub, and shared my wishes, hope others do so too.  
  
  
Just wanted to add some information about using "dotnet-pack -l aggressive" and dotnet core 3.0  
  
Scott already explains this uses the mono ILLinker and currently this doesn't work with dotnet core 3.0. Read and follow the issue here: https://github.com/mono/linker/issues/428

[Robin Krom](https://getgreenshot.org/)

[](https://www.hanselman.com/blog/)2019年2月7日 8:26:17 UTC

We'd need support for 3rd party components e.g. DevExpress WinForms. And support for all versions of .Net framework.

[TriSys](https://trisys.co.uk/)

[](https://www.hanselman.com/blog/)2019年2月7日 9:31:31 UTC

Si with an you could use a alpine linux docker image and run it right ? So you can have a 50-60MB docker image with your app :D Awesome ! Gonna try that.

[Rémi BOURGAREL](https://remibou.github.io/)

[](https://www.hanselman.com/blog/)2019年2月7日 9:44:56 UTC

Looks like the command has now been renamed to dotnet-warp  
  
https://github.com/Hubert-Rybak/dotnet-warp/issues/2

Michael

[](https://www.hanselman.com/blog/)2019年2月7日 10:04:59 UTC

I would hope to choose at build time if and how hard to shake the tree.  
  
I would hope to choose at build time between uncompressed for speed and various levels of compression.  
  
I would hope to choose at startup between "uncompress to memory", "uncompress to disk and keep for later" and "uncompress to disk/tmp every time". Also with option to do it all at startup or just in time.

Nils

[](https://www.hanselman.com/blog/)2019年2月7日 10:53:26 UTC

Merge into a single assembly all the way, and use an exe stream compressor if you want to compress it. However, how would you handle C++/CLR and native C++ DLLs in the same assembly?  
  
If you run from a DVD it would be nice to have the whole assembly load linearly into memory. Yes we still use DVDs in the places I frequent because USB devices are not allowed.

John Stewien

[](https://www.hanselman.com/blog/)2019年2月7日 11:00:46 UTC

I don't see the point of a single exe.  
We have over a 100 services running on 1 server, and hundreds of small application across our network.  
We don't want hundreds of copies of the same .NET framework.  
  
We need a way to load .NET framework on server startup and that needs to remain in memory as long as the server (or PC) is running.  
Make .NET (CORE) smaller and faster instead of trying to install it with every application we deliver to our customers.  
  
Maybe an option would be to pack the .NET framework in 1 library and leave the exe just the way it is. When deploying we can put just 1 .NET library on a server (PC) and share it between all applications.

Johan Visser

[](https://www.hanselman.com/blog/)2019年2月7日 12:03:28 UTC

I'm interested to see what would happen if I used Reflection to execute code that doesn't appear to be referenced at compile time.

[Bob](http://www.contrivedexample.com/)

[](https://www.hanselman.com/blog/)2019年2月7日 12:05:07 UTC

I agree with Johan Visser 100%.  
  
I think we need something similar to .NET Framework 4 (which installs on a system/computer level), downloaded either through Windows Update or separate, which offer the possibility to keep the EXE size small.   
  
Like Johan, we also have so many small EXEs (and DLLs) which is used throughout the company servers in so many different ways. Yes, size DO matter.  
  
For the ASP.NET Core road, I register that you already have something called a Runtime & Hosting Bundle. Why not just create a .NET Core Framework Bundle 3.x, and make it installable the same way as you do today with .NET Framework.

Roger D

[](https://www.hanselman.com/blog/)2019年2月7日 12:49:41 UTC

Something that would be useful is a way to bundle a cert/signature into the compiled bits so the host can trust it. We use Costura/Fody on .NET Framework apps to get them into a single .exe and use signtool.exe (code signing certs are a $ rip off but that is another topic); experimenting with conversion to .NET Core for cross-platforminess, we are hunting for a single exe package that can be deployed and trusted. Intellivision developers are 1/3 Windows, 1/3 Linux, 1/3 Mac so we’re trying to hit all of them with our code refreshes. #thisShouldBeEasier

[Intv Prime](http://www.intvprime.com/)

[](https://www.hanselman.com/blog/)2019年2月7日 12:59:03 UTC

40 MB is ridiculous  
Look at other tools, Delphi for example, they do it with 10MB or less even.  
And yes, it works on really every 32/64 windows version without the need for any additional file at all.  


GuidoG

[](https://www.hanselman.com/blog/)2019年2月7日 13:22:28 UTC

You'll have to excuse my ignorance (but am willing to learn). But FoxPro (younger people under 30 may have to look that up!) and later Visual FoxPro is still delivering working exe's for smaller businesses as a matter of course. My list of EXE's in my "test live" directory range in size from 1.7 to 4.9 Kb (not Mb). The DOS system was fully self contained, the Visual one just needs half a dozen dll's in the same directory plus the data directory. I can take a demo system to a client and run the whole thing off a 4Mb USB stick that's not even half full. 

[Alan Pengelly](http://www.pengers.co.uk/)

[](https://www.hanselman.com/blog/)2019年2月7日 13:46:17 UTC

Thanks for mentioning my tool! I changed project name to avoid confict with "dotnet pack"  
https://github.com/Hubert-Rybak/dotnet-warp

Hubert Rybak

[](https://www.hanselman.com/blog/)2019年2月7日 16:16:39 UTC

Static linking and copying code from C lib files instead of DLLs - 1982 Unix System V era technology.  
  
Here are the problems:  
\- GitHub based packages ensure DLL hell for a large application with package inside package dependencies  
\- Transition from .NET framework from 1 per computer to several GitHub packages ensures DLL hell  
\- Shifting the version maintenance from the OS level (MS) to individual GitHub packages ensures a large C# project lasting 5+ years will have man months spent upgrading GitHub packages, debugging problems and dealing with DLL hell  
  
Forcing man years of wasted effort and expense of moving existing c# .net Framework applications to .net core. You can run the old .net framework for a long time, but many of the github packages you use or third party libraries will have moved to .net core. Your option is to stick with the old packages or old third party libraries with all the bugs and limitation or spend many man months doing a full release cycle just to switch to .net core.  
  
Simple put. .NET Core should ship the entire .net framework on Windows 10 machines and later with no need to add many .net core GitHub packages to get small bits of the .net core framework.  
  
.NET core is an OS level component and should be on every Windows 10 machine by default.  
  
Linux users will have to install the entire .net core framework before using your application. This worked well for the regular .net framework when v2.0 was released.  
  
A mid level of frustration in that I spent the last 3 weeks with GitHub .net framework and third party packages DLL hell for a 1 million line .net application. Keeping it as is with old packages did not help as serious bug fixes needed were only in the newer GitHub packages.  
  
MS common business practice is to shift the high maintenance and configuration cost of the Framework side of an application from MS to the paying corporate customers.  
  
 **How can .net dev team make such business decisions without regard to TCO for corporate customers?  
  
The SQL Server team could not make these type of decisions and force huge costly upgrades on the customers every 3 years.  
  
No large company would buy SQL Server if the terms and conditions stated that you'll have to replace SQL Server, reconfigure you data center, and spend lots of money every 3 years to get back to the level of reliability and security you had when initially installing SQL server.**  
  
Half maintained GitHub packages should not be the norm. Development should get more productive over time for very large systems and not just the click-drag-click bam 2 screen demo app in 5 minutes.  
  
Microsoft has community responsibility to push for technologies making development easier, cleaner, more secure and lower cost for long term large projects.  
  
Microsoft has been doing the opposite by breaking the framework into many small pieces, using more soon to be maintenance problem third party tools in VS, shifting more and more of the configuration to corporate developers.  
  
First should come good MS effort towards large scale solution developers which have been ignored by MS for 10 years; secondly should come good MS effort towards developers of small solutions.  
  
My large employer has stopped adopting most MS .net/Azure related technologies outside of central C#, asp.net mvc, sql server on premise or hosted in a basic Azure web site. That's after several 6 month X person projects just to upgrade existing critical business systems to the latest VS and .net framework versions.  
  
The large system I work on has ~40 projects each with up to 25 GitHub packages. Budget for adding new business functionality for the large system comes once every 2 to 3 years. Hotfixes every 3 to 4 months. Budget for syncing up and upgrading just GitHub packages is non-existent.  


new?

[](https://www.hanselman.com/blog/)2019年2月7日 17:10:52 UTC

9 Mb for a console app?  
It's ridiculous - Windows 3.1 (I know, a long time ago) was only 11 Mb, for a complete graphical OS. So i think that 9Mb for a Hello World console application is simply ridiculous.

Theo

[](https://www.hanselman.com/blog/)2019年2月7日 22:33:11 UTC

9MB for a tiny little program? 🤮 But hey, it's a start! And at least it's _possible_. I really like that we'll have more deployment choices for .NET apps.

[Rick Brewster](https://blog.getpaint.net/)

[](https://www.hanselman.com/blog/)2019年2月8日 0:45:24 UTC

The GraalVM project is doing some interesting work with [AOT compiling and native image generation](https://www.graalvm.org/docs/reference-manual/aot-compilation/ "GraalVM") for Java. The resulting program does not run on the Java HotSpot VM, but uses a separate VM (SubstrateVM) resulting in faster startup and lower runtime memory requirements. The code can be compiled to a shared object or exe. The optimizations are achieved by aggressive static analysis that requires knowing all classes and all bytecode that is reachable at run time. Substrate VM has partial support for reflection and predictably it needs to know ahead of time the reflectively accessed program elements.   


Chris Whelan

[](https://www.hanselman.com/blog/)2019年2月8日 9:28:59 UTC

What if c++/cli is part of the app?

[thomas woelfer](https://www.woelfer.com/)

[](https://www.hanselman.com/blog/)2019年2月8日 14:05:50 UTC

This would be nice to have for .NET, but do we have nothing more important to work on?  
  
Hard to come up with use cases for this. If you can deliver a single exe you can usually deliver a single folder full of files.  
  
Many people have commented on the GitHub issue that the ability to do that is not important to them. The team seems stubbornly set on delivering this.

tobi

[](https://www.hanselman.com/blog/)2019年2月8日 19:21:31 UTC

I would like to have these options  
  
native: compile a single native instruction set  
all: all the referenced dll in the same package  
reference: the referenced dll stays as reference (but the app may be natively compiled)  
shake: perform treeshaking and includes only the code that can be statically reached  
hint: include in the package also the indicated parts of code  
  
different apps may have different needs, as always having options may save the day :)  
  
Sincerely,  
Captain Obvious :P  


[Mose Bottacini](http://photoatomic.com/)

[](https://www.hanselman.com/blog/)2019年2月8日 22:37:32 UTC

Im, old. I remember when Borland users would brag about their ultra-optimized binaries.  
  
IMHO, I see a natural progression:

  

  1. Binaries with static and dynamic shared libraries - this will come and go with different methods of API communication
  

  2. Simple roll it all in binary - easy to share and publish, but big
  

  3. Pruned binary - I like the idea of trimming the unused binary fat
  

  4. Optimized binary - Make grandpa Borland proud, self-managed, self-contained, lean, fast, small
  

  5. 'highlander' binary - only one, runs anywhere, immortal but cut off the head and the body dies (future replacing 'containers')
  

  
  
I feel like decompressing to run might come with it's own set of problems. Although instead of decompressing to memory or the FS, why not decompress to windows container (ala UWP) and ask for permission to do anything beyond what a UWP app can do, but keep it a possibility.  
  
I think some are missing out on where the single binary could be going and that the shared lib option will probably never go away. If you are scared of the single binary, are you also looking into using Docker to scoop up the mess in development and dump it into production? You might want to meditate on what is different. In the long term I would like to see compiling to a single exe the future of containers: Small, Fast, Sandboxed. Maybe future binaries have a completely different extension because ... Container as Executable  
  
just sayin'

Drydenmaker

[](https://www.hanselman.com/blog/)2019年2月12日 14:33:25 UTC

An empty "dotnet new razor" application is 40MB? And 9 if tree shacked?  
  
I am writing my command line utilities "still" in cpp. If 32bit compiled against the "old" Win7 SDK and linked against VS runtime statically, I end up with a exe of 1.2MB running blazing fast and completely standalone from WinXP up to the latest Win10.  
And with all the new C11/C17 features available, cpp has become much less cumbersome than it used to be.  
  
My cmd utilities are often called in tight loops (scripts iterating over file/folder trees). So startup time is important. Much more important than for fat desktop UI apps.  
  
If runtime lightness is more important than dev time, I use cpp. If dev time is more important, I use managed languages as C# or Java.  
Having both at the same time is like wanting to have the cake and still eating it. For me this is a solution looking for a problem. But of course other's MMV.  
  


Joerg

[](https://www.hanselman.com/blog/)2019年2月12日 15:38:52 UTC

Hi Scott -  
I apologize in advance for posting this comment on the wrong blog entry - it is intended for your blog post of Jan 16, 2019 - Installing .NET Core 2.x SDK on a Raspberry Pi; but comments are closed on that post. If possible, I would love the content of this comment to be moved to that post - as it will hopefully help people who are trying to follow that blog (like I was).  
  
I followed your excellent instructions for installing .NET Core on a Raspberry Pi 3. My install was a little tweaked, because I was installing 2.2.103 - but your instructions explain how to find the link for newer versions of the framework.  
  
Note 1) You explain how to also download and install the ASP.NET Runtime; but I wonder if that is no longer needed. It seemed to me that the ASP.NET Runtime is included in the .NET Core SDK with version 2.2.103.  
  
Note 2) This is the reason I wanted to add this comment: After cloning my .NET Core project via Git to my Raspberry Pi; I tried running "dotnet restore" to restore NuGet packages. This caused a ton of very weird SSL errors while talking to the NuGet API host. I spent hours trying to diagnose this and my Internet connection and the packages I used in my project, etc. In the end, what I figured out is that it seems like "dotnet restore" is trying to download too many packages at once from api.nuget.org - and this overwhelms some part of SSL-handling (OpenSSL?) on the Raspberry Pi. After hours of many wild goose chases, the simple fix was to run "dotnet restore --disable-parallel". Wow, I wish I would have figured out to to do that earlier; and hopefully that saves others some pain.  
  
Note 3) There was some discussion in the comments about running WinForms and other GUIs on .NET Core on the Raspberry Pi. I am having some luck with using AvaloniaUI on the Raspberry Pi. There are specific notes/discussion for running "Avalonia on Raspbian" \- so interested people should make sure they Google-Bing that phrase. (I had some trouble with libSkiaSharp - but the correct ARM version is in the latest NuGet package of Avalonia.Skia.Linux.Natives.)  
  
Thank you for your excellent blog!

Jeremy Ellis

[](https://www.hanselman.com/blog/)2019年2月12日 20:58:17 UTC

Need to have a compiler switch to turn off and give compiler error message if any use of reflection is used so that static linking can be done for only parts of classes actually called.  
  
Fact is, the most basic c# subset can produce a much much smaller executable when only the most basic language constructs are used. Too much syntactic sugar has been added in the last many years - *cough* C#'s not a functional language and shouldn't turn into a messy spaghetti tangle of JavaScript/F#/erlang via adopting 2 or 3 language features a year from those languages.   
  


Bob

Comments are closed.

Disclaimer: The opinions expressed herein are my own personal opinions and do not represent my employer's view in any way.

### Blog

  * [Privacy Policy](https://www.hanselman.com/privacy)
  * [Greatest Hits](https://www.hanselman.com/blog/GreatestHits.aspx)
  * [Dev Tool List](https://www.hanselman.com/tools)



### Podcast

  * [Hanselminutes](http://hanselminutes.com/)
  * [This Developer's Life](http://thisdeveloperslife.com/)
  * [Ratchet & The Geek](http://ratchetandthegeek.com/)



### Speaking

  * [Speaking/Videos](https://www.hanselman.com/blog/CategoryView.aspx?category=Speaking)
  * [Presentations Tips](http://www.speakinghacks.com/)



### Books

  * [ASP.NET 4.5](https://www.amazon.com/gp/product/1118311825/ref=as_li_ss_tl?ie=UTF8&camp=1789&creative=390957&creativeASIN=1118311825&linkCode=as2&tag=diabeticbooks)
  * [ASP.NET MVC 4](https://www.amazon.com/gp/product/111834846X/ref=as_li_ss_tl?ie=UTF8&camp=1789&creative=390957&creativeASIN=111834846X&linkCode=as2&tag=diabeticbooks)
  * [Relationship Hacks](http://relationshiphacks.com/)



© Copyright 2018, [Scott Hanselman](https://www.hanselman.com/). Design by [@jzy](http://www.8164.org/)

[](https://www.hanselman.com/blog/)
