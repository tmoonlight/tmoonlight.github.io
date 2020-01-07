---
title: Dapper-Extensions官方文档中文版
date: 2017/1/23 9:30:51
tags:
---


## Dapper-Extensions

> Dapper Extensions is a small library that complements Dapper by adding basic CRUD operations (Get, Insert, Update, Delete) for your POCOs. For more advanced querying scenarios, Dapper Extensions provides a predicate system. The goal of this library is to keep your POCOs pure by not requiring any attributes or base class inheritance.

[github：https://github.com/tmsmith/Dapper-Extensions](https://github.com/tmsmith/Dapper-Extensions "github：https://github.com/tmsmith/Dapper-Extensions")

  * Dapper是一个开源轻的量级的orm，他的优点和用法在[之前写的博客](http://www.cnblogs.com/Sinte-Beuve/p/4231053.html)中有提到。可是它只支持带sql语句的CRUD。
  * Dapper-Extensions也是一个开源库，他在Dapper的基础上封装了基本的CRUD操作，使得一些简单的数据库操作可以不用自己写sql语句。使用起来更方面。



  


### Introduction

Dapper Extensions是github上的一个开源库是对StackOVerflow开发的Dapper ORM的一个扩展。它增加了基础的CRUD操作（(Get, Insert, Update, Delete)），对更高级的查询场景，该类库还提供了一套谓词系统。它的目标是保持POCOs的纯净，不需要额外的attributes和类的继承。

自定义映射请参见 [ClassMapper](http://www.cnblogs.com/Sinte-Beuve/p/4612971.html)

### Features

  * 零配置
  * 自动映射POCOs的CRUD操作
  * GetList, Count等方法可以用于更高级的场景。
  * GetPage for returning paged result sets.支持分页
  * 自动支持Guid和Integer类型的主键，（也可以手动指定其他键类型）
  * 通过使用ClassMapper可以使保证POCOs纯净。 (Attribute Free!)
  * 可以通过使用ClassMapper来自定义entity-table映射
  * 支持联合主键
  * POCO类名默认与数据表名相匹配，也可以自定义
  * 易于使用的 [Predicate System](https://github.com/tmsmith/Dapper-Extensions/wiki/Predicates)适用于高级场合
  * 在生成SQL语句时正确转义表名和类名 (Ex: SELECT FirstName FROM Users WHERE Users.UserId = @ UserId_0）
  * 覆盖单元测试（覆盖了150+个单元测试）



### Naming Conventions(命名约定)

  * POCO类名与数据库中表名匹配，多元化（Pluralized）的表名（暂时理解为别名吧）可以通过PlurizedAutoClassMapper来自定义。
  * POCO类中属性名和数据库表中的类名匹配。
  * 暂约定主键被命名为Id.使用其他主键需要自定义映射（ClassMapper）。



### Installation

Nuget:

<http://nuget.org/List/Packages/DapperExtensions>

PM> Install-Package DapperExtensions

### Examples

pernson POCO的定义

public class Person

{

public int Id { get; set; }

public string FirstName { get; set; }

public string LastName { get; set; }

public bool Active { get; set; }

public DateTime DateCreated { get; set; }

}

### Get Operation

using (SqlConnection cn = new SqlConnection(_connectionString))

{

cn.Open();

int personId = 1;

Person person = cn.Get<Person>(personId);

cn.Close();

}

### Simple Insert Operation

using (SqlConnection cn = new SqlConnection(_connectionString))

{

cn.Open();

Person person = new Person { FirstName = "Foo", LastName = "Bar" };

int id = cn.Insert(person);

cn.Close();

}

### Advanced Insert Operation (Composite Key)复合主键

//返回dynamic类型，若主键为单，返回主键值，若主键为复合的，返回IDictionary<string,object>

public static dynamic Insert<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class

public class Car

{

public int ModelId { get; set; }

public int Year { get; set; }

public string Color { get; set; }

}

  


using (SqlConnection cn = new SqlConnection(_connectionString))

{

cn.Open();

Car car = new Car { Color = "Red" };

//返回o

var multiKey = cn.Insert(car);

cn.Close();

  


int modelId = multiKey.ModelId;

int year = multiKey.Year;

}

### Simple Update Operation

using (SqlConnection cn = new SqlConnection(_connectionString))

{

cn.Open();

int personId = 1;

Person person = _connection.Get<Person>(personId);

person.LastName = "Baz";

cn.Update(person);

cn.Close();

}

### Simple Delete Operation

using (SqlConnection cn = new SqlConnection(_connectionString))

{

cn.Open();

Person person = _connection.Get<Person>(1);

cn.Delete(person);

cn.Close();

}

### GetList Operation (with Predicates)

using (SqlConnection cn = new SqlConnection(_connectionString))

{

cn.Open();

var predicate = Predicates.Field<Person>(f => f.Active, Operator.Eq, true);

IEnumerable<Person> list = cn.GetList<Person>(predicate);

cn.Close();

}

Generated SQL

SELECT

[Person].[Id]

, [Person].[FirstName]

, [Person].[LastName]

, [Person].[Active]

, [Person].[DateCreated]

FROM [Person]

WHERE ([Person].[Active] = @Active_0)

### Count Operation (with Predicates)

using (SqlConnection cn = new SqlConnection(_connectionString))

{

cn.Open();

var predicate = Predicates.Field<Person>(f => f.DateCreated, Operator.Lt, DateTime.UtcNow.AddDays(-5));

int count = cn.Count<Person>(predicate);

cn.Close();

}

Generated SQL

SELECT

COUNT(*) Total

FROM [Person]

WHERE ([Person].[DateCreated] < @DateCreated_0)

之前翻译了Dapper-Extensions项目首页的readme.md，大家应该对这个类库的使用有一些了解了吧，接下来是wiki的文档翻译，主要提到了AutoClassMapper、KeyTypes、Predicates System的高级用法用法。

若不熟悉Dapper及Dapper-Extensions的，请移步：

  * [《Dapper的基本使用》:http://www.cnblogs.com/Sinte-Beuve/p/4231053.html](http://www.cnblogs.com/Sinte-Beuve/p/4231053.html)
  * [《Stackoverflow/dapper的Dapper-Extensions用法（一）》:http://www.cnblogs.com/Sinte-Beuve/p/4612971.html](http://www.cnblogs.com/Sinte-Beuve/p/4612971.html)
  * [(扩展)《利用Dapper ORM搭建三层架构》:http://www.cnblogs.com/Sinte-Beuve/p/4230943.html](http://www.cnblogs.com/Sinte-Beuve/p/4230943.html)



# [AutoClassMapper](https://github.com/tmsmith/Dapper-Extensions/wiki/AutoClassMapper)

在不确定的情况下Dapper Extensions使用一个默认的ClassMapper(AutoClassMapper)来处理POCO和数据表之间的映射。ClassMapper非常重要，它可以保证POCOs的纯净，不需要增加额外的attribute。你可以通过继承ClassMapper来自定义自己的映射规则去改变Dapper Extensions的行为。(这边我觉得文档有些问题，应该是继承AutoClassMapper，因为DefaultMapper是用来设置全局的映射器的而不是单表的自定义。)

DapperExtensions.DapperExtensions.DefaultMapper = typeof(MyCustomClassMapper<>);

## AutoClassMapper Assumptions

默认的AutoClassMapper是建立在数据库与POCOs之间有一定约束的条件下才可以工作的。

约定条件如下：

  * AutoClassMapper假定数据表名与POCOs名一致(Ex: Car table name and Car POCO name).。
  * 每个POCOs必须包含一个属性名为Id或以Id结尾的。
  * 如果有多个属性以Id结尾，Dapper Extensions会使用第一个"Id"属性作为主键。
  * 如果Id属性的类型为整形，则它的 [KeyType](https://github.com/tmsmith/Dapper-Extensions/wiki/KeyTypes)属性会被设成Identity。
  * 如果Id属性的类型为GUID，则它的 [KeyType](https://github.com/tmsmith/Dapper-Extensions/wiki/KeyTypes)属性会被设成GUID。
  * 如果Id属性的类型为其他类型，则它的 [KeyType](https://github.com/tmsmith/Dapper-Extensions/wiki/KeyTypes)属性会被设成Assigned。（此类型为用户自定义主键）。



## Customized PluralizedAutoClassMapper

英语有很多多元化（pluralization）的规则，PluralizedAutoClassMapper会提供给我们一些正确的多元化命名为任何可能的场景。但是仍然有很多情况，当你的数据库表名和默认的Dapper Extensions的多元化规则不相匹配时，你可以自定义该类，注入自己的规则。

通俗的话来讲，就是说数据库表名和POCOs的命名可能会因为英文语义或者说文化等的影响而不同，这时候Dapper Extensions提供给我们PluralizedAutoClassMapper，提供一些转换的规则。例如Person和Person。

下面的例子，使用CustomPluralizedMapper对来Person到Persons数据表进行映射。如果不重写，PluralizedAutoClassMapper默认会把Person映射到People表。

public class CustomPluralizedMapper<T> : PluralizedAutoClassMapper<T> where T : class

{

protected override void Table(string tableName)

{

if(tableName.Equals("Person", StringComparison.CurrentCultureIgnoreCase))

{

TableName = "Persons";

}

  


base.Table(tableName);

}

}

全局配置：

DapperExtensions.DapperExtensions.DefaultMapper = typeof(CustomPluralizedMapper<>);

# [KeyTypes](https://github.com/tmsmith/Dapper-Extensions/wiki/KeyTypes)

KeyType这个enum允许Dapper Extensions知道如何去映射POCOs的主键。当你的POCOs被Dapper Extensions检查后，映射关系将被建立和缓存下下来。这个类型映射用于生产合适的SQL语句来进行子查询的调用。举个例子，在不确定的情况下，Dapper-Extensions必须知道哪一个属性是POCOs的标识。

下列KeyTypes是可用的：

public enum KeyType{

NotAKey,

Identity,

Guid,

Assigned

}

## NotAKey

非主键属性映射到该枚举值。

## Identity

任何整形的主键将被映射成这个枚举值。

## Guid

Guid类型的主键属性被映射成这个枚举值。这个标识会使得Dapper Extensions在数据库执行插入命令前先生成一个新的Guid值。Dapper-Extensions生成[COMB](http://davybrion.com/blog/2009/05/using-the-guidcomb-identifier-strategy/)(see: GetNextGuid)

## Assigned

任意非Guid和整形的主键将会映射到该枚举值。这个标识会使得Dapper Extensions在数据库执行插入命令前不会生成一个key。再者，Dapper-Extensions在插入后不会获得任意一个键的值，总而言之，这个是由开发者决定的。即自定义主键值或者是联合主键时的标识。

# [Predicates](https://github.com/tmsmith/Dapper-Extensions/wiki/Predicates)

谓词系统在Dapper-Extensions中是非常容易去使用的。现在我们使用下面这个Model来展开它的用法。

public class Person

{

public int Id { get; set; }

public string FirstName { get; set; }

public string LastName { get; set; }

public bool Active { get; set; }

public DateTime DateCreated { get; set; }

}

## Simple FieldPredicate Operation

为了创建一个简单的谓词，我们仅仅创建一个字段谓词并且把它传给查询操作。FieldPredicate需要一个一般的类型，允许使用强类型。

下面的已给例子，我们返回Active值为true的所有Persons。

Code

using (SqlConnection cn = new SqlConnection(_connectionString))

{

cn.Open();

var predicate = Predicates.Field<Person>(f => f.Active, Operator.Eq, true);

IEnumerable<Person> list = cn.GetList<Person>(predicate);

cn.Close();

}

Generated SQL

SELECT

[Person].[Id]

, [Person].[FirstName]

, [Person].[LastName]

, [Person].[Active]

, [Person].[DateCreated]

FROM [Person]

WHERE ([Person].[Active] = @Active_0)

IN Clause TODO: Demonstrate that you can pass an IEnumerable as the value to acheive WHERE x IN ('a','b') functionality

## Compound Predicate (Predicate Group)复合谓词

复合谓词谓词可以通过谓词组来实现。对于每个谓词组，你必须选择一个操作符（AND/OR），每个被添加到组中的谓词将会被加入具体的操作。

如果每个谓词组都实现了IPredicate，那么多谓词可以被同时添加到一起。

下面的例子，我们建立一个通过AND操作符连接的谓词组。

Code

using (SqlConnection cn = new SqlConnection(_connectionString))

{

cn.Open();

var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

pg.Predicates.Add(Predicates.Field<Person>(f => f.Active, Operator.Eq, true));

pg.Predicates.Add(Predicates.Field<Person>(f => f.LastName, Operator.Like, "Br%"));

IEnumerable<Person> list = cn.GetList<Person>(pg);

cn.Close();

}

Generated SQL

SELECT

[Person].[Id]

, [Person].[FirstName]

, [Person].[LastName]

, [Person].[Active]

, [Person].[DateCreated]

FROM [Person]

WHERE (([Person].[Active] = @Active_0)

AND ([Person].[LastName] LIKE @LastName_1))

## Multiple Compound Predicates (Predicate Group)

只要每个谓词组都实现了IPredicate，你可以创建复杂的复合谓词，并把它们联合起来。

在下面的例子中，我们创建两个谓词组，并且把它们组合在第三个谓词组中。

Code

using (SqlConnection cn = new SqlConnection(_connectionString))

{

cn.Open();

var pgMain = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };

  


var pga = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

pga.Predicates.Add(Predicates.Field<Person>(f => f.Active, Operator.Eq, true));

pga.Predicates.Add(Predicates.Field<Person>(f => f.LastName, Operator.Like, "Br%"));

pgMain.Predicates.Add(pga);

  


var pgb = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

pgb.Predicates.Add(Predicates.Field<Person>(f => f.Active, Operator.Eq, false));

pgb.Predicates.Add(Predicates.Field<Person>(f => f.FirstName, Operator.Like, "Pa%", true /* NOT */ ));

pgMain.Predicates.Add(pgb);

  


IEnumerable<Person> list = cn.GetList<Person>(pgMain);

cn.Close();

}

Generated SQL

SELECT

[Person].[Id]

, [Person].[FirstName]

, [Person].[LastName]

, [Person].[Active]

, [Person].[DateCreated]

FROM [Person]

WHERE

((([Person].[Active] = @Active_0) AND ([Person].[LastName] LIKE @LastName_1))

OR (([Person].[Active] = @Active_2) AND ([Person].[FirstName] NOT LIKE @FirstName_3)))

## PropertyPredicate

TODO

## Exists Predicate

var subPred = Predicates.Field<User>(u => u.Email, Operator.Eq, "[someone@somewhere.com](mailto:someone@somewhere.com)");

var existsPred = Predicates.Exists<User>(subPred);

var existingUser = cn.GetList<User>(existsPred , null, tran).FirstOrDefault();

以上就是Dapper-Extensions所有的文档了。
