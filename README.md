**Project README: Asset Management Website**

---

### Overview
This project is an Asset Management Website developed using React.js for the frontend, with React-Admin for UI components. The backend is built with .NET Core, utilizing Entity Framework for data access. SQL Server is employed as the database management system. Additionally, project management, Continuous Integration/Continuous Deployment (CI/CD), and application deployment are handled through Azure services.

---

### Technologies Used

- **Frontend**: React.js
- **UI Framework**: React-Admin
- **Backend**: .NET Core
- **Data Access**: Entity Framework
- **Database**: SQL Server
- **Cloud Services**: Azure

---

### Features

1. **Asset Management**: Users can view, add, edit, and delete assets.
2. **User Authentication**: Secure login and authentication mechanisms.
3. **Role-based Access Control**: Different user roles with varying levels of access permissions.
4. **Dashboard**: Provides an overview of asset statistics and data visualization.
5. **Reporting**: Generate reports on asset inventory, usage, etc.

---

### Setup Instructions

1. **Frontend Setup**:
   - Navigate to the `frontend` directory.
   - Install dependencies: `npm install`.
   - Start the development server: `npm start`.

2. **Backend Setup**:
   - Navigate to the `backend` directory.
   - Install dependencies: `dotnet restore`.
   - Configure the connection string for SQL Server in `appsettings.json`.
   - Run the migrations: `dotnet ef database update`.
   - Start the backend server: `dotnet run`.

3. **Azure Setup**:
   - Create Azure resources for CI/CD pipelines, Azure App Service for deployment, etc.
   - Configure Azure DevOps or GitHub Actions for CI/CD.

4. **Database Setup**:
   - Create a SQL Server database and configure the connection string in the backend.

---

### CI/CD Pipeline

The CI/CD pipeline is set up to automatically build, test, and deploy the application to Azure App Service upon code changes. Azure DevOps or GitHub Actions can be configured for this purpose.

---

### Contribution Guidelines

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/feature-name`.
3. Commit your changes: `git commit -am 'Add new feature'`.
4. Push to the branch: `git push origin feature/feature-name`.
5. Submit a pull request.

---

### Support

For any issues or questions, please contact [dattm283@gmail.com](dattm283@gmail.com).
