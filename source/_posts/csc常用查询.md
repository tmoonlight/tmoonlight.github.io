---
title: csc常用查询
date: 2015/10/21 21:10:46
tags:
---


support文件 zed 中兴通讯学院反馈问题

select * from new_support.SYN_TSM_ATTACHMENT@ecccsc where sownerrecid = '30461188'

  


常见故障表：

license表

政企工单request

政企备件request

license状态

<option value="1">待提交</option>

<option value="2">待审核</option>

<option value="3">待审批</option>

<option value="4">待制作</option>

<option value="5">已发布</option>

<option value="6">已作废</option>

<option value="7">制作失败</option>

软件许可证大权限用户 00006849

select t1.request_type,t1.inner_status_id,t1.createdate, t1.rowid, t2.lcs_id, t2.license_id, t1.request_no

  from CC_SL_REQUEST t1, cc_sl_made_license t2 where length(t2.license_id)=30
