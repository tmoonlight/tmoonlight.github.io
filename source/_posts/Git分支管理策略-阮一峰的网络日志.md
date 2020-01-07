---
title: Git分支管理策略-阮一峰的网络日志
date: 2016/8/9 0:23:16
tags:
---


                                   

# Git分支管理策略

                                           

[](http://www.bshare.cn/share)

                                   

                                                                                   

作者： [阮一峰](http://www.ruanyifeng.com/)

                                   

日期： [2012年7月 5日](http://www.ruanyifeng.com/blog/2012/07/)

                                   

                                                               

                                                                           

如果你严肃对待编程，就必定会使用"[版本管理系统](http://www.ruanyifeng.com/blog/2008/12/a_visual_guide_to_version_control.html)"（Version Control System）。

                                                                                                               

眼下最流行的"版本管理系统"，非[Git](http://git-scm.com/)莫属。

相比同类软件，Git有很多优点。其中很显著的一点，就是版本的分支（branch）和合并（merge）十分方便。有些传统的版本管理软件，分支操作实际上会生成一份现有代码的物理拷贝，而Git只生成一个指向当前版本（又称"快照"）的指针，因此非常快捷易用。

但是，太方便了也会产生副作用。如果你不加注意，很可能会留下一个枝节蔓生、四处开放的版本库，到处都是分支，完全看不出主干发展的脉络。

[Vincent Driessen](http://nvie.com/)提出了一个分支管理的[策略](http://nvie.com/posts/a-successful-git-branching-model/)，我觉得非常值得借鉴。它可以使得版本库的演进保持简洁，主干清晰，各个分支各司其职、井井有条。理论上，这些策略对所有的版本管理系统都适用，Git只是用来举例而已。如果你不熟悉Git，跳过举例部分就可以了。

 **一 、主分支Master**

首先，代码库应该有一个、且仅有一个主分支。所有提供给用户使用的正式版本，都在这个主分支上发布。

Git主分支的名字，默认叫做Master。它是自动建立的，版本库初始化以后，默认就是在主分支在进行开发。

 **二 、开发分支Develop**

主分支只用来分布重大版本，日常开发应该在另一条分支上完成。我们把开发用的分支，叫做Develop。

这个分支可以用来生成代码的最新隔夜版本（nightly）。如果想正式对外发布，就在Master分支上，对Develop分支进行"合并"（merge）。

Git创建Develop分支的命令：

> 　　git checkout -b develop master

将Develop分支发布到Master分支的命令：

> 　　# 切换到Master分支  
> 　　git checkout master
> 
> 　　# 对Develop分支进行合并  
> 　　git merge --no-ff develop

这里稍微解释一下，上一条命令的\--no-ff参数是什么意思。默认情况下，Git执行"快进式合并"（fast-farward merge），会直接将Master分支指向Develop分支。

使用\--no-ff参数后，会执行正常合并，在Master分支上生成一个新节点。为了保证版本演进的清晰，我们希望采用这种做法。关于合并的更多解释，请参考Benjamin Sandofsky的[《Understanding the Git Workflow》](http://sandofsky.com/blog/git-workflow.html)。

 **三 、临时性分支**

前面讲到版本库的两条主要分支：Master和Develop。前者用于正式发布，后者用于日常开发。其实，常设分支只需要这两条就够了，不需要其他了。

但是，除了常设分支以外，还有一些临时性分支，用于应对一些特定目的的版本开发。临时性分支主要有三种：

> 　　* 功能（feature）分支
> 
> 　　* 预发布（release）分支
> 
> 　　* 修补bug（fixbug）分支

这三种分支都属于临时性需要，使用完以后，应该删除，使得代码库的常设分支始终只有Master和Develop。

 **四 、 功能分支**

接下来，一个个来看这三种"临时性分支"。

第一种是功能分支，它是为了开发某种特定功能，从Develop分支上面分出来的。开发完成后，要再并入Develop。

功能分支的名字，可以采用feature-*的形式命名。

创建一个功能分支：

> 　　git checkout -b feature-x develop

开发完成后，将功能分支合并到develop分支：

> 　　git checkout develop
> 
> 　　git merge --no-ff feature-x

删除feature分支：

> 　　git branch -d feature-x

 **五 、预发布分支**

第二种是预发布分支，它是指发布正式版本之前（即合并到Master分支之前），我们可能需要有一个预发布的版本进行测试。

预发布分支是从Develop分支上面分出来的，预发布结束以后，必须合并进Develop和Master分支。它的命名，可以采用release-*的形式。

创建一个预发布分支：

> 　　git checkout -b release-1.2 develop

确认没有问题后，合并到master分支：

> 　　git checkout master
> 
> 　　git merge --no-ff release-1.2
> 
> 　　# 对合并生成的新节点，做一个标签  
> 　　git tag -a 1.2

再合并到develop分支：

> 　　git checkout develop
> 
> 　　git merge --no-ff release-1.2

最后，删除预发布分支：

> 　　git branch -d release-1.2

 **六 、修补bug分支**

最后一种是修补bug分支。软件正式发布以后，难免会出现bug。这时就需要创建一个分支，进行bug修补。

修补bug分支是从Master分支上面分出来的。修补结束以后，再合并进Master和Develop分支。它的命名，可以采用fixbug-*的形式。

创建一个修补bug分支：

> 　　git checkout -b fixbug-0.1 master

修补结束后，合并到master分支：

> 　　git checkout master
> 
> 　　git merge --no-ff fixbug-0.1
> 
> 　　git tag -a 0.1.1

再合并到develop分支：

> 　　git checkout develop
> 
> 　　git merge --no-ff fixbug-0.1

最后，删除"修补bug分支"：

> 　　git branch -d fixbug-0.1

（完）

                                                                   

                                   

### 文档信息

  * 版权声明：自由转载-非商用-非衍生-保持署名（[创意共享3.0许可证](http://creativecommons.org/licenses/by-nc-nd/3.0/deed.zh)）
  * 发表日期： 2012年7月 5日
  * 更多内容： [ 档案](http://www.ruanyifeng.com/blog/archives.html)  » [ 开发者手册](http://www.ruanyifeng.com/blog/developer/) » [ 开发者手册](http://www.ruanyifeng.com/blog/developer/)
  * 博客文集：[《前方的路》](http://road.ruanyifeng.com/)，[《未来世界的幸存者》](https://ruanyf.github.io/survivor)
  * 社交媒体：[![](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAk1JREFUeNqsVc9rE0EU/mZ3dhPNJl2bVIk/Km09aFBE8GLvnrwJXvwLxIsgCP4Fnit6E8Gb0IMHwUsL9SSNgiAoooJIpE1Joo1h82uT7IxvxmRN3Q32kAdvdrI773vfe+/bDTu9WioDcDAda3Ja8piepQ1avCkCeny0Y+R9KenKwOmHHLvPaKn7AtVOoO8dOWDCMpg+41gMQv5FNEJACgoE8MsPNLDBhgfoutMOsJDmWLmU037C4TApciljwQ/kHoohQxV06+wMXNvE7WINuaSpvdYVKLgWnl3O4zAxU3Z1IYVGT+B5qYWV9z2kbQaT/cOwTsxOUUYF+mB5Dkmi8HG3h2q9hxsFNwRTdvQgxxnXRrMvIdkEhllis/qtiWuLDm4WZnBlPoWnXz28LHdw0uGR7m9Wu3j0qYGEubfn4UlBfXO4gTZltRNMg9w9f0i7kNFxllsBlS0x75gYfxyWTBUjT2W5CSMSPBrQuG21+hiMDS8CeCzF8eSLp13uQ3Ab1IqMFZN8tFHPmgOBF99bYP8Be1Xp6t7OJszJgKpPSiZvaj7uf2hguz2IBQuozDvFH6RDBjuK9wdQDvt0nMpW+8efGyh5UcAeZb2+UcHbnz5Jx4wdlp4yJYU3kLiQtfBwOYeLc8nIwfXtNu69q2Oz4mMxbemYOOOjKarhvq51KUjg3GwiFPIuvcNF6pnSnWK0lOEabNLgQh1aJFAhlGB9rG110B2+oyRNPc0sZbRNFltmLKDKqj4Qrm1ojzOxDz2pyPQ0P7CK4c40/wJ+CzAAGYrXsvfFXR4AAAAASUVORK5CYII=) twitter](https://twitter.com/ruanyf)，[![](data:image/png;base64,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) weibo](http://weibo.com/ruanyf)
  * Feed订阅： [![](data:image/gif;base64,R0lGODlhFAAUANU2APV1GfmrePZ8IeR5VPq2ieNkLf7p3NpFJveCJ/NoFfibVPR1IexiJfvdzuZrNfXGuOhUEueTevaFOfzIpNxDGeZZJNU6IOubifiVTfOnid1aLuZpLOFSJf/28vJtIvGcefaLSd5kNu9iFv/z6//48//t4uFfLfNkCeJNGuGNc/rCof/+/PVxEeldI/V5KeKMbu9nIvB2OvzQtNxMKfOPXP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAEAADYALAAAAAAUABQAAAb/QJstEjKZCsjCZsl0pIS2QWXaYsA8noVWIEAgFoNhhWNyMESi7Jbr/aZCFdRnZJAFJCwAt+vlOkwtEBk1hDUkExJ6bQJaGyYMLTQTDSOFHQF8jAseSFYiJwkuAQ2FE1xaHjBIMDAgARguJwABJIQTLJswDEgeIgSEJQQILAqVNQEiui1HHgkBhTUGCiwYtR0xLS0VJhtaEgoEBoQkGCfPNSoQY9xaCwAsCBPALgmkJA4cZN0CAAgY/PJqEIDwgVAECjM0bOAnwcAKGQJclKhRgoGDDjUeUDigkN8vQiBOfByAglSDAxw3IBCgYEU0FyJoEPpA4UGNDjMsaHDQR8EdPVYOMmQYQGHAhQgHLIRIsWAlgASsGMRBMWMGBQtYZ7yIsoBRKkhjOMxAiXJGGCEpHGxIYsSIhrcaQmy1EQQAOw==)](http://www.ruanyifeng.com/feed.html)

                               
  *[2012年7月 5日]: 2012-07-05T18:23:30+08:00
