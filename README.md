# factory-girl

[![Build status](https://ci.appveyor.com/api/projects/status/tqxdkb8yik3fv4sp/branch/master?svg=true)](https://ci.appveyor.com/project/UmairChagani/factory-girl/branch/master)

A library for setting up .NET objects as test data.  Inspired by the original [factory_girl](https://github.com/thoughtbot/factory_girl) for ruby, this implementation offers a simpler, feature-concise version for .NET that was influenced by [FactoryGirl.NET](https://github.com/JamesKovacs/FactoryGirl.NET) and [Plant](https://github.com/jbrechtel/plant).  

## Getting Started

Install factory-girl from Nuget

```
Install-Package factory-girl
```

# Documentation

In the beginning, there was a model

### Model

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}
```

To get started, implement the ```IDefinable``` interface. 

### Factory

```csharp
public class UserFactory : IDefinable
{
    public void Define()
    {
        FactoryGirl.Define<User>(() => new User
        {
            Name = "Bruce Wayne",
            Address = "Wayne Manor, Gotham City"
        });
    }
}
```

By implementing the interface, you can initialize all your factories in the given Type's assembly by calling the ```Initialize``` method.

```csharp
FactoryGirl.Initialize(typeof(UserFactory));
```


### Default and Named Factories

For each Type you can define two types of factories:  ```Default``` or ```Named```.  You can have as many uniquely ```Named``` factories for a given type as you would like but you can only have one ```Default``` type.

### Default Factory

```csharp
public void DefineDefaultUser()
{
    FactoryGirl.Define<User>(() => new User
    {
        Name = "Peter Griffin",
        Address = "31 Spooner Street, Quahog, RI"
    });
}
```

### Named Factories

```csharp
private void DefineSeriousUser()
{
    FactoryGirl.Define("SeriousUser", () => new User
    {
        Name = "Sirius Black",
        Address = "12 Grimmauld Place"
    });
}
```

```csharp
private void DefineAdminUser()
{
    FactoryGirl.Define("OldUser", () => new User
    {
        Name = "Fred Flinstone",
        Address = "301 Cobblestone Way"
    });
}
```

### Building Test Objects

Building test objects of your models is now possible.

To build a ```Default``` ```User```:
```csharp
FactoryGirl.Build<User>();
```

To build a ```Named``` ```User```:
```csharp
FactoryGirl.Build<User>("SeriousUser");
```

You can also build a list of objects by calling the ```BuildList``` method

```csharp
FactoryGirl.BuildList<User>();
```

If you need to clear your Factory call the ```ClearFactoryDefinitions``` method
```csharp
FactoryGirl.ClearFactoryDefinitions();
```

### Persisting Objects

To persist your object, implement the ```IRepository``` interface and use the ```Create``` and ```CreateList``` methods.  It is important to remember that, like the original ```factory_girl```, this implementation only calls ```Save()``` on the model.  What ```Save()``` does is up to you.  

```csharp
public class User : IRepository
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    
    public User Save()
    {
        // Your logic for saving the model to a Database.
        // Or calling a web service.
        // Or whatever persistence means to you!
        return this;
    }
}
```

Now we can call ```Create``` and ```CreateList``` on our Factory

```csharp
var user = FactoryGirl.Create<User>();
```

```csharp
// Create 5 Users
var users = FactoryGirl.CreateList<User>(5);
```
