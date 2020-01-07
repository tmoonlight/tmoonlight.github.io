---
title: java备忘
date: 2019/6/7 15:52:02
tags:
---


1.string x=“test”

string y=“test”

x==y 返回 false  连等号是引用比较 java创建这两个变量引用地址不同

需要用equal

  


2.匿名类

  


HttpProxyServer server =

DefaultHttpProxyServer.bootstrap()

.withPort(7080)

.withFiltersSource(new HttpFiltersSourceAdapter() {

public HttpFilters filterRequest(HttpRequest originalRequest, ChannelHandlerContext ctx) {

//匿名类，C#这种写法是匿名对象

return new HttpFiltersAdapter(originalRequest) {

@Override

public HttpResponse clientToProxyRequest(HttpObject httpObject) {

// TODO: implement your filtering here

return null;

}

  


@Override

public HttpObject serverToProxyResponse(HttpObject httpObject) {

// TODO: implement your filtering here

return httpObject;

}

};

}

})

.start();

3.打包

找不到主清单属性

个人分类： [IDE](https://blog.csdn.net/u013767472/article/category/6402117)

报错原因是因为MANIFEST.MF文件下找不到MAIN-CLASS的属性

可以打卡导出的jar包MANIFEST.MF文件查看验证

解决方法：

再添加jar包的时候，修改DIRECT FOR MANIFEST.MF

idea默认是src/main/java

我们需要设置为src目录即可 注意：ideaJ可以自动加上

![](https://img-blog.csdn.net/20160920142516295?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQv/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/Center)

3.坑爹的OutputStream和InputStream

java把stream分两个对象， inputStream用来读 OutputStream用来写。。颠覆

using方法在这里变为try（OutputStream  opt= new OutputStream(),InputStream ipt = new InputStream()）

  

