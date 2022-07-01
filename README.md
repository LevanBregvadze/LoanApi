# LoanAPI

Simple Loan Api created with Asp.net and  EntityFrameworke.



## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)

## General Info

RestAPI for simple financial services, with role-based Authentication. It has the role of User and Admin and any registration as a User application automatically creates a Custome record in DataBase. The user has limited access. The password is stored in the database by hashing. Unit-test is written for each service. 


## Technologies

* .NET 5.0
* EntityFramework 5.0



## SetUp

Use the package manager to create DB locally.

```bash
 EntityFrameworkCore\Add-Migration name
```

```bash
 EntityFrameworkCore\Update-Database
```



## Usage

1. Admin has access to every Endpoint
2. User has limited access. It can GET, PUT and POST for only himself. 


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
