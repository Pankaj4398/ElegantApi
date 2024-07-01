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
