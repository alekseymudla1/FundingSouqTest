# FundingSouqTest

This implemented test task for Finding Souq by Aleksey Mudla

## Description

The app provides API for client managment
You can view swagger: https://localhost:7121/swagger/index.html

The api has two parts: 
- Identity managment part:
  Registration: POST https://localhost:7121/register
  Login: POST https://localhost:7121/login
  And other endpoints
- Client CRUD operations https://localhost:7121/api/clients

To get access to client endpoints you should be athorized as admin
The admin user account is seeded when the app starts
it has parameters:
email: admin@app.com
password: Password!1

Getting list of clients is at GET https://localhost:7121/api/clients endpoint
It has filtering, sorting and paging parameters
These parameters are being saved, you can see last 3 at GET https://localhost:7121/api/queries
