# Payment Gateway

## Background

## Requirements
The product requirements for this initial phase are the following:
1. A merchant should be able to save a card through the payment gateway and receive either a
successful or unsuccessful response
2. A merchant should be able to retrieve the details of a previously made payment

## Deliverables
1. Build an API that allows a merchant to
  • Process a payment through your payment gateway.
  • Retrieve details of a previously made payment.

##  Description

A simple web api. It consists of a backend only.

## Solution Description
 
## Target Frameworks
Target frameworks: .NET core 3.1

## Technology
 - C#
 - .Net core
 - Swagger
 - NUnit
 - Moq
 - Dapper
 - SQL Database
 - Docker conatins SQL Database
 - Docker to build the project

## Architecture
### CQRS
It clears that we need two paths one path to write and other path to read. so CQRS is best soluation.
CQRS stands for Command Query Responsibility Segregation. We use a different model to update information than the model you use to read information.

### EventSourcing
The gateway should be stable when doing a huge number of requests so we are using Event sourcing.
Capture all changes to an application state as a sequence of events.
Event Sourcing ensures that all changes to application state are stored as a sequence of events.  
1- we query these events,  
2- we can also use the event log to reconstruct past states, and as a foundation to automatically adjust the state to cope with retroactive changes.

### Onion architecture
The Onion Architecture is an Architectural Pattern that enables maintainable and evolutionary enterprise systems.

### Unit tests 
 It validates if that code results in the expected state (state testing) or executes the expected sequence of events (behavior testing).  
 It covers more than 83% of all code and more than 98% for Domain.  

### Integration tests
individual software modules are combined and tested as a group

### Swagger documentation
  - Swagger generate file for last version of api under this link ```/swagger/v1/swagger.json```
  - I created another swagger file before start working under ```Git\Payment.Gateway\WebApi\docs\WebApi.swagger.json```

### Database scripts 
Path under ```WebApi\DBScript\```
Database scripts to create SQL tables 

##  How to run the code
To start the internal service and its dependencies locally, execute:
    ```
    docker-compose up --build
    ```
    run from visual studio

## Deliverables  
1. Build an API that allows a merchant to  
    A. Process a payment through your payment gateway.  
      Using API url Post action ```/api/v1/payment/request-payment``` 
        Input Dto like
    ```
                {
                  "Card": {
                      "Number": "5105105105105100",
                      "Expiry": "9/24",
                      "Cvv": "123"
        
                  }
                }
    ```
    • In case of Accepted result will be like :-
        Http status 200 Ok
    ```
        {
          "PaymentId": "e075b909-1971-4e83-a4e5-c4c79eb09501",
        }
    ```
    PaymentId:- is id generating by our gateway system and it's unique for every transaction.

    B. Retrieve details of a previously made payment
    • Get details for payment by gateway payment id 
      ```
      api/v1/payment-details/{PaymentId}
      ``` 
      it should return one payment
      ```
      {
            "paymentId": "183ed540-cb9a-4c37-bbbd-ca0804f266cf",
            "cardNumber": "510510xxxxxx5100",
            "cardExpiry": "9/24",
            "cardCvv": "123",
            "lastUpdatedDate": "2020-12-24T08:47:44",
        }
      ```

## ToDo
1. Added more unit tests.
2. Replace Inmemory Service bus by any message bus service like Azure service bus or Amazon.
3. Using graphql at Api controller.
