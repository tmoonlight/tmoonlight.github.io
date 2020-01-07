---
title: post参数接口传值规范传空值还是
date: 2017/8/23 3:07:08
tags:
---


post 参数 接口传值规范 传空值还是""? 

  


  if (pCusName != null)

            {

                req.AddParameter("CusName1", pCusName);

            }

  


需要改进

  


1.时间戳无法插入,时间戳字段需要在代码生成器中增加例外规则 

guid和id字段需要区分 dapperextension

  


2.非空字段如果 传null 会导致service因参数类型问题无法接收到值

  


3.代码生成器模板尚未更新

  


4.
