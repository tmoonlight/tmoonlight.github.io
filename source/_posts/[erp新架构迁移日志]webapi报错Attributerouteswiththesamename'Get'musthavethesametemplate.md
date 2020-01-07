---
title: [erp新架构迁移日志]webapi报错Attributerouteswiththesamename'Get'musthavethesametemplate
date: 2017/5/27 2:55:38
tags:
---


  1. webapi报错  Attribute routes with the same name 'Get' must have the same template:



// GET: api/haha/5

        [HttpGet("{id}", Name = "Get")]

        public string Get(int id)

        {

            return "value";

        }

如上代码 Name可能有问题

改为如下即可

[HttpGet]

[Route("Get/{id}")]

public string Get(int id)

{

    return "value";

}

  


  2. 3.5迁移到4.0出现诡异问题 该语法不能用? return (T)_dapper.Get<T>(Connection, id, _transaction, commandTimeout);查到是dynamic相关语法不能用,引入如下命名空间即解决问题 ,4.0很多第三方库不提供支持,引入新框架难度大



  


  3. Dto是否需要改名? 映射如何实现? 数据库连接如何动态切换?



frombody问题无法解决?

  


  4. 引用程序集没有强名称? 需要对应用程序集进行"签名" 加入snk



  


  5. 服务问题 asmx



  


<http://119.135.176.146:9981/erpws/SealWebService.asmx> SealWebServiceQY

SealWebService

<http://203.91.46.228/erpws/SealWebService.asmx> SealWebService

  


  8. wcf3.5升级到 4.0各种诡异的问题(非wcf引起)
  9. 4.0 await/async问题解决: 引入Microsoft.Bcl.Async
  10. dapper产生的 dynamic类型无法automapper成实际类型,解决方法:



使用扩展的业务对象承载 混合对象使用混合实体来承载 dapper表连接查询不出数据?

  


  


(ps调用服务失败,切换wcf搞定)

webapi不能接收到frombody的值?

  


dapper分页出现诡异的问题(extension引起),tolist的时候会再次查询一次,问题解决 bufferd参数导致延迟查询

11删除接口出现问题,已解决:删除时对接口传的值需要跟在接口后面delete/1

间歇性查不到值,返回null 已解决:查不到值是操作超时的问题...

restsharp调用成功但message却出现int-dictionary-object 转换失败, 已解决:如果返回的类型是基元类型,excute restsharp时不输入参数即可.

  


  


12.vs插件操作,如果要确定某个目录是否存在,既要判断项目中的目录是否存在还要判断目录文件是否存在,(存在项目里把文件删掉)

13.目录复制操作存在问题 因为文件夹已存在,但程序未能判断出文件夹的类型(判断是否是OAProject的方法是错误的), 导致重复复制文件夹

14.项目选择无法选择到目录下的项目(封装一个可以取到文件夹目录的方法),遍历所有目录

if(folder){foreach}

15.右键菜单做一个到项目菜单上(已完成)

  


  

