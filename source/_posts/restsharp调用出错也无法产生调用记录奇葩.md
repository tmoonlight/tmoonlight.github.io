---
title: restsharp调用出错也无法产生调用记录奇葩
date: 2017/8/6 10:03:27
tags:
---


restsharp调用出错 也无法产生调用记录 奇葩

        RestRequest req = new RestRequest("GetSalesPriceNew", Method.GET);

            if (cInvCode != null)

                req.AddParameter("cInvCode", cInvCode);

            if (digitalNum != null) req.AddParameter("digitalNum", digitalNum);

            if (cInvDefine2 != null) req.AddParameter("cInvDefine2", cInvDefine2);

            if (cInvDefine3 != null) req.AddParameter("cInvDefine3", cInvDefine3);

  


必须使用这种麻烦的写法
