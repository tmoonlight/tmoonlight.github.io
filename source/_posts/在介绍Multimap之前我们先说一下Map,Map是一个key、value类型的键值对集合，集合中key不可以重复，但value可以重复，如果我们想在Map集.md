---
title: 在介绍Multimap之前我们先说一下Map,Map是一个key、value类型的键值对集合，集合中key不可以重复，但value可以重复，如果我们想在Map集
date: 2015/2/27 12:02:17
tags:
---


#   


在介绍Multimap之前我们先说一下Map,Map是一个key、value类型的键值对集合，集合中key不可以重复，但value可以重复，如果我们想在Map集合中存入一个相同的key，不同的value值得时候就必须使用Map<Integer,List<Object>>模式来存数据，这样做很麻烦，而且编程效率又很低；

##### 1\. 现在我们来介绍一种更高效的集合Multimap，它可以很简单的实现上面我们所说的功能，先看下Multimap接口的源码

@GwtCompatiblepublic interface Multimap<K, V> {

//返回Multimap集合的key、value pair的数量

int size();

//判断Multimap是否包含key、value pair

boolean isEmpty();

//判断Multimap中是否包含指定key的value值

boolean containsKey(@Nullable Object key);

//判断Multimap中是否包含指定value的key

boolean containsValue(@Nullable Object value);

//判断Multimap中是否包含指定的key-value pair的数据

boolean containsEntry(@Nullable Object key, @Nullable Object value);

//将数据加入到Multimap中

boolean put(@Nullable K key, @Nullable V value);

//删除Multimap中指定key-value pair

boolean remove(@Nullable Object key, @Nullable Object value);

//将指定的key-集合数据加入Multimap中

boolean putAll(@Nullable K key, Iterable<? extends V> values);

//将指定的Multimap和当前的Multimap合并

boolean putAll(Multimap<? extends K, ? extends V> multimap);

//替换指定key的value

Collection<V> replaceValues(@Nullable K key, Iterable<? extends V> values);

//删除Imultimap中的指定key数据

Collection<V> removeAll(@Nullable Object key);

//清空Imultimap中的数据

void clear();

//获取指定key的值

Collection<V> get(@Nullable K key);

//获取所有的key集合

Set<K> keySet();

  


Multiset<K> keys();

  


Collection<V> values();

  


Collection<Map.Entry<K, V>> entries();

  


Map<K, Collection<V>> asMap();

  


@Override

boolean equals(@Nullable Object obj);

  


@Override

int hashCode();

}

  


Multimap接口的实现类HashMultimap使用方法详解

创建HashMultimap对象

Multimap<Integer, Integer> map = HashMultimap.<Integer, Integer>create();

  


map.put(1, 2);

map.put(1, 2);

map.put(1, 3);

map.put(1, 4);

map.put(2, 3);

map.put(3, 3);

map.put(4, 3);

map.put(5, 3);

System.out.println(map);

  


{1=[4, 2, 3], 2=[3], 3=[3], 4=[3], 5=[3]}

1

从上面的结果集可以看出，key不可以重复，相同key的key-value pair 的value值是放在同一个数组中，相同的value会去重。

常用的方法示例

//判断集合中是否存在key-value为指定值得元素

System.out.println(map.containsEntry(1, 2));

System.out.println(map.containsEntry(1, 6));

//获取key为1的value集合

Collection<Integer> list = map.get(1);

System.out.println(list);

//返回集合中所有key的集合，重复的key将会用key * num的方式来表示

Multiset<Integer> set = map.keys();

System.out.println(set);

//返回集合中所有不重复的key的集合

Set<Integer> kset = map.keySet();

System.out.println(kset);

  


truefalse[4, 2, 3][1 x 3, 2 x 2, 3, 4, 5][1, 2, 3, 4, 5]

  


replaceValues替换Multimap中指定key的值

Collection<Integer> coll = map.replaceValues(1, Arrays.asList(1,7,8,9,10));

System.out.println(coll);

System.out.println(map);

  


[4, 2, 3]{1=[8, 9, 1, 10, 7], 2=[4, 3], 3=[3], 4=[3], 5=[3]}

  


replaceValues方法会替换掉key的value值，并且返回之前对应的值。

##### 2\. ImmutableMultimap不可变集合

ImmutableMultimap中提供了三个主要的静态方法of、builder、copyof;

//创建一个静态不可变的Multimap对象

Multimap<Integer, Integer> map = ImmutableMultimap.<Integer, Integer>of();

Multimap<Integer, Integer> map1 = ImmutableMultimap.<Integer, Integer>builder().build();

//从另外一个集合中复制数据元素到Multimap对象中

Multimap<Integer, Integer> map2 = ImmutableMultimap.copyOf(map);

  


##### 3.LinkedHashMultimap实现类

LinkedHashMultimap实现类与HashMultimap类的实现方法一样，唯一的区别是LinkedHashMultimap保存了记录的插入顺序，在使用Iterator循环遍历的时候先得到的肯定是先放入Multimap中的数据。

Multimap<Integer, Integer> map = LinkedHashMultimap.<Integer, Integer>create();

map.putAll(4, Arrays.asList(5,3,4,2,1,56));

map.putAll(3, Arrays.asList(3,4,2,6,8,7));

map.put(1, 2);

System.out.println(map);

  


运行结果是：

{4=[5, 3, 4, 2, 1, 56], 3=[3, 4, 2, 6, 8, 7], 1=[2]}

  


##### 4.TreeMultimap实现类

TreeMultimap类继承成了Multimap接口，它的所有方法跟HashMultimap一样，但是有一点不同的是该类实现了SortedSetMultimap接口，该接口会将存入的数据按照自然排序，默认是升序。

Multimap<Integer, Integer> map = TreeMultimap.<Integer, Integer>create();

map.putAll(4, Arrays.asList(5,3,4,2,1,56));

map.putAll(3, Arrays.asList(3,4,2,6,8,7));

map.put(1, 2);

System.out.println(map);

  


返回的结果是：

{1=[2], 3=[2, 3, 4, 6, 7, 8], 4=[1, 2, 3, 4, 5, 56]}

Multimap接口的实现类不仅仅只有HashMultimap、TreeMultimap、LinkedHashMultimap、ImmutableMultimap这几种，还有其它的实现类，我会在以后的学习过程中详解。

  

