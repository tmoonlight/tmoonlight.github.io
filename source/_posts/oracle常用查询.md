---
title: oracle常用查询
date: 2017/3/4 9:12:24
tags:
---


# 获取表：

 select table_name from user_tables; //当前用户的表      

 select table_name from all_tables; //所有用户的表  

 select table_name from dba_tables; //包括系统表

 select table_name from dba_tables where owner='用户名'

 user_tables：

 table_name,tablespace_name,last_analyzed等

 dba_tables：

 ower,table_name,tablespace_name,last_analyzed等

 all_tables：

 ower,table_name,tablespace_name,last_analyzed等

 all_objects：

 ower,object_name,subobject_name,object_id,created,last_ddl_time,timestamp,status等

  


获取表字段：

 select * from user_tab_columns where Table_Name='用户表';

 select * from all_tab_columns where Table_Name='用户表';

 select * from dba_tab_columns where Table_Name='用户表';

 user_tab_columns：

 table_name,column_name,data_type,data_length,data_precision,data_scale,nullable,column_id等

 all_tab_columns ：

 ower,table_name,column_name,data_type,data_length,data_precision,data_scale,nullable,column_id等

 dba_tab_columns：

 ower,table_name,column_name,data_type,data_length,data_precision,data_scale,nullable,column_id等

  


获取表注释：

 select * from user_tab_comments

 user_tab_comments：table_name,table_type,comments

 \--相应的还有dba_tab_comments，all_tab_comments，这两个比user_tab_comments多了ower列。

  


获取字段注释：

 select * from user_col_comments

 user_col_comments：table_name,column_name,comments

  


日期范围查询：

  


  1. select * from tablename where time>=  
  2. to_date('2011-06-02','yyyy-mm-dd')    
  3. and  time<=to_date('2011-06-30','yyyy-mm-dd') 



  


运行的结果是：可以显示06-02的数据，但是不能显示06-30的数据。

所有可以得出结论：

①如果想显示05-30的数据可以<to_date('2011-05-31','yyyy-mm-dd')，这样就能显示30号的了。

②如果想要显示05-30的数据可以<=to_date('2011-05-30 23:59:59 999','yyyy-mm-dd hh24:mi:ss')也是可以查出来的。  


  


 **最终解决方案:**

  1. select * from tablename where to_char(time,'yyyy-mm-dd')>='2011-05-02'    
  2. and to_char(time,'yyyy-mm-dd')<='2011-05-30' 



  


  


  


获取存储过程内容：

SELECT * FROM ALL_SOURCE@ecccsc where TYPE='PROCEDURE' and owner='NEW_SUPPORT' AND name LIKE '%INSERTNEWDOC%' order by line

  


 **建一个job：**

declare job_id number ;

begin

  


  sys.dbms_job.submit(job_id,

                      what => 'ECC_CSC.SYN_SL_PRODUCT;',

                      next_date => to_date( '05-12-2014 12:00:00', 'dd-mm-yyyy hh24:mi:ss' ),

                      interval => 'sysdate+1' );

 

end;

每天运行一次                        'SYSDATE + 1'

每小时运行一次                     'SYSDATE + 1/24'

每10分钟运行一次                 'SYSDATE + 10/（60*24）'

每30秒运行一次                    'SYSDATE + 30/(60*24*60)'

每隔一星期运行一次               'SYSDATE + 7'

每个月最后一天运行一次         'TRUNC(LAST_DAY(ADD_MONTHS(SYSDATE,1))) + 23/24'

每年1月1号零时                    'TRUNC(LAST_DAY(TO_DATE(EXTRACT(YEAR FROM SYSDATE)||'12'||'01','YYYY-MM-DD'))+1)'

每天午夜12点                       'TRUNC(SYSDATE + 1)'

每天早上8点30分                  'TRUNC(SYSDATE + 1) + (8*60+30)/(24*60)'

每星期二中午12点                 'NEXT_DAY(TRUNC(SYSDATE ), ''TUESDAY'' ) + 12/24'

每个月第一天的午夜12点        'TRUNC(LAST_DAY(SYSDATE ) + 1)'

每个月最后一天的23点           'TRUNC (LAST_DAY (SYSDATE)) + 23 / 24'

每个季度最后一天的晚上11点  'TRUNC(ADD_MONTHS(SYSDATE + 2/24, 3 ), 'Q' ) -1/24'

每星期六和日早上6点10分      'TRUNC(LEAST(NEXT_DAY(SYSDATE, ''SATURDAY"), NEXT_DAY(SYSDATE, "SUNDAY"))) + (6*60+10)/(24*60)'

  


  


  


  

