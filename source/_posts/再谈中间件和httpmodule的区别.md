---
title: 再谈中间件和httpmodule的区别
date: 2018/12/24 12:39:23
tags:
---


再谈中间件和httpmodule的区别

  


中间件的特性

可使用 [app.Use](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.builder.useextensions) 将多个请求委托链接在一起。 next 参数表示管道中的下一个委托。 （请记住，可通过不调用 next 参数使管道短路。）通常可在下一个委托前后执行操作，如以下示例所示：

C#

public class Startup

{

public void Configure(IApplicationBuilder app)

{

app.Use(async (context, next) =>

{

// 这里设置request

await next.Invoke();

// 这里设置响应

});

  


app.Run(async context =>

{

await context.Response.WriteAsync("Hello from 2nd delegate.");

});

}

}

警告

在向客户端发送响应后，请勿调用 next.Invoke。 响应启动后，针对 HttpResponse 的更改将引发异常。 例如，设置标头、状态代码等更改将引发异常。 调用 next 后写入响应正文：

  * 可能导致违反协议。 例如，写入的长度超过规定的 content-length。

  * 可能损坏正文格式。 例如，向 CSS 文件中写入 HTML 页脚。




[HttpResponse.HasStarted](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.http.features.httpresponsefeature#Microsoft_AspNetCore_Http_Features_HttpResponseFeature_HasStarted) 是一个有用的提示，指示是否已发送标头和/或已写入正文。

  


httpmodule会依次处理请求和响应

  


startup.cs有如下写法：

 public void Configure(IApplicationBuilder app, IHostingEnvironment env)

        {

            if (env.IsDevelopment())

            {

                app.UseDeveloperExceptionPage();

            }

            app.Use(async (ctx, next) =>

            {

                //  string str = ctx.Request.QueryString.Value;

                Console.WriteLine("A1");

                await next.Invoke();

                Console.WriteLine("A2");

            });

            app.Use(async (ctx, next) =>

            {

                //  string str = ctx.Request.QueryString.Value;

                Console.WriteLine("B1");

                await next.Invoke();

                Console.WriteLine("B2");

            });

            app.Use(async (ctx, next) =>

            {

                //  string str = ctx.Request.QueryString.Value;

                Console.WriteLine("C1");

                await next.Invoke();

                Console.WriteLine("C2");

            });

            app.UseMvc();

运行结果：

  

