---
title: linux下freetds的安装(centos连接数据库)
date: 2017/1/9 16:36:08
tags:
---


#./configure

 提示没有GCC编译器环境）

configure: error: no acceptable C compiler found in $PATH

因为是centos linux，默认可以采用yum方式安装，则采用如下命令安装gcc编译器即可：

 # yum -y install gcc

  


1.下载freetds，点此下载 <http://www.jb51.net/database/201983.html>

2.将其解压到任意目录，进入到解压后的文件夹里。

3.切换到root，配置： ./configure –prefix=/usr/local/freetds –with-tdsver=8.0 –enable-msdblib 解释：–prefix为设置FreeTDS的安装目录，–with-tdsver是设置TDS版本， –enable-msdblib为是否允许Microsoft数据库函数库

4.make & make install

5.配置环境变量：vim ~/.bashrc向此文件中加入： export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/usr/local/freetds/lib/

FreeTDS测试：

FreeTDS安装好了，接下来就可以查看下FreeTDS状态了；

运行./tsql  -C ，在安装目录的bin目录下可以找到tsql ，查看终端打印出来信息，这个-with-tdsver=7.1：

关于安装参考

<http://linux.chinaunix.net/techdoc/database/2008/10/31/1042291.shtml> 或者：<http://www.linuxdiyf.com/viewarticle.php?id=109086>

 FreeTDS的配置

　　freeTDS 的配置文件，FreeTDS也支持一个旧的配置文件interfaces，但请使用freetds.conf 除非你的环境必须使用interfaces。FreeTDS首先找freetds.conf文件如果没有找到才去找 interfaces文件。 freetds.conf文件默认在/usr/local/freetds/etc目录下，但是可以在configure时配置 sysconfdir选项，这个选项就是freetds.conf文件所存在的目录。freetds.conf配置文件分为两部分：一是[global]部分,另外一个是[dataserver]部分，其中 [dataserver]对应一个数据库。在golbal中的设置是对全部数据库起作用的，但在dataserver 部分的设置只对自己的数据库起作用，并且可以覆盖全局的设置。

例如: freetds.conf文件：

[global] 

tds version = 4.2 

[myserver] 

host = [ntbox.mydomain.com](http://ntbox.mydomain.com/) 

port = 1433 

[myserver2] 

host = [unixbox.mydomain.com](http://unixbox.mydomain.com/) 

port = 4000 

tds version = 5.0

　　这个文件中global设置所有数据库使用tds版本为4.2，但在myserver2中使用的版本却是5.0， 如果myserver2中没有这一项，那就是用4.2版本的如myserver。

其配置项解释如下：

ltds version       : 指明tds协议的版本，连接数据库时使用，如果在环境变量中没有设置 此项,则由此配置决定，协议版本可取4.2,5.0,7.0,8.0。

lhost                 : 数据库服务器的主机名或者ip地址。

lport                 : 数据库服务器的监听端口，可以取任何有效的端口值，一般而言Sybase SQL10以前为 1433，10以上用5000，而Sybase SQLAnywhere 7是2638，Microsoft SQL server则用 1433。此配置可以被环境变量中的TDSPORT改写。

linitial block size : 此值只能取512的倍数，默认为512，指定了协议块的最大值， 一般不要改变此默认 配置。

ldump file          : 任何有效的文件名，指明了转储文件的路径并且会打开日志记录。

ldump file append: yes或者no，决定是否追加保存到dump file文件中。

ltimeout            ：设置处理的最大等待时间。

lconnect timeout： 设置连接的最大等待时间。

lemulate little endian: yes或者no,是否强制大端机使用小端方式与MS Server通信。

lclient charset   : 任何有效的iconv字符集。默认值为ISO-8859-1,使FreeTDS使用 iconv在数据库服务器和用户程序之间转换。

FreeTDS函数 

1． Dbcmd和dbfcmd

函数原形： Dbcmd(DBPROCESS *proc,char * sql);

   　　　　 Dbcmd(DBPROCESS *proc, char * format,char *args);

功      能：该函数主要是构造sql语句，一个是带参数的，一个不带参数。

2． Dbsqlexec

函数原形：Dbsqlexec（DBPROCESS *proc）；

功      能：该函数负责执行你所构造的sql语句。

3． Dbresults

函数原形：Dbrerults（DBPROCESS *proc）；

功      能：得到sql语句的执行结果。返回值如果为NO_MORE_RESULTS=0，表明sql查询为空值（就是没有一条满足条件的结果），如果为（FAIL）=-1，表明查询出错，如果为（SUCCESS）=1，表明有结果且不为空。

4． DBROWS（全大写）

函数原形：DBROWS（DBPROCESS *proc）；

功      能：取出一行记录的信息。

5． Dbbind

函数原形：Dbbind（DBPROCESS *proc，int colmn，

功      能：将sql查询出来的结果绑定到一个变量。第一个参数为从数据库那里拿的句柄，第二个参数是对应你的select语句中查询需要的字段（注：必须是按照select顺序绑定的，例如select user，password from hist1 ，如果值为1，就是绑定的user），第三个参数是绑定字段的类型，最后一个参数是绑定的变量。

6．    Dbnextrow

函数原形：Dbnextrow（DBPROCESS *proc）；

功      能：该函数将取出满足sql语句的每一行，返回值为0，代表处理结束，返回值为-1出错。

7．  Dbcancel

函数原形：Dbcancel（DBPROCESS *proc）；

功      能：清空上次查询得到的数据集，如果是一个句柄的话，每次重新执行select语句之前都要调用它清空结果，不然数据库会报错的。

8． Dbclose

函数原形：Dbclose（DBPROCESS *proc）；

功      能：关闭句柄。当不再使用时必须关闭句柄。

9．  Dbinit

函数原形：Dbinit（）

功      能：初识化数据库连接。返回值为-1出错。

10． Dblogin

函数原形：LOGINREC       *Dblogin（）；

             DBSETLUSER(login,SOFT);  //set the database user 

             DBSETLPWD(login,SOFTPASS);//set password

功     能：根据用户名和密码连接数据库。

11．Dbcount

函数原形：Dbcount（DBPROCESS *proc）；

功      能：该函数将得到sql结果集被处理的行数，可以用它来判断你的select语句是否得到正确的处理。

12．Dbopen

函数原形：DBPROCESS * Dbopen（LOGINREC     *login，NULL）；

功      能：返回一个操作数据库的句柄。

另外再介绍两个关于数据库的出错信息的函数：

dberrhandle(int *err);

dbmsghandle(int* err);

实例代码

#include <stdio.h>

#include <stdlib.h>

#include <string.h>

#include <unistd.h>

#include <sqlfront.h> /* sqlfront.h always comes first */

#include <sybdb.h> /* sybdb.h is the only other file you need */

#define SQLDBIP " " //SQL数据库服务器IP

#define SQLDBPORT " " //SQL数据库服务器端口

#define SQLDBNAME " " //SQL数据库服务器数据库名

#define SQLDBUSER " " //SQL数据库服务器数据库用户名

#define SQLDBPASSWD " " //SQL数据库服务器用户密码

#define SQLDBSERVER SQLDBIP":"SQLDBPORT

#define DBSQLCMD "select * from yancao"

int main(int argc, char *argv[])

{

    int i, ch;

    LOGINREC *login; //描述客户端的结构体，在连接时被传递到服务器.

    DBPROCESS *dbproc; //描述连接的结构体，被dbopen()函数返回

    RETCODE erc; //库函数中最普遍的返回类型.

/*************************************************************/

//在开始调用本库函数前常常要先调用dbinit()函数

    if (dbinit() == FAIL) {

        fprintf(stderr, "%s:%d: dbinit() failed\n",argv[0], __LINE__);

        exit(1);

     }

//dblogin()函数申请 LOGINREC 结构体，此结构体被传递给dbopen()函数，用来创建一个连接。

//虽然基本上不会调用失败，但是检查它！.

    if ((login = dblogin()) == NULL) {

        fprintf(stderr, "%s:%d: unable to allocate login structure\n",argv[0],__LINE__);

        exit(1);

    }

//LOGINREC结构体不能被直接访问，要通过以下宏设置，下面设置两个必不可少的域

    DBSETLUSER(login, SQLDBUSER);

    DBSETLPWD(login, SQLDBPASSWD); 

/*************************************************************/

//dbopen()与服务器建立一个连接. 传递 LOGINREC 指针和服务器名字

     if ((dbproc = dbopen(login, SQLDBSERVER)) == NULL) {

        fprintf(stderr, "%s:%d: unable to connect to %s as %s\n",

 argv[0], __LINE__,

        SQLDBSERVER, SQLDBUSER);

        exit(1);

    }

// 可以调用dbuser()函数选择我们使用的数据库名，可以省略，省略后使用用户默认数据库.

     if (SQLDBNAME && (erc = dbuse(dbproc, SQLDBNAME)) == FAIL) {

        fprintf(stderr, "%s:%d: unable to use to database %s\n",

argv[0], __LINE__, SQLDBNAME);

         exit(1);

     }

/*************************************************************/

    dbcmd(dbproc, DBSQLCMD);//将SQL语句填充到命令缓冲区

     printf("\n");

    if ((erc = dbsqlexec(dbproc)) == FAIL) {

        fprintf(stderr, "%s:%d: dbsqlexec() failed\n", argv[0], __LINE__);

        exit(1); //等待服务器执行SQL语句，等待时间取决于查询的复杂度。

    }

/*************************************************************/

//在调用dbsqlexec()、dbsqlok()、dbrpcsend()返回成功之后调用dbresults()函数

    printf("then fetch results:\n");

    int count = 0;

    while ((erc = dbresults(dbproc)) != NO_MORE_RESULTS) {

        struct col { //保存列的所有信息

        char *name; //列名字

        char *buffer; //存放列数据指针

        int type, size, status;

    } *columns, *pcol;

    int ncols;

    int row_code; 

    if (erc == FAIL) {

        fprintf(stderr, "%s:%d: dbresults failed\n",

argv[0], __LINE__);

        exit(1);

     } 

    ncols = dbnumcols(dbproc);//返回执行结果的列数目 

    if ((columns = calloc(ncols, sizeof(struct col))) == NULL) {

        perror(NULL);

        exit(1);

     }

 /* read metadata and bind. */

    for (pcol = columns; pcol - columns < ncols; pcol++) {

        int c = pcol - columns + 1;

         pcol->name = dbcolname(dbproc, c); //返回指定列的列名

        pcol->type = dbcoltype(dbproc, c);

        pcol->size = dbcollen(dbproc, c); 

         printf("%*s(%d)", 20, pcol->name, pcol->size);

        if ((pcol->buffer = calloc(1, 20)) == NULL) {

        perror(NULL);

         exit(1);

    }

    erc = dbbind(dbproc, c, NTBSTRINGBIND, 20, (BYTE*)pcol->buffer);

    if (erc == FAIL) {

        fprintf(stderr, "%s:%d: dbbind(%d) failed\n",

argv[0], __LINE__, c);

        exit(1);

    }

    erc = dbnullbind(dbproc, c, &pcol->status); //(5)

     if (erc == FAIL) {

        fprintf(stderr, "%s:%d: dbnullbind(%d) failed\n",

argv[0], __LINE__, c);

        exit(1);

    }

 }

    printf("\n");

/* 打印数据 */

    while ((row_code = dbnextrow(dbproc)) != NO_MORE_ROWS) {//读取行数据

    switch (row_code) {

    case REG_ROW:

     for (pcol=columns; pcol - columns < ncols; pcol++) {

    char *buffer = pcol->status == -1?

"null" : pcol->buffer;

    printf("%*s ", 20, buffer);

    }

    printf("\n"); break;

    case BUF_FULL: break;

    case FAIL:

     fprintf(stderr, "%s:%d: dbresults failed\n",

     argv[0], __LINE__);

exit(1); break;

 default: // (7)

 printf("data for computeid %d ignored\n", row_code);

}

 }

 /* free metadata and data buffers */

 for (pcol=columns; pcol - columns < ncols; pcol++) {

free(pcol->buffer);

}

 free(columns);

if (DBCOUNT(dbproc) > -1) /* 得到SQL语句影响的行数 */

fprintf(stderr, "%d rows affected\n", DBCOUNT(dbproc))

}

dbclose(dbproc);

dbexit();

}   

  

