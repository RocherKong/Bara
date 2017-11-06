# Bara
| Codacy | Appveyor | NuGet | Gitchat |
|--------|----------|-------|---------|
|[![Codacy Badge](https://api.codacy.com/project/badge/Grade/2a3146f3f09246d3a1a25826189aa656)](https://www.codacy.com/app/kx.1990/Bara?utm_source=github.com&utm_medium=referral&utm_content=RocherKong/Bara&utm_campaign=badger)|[![Build status](https://ci.appveyor.com/api/projects/status/w20ms4cct4nyl5ar/branch/master?svg=true)](https://ci.appveyor.com/project/RocherKong/bara/branch/master)|  [![NuGet](https://img.shields.io/nuget/v/Bara.svg)](https://www.nuget.org/packages/Bara/)|[![Join the chat at https://gitter.im/Bara](https://badges.gitter.im/Bara.svg)](https://gitter.im/ORM-Bara/Lobby?utm_source=share-link&utm_medium=link&utm_campaign=share-link)|

Bara is a .Net library for Orm. Bara provides quick access to DataBase. Also it is a cross-platform orm which is written by .net Standard version 1.4 . You Can use it on .net Framwork platform or .net Core platform with Any type DataBase which Dapper Support.
## Features
* Using Dapper for DataMapping and DataAccess. 
* Using Xml Config and Manage your sql like Ibatis.
* Hot Update Sql when you changed your sql sentence.

## Installing
Just install the [Bara NuGet package](http://www.nuget.org/packages/Bara/):

```
PM> Install-Package Bara
```

If your want Bara Extension.(A collection of commonly used methods already included)
```
PM> Install-Package Bara.DataAccess
```

## How To Use After Intalled
### 1.Add **BaraMapConfig.xml** To Root Path.
  > * Config whether the config need Watched.It will be update when setting true.
  > * Config DataBase ConnectString  
  > * Split Writen DB and Read DB. Specify each Database Weight which indicate the Access probability.
  > * Specify the ParameterPrefix（MSSQL:'@',Mysql:'#'...）
  > * Config your Maps which your sql in.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<BaraMapConfig xmlns="https://github.com/RocherKong/Bara/schemas/BaraMapConfig.xsd">
  <Settings
    IsWatchConfigFile="true"
  />
  <Database>
    <DbProvider Name="SqlClientFactory" ParameterPrefix="@" Type="System.Data.SqlClient.SqlClientFactory,System.Data.SqlClient"/>
    <Write Name="WriteDB" ConnectionString="Data Source=.;database=GoodJob;uid=sa;pwd=App1234"/>
    <Read Name="ReadDB-0" ConnectionString="Data Source=.;database=GoodJob;uid=sa;pwd=App1234" Weight="50"/>
    <Read Name="ReadDB-1" ConnectionString="Data Source=.;database=GoodJob;uid=sa;pwd=App1234" Weight="50"/>
  </Database>
  <BaraMaps>
    <BaraMap Path="Maps/" Type="Directory"></BaraMap>
  </BaraMaps>
</BaraMapConfig>
```
### 2.Config BaraMap Xml Files(eg:**T_Test.xml** in Root-Folder Maps)

### 3.Register To DI in Startup.cs
```c#
 services.AddSingleton<IBaraMapper>(MapperContainer.Instance.GetBaraMapper());
```

### 4.Use Any Where
#### 4.1 GetEntity
```c#
var mapper = new BaraMapper();
var result = mapper.QuerySingle<T_Test>(new Core.Context.RequestContext
  {
      SqlId = "GetEntity",
      Scope = "T_Test",
      Request = new { Id = 1 }
  });
```

#### 4.2 QueryList
```c#
var list= mapper.Query<T_Test>(new Core.Context.RequestContext
  {
      SqlId="GetList",
      Scope="T_Test",
  });
```

#### 4.3 Add
```c#
int i = mapper.Execute(new Core.Context.RequestContext
  {
      Scope = "T_Test",
      SqlId = "Insert",
      Request = new { Id = 4, Name = "Rocher4" }
  });
```

#### 4.4 Update
```c#
int i = mapper.Execute(new Core.Context.RequestContext
  {
      Scope = "T_Test",
      SqlId = "Update",
      Request = Entity
  });
```

## Cache Config
### Firstly You Need Config Cache in BaraMap file .
```xml
<Caches>
    <Cache Id="T_Test.LruCache" Type="Lru">
      <Parameter Key="CacheSize" Value="100"/>
      <FlushInterval Hours="0" Minutes="10" Seconds="0"/>
      <FlushOnExecute Statement="T_Test.Insert"/>
      <FlushOnExecute Statement="T_Test.Update"/>
    </Cache>
</Caches>
```
### Secondly You Need Relate Cache to your statement.
```xml
<Statement Id="QueryList" Cache="T_Test.LruCache">
  SELECT Top 10 T.* From T_Test T With(NoLock)
  <Include RefId="QueryParams"/>
</Statement>
```
### After config Metioned.The Same-Query after first Query Will fetch Data from Cache.Also it will dismiss after Insert or Update method executed. 

## Next Step 
* 0.Tags(Logic bit operator)
* 1.Test
* 2.Test Compare
* 3.redis cache
* 4.zookeeper(options)
* 5.add Doc 
* 6.Release

## Tips
* Can DataAccess be more Simple?(Auto Generate dao Code for Mapping Method Name.)

## Thanks
[Ahoo-Wang/SmartSql](https://github.com/Ahoo-Wang/SmartSql/):




