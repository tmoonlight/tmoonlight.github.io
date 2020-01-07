---
title: C++问题记录
date: 2016/1/16 18:30:13
tags:
---


传参的理解

  


int myint=0;

int* x= &myint;//两个变量指向一处

  


int* myint=0xA00012345;

  


直观理解 

int ttmain(int argc, char* argv[]){}

ttmain(1,&test)

等价

ttmain

ttmain {int argc = 1,char* argv =&test}

等同于

int myint=0;

int* x;

setvalue()

  


  


地址强转

((in_addr *)0x27318)->s_addr

  


指针加1 地址增加类型占用空间的长度

比如int指针加1：(p+1) 返回的地址就是比p多4位

  


宏：一段代码，可以不编译通过

#define BEGIN_MESSAGE_MAP(theClass, baseClass) \

       PTM_WARNING_DISABLE \

       const AFX_MSGMAP* theClass::GetMessageMap() const \

              { return GetThisMessageMap(); } \

       const AFX_MSGMAP* PASCAL theClass::GetThisMessageMap() \

       { \

              typedef theClass ThisClass;                                      \

              typedef baseClass TheBaseClass;                                  \

              __pragma(warning(push))                                                 \

              __pragma(warning(disable: 4640)) /* message maps can only be called by single threaded message pump */ \

              static const AFX_MSGMAP_ENTRY _messageEntries[] =  \

              {

#define END_MESSAGE_MAP() \

              {0, 0, 0, 0, AfxSig_end, (AFX_PMSG)0 } \

       }; \

              __pragma(warning(pop))     \

              static const AFX_MSGMAP messageMap = \

              { &TheBaseClass::GetThisMessageMap, &_messageEntries[0] }; \

              return &messageMap; \

       }     

文件操作

  


endl的作用是换行 可以插入到输出流中,效果为在输出结果中插入换行符'\n'
