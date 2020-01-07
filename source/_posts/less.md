---
title: less
date: 2018/1/28 9:07:37
tags:
---


# 

# 概述

> Less （Leaner Style Sheets 的缩写） 是一门向后兼容的 CSS 扩展语言。这里呈现的是 Less 的官方文档（中文版），包含了 Less 语言以及利用 JavaScript 开发的用于将 Less 样式转换成 CSS 样式的 Less.js 工具。

因为 Less 和 CSS 非常像，学习很容易。而且 Less 仅对 CSS 语言增加了少许方便的扩展，这就是 Less 如此易学的原因之一。

  * For detailed documentation on Less language features, see [Features](https://less.bootcss.com/features/)
  * For a list of Less Built-in functions, see [Functions](https://less.bootcss.com/functions/)
  * For detailed usage instructions, see [Using Less.js](https://less.bootcss.com/usage/)
  * For third-party tools for Less, see [Tools](https://less.bootcss.com/tools/)



What does Less add to CSS? Here's a quick overview of features.

# 变量（Variables）

These are pretty self-explanatory:

@nice-blue: #5B83AD;

@light-blue: @nice-blue \+ #111;

  


#header {

color: @light-blue;

}

编译为：

#header {

color: #6c94be;

}

[Learn More About Variables](https://less.bootcss.com/features/#variables-feature)

# 混合（Mixins）

Mixins are a way of including ("mixing in") a bunch of properties from one rule-set into another rule-set. So say we have the following class:

.bordered {

border-top: dotted 1px black;

border-bottom: solid 2px black;

}

And we want to use these properties inside other rule-sets. Well, we just have to drop in the name of the class where we want the properties, like so:

#menu a {

color: #111;

.bordered;

}

  


.post a {

color: red;

.bordered;

}

The properties of the .bordered class will now appear in both #menu a and .post a. (Note that you can also use #ids as mixins.)

[Learn More About Mixins](https://less.bootcss.com/features/#mixins-feature)

# 嵌套（Nesting）

Less gives you the ability to use nesting instead of, or in combination with cascading. Let's say we have the following CSS:

#header {

color: black;

}

#header .navigation {

font-size: 12px;

}

#header .logo {

width: 300px;

}

In Less, we can also write it this way:

#header {

color: black;

.navigation {

font-size: 12px;

}

.logo {

width: 300px;

}

}

The resulting code is more concise, and mimics the structure of your HTML.

You can also bundle pseudo-selectors with your mixins using this method. Here's the classic clearfix hack, rewritten as a mixin (& represents the current selector parent):

.clearfix {

display: block;

zoom: 1;

  


&:after {

content: " ";

display: block;

font-size: 0;

height: 0;

clear: both;

visibility: hidden;

}

}

[Learn More About Parent Selectors](https://less.bootcss.com/features/#parent-selectors-feature)

## Nested At-Rules and Bubbling

At-rules such as @media or @supports can be nested in the same way as selectors. The at-rule is placed on top and relative order against other elements inside the same ruleset remains unchanged. This is called bubbling.

.component {

width: 300px;

@media (min-width: 768px) {

width: 600px;

@media (min-resolution: 192dpi) {

background-image: url(/img/retina2x.png);

}

}

@media (min-width: 1280px) {

width: 800px;

}

}

  


编译为：

.component {

width: 300px;

}

@media (min-width: 768px) {

.component {

width: 600px;

}

}

@media (min-width: 768px) and (min-resolution: 192dpi) {

.component {

background-image: url(/img/retina2x.png);

}

}

@media (min-width: 1280px) {

.component {

width: 800px;

}

}

# 运算（Operations）

Arithmetical operations +, -, *, / can operate on any number, color or variable. If it is possible, mathematical operations take units into account and convert numbers before adding, subtracting or comparing them. The result has leftmost explicitly stated unit type. If the conversion is impossible or not meaningful, units are ignored. Example of impossible conversion: px to cm or rad to %.

// numbers are converted into the same units@conversion-1: 5cm \+ 10mm; // result is 6cm@conversion-2: 2 \- 3cm \- 5mm; // result is -1.5cm

  


// conversion is impossible@incompatible-units: 2 \+ 5px \- 3cm; // result is 4px

  


// example with variables@base: 5%;

@filler: @base * 2; // result is 10%@other: @base \+ @filler; // result is 15%

Multiplication and division do not convert numbers. It would not be meaningful in most cases - a length multiplied by a length gives an area and css does not support specifying areas. Less will operate on numbers as they are and assign explicitly stated unit type to the result.

@base: 2cm * 3mm; // result is 6cm

You can also do arithemtic on colors:

@color: #224488 / 2; //results in #112244background-color: #112244 + #111; // result is #223355

However, you may find Less's [Color Functions](https://less.bootcss.com/functions/#color-operations) more useful.

## calc() 特例

Released [v3.0.0](https://github.com/less/less.js/blob/master/CHANGELOG.md)

为了与 CSS 保持兼容，calc() 并不对数学表达式进行计算，但是在嵌套函数中会计算变量和数学公式的值。

@var: 50vh/2;

width: calc(50% \+ (@var \- 20px)); // result is calc(50% + (25vh - 20px))

# Escaping

Escaping allows you to use any arbitrary string as property or variable value. Anything inside ~"anything" or ~'anything' is used as is with no changes except [interpolation](https://less.bootcss.com/features/#variables-feature-variable-interpolation).

@min768: ~"(min-width: 768px)";

.element {

@media @min768 {

font-size: 1.2rem;

}

}

编译为：

@media (min-width: 768px) {

.element {

font-size: 1.2rem;

}

}

# 函数（Functions）

Less 内置了多种函数用于转换颜色、处理字符串、算术运算等。这些函数在[函数手册](https://less.bootcss.com/functions/)中有详细介绍。

函数的用法非常简单。下面这个例子将介绍如何利用 percentage 函数将 0.5 转换为 50%，将颜色饱和度增加 5%，以及颜色亮度降低 25% 并且色相值增加 8 等用法：

@base: #f04615;

@width: 0.5;

  


.class {

width: percentage(@width); // returns `50%`

color: saturate(@base, 5%);

background-color: spin(lighten(@base, 25%), 8);

}

[参见：函数手册](https://less.bootcss.com/functions/)

# Namespaces and Accessors

(Not to be confused with [CSS](http://www.w3.org/TR/css3-namespace/)[ ](http://www.w3.org/TR/css3-namespace/)[@namespace](http://www.w3.org/TR/css3-namespace/) or [namespace selectors](http://www.w3.org/TR/css3-selectors/#typenmsp)).

Sometimes, you may want to group your mixins, for organizational purposes, or just to offer some encapsulation. You can do this pretty intuitively in Less.Say you want to bundle some mixins and variables under #bundle, for later reuse or distributing:

#bundle() {

.button {

display: block;

border: 1px solid black;

background-color: grey;

&:hover {

background-color: white

}

}

.tab { ... }

.citation { ... }

}

Now if we want to mixin the .button class in our #header a, we can do:

#header a {

color: orange;

#bundle > .button; // can also be written as #bundle.button

}

Note: append () to your namespace if you don't want it to appear in your CSS output i.e. #bundle .tab.

Also note that variables declared within a namespace will be scoped to that namespace only and will not be available outside of the scope via the same syntax that you would use to reference a mixin (#Namespace > .mixin-name). So, for example, you can't do the following: #Namespace > @this-will-not-work.

# 作用域（Scope）

Scope in Less is very similar to that of programming languages. Variables and mixins are first looked for locally, and if they aren't found, the compiler will look in the parent scope, and so on.

@var: red;

  


#page {

@var: white;

#header {

color: @var; // white

}

}

Mixin and variable definitions do not have to be placed before a line where they are referenced. So the following Less code is identical to the previous example:

@var: red;

  


#page {

#header {

color: @var; // white

}

@var: white;

}

[参见：懒加载](https://less.bootcss.com/features/#variables-feature-lazy-loading)

# 注释（Comments）

块注释和行注释都可以使用：

/* 一个块注释

* style comment! */@var: red;

  


// 这一行被注释掉了！@var: white;

# 导入（Importing）

“导入”的工作方式和你预期的一样。你可以导入一个 .less 文件，此文件中的所有变量就可以全部使用了。如果导入的文件是 .less 扩展名，则可以将扩展名省略掉：

@import "library"; // library.less@import "typo.css";

  

