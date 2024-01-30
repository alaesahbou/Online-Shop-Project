# Online Shop Project

The `Online Shop` project was developed using Angular for the front end, .Net Core 5 (API), and Microsoft SQL Server for the backend.

## Technology Stack

- **Angular:** This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 13.2.5.
- **Backend:** Web API using .Net 5.0 with Swagger code-generated documentation.
- **Database:** Microsoft SQL Server 2019.

## About Me

Hello, I'm [Your Name], the creator of this project. You can connect with me on [LinkedIn](https://www.linkedin.com/in/karthiksreenivasan/).

## User Interface

![Online Shop - Home Page](/gitimages/YourName.OnlineShop-AngularUserInterface_Part-1.png)
![Online Shop - Cart Items](/gitimages/YourName.OnlineShop-AngularUserInterface_Part-2.png)

## Swagger API Documentation

![Web API - User Controller](/gitimages/YourName.OnlineShop-SwaggerAPI_Doc.png)

## Project Description

Please find the description of the concepts implemented in this project detailed below:

1. **Front End - Online Shop Application**

- Angular with a bootstrap website template design from http://w3layouts.com/
- **Concepts:**

  - **Registration control validations:** Template-driven forms.
  - **Login control validations:** Reactive forms.
  - **Client-side authorization:** Route Guard.
    - Example: First-level verification to check for user authentication ensuring a JWT token was created in local storage.
  - **Server-side authorization:** HttpInterceptor.
    - Example: Sends JWT web token as an `HttpHeader` check for authorization of each API call.
  - **Services:** Services to make API calls and establish communication between unrelated components.
    - Example: Searching for products in the `Header` component triggers the search in the `Listproducts` component.
  - **Custom pipes:**
    - Example: Used for shortening product description for each product displayed on the home page.
  - **Dynamic routing:** Dynamic routing using URL parameters.
    - Example: Filter products based on their `Category ID`.

- **Performance:**

  - **Eager loading:** Modules that do not need authentication.
    - Example: Browsing products on the home page.
  - **Lazy loading:** Feature module that gets downloaded asynchronously using route parameter detailed below.
    - RouterModule.forRoot | preloadingStrategy parameter: PreloadAllModules.

- **Unit Testing:**

  - Added basic unit testing to initialize and verify component reference for the login component.

- **To-do: Part 1**

  - Detailed unit testing for each component.
  - User address capturing feature.
  - Payment feature.
  - Update or remove products.
  - Content for static pages.

2. **Backend End - .Net Core 5 Web API**

- .Net Core 5 Web API with Swagger documentation generated by code.
- **Concepts:**

  - **Swagger documentation and API testing:**
    - The API details are generated from the NuGet package `Swashbuckle.AspNetCore`.
    - The `swagger.json` is generated by code after compilation using information from `XML comments` in code using NuGet package `Swashbuckle.AspNetCore.Annotations`.
    - API testing is possible from this user interface.
  - **User Registration:** Passwords are hashed and stored in the database.
  - **Authentication:** Once the user credentials are validated, a JWT token is sent as a response.
  - **Authorization:** Endpoints that need authorization are protected by validating the JWT token using the concept `Authorize Attribute`.

- **To-do: Part 1**

  - Advanced logging support.
  - Implement microservices to further modularize each controller's endpoint to provide better maintenance and scalability.
    - Example: `UsersV1Controller` can be created as a new microservice.
  - Use the data access technology `Microsoft.EntityFrameworkCore` to perform CRUD operations in the database.

- **API Deployment - Prerequisites:**

  - While attempting to run this API, please create the `images` folder inside `wwwroot` folder manually before deployment.

3. **Database - Microsoft SQL Server 2019**

- **Database details:**
  - **Name:** OnlineShopDB.
- **DDL - Tables:**

  - **T_Users:** Entity that holds all the user credentials.
  - **T_Products:** Entity that holds product details.
  - **T_LU_ProductCategories:** Lookup table that holds category types, based on which products can be filtered.
  - **T_Orders:** Entity that holds a collection of cart items that is associated with a single active order. There can be multiple completed orders but only one active order for a single user.
  - **T_LU_OrdersStatus:** Lookup table that specifies the state of the order.
    - Example:
      - **In Progress:** Indicates an active order.
      - **Completed:** Indicates orders where the purchase was successful.
  - **T_Cart:** Entity that holds a list of products for a given order.

- **MSSQL Deployment - Prerequisites:**

  - It is mandatory to stage the lookup data for the below lookup tables to work correctly.
    - Insert_Into_T_LU_ProductCategories.sql
    - Insert_Into_T_LU_OrdersStatus.sql

- **To-do: Part 1**
  - Entity Relationship Diagram (ERD).