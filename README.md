# 🛠️ Fix It Now  

**Fix It Now** is a property maintenance management application that connects **tenants** and **property admins** in one place.  
It replaces manual calls, emails, or WhatsApp messages with a **streamlined system** for reporting, tracking, and resolving issues quickly and transparently.  

---

## 📌 Overview  
Property management often relies on outdated communication methods. Tenants report issues by phone or text, admins manually track requests, and there’s little transparency in how problems are resolved.  

**Fix It Now** solves this by:  
- Giving tenants an easy way to report issues (with photos/videos).  
- Allowing admins to track and update issue statuses in real time.  
- Supporting multiple admins per property, with different permission levels.  
- Creating transparency and accountability with status history, notifications, and reports.  

The result: **faster resolutions, fewer miscommunications, and better tenant satisfaction.**  

---

## ⚙️ How It Works  
1. Tenants log in, select their property/unit, and submit a maintenance request.  
2. The request is tracked through statuses: **Pending → In Progress → Completed**.  
3. Admins review requests, update statuses, and communicate directly with tenants.  
4. Tenants confirm resolution and provide feedback.  

---

## 👥 User Roles & Features  

### 🔹 Tenants  
- Create an account linked to their property/unit.  
- Submit maintenance requests (with description, category, and media).  
- Track the status of requests.  
- Receive notifications for updates.  
- Chat with admins.  
- Confirm resolution and leave feedback.  

### 🔹 Property Admins / Co-Admins  
- Role-based login (super admin, co-admin).  
- Manage multiple properties and units.  
- View all requests in a dashboard with filters.  
- Update request statuses (**Pending, In Progress, Completed**).  
- Communicate with tenants.  
- Generate reports (issue frequency, resolution times, etc.).  
- Add/remove co-admins.  

---

## 🏗️ Core Features  
- 📲 **Request Management** – Tenants submit, admins update, everyone tracks progress.  
- 🔔 **Notifications** – Email, push, or in-app updates for important events.  
- 🗂️ **Multi-Property Support** – Manage many properties in one system.  
- 👥 **Multiple Admins** – Assign different roles per property.  
- 📊 **Reports** – Insights on issue frequency and resolution performance.  
- 📝 **Status History** – Track every change made for accountability.  

---

## 🚀 Why This Project?  
The project is built to:  
- Practice **multi-role application design**.  
- Apply **Agile development methods** (user stories, sprints, documentation).  
- Explore **scalable backend + structured database design**.  
- Deliver a **real-world tool** that improves property maintenance management.  

---

## How to run the application

### 0. Clone the project and go to the apps directory
A. Clone the project
```bash
git clone https://github.com/Alexander-Agu/FIN.git
cd FIN
```

B. Activate scripts
```bash
chmod +x ./scripts/Activate-Script.sh
```

if you are on `linux` or if you have `git bash`
```bash
make activate-scripts
```

### 1. Restore the application
A. Using dotnet command
```bash
dotnet restore
```

B. Using `linux` or `git bash` on windows
```bash
make restore
```

### 2. Build the application
A. Using dotnet command
```bash
dotnet build
```

B. Using `linux` or `git bash` on windows
```bash
make build
```

### 3. Run the application
A. Using dotnet command
```bash
dotnet run
```

B. Using `linux` or `git bash` on windows
```bash
make run
```

## How to run the application using docker

### 1. Build the image first
A. Build in the terminal
```bash
docker build -t "fin" .
```

B. Using `linux` or `git bash` on windows
```bash
make build-image docker_name="fin"
```

### 2. Run the container
A. Run in the terminal
```bash
docker run -it -p 5000:5000 "fin"
```

B. Using `linux` or `git bash` on windows
```bash
make run-image docker_name="fin" docker_port="5000"
```

## How to run tests
### 1. From the root directory go to the Tests folder
```bash
cd Tests
dotnet test --logger "console;verbosity=detailed"
```

Or if using `linux` or `git bash` on windows -> just go the root directory and run
```bash
make test
```