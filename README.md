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
  4. For security considrations, all request and responses will be encrypted via ssl. IP can be used to limit access(for now just skipped)
  5. JwtBearer used for authentication
  6. Validation on card number is based on Master and Visa card


# Technology Stack

Asynchronous Request-Reply pattern
Command Query Responsibility Segregation (CQRS) 
Dapper, NLog



Why CQRS
  > The Command and Query Responsibility Segregation (CQRS) pattern separates read and update operations for a data store. Implementing CQRS in your application can maximize its performance, scalability, and security. 
  > If separate read and write databases are used, they must be kept in sync. Typically this is accomplished by having the write model publish an event whenever it updates the database. 

Usually CQRS is used with Event sourcing, but because it was out of scope of this task, I made it a little simple. Normally for small project with less request, writhing simple solution which has both read and write in one repository is best choice,  but because of first assumption, ability to scale the project, I chose this design pattern.

Note : my first priritoy was make the project simple, I had to skip some part of CQRS, I know about it. The main idea of using CQRS was, separates read and update operations. First I want to use MediatR to handle commands, but because it was my first time to use it and time was limited, I'd prefer to go with simpler way for this project.

# API
  The api has two controllers
  1.AuthController , used for login and create JwtBearer token
  2.PaymentsController , used for processing the payment requests and retrieve details of a previously made payment
  3.I assume merchant should be define in system and the get valid merchantId, for now only valid "merchantId" is "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  
  *sample requests 
  
  1. For Login
  ```
  url : http://localhost:7001/api/Auth/Login
  
  request json body  :
  {
    "email": "merchant@mail.com",
    "password": "StrongPasswordMerchant123!"
  }
  
  response :
  
  {
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjAwMDAwMDAwLTAwMDAtMDAwMC0wMDAwLTAwMDAwMDAwMDAwMCIsInN1YiI6Im1lcmNoYW50QG1haWwuY29tIiwiZW1haWwiOiJtZXJjaGFudEBtYWlsLmNvbSIsImp0aSI6IjY4NTc3ODE1LTMyZDYtNGYwMC04ZjFhLWI0YzI3MmJmMGMwOCIsIm5iZiI6MTYxOTk0NTU2OCwiZXhwIjoxNjE5OTY3MTY4LCJpYXQiOjE2MTk5NDU1Njh9.xe2ixGCTDKo3OD8LfHgGb0prRVb_ztHJlWSu4tJToRV7HoxqRueroSM1wf1KT0OnCRMHZQyK59Fm3li2KC9hhg",
  "result": true,
  "errors": null
  }
  ```
  
  2. For processing the payment request
  
   ```
  url : http://localhost:7001/api/Payments
  
  request json body  :
  also need to add Bearer token in header 
  
  {
    "merchantId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "amount": 180,
    "currency": "USD",
	"cardNumber": "5555555555554344",
	"cardExpiry": "12/22",
	"cardCvv": 22
  }
  
  response sample :
  {
    "paymentResultId": "c113b134-4a1c-48b0-6fb2-08d90d667a61",
    "status": 0,
    "errorDescription": ""
  }
  ```  
  
  3. For retrieve details of a previously made payment
  
   ```
  url :
  http://localhost:7001/api/Payments/details?merchantId=3fa85f64-5717-4562-b3fc-2c963f66afa6&paymentId=d672a444-f1de-4345-e4c3-08d90d74c72a
  
  It has two parameter : merchantId, paymentId
  need to add Bearer token in header. 
  
  
  response sample :
  
  {
	  "paymentId": "f923fc9b-8f57-4a8d-a60e-08d90e047bb6",
	  "card": {
		"maskedCardNumber": "**** **** **** 4344",
		"expiry": "12/02"
	  },
	  "amount": {
		"currency": "USD",
		"value": 180
	  },
	  "status": 0
  }
  
  ```  




# Run the project

All you need to do is running this command

```
docker-compose up
```

It will user port : 7001 and 7002
In case, these ports are not empty, you can change the default config in docker compose file



* Note : Https is disabled because of certificate issued, but in real production, it should be active.
* Note : First I want to add a gateway (Ocelot) to manage authentication and logging all requests and response in one place, but then I changed my mind to make the project simple.
* Note : For scalability considerations, it needs to add kubernetes. we are using Helm in order to run dockers on kubernetes. but adding and configuring it needs more time, so I just skipp it for now.


# History of commits
All my commits which has codes, commited by this name "Mohammad Joneidi" and all the change readme file which I did it in github directly done by this name "MJoneid"

Samples
1. Mohammad Joneidi committed 1 minute ago
2. MJoneidi committed 6 days ago

It's because my official git account which I work for my current company set in VS by this name "Mohammadi Joneidi"


