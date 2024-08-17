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

7. Add identity service in program.cs
    ```C#
        builder.Services.AddIdentityCore<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Shop")
            .AddEntityFrameworkStores<ShopAuthDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });
    ```


## CORS

Cross-Origin Reasource Sharing is a security feature implemented by web browsers to allow or restrict resources(such as HTML, CSS, JS, fonts and APIs) to be requested from a domain different from the domain from which the resource originated.

Same-Origin Policy(SOP) - enforced by browsers to restrict how documents or scripts loaded from one origin can interact with resources from another origin.

CORS Mechanism - CORS provides a way for a server to allow some cross-origin requests while still protecting its resources. It works by using HTTP headers to tell the browser whether to allow or deny a request from a different origin.

Key CORS Headers:

1. Access-Control-Allow-Origin: Specifies which origins are allowed to access the resource.
2. Access-Control-Allow-Methods: Indicates which HTTP methods are allowed (e.g., GET, POST).
3. Access-Control-Allow-Headers: Specifies which headers can be used during the actual request.
4. Access-Control-Allow-Credentials: Indicates whether credentials (such as cookies) are allowed to be sent in the cross-origin request.
5. Access-Control-Expose-Headers: Lists headers that are safe to expose to the API of a CORS API specification.

Example - 
- A JS application on 'http://example.com' makes request to 'http://api.example.org'.
- The browser first sends a preflight requesrt to 'http://api.example.org'.
- If the server responds with appropriate CORS headers, the browser allows the actual request to proceed.


## Class, methods and property

```C#
    public class Car{
        public string Name {get; set;}
        public string Mileage {get; set;}
    }

    public static void Main(string[] args){
        Car myCar = new Car();
        myCar.Name = "Maruti";
        Console.WriteLine(myCar.Name); //Maruti
        Console.WriteLine(myCar.Mileage); //0- default value of that datatype
        Console.WriteLine(myCar.gePrice); //100

        public int getPrice(){
            return 100;
        }
    }
```

## C# data types

### Value types
1. Integer
2. Float
3. Boolean
4. Enum
5. Nullable
6. Struct

### Reference types
1. Class
2. Interface
3. Array
4. Delegates

## Difference between Parse() and tryParse()

1. TryParse returns False when not able to convert but Parse throws error
2. In TryParse you need to provide the out parameter but not in Parse

## Structures

Struct is a value type data type that represent a data structure. It can contain constants, fields, parameterized constructors, static constructors,  methods, properties, operators

## Difference between struct and class

1. Struct is value type - stored in stack and class is reference type strored in heap
2. Struct cannot have explicit parameterless constructor, class can have parameterless constructor
3. Struct does not support inheritance except from when implementation of interface, class supports inheritance
4. Struct are more efficient in memory allocation because they are stored in stack, but not for large data

## Enum

Enum are used to assign names to a group of integers
``` C#
    enum WeekDays
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
    Console.WriteLine(WeekDays.Tuesday); //Tuesday
    int day = (int) Weekdays.Tuesday; //1
    var weekday = (WeekDays)5; //Saturday
```

## Passing string as a parameter in function
```C#
    static void ChangeReferenceType(string name)
    {
        name = "Steve";
    }

    static void Main(string[] args)
    {
        string name = "Bill";
    
        ChangeReferenceType(name);

        Console.WriteLine(name);
    }
```
Output - Bill
because here we are passing the string and string is reference type but it is immutable so compiler creates a new copy of string so original string value is mantained



## Basic questions on list manipulation 
```C#
    using System;
    using System.Collections.Generic;
    class HelloWorld
    {
    static int findPeak (List < int >arr)
    {
	    int len = arr.Count;
	    if (arr[0] > arr[1])
	    {
		    return 1;
	    }
	    else if (arr[len - 1] > arr[len - 2])
	    {
		    return len - 1;
	    }
	    for (int i = 1; i < len - 1; i++)
	    {
		    int j = i - 1;			//left element
		    int k = i + 1;			//right element
		    if (arr[i] > arr[j] && arr[i] > arr[k])
		    {
			    return i + 1;
		    }
	    }
	    return 0;
    }

    static int findMax (List < int >arr)
    {
	    arr.Sort ();
	    return arr[arr.Count - 1];
    }

    static int findMin (List < int >arr)
    {
	    arr.Sort ();
	    return arr[0];
    }

    static void reverseArr (List < int >arr)
    {
	    int end = arr.Count;
	    int start = 0;
	    int temp;
	    while (start < end)
	    {
		    temp = arr[start];
		    arr[start] = arr[end];
		    arr[end] = temp;
		    start++;
		    end--;
	      }
    }

    static void Main ()
    {
	    List < int >arr = new List < int >{ 10, 2, 3, 4, 11, 5, 6, 4 };
	    // Console.WriteLine(reverseArr(arr));
	    reverseArr (arr);
	    Console.WriteLine (findPeak (arr));
	    Console.WriteLine (findMax (arr));
	    Console.WriteLine (findMin (arr));
        }
    }
```

## Selection Sort

```C#
    using System;
    using System.Collections.Generic;
    class HelloWorld
    {
    static List<int> sortList (List < int >arr)
    {

        for(int i=0;i<arr.Count-1;i++){
            int smallestIndex = i;
            for(int j=i+1;j<arr.Count-1;j++){
                if(arr[j]<arr[smallestIndex]){
                    smallestIndex = j;
                }
            }
            int temp = arr[i];
            arr[i] = arr[smallestIndex];
            arr[smallestIndex] = temp;
        }
        for(int i=0;i<arr.Count-1;i++){
              Console.WriteLine(arr[i]);
        }
          return arr;
    }
    static void Main ()
    {
	    List < int >arr = new List < int >{ 10, 2, 3, 4, 11, 5, 6, 4 };
	    // Console.WriteLine(reverseArr(arr));
	    Console.WriteLine(sortList(arr));
    }
    }
```

## Sliding window for finding sum subarray
```C#
    using System;
    using System.Collections.Generic;
    class HelloWorld
    {
    static void sortList (List < int >arr, int target)
    {
        int sum = 0;
        int start = 0;
        int end = 0;
        int n = arr.Count;
        while(end<n){
            sum+=arr[end];
            if(sum>target){
                sum-=arr[start];
                start++;
            }
            if(sum == target){
                Console.WriteLine($"{start} {end}");
            }
            end++;
        }
    
    }
  

    static void Main ()
    {
	    List < int >arr = new List < int >{ 10, 2, 3, 4, 11, 5, 6, 4 };
	    // Console.WriteLine(reverseArr(arr));
        sortList(arr, 7);
    }
    }
```

## Delegates 
The delegate is a reference type data type that defines the method signature.
A delegate is a type that represents references to methods with a specific parameter list and return type.
Delegates are used to pass methods as arguments to other methods.

1. Define a delegate
```C#
public delegate void MyDelegate(string message);
```
2. Initialize and use delegate
```C#
using System;

public delegate void MyDelegate(string message);

class Program
{
    static void Main()
    {
        // Create an instance of the delegate and assign a method to it
        MyDelegate del = new MyDelegate(DisplayMessage);

        // Invoke the delegate
        del("Hello, World!");

        // Alternatively, you can use the shorter syntax
        del = DisplayMessage;
        del("Hello again!");
    }

    static void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }
}
```


## LINQ
LINQ provides a common syntax for querying the data from various data sources

### Advantages of LINQ
1. We don't need to learn different query languages
2. Less code
3. Provides compile time error checking
4. LINQ provides features such as filtering, sorting, ordering and grouping which makes the taks easier

### Disadvantages of LINQ
1. Not easy to write complex queries
2. if make change to the query we need to recompile the application and load dll on the server
3. Worst performance if query is not optimized

### IEnumerable
1. IEnumerable in C# is an interface that defines GetEnumerator method which returns IEnumerator object, it is used to iterate over a collection of objects.
2. Whenever we want to work with in-memory objects, we need to use the IEnumerabe interface

```C#
using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> integerList = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };

            IEnumerable<int> QuerySyntax = from obj in integerList
                              where obj > 5
                              select obj;
            
            foreach (var item in QuerySyntax)
            {
                Console.Write(item + " ");
            }

            Console.ReadKey();
        }
    }
}
```

### IQueryable
1. It is a c# interface used to query data from a data source
2. This is particularly useful for remote data sources, like databases, enabling efficient querying by allowing the query to be executed on the server side. 

```C#
using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Student> studentList = new List<Student>()
            {
                new Student(){ID = 1, Name = "James", Gender = "Male"},
                new Student(){ID = 2, Name = "Sara", Gender = "Female"},
                new Student(){ID = 3, Name = "Steve", Gender = "Male"},
                new Student(){ID = 4, Name = "Pam", Gender = "Female"}
            };
            
            //Linq Query to Fetch all students with Gender Male
            IQueryable<Student> MethodSyntax = studentList.AsQueryable()
                                .Where(std => std.Gender == "Male");
                                              
            //Iterate through the collection
            foreach (var student in MethodSyntax)
            {
                Console.WriteLine( $"ID : {student.ID}  Name : {student.Name}");
            }

            Console.ReadKey();
        }
    }

    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
    }
}
```



### LINQ Operators
#### Select
It is used to format the query result as per requirement
```C#
List<Employee> basicQuery = (from emp in Employee.GetEmployees()
                              select emp).ToList();
```

#### SelectMany
It is used to project each element to IEnumerable type
##### It can be used to flattent the array
For example if we have an array of three element and all three element contains three integers then SelectMany would return list with 9 elements
```C#
    List<string> nameList =new List<string>(){"Pranaya", "Kumar" };
    IEnumerable<char> methodSyntax = nameList.SelectMany(x => x);
```

#### OfType
It is used to return only those elements that can be casted to the specified type
```C#
static void Main(string[] args)
        {
            //Data Source Contains both Integer and String Data
            List<object> dataSource = new List<object>()
            {
                "Tom", "Mary", 50, "Prince", "Jack", 10, 20, 30, 40, "James"
            };
            //Fetching only the Integer Data from the Data Source
            //using Linq Method Syntax and OfType Method
            List<int> intData = dataSource.OfType<int>().ToList();
        }
```




## Constructors
Constructors are special functions that are called when an instance of a class is created.
1. Constructor has same name as class.
2. Constructor does not have a return type.
3. Constructor can be overloaded.
4. Constructor can have access modifier.

### Type of constructors

#### Default contructor
A constructor with no parameter, if no constructor is defined then compiler creates own default constructor.

#### Parameterized constructor
A constructor that takes one or more parameters

#### Static constructor
Called once when the first instance of class is created or when the static member of the class is called

#### Private constructor
A constructor that is used to prevent the creation of instance of class 


### Static constructor
A static cosntructor is a special type of constructor that is used to initialize the static members of a class. Static constructor is called automatically before static members are accessed.
1. Static constructor cannot have parameters
2. Static constructor is called automatically before first instance is created
3. Static constructor is called only once per type, not per instance
4. Static consttuctor cannot have access modifiers. They are always private
5. You cannot call static constructor directly
6. Cannot use 'this' and 'base' in static constructor

#### When to use static constructor
1. To initialize static members of a class.
2. To perform action only once, regardless of how many instances of the class are created

#### Limitation of static constructor
1. Cannot take parameters.
2. Cannot access instance members
3. Cannot be called explicitly

```C# 
public class Logger{
    private static string logFilePath;

    public static string LogFilePath{
        get { return logFilePath; }
    }

    static Logger(){
        logFilePath = "log.txt";
        Console.WriteLine("Static constructor called. Log file path initialized");
    }

    public static void Log(string message){
        Console.WriteLine($"Log: {message}");
    }

}

class Program{
    static void Main(){
        Console.WriteLine($"Log file path: {Logger.LogFilePath}");

        Logger.Log("Application started");
    }
}
```
Output: 
Static constructor called. Log file path initialized
Log file path: log.txt
Log: Application started

## 'this' keyword
It is used to refer to current instance of the class
1. it is used to access instance variables, methods, or properties of the current object
2. To call another constructor within the same class
3. To distinguish between instance variables and parameters with the same name

```C#
public class Person{
    private string name;
    private int age;
    public Person(string name, int age){
        this.name = name;
        this.age = age;
    }

    public void Display(){
        Console.WriteLine($"Name: {this.name}, Age: {this.age}");
    }
}
```

### Constructor Chaining
To call another constructor within the same class
This helps to avoid code duplication

#### Example 1
```C#
public class Person{
    private string name;
    private int age;
    public Person() : this("Unknown", 0){

    }
    public Person(string name, int age){
        this.name = name;
        this.age = age;
    }
}
```

#### Exmaple 2
```C#
public class Car{
    private string brand;
    private string model;
    private int year;

    public Car() : this("Unknown", "Unknown", 0){
        Console.WriteLine("Default constructor called");
    }

    public Car(string brand, string model, int year){
        this.brand = brand;
        this.model = model;
        this.year = year;
        Console.WriteLine("Constructor with 3 parameters called");
    }

    public void Display(){
        Console.WriteLine($"Brand: {brand}, Model: {model}, Year: {year}");
    }
}

class Program{
    static void Main(){
        Car car1 = new Car();
        car1.Display();
    }
}
```
Output:
Consturctor with 3 parameters called
Defualt constructor called
Brand: Unknown, Model: Unknown, Year: 0

#### Example 3
```C#
public class Book
{
    private string title;
    private string author;
    private double price;

    // Constructor with title only
    public Book(string title) : this(title, "Unknown", 0.0)
    {
        Console.WriteLine("Constructor with title only called");
    }

    // Constructor with title and author
    public Book(string title, string author) : this(title, author, 0.0)
    {
        Console.WriteLine("Constructor with title and author called");
    }

    // Constructor with all parameters
    public Book(string title, string author, double price)
    {
        this.title = title;
        this.author = author;
        this.price = price;
        Console.WriteLine("Constructor with all parameters called");
    }

    public void Display()
    {
        Console.WriteLine($"Title: {title}, Author: {author}, Price: {price}");
    }
}

class Program
{
    static void Main()
    {
        Book book1 = new Book("C# Programming");
        book1.Display();

        Book book2 = new Book("C# Programming", "John Doe");
        book2.Display();

        Book book3 = new Book("C# Programming", "John Doe", 29.99);
        book3.Display();
    }
}
```
Output: 
Constructor with all parameters called
Constructor with title only called
Title: C# Programming, Author: Unknown, Price: 0.0
Consturctor with all parameters called
Constructor with title and author called
Title: C# programming, Author: John Doe, Price: 0.0
Constructor with all parameters called 
Title: C# Programming, Author; John Doe, Price: 29.99


#### Example 4
```C#
public class Student
{
    private string name;
    private int age;
    private double grade;

    // Default constructor
    public Student() : this("Unknown", 0, 0.0)
    {
        Console.WriteLine("Default constructor called");
    }

    // Constructor with name
    public Student(string name) : this(name, 0, 0.0)
    {
        Console.WriteLine("Constructor with name called");
    }

    // Constructor with name and age
    public Student(string name, int age) : this(name, age, 0.0)
    {
        Console.WriteLine("Constructor with name and age called");
    }

    // Constructor with all parameters
    public Student(string name, int age, double grade)
    {
        this.name = name;
        this.age = age;
        this.grade = grade;
        Console.WriteLine("Constructor with all parameters called");
    }

    public void Display()
    {
        Console.WriteLine($"Name: {name}, Age: {age}, Grade: {grade}");
    }
}

class Program
{
    static void Main()
    {
        Student student1 = new Student();
        student1.Display();

        Student student2 = new Student("Alice");
        student2.Display();

        Student student3 = new Student("Bob", 20);
        student3.Display();

        Student student4 = new Student("Charlie", 21, 3.8);
        student4.Display();
    }
}

```
Output:
Constructor with all parameters called
Default constructor called
Name: Unknown, Age: 0, Grade: 0.0
Constructor with all parameters called
Constructor with name called
Name: Alice, Age: 0, Grade: 0.0
...




## 'base' keyword
base keyword is used to refer to the parent class of the current class.
It is used to call the methods or access properties and fields from the base class
It is used to call the constructor from the base class

```C#
public class Animal{
    public void Eat(){
        Console.WriteLine("Eating...");
    }
}

public class Dog : Animal{
    public void Bark(){
        Console.WriteLine("Barking...");
    }

    public void PerformActions(){
        base.Eat();
        this.Bark();
    }
}
```

#### Constructor Chaining
```C#
public class Animal{
    protected string name;
    public Animal(string name){
        this.name = name;
    }
}

public class Dog : Animal{
    private string breed;
    public Dog(string name, string breed) : base(name){
        this.breed = breed;
    }
    public void Display(){
        Console.WriteLine($"Name: {name}, Breed: {breed}");
    }
}
```

#### Method overriding 

```C#
using System;

public class Animal
{
    // Virtual method in the base class
    public virtual void MakeSound()
    {
        Console.WriteLine("Animal makes a sound");
    }

    // Non-virtual method in the base class
    public void Eat()
    {
        Console.WriteLine("Animal is eating");
    }
}

public class Dog : Animal
{
    // Overriding the virtual method from the base class
    public override void MakeSound()
    {
        // Call the base class method
        base.MakeSound();
        Console.WriteLine("Dog barks");
    }

    // Hiding the non-virtual method from the base class
    public new void Eat()
    {
        // Call the base class method
        base.Eat();
        Console.WriteLine("Dog is eating");
    }
}

class Program
{
    static void Main()
    {
        Dog dog = new Dog();
        dog.MakeSound();
        dog.Eat();

        // Demonstrate polymorphism
        Animal animal = dog;
        animal.MakeSound();
        animal.Eat();
    }
}
```
Output:
Animal makes a sound
Dog barks
Animal is eating
Dog is eating
Animal makes a sound
Animal is eating


## OOPs Concepts

### Encapsulation

Encapsulation is the mechanism of restricting access to certain detials of an object and only exposing specific aspects of the object.

It has 3 components

1. Access Modifiers
2. Properties - Provides controlled access to private fields
3. Methods - Encapsulates the behavior that operates on the internal state

#### Access Modifiers
1. Private: Members are accessible only within the same class
2. Public: Members are accessible from any other class
3. Protected: Members are accessible within the same class and in derived class
4. Internal: Members are accessible within same assembly

### Inheritance

#### Sealed class

To restrict a class from being used as a base class we use sealed class in this way this class is protected from any extension.

A method can also be marked as sealed but first the method must be overridden in the derived class then only you can mark it as sealed.

```C#
public class BaseClass
{
    public virtual void Display()
    {
        Console.WriteLine("Base class display method.");
    }
}

public class DerivedClass : BaseClass
{
    public sealed override void Display()
    {
        Console.WriteLine("Derived class sealed display method.");
    }
}

public class FurtherDerivedClass : DerivedClass
{
    // The following would cause a compilation error
    // public override void Display() { }
}
```



#### Static class

A static class cannot be instantiated and cannot serve as a base class

all members inside the static class must also be static 

```C#
public class MyClass
{
    public static int MyStaticField = 10;
    public static int MyStaticMethod()
    {
        return MyStaticField;
    }
}

// Usage
int value = MyClass.MyStaticMethod();
```



### Abstraction

Hiding complex implementation details and exposing only the necessary features or behaviors to the outside world. It is achieved through abstract class, interface and access modifiers

#### Abstract class

- Used as a base class, cannot instantiated, provide blueprint for derived class
- Contains both declaration and definition of methods
- Contains methods, fields, constructor and other class members
- Does not support multiple inheritance
- Not full abstraction

#### Interface 

- contains only declaration of methods (abstract methods only)
- contains only methods
- supports multiple inheritance
- full abstraction



### Polymorphism

ability of variable, objects, funcitons to take on multiple forms

#### Overloading

Method overloading is a feature in C# that allows you to define multiple methods with same name but different signature including number, types, order of parameters

#### Overriding

Occurs between parent and child classes, virtual for base class and override for child class

#### 
```C#
public class Person
{
    private string name; // Encapsulated field
    private int age; // Encapsulated field

    // Public property to access name
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    // Public property to access age
    public int Age
    {
        get { return age; }
        set
        {
            if (value > 0)
            {
                age = value;
            }
        }
    }

    public void Display()
    {
        Console.WriteLine($"Name: {Name}, Age: {Age}");
    }
}

class Program
{
    static void Main()
    {
        Person person = new Person();
        person.Name = "John";
        person.Age = 30;
        person.Display();
    }
}
```





## string and stringBuilder

A string is a collection or an array of characters. So, string can be created using a char array or accessed like a char array.

A string is immutable in c#. It means it is read-only and cannot be changed once created in memory. Each time you change a string .NET CLR will create a new memory location for the concatenated string.

StringBuilder doesn't create a new object in the memory but dynamically expands memory to accomodate the modified string.





## Why can't we use normal class instead of interface

1. Interface defines a contract that implementing class must follow
2. Interface allows changes in implementation without affecting other clients that are using interface
3. Interface allows multiple inheritance
4. Interface allows polymorphism or multiple implementations can be there for a interface



## Difference between singleton, scoped and transient

1. Singleton - Singleton service is created once per application lifetime and is reused whenever it is requested. Example: Configuration settings, logging services.
2. Scoped - A Scoped service is created once per request(scope). Particularly useful for the web applications to give new instance of service for each http request. Example: Database contexts, unit of work patterns.
3. Transient - A Transient service is created it is requested. Example: Lightweight stateless services, utility services.

## SOLID Principles

1. Single Responsibility - A class should be responsible for only oner single task
2. Open/Close Principle - A class should be open for extension and closed for modification
3. Liskov's Sbustitution principle - objects of super class should be repleacable by the objects of sub class
4. Interface Segregation - Interfaces should be made so that no base class use to implement unnecessary methods
5. Dependency Inversion Principle - High level modules should not be dependent on the low level modules. Both should depend on abstraction

## APIs

### What is an API?
API(Applcation Programming Interface) is a set of rules and protocols for building and interacting with software applications. It defines methods and data format that applications use to interact with external system or services. It allows developers to use the functionalities of other applications without needing to understand their internal working.

### Types of APIs
#### REST(Representational State Transfer)
1. Uses standard HTTP methods.
2. Stateless Architecture.
3. Resources identified by URLs.
4. Widely used due to simplicity and scalability.

#### SOAP(Simple Access Object Protocol)
1. Protocol for exchanging structured information.
2. Relies on XML.
3. Supports complex operations and higher security.

#### GraphQL



# Basics of Angular and javascript    

### What is angular?

Angular is a typescript based open source front-end platform that makes it easy to build web, mobile and desktop applications.

### What are building blocks of angular application

1. Modules - Modules are container for holding the block of code

```Typescript
// app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

```
1.1. declaration - contains all the components that are part of this module
1.2. imports - other modules that this module has a dependency on
1.3. providers - contains all the services whose single instance would be available in this module

2. Component - Components are basic building block of the angular application they hold the template and logical part associated with it

```Typescript
// app.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'My Angular App';
}
```

3. Templates - Templates are HTML structures associated with the component

```HTML
<!-- app.component.html -->
<h1>{{ title }}</h1>
<button (click)="doSomething()">Click Me</button>
```

4. Directives - They are used to add behavior to the elements in angular component- three types- component, structural and attribute

```Typescript
// example.directive.ts
import { Directive, ElementRef, Renderer2, HostListener } from '@angular/core';

@Directive({
  selector: '[appHighlight]'
})
export class HighlightDirective {
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  @HostListener('mouseenter') onMouseEnter() {
    this.renderer.setStyle(this.el.nativeElement, 'backgroundColor', 'yellow');
  }

  @HostListener('mouseleave') onMouseLeave() {
    this.renderer.removeStyle(this.el.nativeElement, 'backgroundColor');
  }
}
```

5. Services - Services are classes that are used to provide a functionality or are used to share the data among the components
    Dependency injection - Angular uses dependency injection to provide instance of service across the application
    Singleton - A single instance of service is shared among the components in the angular application

```Typescript
// example.service.ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ExampleService {
  getData() {
    return 'Data from service';
  }
}
```

6. Pipes - Pipes are used to transform the data in the templates, two types - built-in and custom pipes

```Typescript
// custom.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'customPipe'
})
export class CustomPipe implements PipeTransform {
  transform(value: string): string {
    return value.toUpperCase();
  }
}
```

7. Routing - Angular router modules helps in configuration of routes

8. Forms - Template driven form and Reactive forms


### What are directives and different types of directives?
Directives allows you to add behavior to the HTML elements, manipulate DOM or create reusable components.
There are 3 types of directives - 
1. Component Directive - Contains HTML template and logical Typescript class

```Typescript
// example.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-example',
  template: `<h1>{{ title }}</h1>`,
  styles: [`h1 { color: blue; }`]
})
export class ExampleComponent {
  title = 'Hello, World!';
}
```

2. Structural Directive - They are used to add or remove the elements from DOM. eg - *ngIf, *ngFor, *ngSwitch

```Typescript
<!-- example.component.html -->
<div *ngIf="isVisible">This element is visible</div>
```

```Typescript
<!-- example.component.html -->
<ul>
  <li *ngFor="let item of items">{{ item }}</li>
</ul>
```

3. Attribute directive - They are used to change the behavior or appearance of the element

```Typescript
// highlight.directive.ts
import { Directive, ElementRef, Renderer2, HostListener } from '@angular/core';

@Directive({
  selector: '[appHighlight]'
})
export class HighlightDirective {
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  @HostListener('mouseenter') onMouseEnter() {
    this.renderer.setStyle(this.el.nativeElement, 'backgroundColor', 'yellow');
  }

  @HostListener('mouseleave') onMouseLeave() {
    this.renderer.removeStyle(this.el.nativeElement, 'backgroundColor');
  }
}
```
### Angular lifecycle hooks

There are 8 lifecycle hooks in angular 

ngOnInit is called when component initialize. It is called once. Mostly, I used for variable initialize and API call. ngOnDestroy is called before detroying the component. I heavily used for unsubscribe the subscription to prevent the memory leak.

Several times I have used ngOnChanges, ngAfterContentInit, ngAfterViewInit in my career. ngOnChanges method is called once on component’s creation and then every time changes are detected in one of the component’s input properties. It receives a SimpleChanges object as a parameter. ngAfterViewInit is called after the component view and its child views has been initialized. ngAfterContentInit is called after components external content (or from parent ) has been initialized.

There are other hooks like ngDoCheck, ngAfterContentChecked, ngAfterViewChecked, I did not use them too much.

### Data binding in angular

#### From component to DOM

##### Interpolation 

{{value}} used to add value of a property from the component

##### Property binding

[property] = "value" value is passed from component to the specified property

#### From DOM to component

##### Event binding

(event) = "function" when a specific event occurs call the specified function

#### Two way binding

[(ngModel)] = "value" allows data to flow both ways from DOM to component and vice versa


### What is metadata

Metadata is used to decorate a class so that it can configure the expected behavior of the class.

Types- 
1. Class decorators - @Component, @Module
2. Property decorators - @Input, @Output
3. Method decorators - @HostListener


### What is the difference between constructor and ngOnInit

Both constructor and ngOnInit lifecycle hook are used to initialize the component but they serve different purpose and are used in different contexts.

#### Constructor

The constructor is a special method in a class that is called when an instance of the class is created. In angular it is used for initialization tasks and for injecting dependencies via Angular's dependency injection

Constructor runs befor angular has fully initialized the component and before the component's input properties are set

#### NgOnInit

It is a lifecycle hook and is called after angular has fully initialized the component, including its input properties

ngOnInit runs after the constructor and after angular has set all the input properties of the component


### What are services

Services are used to share business logic, data access or any reusable functionality among different components of an application.
Angular uses DI to provide instances of services to components or other services. This promotes loose coupling.
Services are ofter singleton, meaning that angular creates single instance of the service and shares it across the application.

```Typescript
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private data: string[] = ['Item 1', 'Item 2', 'Item 3'];

  getData(): string[] {
    return this.data;
  }

  addData(item: string): void {
    this.data.push(item);
  }
}
```
If you want to create different instance of the service for different components then in providers array of both components add the service to create different instance of the service for both the components

### What is dependency injection in angular

Dependency injection is a design pattern in which a class asks for dependencies from external source rather creating them itself. This helps in efficient management of services and other dependencies across the application.

DI works with @Injectable decorator which marks a class as a service that can be injected into other classes. When class is declared as dependency angular injector looks up the provider for that dependency and supplies an instance of it.

### Pipes in angular

Pipes allows you to transform data within your templates they are used to format, transform or manipulate data for display pupose without altering the underlying data model

Built in pipes - 
DatePipe: Formats a date value according to locale rules.
UpperCasePipe: Transforms text to uppercase.
LowerCasePipe: Transforms text to lowercase.
CurrencyPipe: Formats a number as currency.
DecimalPipe: Formats a number as decimal.
PercentPipe: Formats a number as a percentage.
JsonPipe: Converts a value into a JSON string.
SlicePipe: Creates a new array or string containing a subset (slice) of the elements.

### Async pipe 

Async pipe in angular is a special pipe that automatically subscribes to an observable or promise and returns the latest value it has emitted.

### What happen if you use script tag inside template

To prevent cross site scripting angular sanitizes the content and removes the script tag

### Difference between pure and impure pipes

Pure pipes are default type of pipes in Angular, they are called only when the inputs to the pipe changes

Impure pipes are executed during every change detection cycle, regardless of input value change


### What are observables

Observables are concept for handling asynchronous data and events they are more flexible then the promises and callbacks
An observable is a stream of data that can emit multiple values over time.
```Typescript
import {Observable} from "rxjs";

... 

const myObservable = new Observable( observer =>{     
   let value = 0;
   setInterval ( () =>
   {
      observer.next(value); //this is what sends a new value
      value = value + 10;
      if(value > 50){
         observer.complete(); //finish sending values
       }
    }  ,1000 );
  }
  );

myObservable.subscribe(
   val=> console.log(val), //for a value returned
   error => console.log("problem"), //if something happens
   () => console.log("Done")
   ); //once it is done

myObservable.subscribe( {
      next: val => console.log("Second:" + val),
      error: error => console.log(error),
      complete: () => console.log("Completed")
   } );

```

With the above code both subscriptions are going to get the values independently. Sometimes, instead of starting an independent execution for each subscriber, you want each subscription to get the same values, even if the values have already started emitting. In this case you will need multicast.

### What are subjects



### What are different rxjs operators

1. of(value1, value2, ...): Creates an observable that emits the provided arguments as values
2. map(fn): Applies a function to each emitted value and emits the transformed value
3. filter(fn): Only emits the value passed by the test in the provided function

### What is subscribing

subscribing is listening and reacting to the stream of data emitted by an observable.
When you subscribe to an observable you provide an observer object. This observer decides how you want to handle the data with three callback functions
next(value): this is called and obseervable emits a new value
error(error): this is called when observable encounters an error
complete(): this is called and observable has finished emitting the data, you can use this to unsubscribe the observable


### What are dynamic components

These are components that are created and inserted into the view at runtime rather than deing defined statically in the template

eg.
```Typescript
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-message',
  template: `
    <p>{{ message }}</p>
  `
})
export class MessageComponent {
  @Input() message: string = '';
}
```

```Typescript
import { Component, ViewChild, ViewContainerRef, ComponentFactoryResolver } from '@angular/core';
import { MessageComponent } from './message.component';

@Component({
  selector: 'app-parent',
  template: `
    <button (click)="showMessage()">Show Message</button>
    <div #messageContainer></div>
  `
})
export class ParentComponent {
  @ViewChild('messageContainer', { read: ViewContainerRef }) container: ViewContainerRef;

  constructor(private componentFactoryResolver: ComponentFactoryResolver) {}

  showMessage() {
    const message = 'This is a dynamically created message!';

    // Create component factory
    const factory = this.componentFactoryResolver.resolveComponentFactory(MessageComponent);

    // Create component instance
    const componentRef = this.container.createComponent(factory);

    // Set message input
    componentRef.instance.message = message;
  }
}
```


### JS arrays

An array is a spoecial type of object used to store multiple values in a single variable.
They are dynamic meaning they can grow and shrink in size.

creating an array- 
let array = [1,2,3,4]

accessing the element
console.log(array[0]);

modifying the element
array[1]=5;

#### Properties of array

1. length - returns number of elements in the array

```javascript
let array = [1,2,3,4];
console.log(array.length);
```

2. push - adds one or more elements to the end of the array and returns new length of the array

```javascript
let array = [1,2,3];
array.push(4);
console.log(array) // [1,2,3,4]
```

3. pop - removes the last element from the array and returns that element 

```javascript
let array = [1,2,3];
let lastElement = array.pop();
console.log(array) // [1,2]
console.log(lastElement) // 3
```

4. shift - removes the first element from the array and returns that element

```javascript
let array = [1,2,3];
let firstElement = array.shift();
console.log(array) //[2,3]
console.log(firstElement) //1
```

5. unshift - adds one or more elements to the beginning of an array and returns the new length of the array

```javascript
let array = [1,2,3];
array.unshift(0);
console.log(array) //[0,1,2,3]
```

6. splice - used to add and remove the elements from the array

```javascript
const fruits = ["banana", "orange", "apple", "mango"];
fruits.splice(2,0,"lemon", "kiwi");
console.log(fruits) //["banana", "orange","lemon", "kiwi", "apple", "mango"]
```

first param 2 defines position where the new element should be added
second param 0 defines how many elements should be removed
rest params (lemon, kiwi) defines the new elements to be added

splice returns the removed elements


7. slice - used to slice out a piece of array

```javascript
const fruits = ["banana", "orange", "apple", "mango"];
const citrus = fruits.slice(1);
console.log(citrus) //["orange"]
console.log(fruits) // ["banana", "orange", "apple", "mango"]

const fav = fruits.slice(2,3);
console.log(fav)// ["apple", "mango"]
```

8. toString - converts an array to a comma separated string 

```javascript
const fruits = ["banana", "orange", "apple", "mango"];
console.log(fruits.toString()); //Banana,Orange,Apple,Mango
```

9. join - used to join all array elements in a single string

```javascript
const fruits = ["Banana", "Orange", "Apple", "Mango"];
document.getElementById("demo").innerHTML = fruits.join(" * ");

result - Banana * Orange * Apple * Mango
```

10. concat - used to join two arrays

```javascript
const myGirls = ["Cecilie", "Lone"];
const myBoys = ["Emil", "Tobias", "Linus"];

const myChildren = myGirls.concat(myBoys); //["Cecilie", "Lone", "Emil", "Tobias", "Linus"];
```

11. flat - create a new array with sub array elements concatenated to a specific depth
``` javascript
const nestedArray = [1, [2, 3], 4, [5, [6]]];
const flattenedArray = nestedArray.flat(); // Flattens one level by default
console.log(flattenedArray); // Output: [1, 2, 3, 4, 5, [6]]

const deeperFlattenedArray = nestedArray.flat(2); // Flattens two levels
console.log(deeperFlattenedArray); // Output: [1, 2, 3, 4, 5, 6]
```

12. reduce - applies a function against an accumulator and each element in an array to reduce it to a single value.

```javascript
const numbers = [1, 2, 3, 4];
const sum = numbers.reduce((acc, num) => acc + num, 0); // Initial value (0) for accumulator
console.log(sum); // Output: 10
```

to flatten the array with use of .flat() function we can do -
```js
const flattenedArray = nestedArray.reduce((acc, val) => acc.concat(val), []);
```

13. indexOf - searches an event for element value and retruns its position, if the itemn is not found then return -1 and if more than 1 then return first occurence
```js
const fruits = ["Apple", "Orange", "Apple", "Mango"];
let position = fruits.indexOf("Apple")+1; //1
```

14. lastIndexOf - get the position of last occurence of the specified element

15. includes - check if the element is present in the array

```js
const fruits = ["Banana", "Orange", "Apple", "Mango"];

fruits.includes("Mango"); // is true
```

16. find - returns the value of the first array element that passes a test function

```js
const numbers = [4, 9, 16, 25, 29];
let first = numbers.find(myFunction);

function myFunction(value, index, array) {
  return value > 18;
}
```

17. forEach - calls a function once for each array element

```js
const numbers = [45, 4, 9, 16, 25];
let txt = "";
numbers.forEach(myFunction);

function myFunction(value, index, array) {
  txt += value + "<br>";
}
```

18. map - creates a new array by performing a function on each array element. this method does not execute or array element without values, it does not change the original array

```js
const numbers1 = [45, 4, 9, 16, 25];
const numbers2 = numbers1.map(myFunction);

function myFunction(value, index, array) {
  return value * 2;
}
```

19. filter - creates a new array with array elements that pass a test

```js
const numbers = [45, 4, 9, 16, 25];
const over18 = numbers.filter(myFunction);

function myFunction(value, index, array) {
  return value > 18;
}
```

#### how to check if the datatype of the object is array
use the function 
Array.isArray(fruits);

#### Spread operator

Spread operator is represented by (...) used for working with arrays and objects

1. Copying array - this is creating shallow copy, first level would not be shallow but from second level it is a shallow copy

```js
let array1 = [1, 2, 3];
let array2 = [...array1];

console.log(array2); // Output: [1, 2, 3]
```

2. Combining array

```js
let array1 = [1, 2];
let array2 = [3, 4];
let combinedArray = [...array1, ...array2];

console.log(combinedArray); // Output: [1, 2, 3, 4]
```

3. Adding elements

```js
let array = [1, 2, 3];
let newArray = [0, ...array, 4, 5];

console.log(newArray); // Output: [0, 1, 2, 3, 4, 5]
```

4. Copying object - creates shallow copy

```js
let obj1 = { a: 1, b: 2 };
let obj2 = { ...obj1 };

console.log(obj2); // Output: { a: 1, b: 2 }
```

5. Merging objects

```js
let obj1 = { a: 1, b: 2 };
let obj2 = { c: 3, d: 4 };
let combinedObj = { ...obj1, ...obj2 };

console.log(combinedObj); // Output: { a: 1, b: 2, c: 3, d: 4 }
```

6. Overriding properties

```js
let obj1 = { a: 1, b: 2 };
let obj2 = { b: 3, c: 4 };
let combinedObj = { ...obj1, ...obj2 };

console.log(combinedObj); // Output: { a: 1, b: 3, c: 4 }
```

### Questions on array

#### Given an array of numbers, write a function to find the sum of all elements.

```js
const arr = [1,2,2,3];

function sumArr(arr){
  return arr.reduce((acc, num)=>acc+num, 0);
}

console.log(sumArr(arr));

```

#### Write a function to find the maximum or minimum element in an array.

```js
const arr = [3,2,2,3];

function minArr(arr){
  arr.sort((a,b)=>a-b);
  return arr[0]
}

console.log(minArr(arr));
```

#### Given an array and a target element, write a function to check if the target element exists in the array.

```js
const arr = [3,2,2,3];

function checkArr(arr, target){
  return arr.indexOf(target);
}

console.log(checkArr(arr,2));
```

#### Given an array of numbers and a target sum, find two numbers in the array that add up to the target sum. Return their indices or the numbers themselves

```js
const arr = [3,2,2,3];

function checkArr(arr, target){
  var start = 0;
  var end = arr.length -1;
  arr.sort((a,b)=>a-b);
  while(start<=end){
    let sum = arr[start]+arr[end];
    if(sum>target){
      end-=1;
    }
    else if(sum<target){
      start+=1;
    }
    else{
      return [arr[start], arr[end]]
    }
  }
}

console.log(checkArr(arr,4));

```

#### Write a function to reverse the order of elements in an array (in-place or by creating a new array).

```js
const arr = [3,2,2,4];

function reverseArr(arr){
  arr.reverse()
  return arr;
}

console.log(reverseArr(arr));
```

#### Given an array that may contain duplicates, write a function to remove duplicates and return a new array with unique elements.

```js
const arr = [3,2,2,4];

function removeDupArr(arr){
  return arr.reduce((acc, num)=>{
    if(!acc.includes(num)){
      acc.push(num);
    }
    return acc
  }, [])
}

console.log(removeDupArr(arr));

```

#### Write a function that takes an array of numbers and moves all zeros to the end of the array while maintaining the relative order of non-zero elements. (Do this in-place if possible)

```js
const arr = [0,2,0,4];

function removeDupArr(arr){
  var zeroIndex = 0;
  for(let i=0;i<arr.length;i++){
    if(arr[i]!=0){
      let temp = arr[i];
      arr[i] = arr[zeroIndex];
      arr[zeroIndex] = temp;
      zeroIndex+=1;
    }
  }
  return arr;
}

console.log(removeDupArr(arr));
```


#### Given two arrays, write a function to find the elements that are present in both arrays (the intersection).

```js
const arr = [0,2,0,4];
const arr2 = [0,3,0,5];

function intersectArr(arr1, arr2){
  let uniquiSet = new Set([...arr1]);
  let intersect = arr2.filter(num=>uniquiSet.has(num));
  return intersect;
}

console.log(intersectArr(arr, arr2));
```


## SQL

### SELECT 
to select data from the table
```SQL
SELECT CustomerName, city from Customers;
```

### SELECT DISTINCT
to select distinct values 
```SQL
SELECT DISTINCT Country from Customers;
```

### INSERT 
to add data in the table
```SQL
INSERT INTO Customers (CustomerName, Contact)
VALUES
('Ram', '00000'),
('Shyam', '11111')
```

### NULL Value
A field with a NULL value is a field with no value
If a field is optional, it is possible to not insert a value in this field then the field will be saved with NULL value

#### Check if NULL value or not
```SQL
SELECT column_names from table_name where column_name IS NULL
select column_names from table_name where column_name IS NOT NULL
```

### UPDATE 
to modify a existing record
```SQL 
UPDATE table_name set column1 = value1, column2 = value2, ...
where condition
```
If you are not using the where clause then all the rows data will update

### DELETE
used to delete an existing record
```SQL
DELETE FROM table_name WHERE condition
```
if delete is used without condition then all data will be deleted 

### TRUNCATE
removes all rows from a table resetting it to empty table
```SQL
TRUNCATE TABLE table_name
```

#### Difference between truncate and delete
1. In delete you can have a condition but not in truncate
2. Truncate resets the auto-increment column counter to starting value whereas on delete it does not reset when you remove all the rows
3. Truncate is faster than delete

### SELECT TOP
Used to specify the number of records to return
```SQL
SELECT TOP 3 * FROM Customer;
```

### Aggregate functions
Aggregate function is a function is used with GROUP BY clause of the SELECT statement.
The GROUP BY clause splits the result-set into groups of values and the aggregate function can be used to return a single value for each group
- MIN() - returns the smallest value within the selected column
- MAX() - returns the largest value within the selected column
- COUNT() - returns the number of rows in a set
- SUM() - returns the total sum of a numerical column
- AVG() - returns the average value of a numerical column

Aggregate functions ignore the NULL value except for count

```SQL
SELECT COUNT(*) AS [Number of records], CategoryID
FROM Products
GROUP BY CategoryID;
```

### EXIST Operator
used to test for existence of any record in a subquery
returns true if the subquery returns one or more records

```SQL
SELECT column_name
FROM table_name
WHERE EXISTS
(SELECT column_name FROM table_name WHERE condition);
```

### ANY Operator
used to compare value to any value in a list or subquery

```SQL
SELECT column1, column2, ...
FROM table1
WHERE column1 comparison_operator ANY (subquery)
```

### ALL Operator

ALL means that the condition will be true only if the operatio is true for all values in the range

```SQL
SELECT column1, column2
FROM table1
WHERE column1 comparision_operator ALL (subquery)\

