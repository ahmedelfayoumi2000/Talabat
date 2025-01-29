# Talabat API

## Overview

The **Talabat API** is a robust and scalable RESTful API designed to manage core functionalities for an online food delivery platform. It provides endpoints for user authentication, basket management, order processing, product catalog, and payment integration. Built with **ASP.NET Core**, this API follows best practices for security, performance, and maintainability.

### Key Features:
- **User Management**: Register, login, and manage user profiles, including address information.
- **Basket Management**: Add, update, and delete items in a user's shopping basket.
- **Order Management**: Create, retrieve, and manage orders with support for multiple delivery methods.
- **Product Catalog**: Browse products with filtering, sorting, and pagination options.
- **Payment Integration**: Securely process payments using **Stripe** and handle webhook events for payment status updates.
- **Caching**: Improve performance with response caching for frequently accessed data.
- **Validation**: Ensure data integrity with robust validation and error handling.

### Technologies Used:
- **Backend**: ASP.NET Core
- **Database**: Entity Framework Core (SQL Server)
- **Authentication**: JWT (JSON Web Tokens)
- **Payment Gateway**: Stripe
- **Caching**: In-memory caching
- **Mapping**: AutoMapper
- **Logging**: Built-in logging with ILogger

### Target Audience:
This API is designed for developers building food delivery applications, e-commerce platforms, or any system requiring user management, order processing, and payment integration.

---

## Getting Started

Follow the steps below to set up and run the project locally.

### Prerequisites:
- [.NET SDK](https://dotnet.microsoft.com/download) (version 7.0 )
- [Git](https://git-scm.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Stripe Account](https://stripe.com/) (for payment integration)

### Installation:
1. **Clone the repository**:
   ```bash
   git clone https://github.com/ahmedelfayoumi2000/Talabat.git
2. **Install dependencies**:
   ```bash
   dotnet restore
2. **Configure the database**:
   ```bash
   dotnet ef database update
2. **Run the project**:
   ```bash
   dotnet run
---
# API Endpoints

## 1. **Account Management**

### `POST /api/account/login`
- **Description**: Authenticates a user and returns user details with a token.
- **Input**:
  ```json
  {
    "email": "string",
    "password": "string"
  }
- **Output**:
  ```json
  {
  "displayName": "string",
  "email": "string",
  "token": "string"
  }


### `POST /api/account/register`
- **Description**: Registers a new user.
- **Input**:
     ```json
   {
    "email": "user@example.com",
    "password": "7@/=bRt1~5e75FC&nlU wN76x#_",
    "displayName": "string",
    "phoneNumber": "string",
    "firstName": "string",
    "lastName": "string",
    "city": "string",
    "country": "string",
    "street": "string"
  }
- **Output**:
  ```json
  {
  "displayName": "string",
  "email": "string",
  "token": "string"
  }

### `GET /api/account`
- **Description**: Retrieves the current authenticated user's details.
- **Input**: None.
    
- **Output**:
  ```json
   {
    "email": "string",
    "displayName": "string",
    "token": "string"
  }  

### `GET /api/account/address`
- **Description**:  Retrieves the current user's address.
- **Input**: None.
  
- **Output**:
  ```json
   {
  "firstName": "string",
  "lastName": "string",
  "city": "string",
  "country": "string",
  "street": "string"
  }

### `PUT /api/account/address`
- **Description**: Updates the current user's address.
- **Input**:
     ```json
   {
  "firstName": "string",
  "lastName": "string",
  "city": "string",
  "country": "string",
  "street": "string"
  }
- **Output**: Returns the updated address.

 ### `GET /api/account/emailexists`
- **Description**:Checks if an email exists in the system.
- **Input**:  email as a query parameter.
    
- **Output**:
    ```json
      {
    true
     OR
    false
  }  
   

---

## 2. **Basket Management**

### `GET /api/basket`
- **Description**: Retrieves the user's basket by basketId.
- **Input**: basketId as a query parameter.

- **Output**:
  ```json
  {
  "id": "string",
  "items": [
    {
      "productId": "int",
      "productName": "string",
      "price": "number",
      "quantity": "int"
    }
  ]
  }

### `POST /api/basket`
- **Description**: Updates the user's basket.
- **Input**:
     ```json
   {
    "id": "string",
    "items": [
      {
        "productId": "int",
        "productName": "string",
        "price": "number",
        "quantity": "int"
      }
    ]
  }
    
- **Output**: Returns the updated basket.

### `DELETE /api/basket`
- **Description**: Deletes the user's basket by basketId.
- **Input**: basketId as a query parameter.
- **Input**:
     ```json
   {
    true
     OR
    false
  }

---

## 3. ** Orders Management**

### `POST /api/orders`
- **Description**:Creates a new order.
- **Input**:
    ```json
   {
    "basketId": "string",
    "deliveryMethodId": "int",
    "shipToAddress": {
      "firstName": "string",
      "lastName": "string",
      "city": "string",
      "country": "string",
      "street": "string"
    }
  }

- **Output**:
  ```json
  {
  "id": "1",
  "buyerEmail": "string",
  "orderDate": "string",
  "shipToAddress": {
    "firstName": "string",
    "lastName": "string",
    "city": "string",
    "country": "string",
    "street": "string"
  },
  "deliveryMethod": "string",
  "orderItems": [
    {
      "productName": "string",
      "price": "number",
      "quantity": "int"
    }
  ],
  "total": "1"
  }

### `GET /api/orders`
- **Description**: Retrieves all orders for the current user.
- **Input**: None.

- **Output**:
  ```json
  [
  {
    "id": "int",
    "buyerEmail": "string",
    "orderDate": "string",
    "shipToAddress": {
      "firstName": "string",
      "lastName": "string",
      "city": "string",
      "country": "string",
      "street": "string"
    },
    "deliveryMethod": "string",
    "orderItems": [
      {
        "productName": "string",
        "price": "number",
        "quantity": "int"
      }
    ],
    "total": "number"
  }
  ]
   

### `GET /api/orders/{id}`
- **Description**: Retrieves details of a specific order for the current user.
- **Input**: id as a route parameter.

- **Output**:
  ```json
  {
    "id": "int",
    "buyerEmail": "string",
    "orderDate": "string",
    "shipToAddress": {
      "firstName": "string",
      "lastName": "string",
      "city": "string",
      "country": "string",
      "street": "string"
    },
    "deliveryMethod": "string",
    "orderItems": [
      {
        "productName": "string",
        "price": "number",
        "quantity": "int"
      }
    ],
    "total": "number"
  }

### `GET /api/orders/deliveryMethod`
- **Description**: Retrieves all available delivery methods.
- **Input**: None.

- **Output**:
  ```json
   [
    {
      "id": "int",
      "name": "string",
      "price": "number"
    }
  ]

---

## 4. ** Payments Management**
  
### `POST /api/payments/{basketId}`
- **Description**: Creates or updates a payment intent for a specific basket.
- **Input**: basketId as a route parameter.

- **Output**:
  ```json
  {
  "id": "string",
  "items": [
    {
      "productId": "int",
      "productName": "string",
      "price": "number",
      "quantity": "int"
    }
  ],
  "paymentIntentId": "string"
  }

### `POST /api/payments/webhook`
- **Description**: Handles incoming Stripe webhook events to update payment status.
- **Input**: Stripe webhook event.

- **Output**: None (updates the order status internally).

---

## 5. **Products Management**

### `GET /api/products`
- **Description**: Retrieves a list of products with filtering and pagination options.
- **Input**: Query parameters like pageIndex, pageSize, brandId, typeId, sort.
   
- **Output**:
  ```json
   {
    "pageIndex": "int",
    "pageSize": "int",
    "count": "int",
    "data": [
      {
        "id": "int",
        "name": "string",
        "description": "string",
        "price": "number",
        "pictureUrl": "string",
        "productBrand": "string",
        "productType": "string"
      }
    ]
  }

### `GET /api/products/{id}`
- **Description**:Retrieves details of a specific product.
- **Input**: id as a route parameter.

- **Output**:
  ```json
  {
  "id": "int",
  "name": "string",
  "description": "string",
  "price": "number",
  "pictureUrl": "string",
  "productBrand": "string",
  "productType": "string"
  }

### `GET /api/products/brands`
- **Description**: Retrieves a list of product brands.
- **Input**: None.

- **Output**:
  ```json
   [
    {
      "id": "int",
      "name": "string"
    }
  ]

### `GET /api/products/types`
- **Description**: Retrieves a list of product types.
- **Input**: None.

- **Output**:
  ```json
   [
    {
      "id": "int",
      "name": "string"
    }
  ]


