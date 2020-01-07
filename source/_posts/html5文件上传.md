---
title: html5文件上传
date: 2015/6/29 18:14:47
tags:
---


Html5终于解决了上传文件的同时显示文件上传进度的老问题。现在大部分的网站用Flash去实现这一功能，还有一些网站继续采用Html <form>with enctype=multipart/form-data，但是需要修改服务器端可用才能显示给用户文件上传的进度。本质上你需要做的工作是在服务器端接收一个文件时，你发送给它一个字节流，所以你需要知道你已经接收到多少字节并以某种方式传达这些信息给客户端浏览器，在这个过程一直在不断的进行文件的上传。这种方式运行的非常好，不像Flash上传那这样充满了问题(特别是处理大文件上传的时候)，然而这种方法是相当复杂的并且听起来不容易理解，因为你本质上是接管了整个服务器端的处理（获取字节流的时候）同时包括了在服务器端实现multipart/form-data协议，伴随一系列的其他事情。

# 使用Html5 上传文件

XMLHttpRequest 在Html5 规范中已经有全新的变化，规定了[XMLHttpRequest Level 2](http://www.w3.org/TR/XMLHttpRequest2/)规范（目前最新版本）包含下列新的特性：

  1. 处理字节流，例如作为上传或者下载的File，Blob，FormData对象

  2. 上传或者下载中的进度事件

  3. 跨站点请求

  4. 允许创建匿名请求

  5. 可以设置请求超时




在这篇文章中我们将能够更清楚的看到#1和#2两个特性。通常，上传文件用XMLHttpRequest并且提供上传进度信息给最终的用户，需要注意的是这种方式解决了不需要服务器端做任何改变，至少是目前处理multipart/form-data协议。所以服务器端的处理逻辑保留不变，这使得开发者适应这种技术相当容易。

![](http://static.oschina.net/uploads/img/201406/27110823_2bWn.png) 图1：文件上传画面-准备上传 ![](http://static.oschina.net/uploads/img/201406/27110824_YLVb.png) 图2：显示上传完成画面

注意：上面的图片中，信息提示区域是提供给用户的：

  1. 当前选中文件的信息

文件名

文件大小

文件类型

  2. 上传完成多少的百分比进度条

  3. 上传速度或者上传带宽

  4. 距离上传完成大概还有多长时间

  5. 已上传文件大小

  6. 服务器端的响应




上面第6项或许看起来不重要，但事实上是相当重要的。因为我们用XMLHttpRequest，上传发生在后台，页面没有发生跳转等任何变化，所以对于你用它处理其他一些事情来说是一个非常好的特性。

# Html5 Progress Event

对于Html5 Progress Events规范，Html5 Progess Events提供了下列与本次讨论相关的信息

  1. total - 总的字节数

  2. loaded - 到目前为止上传的字节数

  3. lengthComputable - 可计算的已上传字节




请注意到我们需要用两个信息去计算要显示给用户的其他所有信息。要计算出来其他的信息通过上面我们得到信息是相当容易的，但是那需要一些额外的代码并且创建一个定时器。

# Html5 Progress Event 应该是什么

考虑到有一部分人想更好的提供给用户所有的信息，所以Html5 Progress Event应该更好的满足需要，因为它给浏览器供应商提供这些额外信息是相当简单的，所以建议progress event应该修改成如下：

  1. total - 总的字节数

  2. loaded - 到目前为止上传的字节数

  3. lengthComputable - 可计算的已上传字节

  4. transferSpeed long类型

  5. timeRemaining JavaScript 日期对象




# Html5 上传 用XMLHttpRequest

## 浏览器支持情况

支持这一特性的浏览器最低版本

  1. Firefox 4.0 beta 6

  2. Chrome 6

  3. Safari 5.02




IE 9 Beta and Opera 10.62 不支持这一特性

# 简单的示例

下面是一个完整的Html页面包含了实现文件上传并带有进度提示的JavaScript代码，只是实现了基本的功能，感兴趣的可以自己做扩展。 需要吧上传接口修改成自己服务的。 

 

![](https://common.cnblogs.com/images/copycode.gif)

<!DOCTYPE html>

<html>

<head>

<title>Upload Files using XMLHttpRequest - Minimal</title>

<script type="text/javascript">

function fileSelected() {

var file = document.getElementById('fileToUpload').files[0];

if (file) {

var fileSize = 0;

if (file.size > 1024 * 1024)

fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';

else

fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';

document.getElementById('fileName').innerHTML = 'Name: ' + file.name;

document.getElementById('fileSize').innerHTML = 'Size: ' + fileSize;

document.getElementById('fileType').innerHTML = 'Type: ' + file.type;

}

}

function uploadFile() {

var fd = new FormData();

fd.append("fileToUpload", document.getElementById('fileToUpload').files[0]);

var xhr = new XMLHttpRequest();

xhr.upload.addEventListener("progress", uploadProgress, false);

xhr.addEventListener("load", uploadComplete, false);

xhr.addEventListener("error", uploadFailed, false);

xhr.addEventListener("abort", uploadCanceled, false);

xhr.open("POST", "upload.do");//修改成自己的接口

xhr.send(fd);

}

function uploadProgress(evt) {

if (evt.lengthComputable) {

var percentComplete = Math.round(evt.loaded * 100 / evt.total);

document.getElementById('progressNumber').innerHTML = percentComplete.toString() + '%';

}

else {

document.getElementById('progressNumber').innerHTML = 'unable to compute';

}

}

function uploadComplete(evt) {

/* 服务器端返回响应时候触发event事件*/

alert(evt.target.responseText);

}

function uploadFailed(evt) {

alert("There was an error attempting to upload the file.");

}

function uploadCanceled(evt) {

alert("The upload has been canceled by the user or the browser dropped the connection.");

}

</script>

</head>

<body>

<form id="form1" enctype="multipart/form-data" method="post" action="Upload.aspx">

<div class="row">

<label for="fileToUpload">Select a File to Upload</label><br />

<input type="file" name="fileToUpload" id="fileToUpload" onchange="fileSelected();"/>

</div>

<div id="fileName"></div>

<div id="fileSize"></div>

<div id="fileType"></div>

<div class="row">

<input type="button" onclick="uploadFile()" value="Upload" />

</div>

<div id="progressNumber"></div>

</form>

</body>

</html>

  


  


  

