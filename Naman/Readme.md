# Basics of .NET and C#

## C# vs .Net

• C# is a programming language
• .NET is a framework for building applications on windows
• .Net is not limited to C# there are many languages that can target .NET framework and build applications example F# and VB.NET

## .NET

• .NET framework consist of two components one is called CLR(Common language runtime) and the other is a class library for building applications

## CLR
• When you compile your C# code the result is called IL or intermediate language code
• which is independent of the computer on which it is running
• Now to convert this IL code to native code or the machine code for the machine that is running
• That is done by CLR(Common Language Runtime)
• So CLR is an application that is sitting in the memory whose job is to translate the IL code into the machine code and this process is call JIT(Just in time compilation)
• So with this process you can compile your code in one machine and you don't need to worry about running your code on different machine as long as a machine has CLR
 
## Architecture of .NET Application
• Your application consists of building blocks called classes
• These classes collaborate at the runtime and as a result gives some functinality

### Class
• A class is a container which containes data called attributes and functions which is also called methods

### Namespace
• As the number of classes grows we need to organize these classes so we need a namespace
• A namespace is a container for related classes


### Assembly

• As the number of namespaces grow we need a way to partition our application that is why we use an assembly
• An assembly is a container for related namespaces
• Physically it is a file on the disk which can either be an executable(.exe) or a DLL(Dynamically Linked Library)

So when you compile your application the compiler builds one or more assemblies depending on how you partition your code

### Class 

A class is a blueprint for creating objects which contains attributes and features.

### Solution
It provides architecture for organizing projects inside visual studio
One solution can have multiple projects

### Project
Project is a structure for organizing files and in a single project
It contains project information and the nuget package references when added would be displayed here

### launchSettings.json
Contains launch related information, profiles and launch urls

### appsettings.json
It is used to store the configurations of the application, log levels and connection strings reside in this file

### Program.cs
It is the entry point of the application, here we add the dependencies used in our project and add the middlewares

### Middleware
Middleware are the softwares which are added in the project pipeline to handle request and responses

 
# REST (Representational State Transfer)
1. It is a style of architecture for building web services.
2. REST is a set of principles that define how services should be designed and interact with each other.
3. It is based of the concept of resources, a resource is anything that can be identified and manipulated through web service.
4. Each URL define a specific resource and http verbs are used tro perform action on these resources.
5. REST follows stateless style of architexture, that means server should not store client state between requests and client should provide each and every information with each request.


db reference - "https://user-images.githubusercontent.com/36097162/236610117-425903b3-88da-4064-9fb2-10903c42cfd5.png"
