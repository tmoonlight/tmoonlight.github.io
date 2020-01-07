---
title: vuerouter传值
date: 2016/9/19 19:44:00
tags:
---


标签：
    
    
    <li v-for=" el in hotLins" >
        <router-link :to="{path:‘details‘,query: {id:el.tog_line_id}}">
            <img :src="el.image_list[0]">
            <h3>{{el.tourism_name}} {{el.tog_line_id}}</h3>
            <p>{{el.address}}</p>
        </router-link></li>

在组件中，需要传动态参数时，可以如上例子 
    
    
    <router-link :to="{path:‘details‘,query: {id:el.tog_line_id}}">

query中的参数id就是要传的参数，在组件中获取的方法为：
    
    
    created: function() {
        var id = this.$route.query.id;
        this.getData(id);
     },

如上述，this.$route.query.id就可以获取该参数，也可以通过，this.$root.id = id;传给父组件，父组件中通过
    
    
     　　// 根组件构造器
        var vm = Vue.extend({
          router: router,
          data: function() {
            return {
              id: ‘11484‘ //城会玩明细id        }
          }
        });

定义data中的id，然后子组件中用props传递参数
    
    
    props: {
            id: {
              type: String,
              required: true
            }
          },

router-view中，传递该参数：
    
    
    <router-view :id="id" :order-info="orderInfo"></router-view>

注意orderInfo时，在这边用的是:order-info。

  


  


![](https://segmentfault.com/img/bVODol?w=402&h=121)

  


![](https://segmentfault.com/img/bVODoq?w=518&h=181)

[vue-router2.0 组件之间传参及获取动态参数](http://www.mamicode.com/info-detail-1575683.html "vue-router2.0 组件之间传参及获取动态参数,mamicode.com")
