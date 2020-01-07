---
title: TrailRenderer拖尾渲染器
date: 2014/10/21 23:24:32
tags:
---


The **Trail Renderer** is used to make trails behind objects in the scene as they move about.

  


拖尾渲染器（Trail Renderer）用于制作跟在场景中的物体后面的拖尾效果来代表它们在到处移动。

  


 _The Trail Renderer Inspector_ _拖尾渲染器检视面板_

  


 **Properties 属性**

  *  **Materials** **材质**


  * An array of **materials** used for rendering the trail. Particle shaders work the best for trails.
  * 用于渲染拖尾的材质数组。对于拖尾效果粒子着色器工作的最好。


  *  **Size** **大小**


  * The total number of elements in the **Material** array.
  * 在材质数组中总共有多少元素。


  *  **Element 0** **元素**


  * Reference to the Material used to render the trail. The total number of elements is determined by the **Size** property.
  * 用于渲染拖尾的材质的引用。总共的元素个数由Size参数指定。


  *  **Time** **时间**


  * Length of the trail, measured in seconds.  
拖尾的长度，以秒为单位


  *  **Start Width** **开始宽度**


  * Width of the trail at the object's position.  
物体位置的拖尾宽度


  *  **End Width** **结束宽度**


  * Width of the trail at the end.  
结束位置的拖尾宽度


  *  **Colors** **颜色**


  * Array of colors to use over the length of the trail. You can also set alpha transparency with the colors.  
使用拖尾长度渐变的颜色数组，你也可以在这些颜色中使用alpha透明


  *  **Color0 to Color4**  
The trail's colors, initial to final.  
拖尾的颜色，从初始到结束


  *  **Min Vertex Distance** **最小顶点距离**


  * The minimum distance between anchor points of the trail.  
拖尾锚点之间的最小距离


  *  **AutoDestruct** **自动销毁**


  * Enable this to make the object be destroyed when the object has been idle for **Time** seconds.
  * 将这一项设置为允许来使物体在静止Time秒后被销毁



 **Details 细节**

  


The Trail Renderer is great for a trail behind a projectile, or contrails from the tip of a plane's wings. It is good when trying to add a general feeling of speed.

  


拖尾渲染器（Trail Renderer）用于炮弹后面的拖尾，或是飞机机翼尖端产生的凝结尾的效果非常棒。当尝试添加一个速度的通用感觉时也很好。

  


When using a Trail Renderer, no other renderers on the **GameObject** are used. It is best to create an empty GameObject, and attach a Trail Renderer as the only renderer. You can then parent the Trail Renderer to whatever object you would like it to follow.

  


当使用一个拖尾渲染器（Trail Renderer）时，不能在游戏物体（GameObject）上使用其他的渲染器。最好是创建一个空的游戏物体（GameObject），并附加一个拖尾渲染器作为唯一的渲染器。然后你可以将你想要跟随的物体设置为拖尾渲染器的父物体。

  


 **Materials 材质**

  


Trail Renderers should use a material that has a Particle **Shader**. The **Texture** used for the Material should be of square dimensions (e.g. 256x256 or 512x512).

  


拖尾渲染器（Trail Renderer）将使用一个包含粒子着色器（Paricle Shader）的材质。材质使用的贴图必需是平方尺寸（例如：256×256或512×512）。

  


 **Trail Width 拖尾宽度**

  


By setting the Trail's **Start** and **End Width** , along with the **Time** property, you can tune the way it is displayed and behaves. For example, you could create the wake behind a boat by setting the **Start Width** to 1, and the **End Width** to 2. These values would probably need to be fine-tuned for your game.

  


通过设置拖尾的开始和结束宽度（Width），配合时间（Time）属性，你可以调节它显示和表现的方式。例如，你可以创建一个船后面的浪花（wake? Or wave?），设置开始宽度为1，结束宽度为2。这些值大概需要为你的游戏进行适当的调节。

  


 **Trail Colors 拖尾颜色**

  


You can cycle your trail through 5 different color/opacity combinations. Using colors could make a bright green plasma trail gradually dim down to a dull grey dissipation, or cycle through the other colors of the rainbow. If you don't want to change the color, it can be very effective to change only the opacity of each color to make your trail fade in and out at the head and/or tail.

  


你可以通过5种不同的颜色/透明度组合循环变化你的拖尾。使用颜色能使一个亮绿色的等离子体拖尾渐渐变暗到一个灰色耗散结构，或是使彩虹循环变为其他颜色。如果你不想改变颜色，它可以非常有效的仅仅改变每一个颜色的透明度来使你的拖尾在头部和/或尾部之间进行渐变。

  


 **Min Vertex Distance 最小顶点距离**

  


The **Min Vertex Distance** value determines how far the object that contains the trail must travel before a segment of the trail is solidified. Low values like 0.1 will create trail segments more often, creating smoother trails. Higher values like 1.5 will create segments that are more jagged in appearance. There is a slight performance trade off when using lower values/smoother trails, so try to use the largest possible value to achieve the effect you are trying to create.

  


最小顶点距离（Min Vertex Distance）决定了包含拖尾的物体在一个拖尾的段实体化之前必需经过的距离。较小的值如0.1将更频繁的创建拖尾段，生成更平滑的拖尾。较大的值如1.5将创建显示有更多锯齿的段。当使用较低的值/更平滑的拖尾时有一点点性能损失，所以应该尝试使用尽可能大的值来达到你想要创建的效果。

  


 **Hints 提示**

  * Use Particle Materials with the Trail Renderer.  
在拖尾渲染器（TrailRender）中使用粒子材质（Particle Material）。


  * Trail Renderers must be laid out over a sequence of frames, they can't appear instantaneously.  
拖尾渲染器（TrailRender）必需在一些连续帧之后显现，它不能突然出现。


  * Trail Renderers rotate to display the face toward the camera, similar to other **Particle Systems**.
  * 拖尾渲染器（TrailRender）将旋转为面向摄像机显示，就像其他的粒子系统（Particle Systems）。



页面最后更新: 2011-12-28

  




