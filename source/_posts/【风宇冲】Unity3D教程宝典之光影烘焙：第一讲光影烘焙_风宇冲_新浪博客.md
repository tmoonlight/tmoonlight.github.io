---
title: 【风宇冲】Unity3D教程宝典之光影烘焙：第一讲光影烘焙_风宇冲_新浪博客
date: 2015/4/25 6:22:17
tags:
---


引言:

光影烘焙，英文叫Lightmapping 或 light baking。Unity自带了Lightmapping的功能(是Illuminate Labs出的名为Beast的产品)。

本系列教程分为4讲：

第一讲 光影烘焙

第二讲 AreaLight

第三讲 Light Probes

第四讲 脚本控制

其中第三第四讲讲解的是动态物体与烘焙后场景的融合。

  


 ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** ** **光影烘焙**

  


打开方式 **Window** **–** **Lightmapping**

有几点需要注意：

1.所要烘焙物体的mesh 必须要有合适的lightmapping uv。如果不确定的话，就在导入模型设置中勾选 Generate Lightmap UVs

2.任何Mesh Renderer, Skinned Mesh Renderer 或者 Terrain都要标注为static(lightmap static)

  


界面一：Object

点击Bake Scene即开始烘焙

界面2：Bake

烘焙参数的设置

Mode:

(1)Dual Lightmap mode:近景烘焙图(near lightmaps)和远景烘焙图(far lightmaps)都会被烘焙，只有deferred rendering path支持该模式。

(2)Single Lightmap mode:只有远景烘焙图(far lightmaps)会被烘焙，

(3)Directional Lightmap mode：

Use in forward rendering:

一般可以忽略它。针对Dual lightmaps的设定,只在Mode选的是Dual时才会出现。在forward rendering模式下是否激活dual lightmaps,需要自己写对应的Shader。

Quality：

High low 2个选项。决定采集射线的数量，对比界限，以及反锯齿等设置。

Bounces：

Global Illumination模拟中 light bounces的数量。0表示只有直接光照会被计算，如果要加入柔软的，真实的间接光照的话，则至少填1。

Sky Light Color：

模拟从天空从所有方向射来的光照。对室外场景很重要。

(注意：RenderSettings里的Ambient Light也参与烘焙，并且会覆盖Sky Light。所以使用Sky Light的话，则需要关闭Ambient Light,把Ambient调成（0,0,0,0）即可)

Sky Light Intensity：Sky Light的强度。0表示关闭Sky Light.

Bounce Boost：加强Bounced light也就是间接光照的效果.

Bounce Intensity：间接光照的效果强度。

Final Gather Rays:从所有收集的点发射的射线的数量，最高则效果最好。默认为1000

Contrast Threshold: 越高表示光照越平滑同时细节越低

Interpolation：颜色计算方式。0表示线性插值，1表示基于梯度的高级插值。有时1会产生人工光照的失真效果。

Interpolation Points:越多表示光照越平滑同时细节越低。

Ambient Occlusion：环境闭塞（全局闭塞）在烘焙中的量，和光照无关，仅仅基于距离的效果

Lock Atlas：锁上了，贴图就不会变化，没什么用。

Resolution：

通常来说 Resolution设置平均每单位面积对应lightmap的多少色素。该值越高代表单位面积的色素越多即越精细。反之越低越粗糙。默认值为50。如果一个10x10的plane,resolution是50的话，在lightmap里该plane就占用500x500像素尺寸。

当然也可以单调该数值。选中物体后，在lightmapping界面中会有Scale in lightmap的数值，表示的是该物体的烘焙信息在lightmap占的大小，1表示正常尺寸。0表示该物体没有lightmap效果，但会影响其他物体的lightmap运算。2表示正常尺寸的两倍大小。

Padding:在lightmap里,不同物体的烘焙图的间距，单位为图素(texel)

界面3:Maps

烘焙好后的map在这里显示。

Compressed： 是否压缩lightmap

Array Size（0~254）lightmaps array的尺寸

Lightmaps array:即下方的4张贴图，空格会被视为黑图，指数即下方的 0,1对应Mesh Renderer及Terrain里的lightmap index。这个会自动计算。

按下烘焙后，会在右下角有进度条显示。然后你就可以去喝喝咖啡泡泡妹子神马的，放心，它烘焙好了会有Import asset提示并自动切回Unity的。

烘焙好之后，会产生与场景名称相同的文件夹，一般来说会有两张.exr贴图，LightmapFar-0和LightmapNear-0(仅在Dual模式下有near图),当物体距离摄像机小于Shadow Distance时，使用的是 实时灯 + LightmapNear-0。当大于是使用的是LightmapFar-0，此时灯光对物体不起作用。

    (注意：Shadow Distance的设置

(1)如果是编辑器里运行：Lightmap display设置界面里的Shadow Distance,如下方左图

(2)如果实际运行: Quality设置下的Shadow Distance，如下方右图

  


Lightmap Display界面里的其他设置:

Use Lightmaps：是否使用lightmaps,不勾选的话就没有任何烘焙的效果了。

Show Resolution：当勾选Show Resolution时，会有下图所示类似国际象棋黑白相间的格子，一个格子对应lightmap里所占用的一个texel

Show Probes:是否显示light probes,后面会讲到。

Show Cells:light probes分割形成的空间的数量。

![【风宇冲】Unity3D教程宝典之光影烘焙：第一讲光影烘焙](put-hash-type-here-for-http://s1.sinaimg.cn/bmiddle/47113292td520df05f900)

当物体烘焙好之后，不管是复制物体还是把物体做成prefab,都会继承烘焙的效果。但是如果缩放的话，烘焙好的光容易出现问题。

  


被烘焙的物体并不要求特定的材质或者Shader。 只要物体的Shader使用的是Surface Shader,那么就可以被烘焙。

  


注意：一个场景只能烘焙一次，如果多次烘焙，效果是最后一次烘焙的效果。

  


最后推荐ShadowGun里的烘焙配置，如下图，供大家参考。烘焙时间很短，效果也很好。

极力建议大家搜下ShadowGun的demo来参考学习。里面无论是场景布局，光影分配，Godrays运用，及烘焙等等都是极高的水准。

  


[](http://www.bj.cyberpolice.cn/index.jsp)
