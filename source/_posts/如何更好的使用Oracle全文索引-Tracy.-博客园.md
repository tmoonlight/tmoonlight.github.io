---
title: 如何更好的使用Oracle全文索引-Tracy.-博客园
date: 2016/1/15 21:03:21
tags:
---


**  
**

不使用Oracle text功能,也有很多方法可以在Oracle数据库中搜索文本.可以使用标准的INSTR函数和LIKE操作符实现。

  


SELECT *FROM mytext WHERE INSTR (thetext, 'Oracle') > 0;

SELECT * FROM mytext WHERE thetext LIKE '%Oracle%';

  


有很多时候，使用instr和like是很理想的, 特别是搜索仅跨越很小的表的时候.然而通过这些文本定位的方法将导致全表扫描,对资源来说消耗比较昂贵,而且实现的搜索功能也非常有限，因此对海量的文本数据进行搜索时，建议使用oralce提供的全文检索功能 建立全文检索的步骤步骤一 检查和设置数据库角色首先检查数据库中是否有CTXSYS用户和CTXAPP脚色。如果没有这个用户和角色，意味着你的数据库创建时未安装intermedia功能。你必须修改数据库以安装这项功能。 默认安装情况下，ctxsys用户是被锁定的，因此要先启用ctxsys的用户。 步骤二 赋权 在ctxsys用户下把ctx_ddl的执行权限赋于要使用全文索引的用户，例：

  


grant execute on ctx_ddl to pomoho;  


  


步骤三 设置词法分析器(lexer)

  


Oracle实现全文检索，其机制其实很简单。即通过Oracle专利的词法分析器(lexer),将文章中所有的表意单元(Oracle 称为 term)找出来，记录在一组 以dr$开头的表中，同时记下该term出现的位置、次数、hash 值等信息。检索时，Oracle 从这组表中查找相应的term，并计算其出现频率，根据某个算法来计算每个文档的得分(score),即所谓的‘匹配率’。而lexer则是该机制的核心，它决定了全文检索的效率。Oracle 针对不同的语言提供了不同的 lexer, 而我们通常能用到其中的三个：

  


n  basic_lexer: 针对英语。它能根据空格和标点来将英语单词从句子中分离，还能自动将一些出现频率过高已经失去检索意义的单词作为‘垃圾’处理，如if , is 等，具有较高的处理效率。但该lexer应用于汉语则有很多问题，由于它只认空格和标点，而汉语的一句话中通常不会有空格，因此，它会把整句话作为一个 term,事实上失去检索能力。以‘中国人民站起来了’这句话为例，basic_lexer 分析的结果只有一个term ,就是‘中国人民站起来了’。此时若检索‘中国’，将检索不到内容。

  


n  chinese_vgram_lexer: 专门的汉语分析器，支持所有汉字字符集(ZHS16CGB231280 ZHS16GBK ZHT32EUC ZHT16BIG5 ZHT32TRIS ZHT16MSWIN950 ZHT16HKSCS UTF8 )。该分析器按字为单元来分析汉语句子。‘中国人民站起来了’这句话，会被它分析成如下几个term: ‘中’，‘中国’，‘国人’，‘人民’，‘民站’，‘站起’，起来’，‘来了’，‘了’。可以看出，这种分析方法，实现算法很简单，并且能实现‘一网打尽’，但效率则是差强人意。

  


n  chinese_lexer: 这是一个新的汉语分析器，只支持utf8字符集。上面已经看到，chinese vgram lexer这个分析器由于不认识常用的汉语词汇，因此分析的单元非常机械，像上面的‘民站’，‘站起’在汉语中根本不会单独出现，因此这种term是没有意义的，反而影响效率。chinese_lexer的最大改进就是该分析器 能认识大部分常用汉语词汇，因此能更有效率地分析句子，像以上两个愚蠢的单元将不会再出现，极大 提高了效率。但是它只支持 utf8, 如果你的数据库是zhs16gbk字符集，则只能使用笨笨的那个Chinese vgram lexer.

如果不做任何设置，Oracle 缺省使用basic_lexer这个分析器。要指定使用哪一个lexer, 可以这样操作：

  


第一． 当前用户下下建立一个preference(例：在pomoho用户下执行以下语句)

  


exec ctx_ddl.create_preference ('my_lexer', 'chinese_vgram_lexer');

  


第二．   在建立全文索引索引时，指明所用的lexer:

  


CREATE INDEX myindex ON mytable(mycolumn) indextype is ctxsys.context

  


  


parameters('lexer my_lexer');

  


这样建立的全文检索索引，就会使用chinese_vgram_lexer作为分析器。

  


步骤四 建立索引

  


通过以下语法建立全文索引

  


CREATE INDEX [schema.]index on [schema.]table(column) INDEXTYPE IS ctxsys.context [ONLINE]

  


  


LOCAL [(PARTITION [partition] [PARAMETERS('paramstring')]

  


  


[, PARTITION [partition] [PARAMETERS('paramstring')]])]

  


  


[PARAMETERS(paramstring)] [PARALLEL n] [UNUSABLE];

例：  
  


  


CREATE INDEX ctx_idx_menuname ON pubmenu(menuname)

  


indextype is ctxsys.context parameters('lexer my_lexer')

  


步骤五 使用索引

  


使用全文索引很简单，可以通过：

  


select * from pubmenu where contains(menuname,'上传图片')>0

  


全文索引的种类

  


建立的Oracle Text索引被称为域索引(domain index)，包括4种索引类型：

  


l CONTEXT

  


2 CTXCAT

  


3 CTXRULE

  


4 CTXXPATH

  


依据你的应用程序和文本数据类型你可以任意选择一种。

  


 **对多字段建立全文索引**

  


很多时候需要从多个文本字段中查询满足条件的记录，这时就需要建立针对多个字段的全文索引，例如需要从pmhsubjects(专题表)的 subjectname(专题名称)和briefintro(简介)上进行全文检索，则需要按以下步骤进行操作：

  


Ø  建议多字段索引的preference

  


以ctxsys登录，并执行：

  


EXEC ctx_ddl.create_preference(' ctx_idx_subject_pref',

  


  


'MULTI_COLUMN_DATASTORE');

  


  


Ø 建立preference对应的字段值(以ctxsys登录)

  


  


EXEC ctx_ddl.set_attribute(' ctx_idx_subject_pref ','columns','subjectname,briefintro');

  


  


Ø 建立全文索引

CREATE INDEX ctx_idx_subject ON pmhsubjects(subjectname)

  


INDEXTYPE ISctxsys.CONTEXT PARAMETERS('DATASTORE ctxsys.ctx_idx_subject_pref lexer my_lexer')

Ø 使用索引  


* from pmhsubjects where contains(subjectname,'李宇春')>0  


select

  


 **全文索引的维护**

  


对于CTXSYS.CONTEXT索引，当应用程序对基表进行DML操作后，对基表的索引维护是必须的。索引维护包括索引同步和索引优化。

  


在索引建好后，我们可以在该用户下查到Oracle自动产生了以下几个表：(假设索引名为myindex)：

  


DR$myindex$I、DR$myindex$K、DR$myindex$R、DR$myindex$N其中以I表最重要，可以查询一下该表，看看有什么内容：

  


SELECT token_text, token_count FROM dr$i_rsk1$I WHERE ROWNUM <= 20;  


  


这里就不列出查询接过了。可以看到，该表中保存的其实就是Oracle 分析你的文档后，生成的term记录在这里，包括term出现的位置、次数、hash值等。当文档的内容改变后，可以想见这个I表的内容也应该相应改变，才能保证Oracle在做全文检索时正确检索到内容(因为所谓全文检索，其实核心就是查询这个表)。这就用到sync(同步) 和 optimize(优化)了。

  


 **同步(sync):** 将新的term 保存到I表；

  


 **优化(optimize):** 清除I表的垃圾，主要是将已经被删除的term从I表删除。

  


当基表中的被索引文档发生insert、update、delete操作的时候，基表的改变并不能马上影响到索引上直到同步索引。可以查询视图 CTX_USER_PENDING查看相应的改动。例如：

  


SELECT pnd_index_name, pnd_rowid,

TO_CHAR (pnd_timestamp, 'dd-mon-yyyy hh24:mi:ss') timestamp

FROM ctx_user_pending;  


  


该语句的输出类似如下：

PND_INDEX_NAME                PND_ROWID          TIMESTAMP

\------------------------------ ------------------ --------------------

MYINDEX                        AAADXnAABAAAS3SAAC 06-oct-1999 15:56:50

  


 **同步和优化方法: 可以使用Oracle提供的ctx_ddl包同步和优化索引**

  


一.  对于CTXCAT类型的索引来说， 当对基表进行DML操作的时候，Oracle自动维护索引。对文档的改变马上反映到索引中。CTXCAT是事务形的索引。

  


 **索引的同步**

  


在对基表插入，修改，删除之后同步索引。推荐使用sync同步索引。语法：

  


ctx_ddl.sync_index(

idx_name IN VARCHAR2 DEFAULT NULL

memory IN VARCHAR2 DEFAULT NULL,  


L  
parallel_degree IN NUMBER DEFAUL

part_name IN VARCHAR2 DEFAULT NU

LT 1);  
idx_name  索引名称  


是系统参数DEFAULT_INDEX_MEMORY 。  


memory    指定同步索引需要的内存。默

认  


  


指定一个大的内存时候可以加快索引效率和查询速度，且索引有较少的碎片

part_name 同步哪个分区索引。

parallel_degree 并行同步索引。设置并行度。

  


例如：

  


同步索引myindex:Exec   ctx_ddl.sync_index ('myindex');

  


实施建议：建议通过oracle的job对索引进行同步

  


 **索引的优化**

  


经常的索引同步将会导致你的CONTEXT索引产生碎片。索引碎片严重的影响了查询的反应速度。你可以定期优化索引来减少碎片，减少索引大小，提高查询效率。

当文本从表中删除的时候，Oracle Text标记删除的文档，但是并不马上修改索引。因此，就的文档信息占据了不必要的空间，导致了查询额外的开销。你必须以FULL模式优化索引，从索引中删除无效的旧的信息。这个过程叫做垃圾处理。当你经常的对表文本数据进行更新，删除操作的时候，垃圾处理是很必要的。

  


exec   ctx_ddl.optimize_index ('myidx', 'full');

实施建议：每天在系统空闲的时候对全文索引进行相应的优化，以提高检索的效率

  


 **P.S.定时优化索引**

  


3.定时优化同步域索引

创建定时任务，定期优化和同步域索引

  


SQL> create or replace procedure hsp_sync_index as

  2  begin  
  3  ctx_ddl.sync_index('id_cont_msg');  


0:00.08  
S

  4  end;  
  5  /  
  
Procedure created.  
  
Elapsed: 00:

0QL> VARIABLE jobno number;   
SQL> BEGIN  


ex();',   
  3 SYSDATE, 'SYSDATE + (1/24/4)');  


  2 DBMS_JOB.SUBMIT(:jobno,'hsp_sync_in

d  4 commit;   
  5 END;   
  6 /  
  
PL/SQL procedure successfully completed.  
  


  


2  begin  
  3  ctx_d

Elapsed: 00:00:00.27  
SQL> create or replace procedure hsp_optimize_index as  


  


dl.optimize_index('id_cont_msg','FULL');  
  4  end;  
  5  /  
  
SQL> VARIABLE jobno number;  


1');  


SQL> BEGIN  
  2 DBMS_JOB.SUBMIT(:jobno,'hsp_optimize_index();',   
  3 SYSDATE, 'SYSDATE

\+ 4 commit;  
  5 END;  
  6 /  
Procedure created.  
  
Elapsed: 00:00:00.03  
  


  


PL/SQL procedure successfully completed.  
  
Elapsed: 00:00:00.02  


SQL>  


  


 890890890
