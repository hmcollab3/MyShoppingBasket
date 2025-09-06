# Kantar Shopping Basket — Development Bootstrap Guide

Welcome! This README explains how to bootstrap and run the Kantar Shopping Basket project locally using Docker. Follow these steps to get your development environment up and running.

---

## **Prerequisites**

- [Docker](https://www.docker.com/get-started) and [Docker Compose](https://docs.docker.com/compose/) installed.
- (Optional) [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) if you want to run or build the app outside Docker.

---

## **Setting Up**

### 1. **Clone the Repository**

```sh
git clone https://github.com/hmcollab3/MyShoppingBasket.git
```

### 2. **Configure Environment Variables**

- Create a .env file in the root directory:
    ```
    SA_PASSWORD=YourStrong!Passw0rd
    SQLSERVER_PORT=1433
    ```

---

## **Running the Application**

### **1. Build and Start All Services**

```sh
docker compose up --build
```
This will start:
- **SQL Server** (database)
- **Vault** (secrets management)
- **Vault Init** (initializes secrets)
- **Database Script Runner** (runs initial scripts)
- **WebAPI** (the main application)

**Note:** if you identify any issue with the dependencies, do a `docker compose down` followed by a `docker compose up --build`. I didn't implement healhchecks and, although container initialization has dependency definition, that alone doesn't ensure the dependency readiness.

### **2. Access the Web API**

After successful startup:
- Open: [http://localhost:5020](http://localhost:5020)

---

## **In-App Documentation**

The application includes an **in-app README** page, accessible from within the running WebAPI.  
This page provides a detailed overview of the project, including technical choices, usability walkthroughs, limitations, and future improvement ideas.  
For further insight beyond this bootstrap guide, visit the "Readme" page in the application UI.