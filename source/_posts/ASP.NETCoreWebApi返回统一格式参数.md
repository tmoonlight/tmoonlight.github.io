---
title: ASP.NETCoreWebApi返回统一格式参数
date: 2018/2/18 9:58:41
tags:
---


# [ASP.NET Core WebApi 返回统一格式参数](http://www.cnblogs.com/xishuai/p/asp-net-core-webapi-return-result-middleware.html)

业务场景：

业务需求要求，需要对 WebApi 接口服务统一返回参数，也就是把实际的结果用一定的格式包裹起来，比如下面格式：

{

"response":{

"code":200,

"msg":"Remote service error",

"result":""

}}

具体实现：

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Filters;

  


public class WebApiResultMiddleware : ActionFilterAttribute

{

public override void OnResultExecuting(ResultExecutingContext context)

{

//根据实际需求进行具体实现

if (context.Result is ObjectResult)

{

var objectResult = context.Result as ObjectResult;

if (objectResult.Value == null)

{

context.Result = new ObjectResult(new { code = 404, sub_msg = "未找到资源", msg = "" });

}

else

{

context.Result = new ObjectResult(new { code = 200, msg = "", result = objectResult.Value });

}

}

else if (context.Result is EmptyResult)

{

context.Result = new ObjectResult(new { code = 404, sub_msg = "未找到资源", msg = "" });

}

else if (context.Result is ContentResult)

{

context.Result = new ObjectResult(new { code = 200, msg = "", result= (context.Result as ContentResult).Content });

}

else if (context.Result is StatusCodeResult)

{

context.Result = new ObjectResult(new { code = (context.Result as StatusCodeResult).StatusCode, sub_msg = "", msg = "" });

}

}

}

Startup添加对应配置：

public void ConfigureServices(IServiceCollection services)

{

services.AddMvc(options =>

{

options.Filters.Add(typeof(WebApiResultMiddleware));

options.RespectBrowserAcceptHeader = true;

});

}

  

