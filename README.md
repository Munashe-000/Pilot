# PilotFlow

A platform used for workflow orchestration and approvals, which focuses on real‑time task routing, SLA escalations, and audit tracking. Built with .NET 8 (C#), AWS serverless, and Angular. This project aims to showcase my skills in building scalable and secure backends while still having an eye for style and User Experience. I use AWS services to extend the capability of our app to ensure scalability and availability. 

## What this repo contains
- Clean Architecture backend with CQRS/MediatR
- Serverless-first design (AWS Lambda + API Gateway + DynamoDB + SQS)
- Angular frontend
- Test suites and CI scaffolding

## Status
Feature 1 implemented, reviewed by AI agent and end-to-end tested.
Feature 2 implemented, reviewed by AI agent and end-to-end tested.

**Feature 1**
  - smooth user interface with UX in mind
  - submit an access request                                                                                                                                                                                                                                                        
  - create the related security task                                                                                                                                                                                                                                                
  - show that task in the task inbox  
  
**Feature 2**
  - take the created task                                                                                                                                                                                                                                                           
  - approve or reject it                                                                                                                                                                                                                                                            
  - update the request status                                                                                                                                                                                                                                                       
  - record the audit trail                                                                                                                                                                                                                                                          
  - show that audit data in the UI

**Feature 3**
