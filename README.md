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


#Technology Stack

# API

# Test

# Run the project
