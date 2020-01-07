---
title: jvm中间语言bytecode
date: 2017/11/19 6:18:22
tags:
---


  


## Write Once,Run Anywhere——byteCode

byteCode 平台无关 

java通过存储编译后的字节码，并将字码加载到JVM中，实现了java语言的跨平台。字节码是平台中立的代码存储格式，任意一个平台只要安装了JRE(跟平台有关),那么程序就是可以运行的。 

byteCode 语言无关 

除此之外，更值得注意的是字节码不仅是平台无关的，也同样是语言无关的，并不是说只有java语言才能生成byteCode，byteCode比java语言有着更强的描述性,而且JVM不与语言进行绑定，所以其他语言(Jython,groovy等)，也是可以通过编译为byteCode运行在JVM中的。 

## Class文件

类文件 {

0xCAFEBABE，小版本号，大版本号，常量池大小，常量池数组，

访问控制标记，当前类信息，父类信息，实现的接口个数，实现的接口信息数组，域个数，

域信息数组，方法个数，方法信息数组，属性个数，属性信息数组

}

1

2

3

4

5

一个byteCode文件（.class）一定对应一个java接口或者类，但是一个java接口或者类不一定对应一个.class文件，也有可能只在内存中生成。

一个class文件是一个以Byte为基础单位的二进制流，各项数据、指令紧凑的排列，中间没有任何分隔符、间隙。

javap -verbose 执行后的可视byteCode:

public class com.zs.jvm.byteCode.TestClass

minor version: 0

major version: 51

flags: ACC_PUBLIC, ACC_SUPER

Constant pool:

#1 = Class #2 // com/zs/jvm/byteCode/TestClass

#2 = Utf8 com/zs/jvm/byteCode/TestClass

#3 = Class #4 // java/lang/Object

#4 = Utf8 java/lang/Object

#5 = Utf8 m

#6 = Utf8 I

#7 = Utf8 <init>

#8 = Utf8 ()V

#9 = Utf8 Code

#10 = Methodref #3.#11 // java/lang/Object."<init>":()V

#11 = NameAndType #7:#8 // "<init>":()V

#12 = Utf8 LineNumberTable

#13 = Utf8 LocalVariableTable

#14 = Utf8 this

#15 = Utf8 Lcom/zs/jvm/byteCode/TestClass;

#16 = Utf8 inc

#17 = Utf8 ()I

#18 = Fieldref #1.#19 // com/zs/jvm/byteCode/TestClass.m:I

#19 = NameAndType #5:#6 // m:I

#20 = Utf8 SourceFile

#21 = Utf8 TestClass.java

{

public com.zs.jvm.byteCode.TestClass();

descriptor: ()V

flags: ACC_PUBLIC

Code:

stack=1, locals=1, args_size=1

0: aload_0

1: invokespecial #10 // Method java/lang/Object."<init>":()V

4: return

LineNumberTable:

line 3: 0

LocalVariableTable:

Start Length Slot Name Signature

0 5 0 this Lcom/zs/jvm/byteCode/TestClass;

  


public int inc();

descriptor: ()I

flags: ACC_PUBLIC

Code:

stack=2, locals=1, args_size=1

0: aload_0

1: getfield #18 // Field m:I

4: iconst_1

5: iadd

6: ireturn

LineNumberTable:

line 7: 0

LocalVariableTable:

Start Length Slot Name Signature

0 7 0 this Lcom/zs/jvm/byteCode/TestClass;

}

  


byteCode只存在两种数据类型：无符号数字 与 表。 

无符号数字： 

基本的数据类型，以u1、u2、u4、u8来分别代表1个字节，2个字节、4个字节，8个字节的无符号数字。无符号数字用来描述数字、索引引用、数据值、UTF-8编码的字符串值。

表：由无符号数字或者表构成的复合数据类型。表通常以”_info”结尾。class文件本质上就是一个表，如下图 

 

从头到尾一项项来看: 

magic: 

一个4byte的无符号数字，用来确认当前文件是否是class格式文件，是否可以被jvm接收。 

minor_version、major_version: 

用来判断当前编译的byteCode所属JDK版本号，JDK向下兼容. 

constant_pool_count: 

用来记录常量池中数据类型的个数。最大值为22(1-22)，也就是说常量池总共有21中数据类型。 

constant_pool: 

常量池 通常存储两种数据： 

字面量（litera），如字符串，final常量等 

符号引用（Symblic Reference），如类、接口的全限定名称、字段的名称和描述、方法的名称和描述。

access_flag: 

用来描述类或接口的访问类型、修饰符等。方法内部也会有这个字段用来描述方法的修饰关键字 

this class、super_class、interfaces_count、interfaces 

这几个无符号数字，用来描述当前class所属的类型、父类、接口等继承与实现信息

* 字段段表集合 field_info fields* 

这个表用于描述接口或类中声明的变量，但不包括方法中的局部变量。 

field_info具体结构，如下：

![](http://csdnimg.cn/F/img.blog.csdn.net/20170406231813514?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvbGVtb244OQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

access_flags与之前说过，只是这里的access_flag针对成员变量(类级别的变量)，用来描述这个方法是否使用了final、static、volatile等关键字修饰。 

name_index\descriptor_index这两个都是对ConstantPool的引用。

name_index:用来描述成员变量的简单名称,如int indexOf（…)这个方法，name_index在常量池中对应的描述字符串为：“indexOf”

descriptor_index:参数列表、返回值等。 

对于数组类型，每一维度将使用一个前置的“[”字符来描述，如一个定义为“java.lang.String[][]”类型的二维数组， 

将被记录为：“[[Ljava/lang/String；”，一个整型数组“int[]”将被记录为“[I”。

如：

int indexOf（char[]source,int sourceOffset,int sourceCount,char[]target,int targetOffset,int

targetCount,int fromIndex）

1

2

描述为：([CII[CIII)I。 

注意顺序：“参数列表 返回类型”

方法表集合 method_info methods 

与字段表集合类似，结构如下： 

attribute_info后文详细描述。

<https://kastor.opaak.org/articles/69-0xcafebabe-java-class-file-format-an-overview.htmlo>

属性表集合 attribute_info 

class文件本身、方法表(methods_info)、属性表(fields_info)都可以携带属性表(attribute_info)，用来描述某些信息。

attribute_info属性表预定义属性如下（不完全）： 

 

属性表结构： 

待续。。。
