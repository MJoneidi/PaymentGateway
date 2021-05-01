## Payment Gateway

The payment gateway will need to provide merchants with a way to process a payment.
  >Responsible for validating requests, storing card information and forwarding payment requests and accepting payment responses to and from the acquiring bank.

# Requirements
  The product requirements for this initial phase are the following:
    1. A merchant should be able to process a payment through the payment gateway and receive either a successful or unsuccessful response
    2. A merchant should be able to retrieve the details of a previously made payment


# Assumptions

  1. It needs to handle async requests 
  2. The project should be able to scale and handle large amonut of request.
  3. I assume, we already know the bank and just send our request to one bank which is called `Acquiring bank`
  4. For security purpose, all request and response will encrypt with public key and decrypt with private key. IP will use to increase the security


# Technology Stack

Asynchronous Request-Reply pattern
Command Query Responsibility Segregation (CQRS) 
Dapper



Why CQRS
> The Command and Query Responsibility Segregation (CQRS) pattern separates read and update operations for a data store. Implementing CQRS in your application can maximize its performance, scalability, and security. 
> If separate read and write databases are used, they must be kept in sync. Typically this is accomplished by having the write model publish an event whenever it updates the database. 
Usually CQRS is used with Event sourcing, but because it was out of scope of this task, I made it a little simple. Normally for small project with less request, writhing simple solution which has both read and write in one repository is best choice,  but because of first assumption, ability to scale the project, I chose this design pattern.



# API

# Test

# Run the project

All you need to do is running this command

```
docker-compose up
```

It will user port : 7001 and 7002
In case, these ports are not empty, you can change the default config in docker files
