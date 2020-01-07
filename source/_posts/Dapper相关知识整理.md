---
title: Dapper相关知识整理
date: 2017/1/21 13:03:30
tags:
---


dapper使用mysql开发区别不是很大，主要有以下几点：

  


1、连接对象要使用MySqlConnection，这个类在MySql.Data.dll中。

2、数据库连接字符串有点不一样

3、dapper查询返回List集合要使用AsList()方法，而且对[linq](http://www.lanhusoft.com/Article/163.html "了解linq的实现原理及常用的函数")支持没有sql server好。

4、参数虽然也是通@开头，但是不是真正意义的Command，而是拼接sql。

  


  


标签：

Dapper是一个轻量级的ORM框架，它只是一个IDbConnection的扩展文件。所以我们需要手写很多SQL，但是写CRUD的代码总是很无趣的。所有就有了DapperExtensions。DapperExtensions对Dapper提供了更多的扩展，可以不用写SQL就实现CRUD操作跟简单的查询功能。话不多说还是直接上代码吧。

class ConnectionFactory

{

public static IDbConnection CreateConnection<T>() where T:IDbConnection,new ()

{

IDbConnection connection = new T();

connection.ConnectionString = ConnectionConfig.ConnectionString;

connection.Open();

return connection;

}

  


public static IDbConnection CreateSqlConnection()

{

return CreateConnection<SqlConnection>();

}

}

interface IRepository<T>

{

IEnumerable<T> GetList();

  


T Get(object id);

  


bool Update(T t);

  


T Insert(T apply);

  


bool Delete(T t);

  


IEnumerable<T> Find(Expression<Func<T, object>> expression, Operator op, object param);

}

class SqlRepository<T> : IRepository<T> where T : class, IEntity

{

public virtual IEnumerable<T> GetList()

{

using (var conn = ConnectionFactory.CreateSqlConnection())

{

return conn.GetList<T>();

}

  


}

  


public virtual T Get(object id)

{

using (var conn = ConnectionFactory.CreateSqlConnection())

{

return conn.Get<T>(id);

}

}

  


public virtual bool Update(T t)

{

using (var conn = ConnectionFactory.CreateSqlConnection())

{

return conn.Update(t);

}

}

  


public virtual T Insert(T apply)

{

using (var conn = ConnectionFactory.CreateSqlConnection())

{

conn.Insert(apply);

return apply;

}

}

  


public virtual bool Delete(T t)

{

using (var conn = ConnectionFactory.CreateSqlConnection())

{

return conn.Delete(t);

}

}

  


public virtual IEnumerable<T> Find(Expression<Func<T,object>> expression,Operator op,object param)

{

using (var conn = ConnectionFactory.CreateSqlConnection())

{

return conn.GetList<T>(Predicates.Field<T>(expression, op, param));

}

}

  


}

恩，就是这么简单。
