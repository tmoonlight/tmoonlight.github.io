---
title: C#转JAVA注意事项-CSDN博客
date: 2017/9/18 17:06:01
tags:
---


# C#转 JAVA 注意事项

转载  2016年02月20日 22:05:39


.

J2SE：Java 2 Platform，Standard Edittion(Java 2 平台，标准版本)，包含java核心类和GUI类

J2EE：Java 2 Platform，Enterprise Edition(Java 2 平台，企业版本)，包含开发基于web的应用程序的类和接口，如Servlet、Java Server Pages以及Enterprise JavaBeans等。

J2ME：Java 2 Plateform，Micro Edition(Java 2 平台，微观版)，包含对传呼机，移动电话，掌上电脑，汽车导航系统或其他无线设备等产品提供优化的运行时环境。

 

JRE(Java Runtime Environment)：Java虚拟机和API。

java.lang:这个是系统的基础类，比如String等都是这里面的，这个包是唯一一个可以不用引入(import)就可以使用的包。

java.io:这里面是所有输入输出有关的类，比如文件操作等。

java.nio:为了完善io包中的功能，提高io包中性能而写的一个新包 ，例如NIO非堵塞应用。

java.net:这里面是与网络有关的类，比如URL，URLConnection等。

java.util:这个是系统辅助类，特别是集合类Collection，List，Map等。

java.sql:这个是数据库操作的类，Connection， Statement，ResultSet等。

javax.servlet:这个是JSP，Servlet等使用到的类。

java.awt: 包含用于创建用户界面和绘制图形图像的所有类。

java.awt.event:提供处理由 AWT 组件所激发的各类事件的接口和类。

java.applet:用于实现运行于Internet浏览器中的Java Applet的工具类库。

 

Sun公司：Applet小应用程序是采用Java编程语言编写的程序，该程序可以包含在HTML（标准通用标记语言的一个应用）页中，与在页中包含图像的方式大致相同。

 

Microsoft公司：ActiveX 是一个开放的集成平台，为开发人员、用户和 Web生产商提供了一个快速而简便的在 Internet 和 Intranet 创建程序集成和内容的方法。 使用 ActiveX, 可轻松方便的在 Web页中插入 多媒体效果、 交互式对象、以及复杂程序，创建用户体验相当的高质量多媒体CD-ROM 。

 

Java编译器javac.exe，java解释器java.exe。

 

Java程序有3种执行环境：

a.能够单独运行的程序，称为Java Application(Java应用程序)

b.在Internet浏览器中运行的程序，称为Java Applet(Java小应用程序)。

c.在Web服务器端运行的程序，称为Java Servlet。Servlet实际上是运行在Web服务器上的应用程序，它与协议和平台无关。

 

以下内容比较Java和C#的差别：

1、 Java 使用 import导入命名空间。同C#的using关键字。

 

2、 extends是用于继承类的关键字，implements是用于实现接口的关键字。C#都是用冒号。

    

3、 I、class类的访问控制符：public、protected、private、friendly(默认)

默认可以称为friendly但是，java语言中是没有friendly这个修饰符的，这样称呼应该是来源于c++。默认的访问权限是包级访问权限。即如果写了一个类没有写访问权限修饰符，那么就是默认的访问权限，同一个包下的类都可以访问到，即使可以实例化该类（当然如果这个类不具有实例化的能力除外，比如该类没有提供public的构造函数）。

 

public：可以供所有的类访问。

说明：

a、每个编译单元（类文件）都仅能有一个public class 。

b、public class的名称（包含大小写）必须和其类文件同名。

c、一个类文件(*.java)中可以不存在public class。这种形式的存在的场景：如果我们在某个包内撰写一个class，仅仅是为了配合同包内的(！)其他类工作。

d、class不可以是private和protected。

e、如果不希望那个任何产生某个class的对象，可以将该类得所有构造函数设置成private。但是即使这样也可以生成该类的对象，就是class的 static的成员（属性和方法）可以办到。

protected：修饰的类为保护类，可以同包类和异包类访问。

private：修饰的类为私有类，外界无法访问。

 

II、class的非访问控制符：abstract，final(修饰的类又称最终类)，

C#的密封类使用sealed关键字。

 

java 中的静态类能否实例化，参见如下代码：

public class A { public class B{}} 实例化B：A a = new A(); B b = a.new B();

public class A { public static class B{}} 实例化B：B b = new A.B();

 

4、 抽象类中不一定包含抽象方法，但是一旦某个类中说明了抽象方法，该类必须说明为抽象类。同C#一样。

5、 C#接口方法默认是public的，声明中不需要再使用public 修饰符，否则编译会报错，但是java默认也是public的，但是即使添加修饰符也不报错。

 

6、 类成员变量的访问权限：

访问控制符：public、protected、private

public：紧接public的属性任何类都可以访问到。但是从类的封装性上来考虑将一个类的属性定义成public一般很少使用， 在定义静态常量的时候通畅会这样定义。

如：public static final int PAGE_SIZE=10;

private：只有类本身内部的方法可以访问类的private属性，当然内部类也可以访问其外部类的private成员的。（属性和方法）

默认（friendly）：包级可见，同一个包内的类可以访问到这个属性但是从类的封装性特性来说很少这样使用类的属性的。

protected：关键字所处理的是所谓“继承”的观念。对于同一包的其他类，protected＝默认，对于不同包的类，如果存在继承关系， 而baseClass存在protected属性，则可以被其自继承，而不同包的其他类则不能访问类的protected属性。

非访问控制符：static和final。

使用关键字final声明的成员变量成为最终变量，更确切地说是常量。同C#的const。

用final修饰符声明常量时，需要说明常量的数据类型并要初始化具体的数值。另外因为所有类对象的常量数据成员数值都一样，为了节省内存空间，常量通常声明为static。

C#注意const和static readonly的区别：const的值是在编译期间确定的，因此只能在声明时通过常量表达式指定其值。而static readonly是在运行时计算出其值的，所以还可以通过静态构造函数来赋值。

 

7、 java中没有静态构造函数，java中没有属性也没有特性，java中没有委托。

 

8、 类成员方法的访问权限：

访问控制符public、protected和private

非访问控制符static、abstract、final、native、synchronized。

final:最终方法使得子类不能重新定义与父类同名的方法。

native:因为native方法的方法体是用其他语言在java程序外部实现的，所以native方法没有方法体，用一个分号结束。

例：public static native void nativeMessage();实际上该方法是用其他语言编写的。类似C#的是用DllImport：

[DllImport("user32.dll", CharSet=CharSet.Auto)]

public static extern IntPtr CallWindowProc(IntPtr wndProc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

synchronized：该方法主要用于多线程程序中的线程协调和同步。

 

9、 Java的数组能够按照以下方式定义：

int[] list = new int[6];

int list[] = new int[6];

C#只能这样定义：int[] list = new int[6];

 

10、关于异常处理：如果一个java方法需要抛出异常，则必须在方法声明中使用throws关键字。其语法形式为：

return-type methodName(parameterList) throws exceptionList{ … }

Java抛出的异常会沿着方法调用的层次结构一直传递到具备处理能力的调用方法，最高层次到main()方法为止。如果异常传递到main()方法，而main()不具备处理能力，也没有通过throws声明抛出该异常，将可能出现编译错误，或者导致程序异常终止。

 

C#则不用在方法声明中使用任何关键字，抛出后如果调用的方法有异常捕获则进行捕获，否则向上继续抛出。

 

11、关于将其他类型(非字符串类型)转化为字符串：

java使用如下静态方法：public static String valueOf(type variable)

C#则直接使用ToString()方式。

 

12、关于调用父类方法和构造函数：java使用super，C#使用base关键字。

 

13、java中接口的定义可以使用public和 abstract修饰符，如：

interface Pet {  public abstract void speak();  }

C#中不能这么定义，只能按下列方式：interface Pet{  void speak();  }

 

14、关于Object类：

java包括7个public类型的，2个protected类型的。

object类中的7个public类型的方法分别是toString()、equals()、getClass()、

hashCode()、notify()、nofifyAll()和wait()方法。

2个protected方法clone()、finalize()方法。

需要注意的是：getClass()、notify()、notifyAll()、wait()方法在Object类中已经定义成为final类型，即这4个方法都是最终方法，所以在用户自己定义的类中不能重写这4个方法。

C#中的object类:包含两个静态方法：Equals，ReferenceEquals，六个实例方法，其中有4个public类型的方法分别是ToString()、Equals()、GetType()、GetHashCode()方法。2个protected方法MemberwiseClone()、Finalize()方法。

 

15、java中的多线程使用：

a、继承Thread类:extending the class Thread.

b、实现Runnable接口:implementing the interface Runnable.

     C#中多线程的使用：

Thread thread = new Thread(new ThreadStart(MethodName));

thread.Start();

 

16、java中线程同步使用synchronized：

     以下方法等同：

(1)、public synchronized void methodName(){ }

(2)、public void methodName(){ synchronized (this){ } }       

     C#中线程同步使用lock关键字。

 

17、java中的线程通信使用 wait()、notify()和notifyAll()方法，这三个方法用于协调多个线程对共享数据的存取，所以必须在synchronized语句块内使用。

C#中的线程通信使用Wait()、Pulse()、PulseAll()方法，同java，这三个方法必须从同步的代码块内调用。

 

18、java中Thread类的yield()方法可以用来使具有相同优先级的线程获取执行的机会。如果具有相同优先级的线程是可运行的，yield()将把调用线程放到可运行队列并使另一个线程运行。如果没有相同优先级的可运行进程，yield()什么都不做。注意：sleep()调用会给较低优先级线程一个运行机会。yield()方法只会给相同优先级线程一个执行的机会。

    C#没有此方法。

    java中的join()方法同C#的Join()方法。

  
  


19、java的图形界面：

    (1)、TextField(单行文本)、TextArea(多行文本)继承自TextComponent，又继承于Component，响应事件ActionListener。

   (2)、Button直接派生至Component，按钮的事件相应ActionListener。

   (3)、Checkbox直接派生至Component，将多个Checkbox放入CheckboxGroup中表现为单选的行为，相应事件为ItemListener。

   (4)、Container是一个容器类，派生至Component

   (5)、面板Panel是java中最通用的容器，它直接继承于Container

   (6)、画布Canvas类是一个没有特殊功能的组件，继承于Component

   (7)、窗口Window是个通用容器类，派生至Container，与Panel不同的是，Window有一个独立于Web浏览器或appletviewer窗口的窗口。Window的常用子类：Frame、Dialog、FileDialog。

   (8)、Frame是一个带有标题和缩放角的窗口，有自己的外边框和标题，创建Frame时可以指定其窗口标题。窗口事件WindowListener。

   (9)、菜单条MenuBar 是一种水平菜单，它只能被添加到Frame对象中，作为整个菜单树的根基。其主要功能是被用来放置Menu组件。

        菜单Menu类的主要功能是被用来放置MenuItem、MenuShortCut等组件，它可以被添加到MenuBar中或其他Menu中。

        菜单项MenuItem是菜单树的文本叶子，菜单项通常被添加到一个菜单中。对于MenuItem对象可以添加ActionListener，使其能够完成相应的操作。

        MenuBar和Menu都没有必要注册监听器，只需要对MenuItem添加监听器ActionListener。

   (10)、布局设计：FlowLayout,BorderLayout,CardLayout,GridLayout

         FlowLayout是Panel、Applet的默认布局管理器。

         BorderLayout是Window、Frame和Dialog的默认布局管理器。

  

20、java的事件处理模型

    事件类型

| 

    接口名

| 

             方法  
  
---|---|---  
  
ActionEvent

| 

ActionListener

| 

actionPerformed(ActionEvent e)  
  
ItemEvent

| 

ItemListener

| 

itemStateChange(ItemEvent e)  
  
MouseEvent

| 

MouseMotionListener

| 

mouseDragged(MouseEvent e)  
  
mouseMoved(MouseEvent e)  
  
MouseListener

| 

mouseClicked(MouseEvent e)  
  
mouseEntered(MouseEvent e)  
  
mouseExited(MouseEvent e)  
  
mousePressed(MouseEvent e)  
  
mouseReleased(MouseEvent e)  
  
KeyEvent

| 

KeyListener

| 

keyPressed(KeyEvent e)  
  
keyReleased(KeyEvent e)  
  
keyTyped(KeyEvent e)  
  
FocusEvent

| 

FocusListener

| 

focusGained(FocusEvent e)  
  
focusLost(FocusEvent e)  
  
AdjustmentEvent

| 

AdjustmentListener

| 

adjustmentValueChanged(AdjustmentEvent e)  
  
WindowEvent

| 

WindowListener

| 

windowActivated(WindowEvent e)  
  
windowClosed(WindowEvent e)  
  
windowClosing(WindowEvent e)  
  
windowDeactivated (WindowEvent e)  
  
windowDeiconified(WindowEvent e)  
  
windowIconified(WindowEvent e)  
  
windowOpened(WindowEvent e)  
  
TextEvent

| 

TextListener

| 

textValueChanged(TextEvent  e)  
  
 

21、关于java中的集合主要有：List、Set、Map是这个集合体系中最主要的三个接口。

    其中List和Set继承自Collection接口。

    Set不允许元素重复。HashSet和TreeSet是两个主要的实现类。

    List有序且允许元素重复。ArrayList、LinkedList和Vector是三个主要的实现类。

    Map也属于集合系统，但和Collection接口不同。Map是key对value的映射集合，

   其中key列就是一个集合。key不能重复，但是value可以重复。HashMap、TreeMap和

   Hashtable是三个主要的实现类。

    SortedSet和SortedMap接口对元素按指定规则排序，SortedMap是对key列进行排序。   

    .Net常用的的集合有：List<T>, Dictionary<TKey,TValue>, Hashtable, ArrayList,           

    SortedList, SortedDictionary<TKey,TValue>

 

22、关于for和foreach：

    在java中foreach是JAVA 5推出的特性。

主要用于array和collection类型。

1.用于array:

for (type var : arr) {

         body-of-loop

}

//等价于

for (int i = 0; i < arr.length(); i++) {

         body-of-loop

}

2.用于colloection:

for (type var : coll) {

         body-of-loop

}

//等价于

for (Iterator<TYPE> iter = coll.iterator(); iter.hasNext();) {

         type arr = iter.next();

         body-of-loop

}

在.net中，for与java一样，foreach使用in关键字。

for (type var in arr) {

         body-of-loop

}

 

23.java中有2个数据类型的写法与.net不一样，一个是字符类型，一个是布尔类型。

   Java中使用String和boolean；

.net使用String或string(string是String的别名，大小写都可以)和bool。

 

24.关于迭代器，java中使用Iterator，.Net使用IEnumerator。

   java使用如下方式遍历:

HashMap<String, String> hmTest = new HashMap<String, String>();

Iterator<Entry<String, String>> iterator = hmTest.entrySet().iterator();

while (iterator.hasNext())

{

Map.Entry<String, String> entry = (Map.Entry<String, String>) iterator

                      .next();

             String key = entry.getKey();

             String val = entry.getValue();

             System.out.println(key + "::" + val);

}

.net使用如下方式进行遍历：

Dictionary<string, string> dicTest = new Dictionary<string, string>();

foreach (KeyValuePair<string, string> kv in dicTest){}

 

25.关于变量命名：java中可以$符号，而c#中不允许使用。

26.关于注释：java比c#少一种“///”文档注释。

27.关于switch：c#中的switch如果case后面有内容必须要有break;

java可以没有break；

28.关于多态：抽象类和抽象方法两种语言都用abstract关键字。Java中另外一个类如果继承了它，实现直接重写此方法就可以了；而c#必须加上关键字override实现。C#还比Java多一种虚方法(virtual)来实现多态。

29\. C#中的is操作符与Java中的instanceof操作符一样，两者都可以用来测试某个对象的实例是否属于特定的类型。在Java中没有与C#中的as操作符等价的操作符。as操作符与is操作符非常相似，但它更富有“进取心”：如果类型正确的话，as操作符会尝试把被测试的对象引用转换成目标类型；否则，它把变量引用设置成null。

30.关于传值方式：

在java中简单数据类型的值传参时，都以传值方式;

在c#中如果加ref则会以引用的方式传值（方法内部改变该参数，则外部变量一起跟着变）;

加out与ref基本相同，但out不要求参数一定要初始化
