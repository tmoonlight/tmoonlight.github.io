---
title: 使用FluentAPI(EntityFramework)
date: 2018/6/17 2:49:18
tags:
---


## 1、使用FluentAPI

### 1.1、在OnModelCreating方法下使用

public class FluentApiDb:DbContext

{

public DbSet<Course> Courses { get; set; }

protected override void OnModelCreating(DbModelBuilder modelBuilder)

{

modelBuilder.Entity<Course>().Property(o => o.Title).HasMaxLength(20);

}

}

### 1.1、新增一个类，在OnModelCreating中引用

public class FluentApiDb:DbContext

{

public DbSet<Teacher> Teacher { get; set; }

protected override void OnModelCreating(DbModelBuilder modelBuilder)

{

modelBuilder.Configurations.Add(new TeacherMap());

modelBuilder.Configurations.Add(new StudentMap());

base.OnModelCreating(modelBuilder);

}

}

通过反射动态加入。利：自动加入，无需人工反复加；弊：影响性能。

public class FluentApiDb:DbContext

{

public DbSet<Teacher> Teacher { get; set; }

protected override void OnModelCreating(DbModelBuilder modelBuilder)

{

var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()

.Where(type => !String.IsNullOrEmpty(type.Namespace))

.Where(type => type.BaseType != null && type.BaseType.IsGenericType

&& type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

foreach (var type in typesToRegister)

{

dynamic configurationInstance = Activator.CreateInstance(type);

modelBuilder.Configurations.Add(configurationInstance);

}

  


base.OnModelCreating(modelBuilder);

}

}

  


## 2、FluentAPI详细用法

  * 设置主键

modelBuilder.Entity<x>().HasKey(t => t.Name);

  * 设置联合主键

modelBuilder.Entity<x>().HasKey(t =>new{t.Name,t.ID} );

  * 取消数据库字段标识（取消自动增长）

modelBuilder.Entity<x>().Property(t=>t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

  * 设置数据库字段标识（自动增长）

modelBuilder.Entity<Teacher>().Property(t =>t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

  * 设置字段最大长度

modelBuilder.Entity<ClassA>().Property(t => t.Name).HasMaxLength(100);

  * 设置字段为必需

modelBuilder.Entity<ClassA>().Property(t =>t.Id).IsRequired();

  * 属性不映射到数据库

modelBuilder.Entity<ClassA>().Ignore(t => t.A);

  * 将属性指定数据库列名：

modelBuilder.Entity<ClassA>() .Property(t => t.A) .HasColumnName("A_a");

  * 级联删除（数据库默认是不级联删除的）

modelBuilder.Entity<Course>().HasRequired(t => t.Department).WithMany(t => t.Courses).HasForeignKey(d => d.DepartmentID).WillCascadeOnDelete();

  * 设置为Timestamp

modelBuilder.Entity<OfficeAssignment>() .Property(t => t.Timestamp) .IsRowVersion();

  * 表1对0..1(Instructor实体可以包含零个或一个OfficeAssignment)

modelBuilder.Entity<OfficeAssignment>().HasRequired(t => t.Instructor).WithOptional(t => t.OfficeAssignment);

  * 表1对1

modelBuilder.Entity<Instructor>().HasRequired(t => t.OfficeAssignment).WithRequiredPrincipal(t => t.Instructor);

  * 表1对n（Department为主表）

modelBuilder.Entity<Staff>() .HasRequired(c => c.Department) .WithMany(t => t.Staffs)

  * 指定外键名（指定表Staff中的字段DepartmentID为外键）

modelBuilder.Entity<Staff>() .HasRequired(c => c.Department) .WithMany(t => t.Staffs) .Map(m => m.MapKey("DepartmentID"));

  * 表n对n

modelBuilder.Entity<Course>()

.HasMany(t => t.Instructors)

.WithMany(t => t.Courses)

  * 表n对n指定连接表名及列名

modelBuilder.Entity<Course>()

.HasMany(t => t.Instructors)

.WithMany(t => t.Courses)

.Map(m =>

{

m.ToTable("CourseInstructor");

m.MapLeftKey("CourseID");

m.MapRightKey("InstructorID");

});




  


  


作者：落地成佛

链接：<https://www.jianshu.com/p/c82ef6babf8e>

來源：简书

简书著作权归作者所有，任何形式的转载都请联系作者获得授权并注明出处。
