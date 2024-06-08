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


### REST (Representational State Transfer)
1. It is a style of architecture for building web services.
2. REST is a set of principles that define how services should be designed and interact with each other.
3. It is based of the concept of resources, a resource is anything that can be identified and manipulated through web service.
4. Each URL define a specific resource and http verbs are used tro perform action on these resources.
5. REST follows stateless style of architexture, that means server should not store client state between requests and client should provide each and every information with each request.

### ORM(Object Relation Model) 
1. ORM connects object oriented programming to relational databases.
2. ORM simplifies the interaction with database.

### Entity Framework
1. Entity Framework is a ORM framework.
2. EFCore supports two development approaches 1. Code First 2. Database First

### Dapper
1. Dapper is a micro ORM.
2. Dapper is used to map queries to objects.
3. Dapper does not do SQL generation, cache results and other features that ORM like EFCore provides that is why it is called micro ORM.

### DbContext
1. DbContext is used to reporesent a session with the database and can be used to query and save instance of your entities.
2. DbContext is used for 
    1. Manage database connection
    2. Configure model & relationship
    3. Querying database
    4. Saving data to the database
    5. Configure change tracking
    6. Caching
    7. Transaction management


## Authentication
The process to determine a user's identity using username and password, it checks if we trust the user

## Authorization
1. It specifies if user has permission to perform certain action
2. Users are given permission based on roles, policies, claims

## Authentication flow
1. Server creates JWT token and pass it to client.
2. JWT(Json Web Token) is an compact and self-contained way for securely transmitting information between parties as a JSON object.
3. Client first sends the username and password to the API and API returns JWT token then the client uses this JWT token to make the calls and API verifies this JWT token on each call if the JWT token is right then it returns the data else no data is retruned.

## Packages Required for setting up of Authentication
1. Microsoft.AspNetCore.Authentication.JwtBearer
2. Microsoft.IdentityModel.Tokens
3. System.IdentityModel.Tokens.Jwt
4. Microsoft.AspNetCore.Identity.EntityFrameworkCore

## Process to add Authentication
1. Add the JWT configurations in the appsettings.json
    ```Javascript
        "Jwt": {
        "Key": "lksadjfkljagioherohgljksdglkjsdkljfgsdjkgoifjeriojgklsdfjlkjdslkjfd",
        "Issuer": "https://localhost:7147",
        "Audience": "https://localhost:7147",
        }
    ```

2. Add the AddAuthentication service inside program.cs
```C#
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });
```

3. Before the Authorization middleware add UseAuthentication middleware
```C#
    app.UseAuthentication();
    app.UseAuthorization();
```

4. Add a new DbContext for the authentication 
    1. Create a new Authentication connection string
        ```Javascript
            "ShopAuthConnection": "Server=localhost;Database=ShopAuthDb;User=root;Password=*october2020"
        ```
    2. Add the new DbContext
        ```C#
                public class ShopAuthDbContext : IdentityDbContext
                {
                    public ShopAuthDbContext(DbContextOptions<ShopAuthDbContext> options): base(options) 
                    {

                    }
                }
        ```
    3. Register the new DbContext
        ```C#
            builder.Services.AddDbContext<ShopAuthDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(AuthConnectionString));
            });
        ```

5. Add the roles 
    ```C#
            protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var customerRoleId = "11df2da6-10ef-4b25-a24a-324d17ec7cfd";
            var adminRoleId = "45ec6e0b-9541-4954-b4a1-94250dd4cbc7";

            var roles= new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = customerRoleId,
                    ConcurrencyStamp = customerRoleId,
                    Name = "Customer",
                    NormalizedName = "Customer".ToUpper()
                },

                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    ```

6. Run the migrations but with specifying the DbContext
    ```C#
        Add-Migration "Creating Auth Database" -Context "ShopAuthDbContext"
        Update-Database -Context "ShopAuthDbContext"
    ```



db reference - "https://user-images.githubusercontent.com/36097162/236610117-425903b3-88da-4064-9fb2-10903c42cfd5.png"
