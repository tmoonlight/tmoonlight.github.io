---
title: vue中webpack相对路径解决方法
date: 2016/9/7 22:12:37
tags:
---

    
    
    data () {
        return {
            img: require('path/to/your/source')
        }
    }
    
    然后在template中
    
    <img :src="img" />
    

如果使用的是背景图的方式，那么

`可以这样  
  
data () {  
    return {  
        img: require('path/to/your/source')  
    }  
}  
  
<div :style="{backgroundImage: 'url(' + img + ')'}"></div>  
  
或者直接在css中定义  
  
background-image: url('path/to/your/source');`
