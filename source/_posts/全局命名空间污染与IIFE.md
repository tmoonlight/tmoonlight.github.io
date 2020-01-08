---
title: 全局命名空间污染与IIFE
date: 2018/2/13 14:46:53
tags:
categories: 前端
---

总是将代码包裹成一个 IIFE(Immediately-Invoked Function Expression)，用以创建独立隔绝的定义域。这一举措可防止全局命名空间被污染。

IIFE 还可确保你的代码不会轻易被其它全局命名空间里的代码所修改（i.e. 第三方库，window 引用，被覆盖的未定义的关键字等等）。

不推荐

```
var x = 10,

y = 100;

  


// Declaring variables in the global scope is resulting in global scope pollution. All variables declared like this// will be stored in the window object. This is very unclean and needs to be avoided.

console.log(window.x + ' ' + window.y);
```

推荐

```
// We declare a IIFE and pass parameters into the function that we will use from the global space

(function(log, w, undefined){

'use strict';

  


var x = 10,

y = 100;

  


// Will output 'true true'

log((w.x === undefined) + ' ' \+ (w.y === undefined));

  


}(window.console.log, window));
```



* * *

### IIFE（立即执行的函数表达式）

无论何时，想要创建一个新的封闭的定义域，那就用 IIFE。它不仅避免了干扰，也使得内存在执行完后立即释放。

所有脚本文件建议都从 IIFE 开始。

立即执行的函数表达式的执行括号应该写在外包括号内。虽然写在内还是写在外都是有效的，但写在内使得整个表达式看起来更像一个整体，因此推荐这么做。

不推荐

```
(function(){})();
```

推荐

```
(function(){}());

so，用下列写法来格式化你的 IIFE 代码：

(function(){

'use strict';

  


// Code goes here

  


}());
```

如果你想引用全局变量或者是外层 IIFE 的变量，可以通过下列方式传参：

```
(function($, w, d){

'use strict';

  


$(function() {

w.alert(d.querySelectorAll('div').length);

});

}(jQuery, window, document));
```



* * *

全文阅读：[前端编码风格规范之 JavaScript 规范](http://roshanca.com/2014/web-develop-styleguide-javascript/)

  

