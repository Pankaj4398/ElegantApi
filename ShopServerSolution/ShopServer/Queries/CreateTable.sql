--CREATE DATABASE ShopServer;

USE ShopServer;


CREATE TABLE Users (
    id uniqueidentifier PRIMARY KEY,
    username varchar(50) NOT NULL,
    password varchar(50) NOT NULL,
	first_name varchar(50) NOT NULL,
	last_name varchar(50),
	address varchar(255),
	telephone varchar(50) NOT NULL,
	created_at date NOT NULL,
	modified_at date NOT NULL
);

CREATE TABLE ShoppingSession (
    id uniqueidentifier PRIMARY KEY,
    total decimal NOT NULL,
	created_at date NOT NULL,
	modified_at date NOT NULL,
    user_id uniqueidentifier FOREIGN KEY REFERENCES Users(id)
);

CREATE TABLE Discount (
    id uniqueidentifier PRIMARY KEY,
    name varchar(50) NOT NULL,
	description varchar(255),
	discount_percent decimal ,
	created_at date NOT NULL,
	modified_at date NOT NULL,
 );

 CREATE TABLE Product (
    id uniqueidentifier PRIMARY KEY,
    name varchar(50) NOT NULL,
	description varchar(255),
	category varchar(50),
	price decimal ,
	sku int IDENTITY,
	created_at date NOT NULL,
	modified_at date NOT NULL,
	discount_id uniqueidentifier FOREIGN KEY REFERENCES Discount(id)
 );

 CREATE TABLE CartItem (
    id uniqueidentifier PRIMARY KEY,
    quantity int NOT NULL,
	created_at date NOT NULL,
	modified_at date NOT NULL,
    session_id uniqueidentifier FOREIGN KEY REFERENCES ShoppingSession(id),
	product_id uniqueidentifier FOREIGN KEY REFERENCES Product(id)
);


 CREATE TABLE PaymentDetails (
    id uniqueidentifier PRIMARY KEY,
    order_id int NOT NULL,
	amount decimal NOT NULL,
	provider varchar(50) NOT NULL,
	status varchar(50) NOT NULL,
	created_at date NOT NULL,
	modified_at date NOT NULL,
    
);

ALTER TABLE PaymentDetails 
ALTER COLUMN order_id uniqueidentifier;
--Operand type clash: int is incompatible with uniqueidentifier


ALTER TABLE PaymentDetails
DROP COLUMN order_id;

ALTER TABLE PaymentDetails
ADD order_id uniqueidentifier;


 CREATE TABLE OrderDetails (
    id uniqueidentifier PRIMARY KEY,
    total decimal NOT NULL,
	created_at date NOT NULL,
	modified_at date NOT NULL,
    user_id uniqueidentifier FOREIGN KEY REFERENCES Users(id),
	payment_id uniqueidentifier FOREIGN KEY REFERENCES PaymentDetails(id)
);

CREATE TABLE OrderItems (
    id uniqueidentifier PRIMARY KEY,
	created_at date NOT NULL,
	modified_at date NOT NULL,
    order_id uniqueidentifier FOREIGN KEY REFERENCES Users(id),
	product_id uniqueidentifier FOREIGN KEY REFERENCES Product(id)
);

--Above one mistake order_id forgein key of OrderDetails mapped from Users table -- Wrong refential integrity

drop table OrderItems

CREATE TABLE OrderItems (
    id uniqueidentifier PRIMARY KEY,
	created_at date NOT NULL,
	modified_at date NOT NULL,
    order_id uniqueidentifier FOREIGN KEY REFERENCES OrderDetails(id),
	product_id uniqueidentifier FOREIGN KEY REFERENCES Product(id)
);

-- Two tables more require and product table discount_id remove direct relationship

CREATE TABLE DiscountProduct (
	id uniqueidentifier PRIMARY KEY,
	product_id uniqueidentifier FOREIGN KEY REFERENCES Product(id),
	discount_id uniqueidentifier FOREIGN KEY REFERENCES Discount(id)
)

CREATE TABLE ImageProduct (
	id uniqueidentifier PRIMARY KEY,
	product_id uniqueidentifier FOREIGN KEY REFERENCES Product(id),
	image_url varchar(255)
)

ALTER TABLE Product 
DROP COLUMN discount_id;