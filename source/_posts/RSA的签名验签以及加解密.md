---
title: RSA的签名验签以及加解密
date: 2017/6/24 23:05:20
tags:
---


作者：刘巍然-学酥  
链接：https://www.zhihu.com/question/25912483/answer/31653639  
来源：知乎  
著作权归作者所有。商业转载请联系作者获得授权，非商业转载请注明出处。  
  


休息完毕，开始进入答题时间！  
在回答之前我看了一下已经有的答案，我个人感觉不太舒服… 首先，题主既然提出了问题，我们还是应该用心来答，而不是打击题主的信心… 其次，题主有这个问题的本质原因是因为RSA体制本身的一个特点决定的。我相信题主在提问前已经进行了很多的资料搜索和查找工作，甚至有可能阅读了RSA那篇原始论文。因此，我们应该更多地考虑：  


  * 为什么题主会提出这个问题，这个问题的本质原因来自于哪里？
  * 我们如何进行详细的解答，帮助题主解决这个问题。



最后，一些答案本身可能会误导大家，所以还请仔细斟酌后再进行回答。

  


=================分割线=================

我们来回顾一下RSA的加密算法。我们从公钥加密算法和签名算法的定义出发，用比较规范的语言来描述这一算法。

RSA公钥加密体制包含如下3个算法：KeyGen（密钥生成算法），Encrypt（加密算法）以及Decrypt（解密算法）。

  * ![\(PK, SK\)\\leftarrow KeyGen\(\\lambda\)](https://www.zhihu.com/equation?tex=%28PK%2C+SK%29%5Cleftarrow+KeyGen%28%5Clambda%29)。密钥生成算法以安全常数![\\lambda](https://www.zhihu.com/equation?tex=%5Clambda)作为输入，输出一个公钥PK，和一个私钥SK。安全常数用于确定这个加密算法的安全性有多高，一般以加密算法使用的质数p的大小有关。![\\lambda](https://www.zhihu.com/equation?tex=%5Clambda)越大，质数p一般越大，保证体制有更高的安全性。在RSA中，密钥生成算法如下：算法首先随机产生两个不同大质数p和q，计算N=pq。随后，算法计算欧拉函数![\\varphi\(N\)=\(p-1\)\(q-1\)](https://www.zhihu.com/equation?tex=%5Cvarphi%28N%29%3D%28p-1%29%28q-1%29)。接下来，算法随机选择一个小于![\\varphi\(N\)](https://www.zhihu.com/equation?tex=%5Cvarphi%28N%29)的整数e，并计算e关于![\\varphi\(N\)](https://www.zhihu.com/equation?tex=%5Cvarphi%28N%29)的模反元素d。最后，公钥为PK=(N, e)，私钥为SK=（N, d）。
  * ![CT \\leftarrow Encrypt\(PK,M\)](https://www.zhihu.com/equation?tex=CT+%5Cleftarrow+Encrypt%28PK%2CM%29)。加密算法以 **公钥PK和待加密的消息M作为输入** ，输出密文CT。在RSA中，加密算法如下：算法直接输出密文为![CT=M^e \\bmod N](https://www.zhihu.com/equation?tex=CT%3DM%5Ee+%5Cbmod+N)  

  * ![M \\leftarrow Decrypt\(SK,CT\)](https://www.zhihu.com/equation?tex=M+%5Cleftarrow+Decrypt%28SK%2CCT%29)。解密算法以 **私钥SK和密文CT作为输入** ，输出消息M。在RSA中，解密算法如下：算法直接输出明文为![M=CT^d \\bmod N](https://www.zhihu.com/equation?tex=M%3DCT%5Ed+%5Cbmod+N)。由于e和d在![\\varphi\(N\)](https://www.zhihu.com/equation?tex=%5Cvarphi%28N%29)下互逆，因此我们有：![CT^d=M^{ed}=M \\bmod N](https://www.zhihu.com/equation?tex=CT%5Ed%3DM%5E%7Bed%7D%3DM+%5Cbmod+N)  




所以，从算法描述中我们也可以看出： **公钥用于对数据进行加密，私钥用于对数据进行解密** 。当然了，这个也可以很直观的理解：公钥就是公开的密钥，其公开了大家才能用它来加密数据。私钥是私有的密钥，谁有这个密钥才能够解密密文。否则大家都能看到私钥，就都能解密，那不就乱套了。

  


=================分割线=================

我们再来回顾一下RSA签名体制。签名体制同样包含3个算法：KeyGen（密钥生成算法），Sign（签名算法），Verify（验证算法）。

  * ![\(PK,SK\) \\leftarrow KeyGen\(\\lambda\)](https://www.zhihu.com/equation?tex=%28PK%2CSK%29+%5Cleftarrow+KeyGen%28%5Clambda%29)。密钥生成算法同样以安全常数![\\lambda](https://www.zhihu.com/equation?tex=%5Clambda)作为输入，输出一个公钥PK和一个私钥SK。在RSA签名中，密钥生成算法与加密算法完全相同。
  * ![\\sigma \\leftarrow Sign\(SK,M\)](https://www.zhihu.com/equation?tex=%5Csigma+%5Cleftarrow+Sign%28SK%2CM%29)。签名算法 **以私钥SK和待签名的消息M作为输入** ，输出签名![\\sigma](https://www.zhihu.com/equation?tex=%5Csigma)。在RSA签名中，签名算法直接输出签名为![\\sigma = M^d \\bmod N](https://www.zhihu.com/equation?tex=%5Csigma+%3D+M%5Ed+%5Cbmod+N)。注意， _签名算法和RSA加密体制中的解密算法非常像_ 。
  * ![b \\leftarrow Verify\(PK,\\sigma,M\)](https://www.zhihu.com/equation?tex=b+%5Cleftarrow+Verify%28PK%2C%5Csigma%2CM%29)。验证算法 **以公钥PK，签名![\\sigma](https://www.zhihu.com/equation?tex=%5Csigma) 以及消息M作为输入**，输出一个比特值b。b=1意味着验证通过。b=0意味着验证不通过。在RSA签名中，验证算法首先计算![M'=\\sigma^e \\bmod N](https://www.zhihu.com/equation?tex=M%27%3D%5Csigma%5Ee+%5Cbmod+N)，随后对比M'与M，如果相等，则输出b=1，否则输出b=0。注意： _验证算法和RSA加密体制中的加密算法非常像_ 。



所以，在签名算法中， **私钥用于对数据进行签名，公钥用于对签名进行验证** 。这也可以直观地进行理解：对一个文件签名，当然要用私钥，因为我们希望只有自己才能完成签字。验证过程当然希望所有人都能够执行，大家看到签名都能通过验证证明确实是我自己签的。

  


=================分割线=================

那么，为什么题主问这么一个问题呢？我们可以看到，RSA的加密/验证，解密/签字过程太像了。同时，RSA体制本身就是对称的：如果我们反过来把e看成私钥，d看成公钥，这个体制也能很好的执行。我想正是由于这个原因，题主在学习RSA体制的时候才会出现这种混乱。那么解决方法是什么呢？建议题主可以学习一下其他的公钥加密体制以及签名体制。其他的体制是没有这种对称性质的。举例来说，公钥加密体制的话可以看一看ElGamal加密，以及更安全的Cramer-Shoup加密。签名体制的话可以进一步看看ElGamal签名，甚至是BLS签名，这些体制可能能够帮助题主更好的弄清加密和签名之间的区别和潜在的联系。

至于题主问的加密和签名是怎么结合的。这种体制叫做签密方案（SignCrypt），RSA中，这种签密方案看起来特别特别像，很容易引起混乱。在此我不太想详细介绍RSA中的加密与签字结合的方案。我想提醒题主的是，加密与签字结合时，两套公私钥是不同的。

  


如果题主还有进一步的问题，欢迎留言。我个人是衷心希望大家都了解一点密码学的知识，以便了解庞大的计算机网络系统到底是如何保护数据安全性的。希望我的回答能对题主有所帮助。

  


  


以上
