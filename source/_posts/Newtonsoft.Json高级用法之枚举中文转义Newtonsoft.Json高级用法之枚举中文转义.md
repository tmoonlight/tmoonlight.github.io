---
title: Newtonsoft.Json高级用法之枚举中文转义Newtonsoft.Json高级用法之枚举中文转义
date: 2019/1/6 7:13:35
tags:
---


 最近看博客园中 [焰尾迭](http://home.cnblogs.com/u/yanweidie/)[ ](http://home.cnblogs.com/u/yanweidie/)的两篇关于"Newtonsoft.Json高级用法"的文章受到了很多人的评论，一度登入到头条推荐。

今天我就不再重复[焰尾迭](http://home.cnblogs.com/u/yanweidie/)博文中的一些提过的Newtonsoft.Json的高级用法。大家如果想知道直接去看。

#### [Newtonsoft.Json高级用法](http://www.cnblogs.com/yanweidie/p/4605212.html)

#### [再谈Newtonsoft.Json高级用法](http://www.cnblogs.com/yanweidie/p/5155268.html)

我主要说[焰尾迭](http://home.cnblogs.com/u/yanweidie/)[ ](http://home.cnblogs.com/u/yanweidie/)没有提到的用法——枚举中文转义

# 枚举值序列化问题（摘自[焰尾迭](http://home.cnblogs.com/u/yanweidie/)[ ](http://home.cnblogs.com/u/yanweidie/)段落）

public enum NotifyType

{

/// <summary>

/// Emil发送

/// </summary>

Mail=0,

  


/// <summary>

/// 短信发送

/// </summary>

SMS=1

}

  


public class TestEnmu

{

/// <summary>

/// 消息发送类型

/// </summary>

public NotifyType Type { get; set; }

}

JsonConvert.SerializeObject(new TestEnmu());

输出结果：![](https://images2015.cnblogs.com/blog/336693/201509/336693-20150910171743028-388882267.png)  现在改造一下，输出"Type":"Mail"

![](https://common.cnblogs.com/images/copycode.gif)

![](https://common.cnblogs.com/images/copycode.gif)

public class TestEnmu

{

/// <summary>

/// 消息发送类型

/// </summary>

[JsonConverter(typeof(StringEnumConverter))]

public NotifyType Type { get; set; }

}

![](https://common.cnblogs.com/images/copycode.gif)

![](https://common.cnblogs.com/images/copycode.gif)

 

其它的都不变，在Type属性上加上了JsonConverter(typeof(StringEnumConverter))表示将枚举值转换成对应的字符串,而StringEnumConverter是Newtonsoft.Json内置的转换类型,最终输出结果

![](https://images2015.cnblogs.com/blog/336693/201509/336693-20150910172109044-1714459886.png)

 

 

# 思考

到这里StringEnumConverter确实为我们解决了很多问题，从枚举值类型转到名称。 如果我们是英国人该多好呀，到这里Newtonsoft.Json 自带的Converter已经为我们解决一切问题。

谁叫我们是中国人，到到枚举转成Mail还是很不爽。

如果能把枚举转成”电子邮箱“才符合我们中国人的习惯。

{

"Type":"电子邮箱"

}

# 枚举值中文序列化

首先根据上面的目标我们首先改造下我们的枚举类型

![](https://common.cnblogs.com/images/copycode.gif)

public enum NotifyType

{

/// <summary>

/// 电子邮箱

/// </summary>

[Description("电子邮箱")]

Mail = 0,

  


/// <summary>

/// 手机短信

/// </summary>

[Description("手机短信")]

SMS = 1

}

![](https://common.cnblogs.com/images/copycode.gif)

接下来我们自定义一个EnumJsonConvert 类，考虑到我们枚举可能会传入不同类型，我们这里使用泛型

![](https://common.cnblogs.com/images/copycode.gif)

public class EnumJsonConvert<T>:JsonConverter where T: struct, IConvertible

{

public void EnumConverter()

{

if (!typeof (T).IsEnum)

{

throw new ArgumentException("T 必须是枚举类型");

}

}

  


public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)

{

if (reader.TokenType == JsonToken.Null)

{

return null;

}

  


try

{

return reader.Value.ToString();

}

catch (Exception ex)

{

throw new Exception(string.Format("不能将枚举{1}的值{0}转换为Json格式.", reader.Value, objectType));

}

}

  


/// <summary>

/// 判断是否为Bool类型

/// </summary>

/// <param name="objectType">类型</param>

/// <returns>为bool类型则可以进行转换</returns>

public override bool CanConvert(Type objectType)

{

return true;

}

  


  


public bool IsNullableType(Type t)

{

if (t == null)

{

throw new ArgumentNullException(nameof(t));

}

  


return t.BaseType != null && (t.BaseType.FullName == "System.ValueType" && t.GetGenericTypeDefinition() == typeof(Nullable<>));

}

  


public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)

{

if (value == null)

{

writer.WriteNull();

return;

}

string bValue = value.ToString();

int isNo;

if (int.TryParse(bValue, out isNo))

{

bValue= GetEnumDescription(typeof(T), isNo);

}

else

{

bValue= GetEnumDescription(typeof(T), value.ToString());

}

  


writer.WriteValue(bValue);

}

  


/// <summary>

/// 获取枚举描述

/// </summary>

/// <param name="type">枚举类型</param>

/// <param name="value">枚举名称</param>

/// <returns></returns>

private string GetEnumDescription(Type type, string value)

{

try

{

FieldInfo field = type.GetField(value);

if (field == null)

{

return "";

}

  


var desc = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

if (desc != null) return desc.Description;

  


return "";

}

catch

{

return "";

}

}

  


/// <summary>

/// 获取枚举描述

/// </summary>

/// <param name="type">枚举类型</param>

/// <param name="value">枚举hasecode</param>

/// <returns></returns>

private string GetEnumDescription(Type type, int value)

{

try

{

  


FieldInfo field = type.GetField(Enum.GetName(type, value));

if (field == null)

{

return "";

}

  


var desc = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

if (desc != null) return desc.Description;

  


return "";

}

catch

{

return "";

}

}

}

![](https://common.cnblogs.com/images/copycode.gif)

到这里我们自定义的JsonConvert已经实现好了。开始使用吧。

我们定义一个类Myclass

![](https://common.cnblogs.com/images/copycode.gif)

public class MyClass

{

public string UserName { get; set; }

  


[JsonConverter(typeof(EnumJsonConvert<NotifyType>))] // 这里就是 我们特殊的地方

public int NotifyType { get; set; }

}

![](https://common.cnblogs.com/images/copycode.gif)

我们写个测试看对不对：

![](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAhgAAAB/CAIAAAAiiCyMAAALeklEQVR4nO3cu44ktxXG8U4MG5BhKJFlw1iHThQJUKI3UKhQj6AX2MeYQOE4tQJnSvwyygyndmBg4UBYSOVgsAUOz4WHPKxb9/+HxaKazSJZXZevWd09twUAgITb0QMAAFwbQQIASCFIAAApBAkAIIUgAQCkECQAgBSCBACQQpAAAFJCQXL7YMNxbN8FAGAL0SApH3723Q/lv5mjIUgA4GoGg6T6v5e1FkECAJczEiQvMnMRggQA7sbMIFFvdlU3wfzbYgQJAFzOtCApH67LaqFc1+8IAHBmM4NEnWqoMw+CBADuxiYzkuHKBAkAXM7Bt7aqRggSALicwSDxP1eXJdYn882OAAAnxy/bAQApXLgBACkECQAghSABAKQQJACAlK2C5Pn5ebe1AAAHOiZInl9z1nq2bTJuAECndpAMXLhfKgdXaQZJsCYA4BCNIBm4dveuQpAAwKVNDpK1jlyoqnFrCwDuw7QgcSJBXvetppiRAMDlzL+1FalMkADA3TgsSLi1BQD34ZggiXfBjAQATo4gAQCkbPI7koUgAYCHcdifSFHzKfLlLj4jAYBT4Y82AgBSCBIAQApBAgBIIUgAACkECbZ1u92qBQB3Zs65fbst6lXipdx6ah9WR9bAigqvTBrM/DarZme1OUU1nrMND8AUE69lI08dqxkk1f/97etrzb2elq0dcqV2OiVIgEdAkDTrjI9+hyA5w6W5azPPMGAAc3ln9XpjqrwL5C+r94uqEqeacytM9mgNYEpHReW6knoTqbq55N/CctqsphdlSdW41VTv6rJHazNlC72b6Q8YwEU1zurqutxbqD5s1mndcUr1Hu/oQx3z5ox6NY/caBpuU0ZIpAtn9YEtCgaGVU6QAPfnjEEya0jNh2NB4swe/HWdcrl68PreGyTxQmszI5szUBnAdV0vSNbKVo+7zUiGK3dd/Rf3op8PErWpKdlAkAAPIhUkt5tSuLQu5c06zUvNSYIkfmvLD5j87CFTuC5HbtD52dCck5EiwF1qB8lN+1x6LakWbsbH3TfjY3m1cmjcolqwd6dc66W+t6MWqneByqdkodqg31H50HlqYHU5pCmbqW6prAPg6jpmJEhy3rnfscfcauCh+Hf8O6YId+bzz5ff/O7drz/+91fff/X+5/dTSta389NbPk/Jp398v//OAnCsh0yJgDd//vnrv3/95Ze//P4v//z2H99+883y8R/+S0mk5OhdB2BvBInux//8+O6nd8uyvHmz/PZP//rVR//74q9fUBIpAfBoCBIAQApBAgBIiQaJ9f1OAMCD6wiSTccBALgoggQAkEKQAABSCBIAQEooHkgRAICFGQkAIIUgAQCkECQAgBSCBACQwi/bAQApZAMAIIUgAQCkECQAgBSCBACQQpAAp/a3Tz6Z28jLcqTZKV3jERAkmObp6alaiHvMrwXKa/paYv2rVm/WaTbbHF588Bh29eP/wkNXXX1/OD777oeXf9Vys/5aLbLWsCo8erNk3Wu3D6rljC3anEK9FvsPm081C8tOtw6SWa+8ulaktdtrA11vRA7mVMPrdeGhWyLH1j4jma4KEr+muiwfzjIrSJzljPI6cpIDwL8QR2YPauSo1crVpwRJfDrS9crHd02wzTXDuhrfmppq5xnegAsP3UKQxDNmFjU2urLkAYMkyL+aBz/82OLWVtDWQTK98j4Iklx/t6V8ucqHL8vqs1W50bIyga1KnHluZPJ7E5qbKUdebWZZs7mlMkjU+129QaKupbZmdbFPkER28VJcueQlTL4zPeoIsfR+RrKI/PCDRNZvjqR3EyrJV94pUVe3VlEblPtrt/0u61hrDRxF+ztgiFVUOIVLLEIW49LjHEmR1ZOqtKgWZGH5v2UNjKXIA3kXqytI1Jtg8cIXySCRp65c7t3Fapvl/36bkZobce5QWSVyXiJrrjl0hiCJn5tWeddZPP0Ay1Nbu0RmqM4SJOtDOSmJtekdQ87bE7+mVSH2JkVZXjdQxsxwkMiSgVtb1uRDrSZX33NGUq2l7g7rKlNdzqpGnJrB/d51hPi6PiPxg6SMkK5veVkNdtktSGQvzdWbw9huvwe36CqOGbp60WzOTtwG2weEWsGvmdGceFXPHhgkzopW6lQle35GkrzuWJHTDJLghswSn5Got7acr37Fb4U5FeLUV/5CQaKOJ48gmdGruGhGlt0GO8LDunbI5YxmkMjlZs9WkPgPB2r23tpatv/W1tguVleJtBl8B72F+AfjzoftmSCJlKjUN+NjQVK1E7/s+m8TncKxN6DDCJJJHYueb+K2z03cBXIbVKaW1nxTHvFWzTHqVsinyvqL9pqUyg/VF3GXKfIRevlRuV/e9Un7i4lBshS7Q5YM1JQP1cpltfV/q5GNTLm+Wx/LVwuRrxQ3yyv+3lFfedmCs4NkoT8AvyO1WvMAy7M6sipP7HojFxgiIjb6dUgXftk+kUwCZ5Lhr159zO6snvw28Bn2YHwMwev4Ppzj/wyvatMFhgjHpj9WB64iPm/YbdrxUHjhAAApBAkAIIUgAQCkECQAgBSC5P7xESJwTpuem2rjG/V4zCUm+OcFZ/UysO76FY71/+YOGNtD/tdF8nt9beHtB9XyaQ2P8O1r0we2sn6XM7HZWW1uwfkxR7yFeC9jq/vNWg/n9tXV5twvj+2WJQcEif8zuk2761IFyXALs6qNKRsvL6zD1+g5wwp3VPbY1ftuQ517DO98dvgDcFRX4c2Gs0kXa2s7bIXT7PDbx+HfyvSuHrd3kOx/bjxykFQtXzFIhnu/YpCcYQoyECQ72O5NuhoqEw2kxdybH/tkyYmCpJrOV3+uQ62p/gGPZo/B+wZqkJS3vKpCOVNWZ+VqZbV3a/YdaVNteb2vtYh3+lXGVDWlqllrdatms/dIoU9Wi/euDt7iHGDO8ake3smzw/n7zdZ45EL8rlrmuHWO2MiR7J9Z1sloNSVXj/dlDcAZjzoMawBy8F3jVDvqKgw6S5BYfyVQnmO9f0/QeeifLevRsLx+idW9tRhHhlNiHdlOC+r5oLapPlSD5K1278iJnGqE1uqyfacjf9kvtMgMC/auDt4ROcCCh/fOZ0fwjZdDXvWs5cgpEz+S/Tr+kHrXirQZqRAc/FibyZcu7+xBYlWIx4Mzm2kGSfnOoiy36kdK5BuKriCRy/6blPKhFSTqDEDWXHqCxKoQub6rHVmFFmtGMjx4izUjseo4h3f+7HAiR52URDanqXkoxk+ZspFIzWahHNKUIImfxZkgqbqLtJl56TIuGST+s/EZSZN/5KmVe0ucBv2uB3Io/qZ7oyDxn906SII95oNElu8WJGpTkWablR2zLqZOU5kgCVboCpLgYOLlVvvB4Vk1e0c7bO8gWWa8vQoWNh+uyzLG1/IltmPUcrlrew+R3iDxT4BmkMhl6y38wA2ribe21A2xKk8Z0tPTk/UnjTO3tqYXLq0gic9I/LOj61Cc8q68ubo6En+c8QEPX+idlJ3YZtcrPz1FlkOCZNE++pOF5UN12V+9qqmWryXxILkV1EJ5Aqgl5RGgri4LrWX5UG7Con2qvNapniorBG8QVS2vD9Vlf3Wnpuy9K0givVuD7wqSzOEtH/auLodk1VE7Wp9aH6qHln/AD58yzmHvnx2RNuU4I1u0PutvplWo9uW3Gd+oyOrNfpvlEccECfaUOT4uR03KLagzA9yrjU6iTc/NrhRJDuaBLjF4BNZUZjrr7TzuiTNHQYkXCACQQpAAAFIIEgBACkECAEhpBAkfNAEAfO2QIEgAAA6CBACQQpAAAFIIEgBASigkyBIAgIUZCQAghSABAKQQJACAFIIEAJDCL9sBACmEBAAghSABAKQQJACAFIIEAJBCkAAAUggSAEAKQQIASCFIAAApBAkAIIUgAQCkECQAgBSCBACQ8n9JYBy7uh/SeQAAAABJRU5ErkJggg==)

 

结果如下：

![](data:image/png;base64,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)

我们实现了最初的目标，一般我们数据库到保存着枚举的值类型，比如这里的 0，1，2，3……。 我们前端一般需要显示我们的语言，比如这里的电子邮箱，手机短信……

# 总结

其实这里自定义实现了一个json自定义类型转换。利用枚举的Description特性利用反射实现枚举中文转义。其实这类转义在我们平时开发过程中经常见到。用JsonConvert可以给我们带来一定的解耦。

省去if esleif 等判断。

从上到下，主要只讲解了一个问题，字里行间语言上都有很多的不严谨。不过知识点涉及到Newtonsoft.Json的自定义类型转换，反射等。代码全部贴上，希望对大家有所帮助。谢谢。

再次感谢[焰尾迭](http://home.cnblogs.com/u/yanweidie/)的分享。 谢谢！，同时也希望也能和你一样被大家推荐。谢谢。

 

  

