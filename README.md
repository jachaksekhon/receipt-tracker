# Receipt Tracker

We all lose track of receipts, whether they’re crumpled in the car or buried in an email. This project started as a personal idea: my mom always told my brother to keep his Costco receipts so she could check if anything went on sale, but of course, he never did.

Receipt Tracker solves that problem. Upload a receipt once, and the app automatically tracks your purchases. If an item you bought goes on sale within 30 days, you’ll get a notification so you can walk into Costco, show your receipt, and get your money back.

Especially in todays economy, every dollar counts. Saving money is making money!

---

## Features (WIP)

- [x] **User Authentication**  – Secure signup/login with JWT.
- [x] **Receipt Upload and Processing**  – Upload, process and store receipts in a database.
- [x] **Receipt Parsing (OCR)** – Automatically extract products, store and date from uploaded receipts using OpenAI API
- [ ] **Price Drop Detection** – Match receipts against weekly flyers for discounts.
- [ ] **User Notifications**   – Alert users when purchased items go on sale.
- [ ] **Deploy application for public use** 

---

## Tech Stack

### Frontend
- Next.js + TypeScript  
- TailwindCSS  

### Backend
- C# / .NET 8 Web API
- PostgreSQL (via Docker) → Production planned for **AWS EC2 + RDS**  
- Entity Framework Core
- JWT Authentication
- OpenAI GPT-4o-mini API for OCR-based receipt parsing

### DevOps / Infrastructure
- Docker (containerized backend + database)
- Environment variables via `.env`  
- Planned deployment on AWS using Docker Compose or ECS  

---


## Getting Started

Clone the repo and follow setup instructions for:

- **Backend Setup** (WIP)  
- **Frontend Setup** (WIP)  

---
