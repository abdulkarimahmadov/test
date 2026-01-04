# Discussion Forum - ASP.NET Core 8.0 MVC

A modern, full-featured discussion forum built with ASP.NET Core 8.0 MVC following Onion Architecture (Clean Architecture) principles.

## 🚀 Features

### Core Functionality
- **User Authentication & Authorization**
  - Register/Login with ASP.NET Core Identity
  - Role-based access control (Admin, Moderator, Member)
  - User profiles with display names

- **Forum Categories**
  - Multiple categories with icons and descriptions
  - Topic count tracking per category
  - Admin management of categories

- **Topics/Threads**
  - Create, edit, and delete topics
  - Pin important topics
  - Lock/unlock topics (Admin/Moderator)
  - View count tracking
  - Last activity timestamp

- **Posts/Replies**
  - Create, edit, and delete posts
  - Rich text support
  - Edit history tracking
  - Moderator can edit/delete any post

- **Voting System**
  - Upvote/downvote posts
  - Vote count display
  - User reputation tracking

- **Search & Filter**
  - Search topics by title and content
  - Filter by category
  - Sort by recent activity

- **Admin Panel**
  - Manage categories (CRUD)
  - Manage users (ban/unban)
  - View forum statistics

### UI/UX
- **Responsive Design** - Works on mobile, tablet, and desktop
- **Modern Bootstrap 5 Styling** - Clean and professional appearance
- **Bootstrap Icons** - Rich iconography throughout
- **Toast Notifications** - Success/error messages
- **Breadcrumb Navigation** - Easy navigation
- **Pagination** - Efficient browsing of topics and posts

## 🏗️ Architecture

### Onion Architecture (4 Layers)

1. **Domain Layer** (`DiscussionForum.Domain`)
   - Core business entities (User, Category, Topic, Post, Vote, Tag)
   - Enums (UserRole, TopicStatus)
   - Repository interfaces
   - No dependencies on other layers

2. **Application Layer** (`DiscussionForum.Application`)
   - Business logic and services
   - DTOs (Data Transfer Objects)
   - ViewModels for MVC views
   - Service interfaces and implementations

3. **Infrastructure Layer** (`DiscussionForum.Infrastructure`)
   - Data access with Entity Framework Core
   - ApplicationDbContext
   - Repository implementations
   - Database migrations
   - Seed data

4. **Web/Presentation Layer** (`DiscussionForum.Web`)
   - ASP.NET Core MVC Controllers
   - Razor Views
   - UI components
   - Dependency injection configuration

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server
- **ORM**: Entity Framework Core 8.0
- **Authentication**: ASP.NET Core Identity
- **UI**: Bootstrap 5 with Bootstrap Icons
- **Frontend**: Razor Views, jQuery

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community, Professional, or Enterprise)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server Express
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (Optional but recommended)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/abdulkarimahmadov/test.git
cd test
```

### 2. Configure Database Connection

Open `DiscussionForum.Web/appsettings.json` and update the connection string if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DiscussionForumDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Note**: If using SQL Server Authentication instead of Windows Authentication, use:
```json
"DefaultConnection": "Server=localhost;Database=DiscussionForumDB;User Id=YourUsername;Password=YourPassword;TrustServerCertificate=True;"
```

### 3. Open Solution in Visual Studio

1. Open Visual Studio 2022
2. Select **File → Open → Project/Solution**
3. Navigate to the cloned repository and open `DiscussionForum.sln`

### 4. Restore NuGet Packages

Visual Studio should automatically restore NuGet packages. If not:
1. Right-click the solution in Solution Explorer
2. Select **Restore NuGet Packages**

Or use Package Manager Console:
```powershell
Update-Package -reinstall
```

### 5. Apply Database Migrations

Open **Package Manager Console** in Visual Studio (Tools → NuGet Package Manager → Package Manager Console) and run:

```powershell
# Set the default project to Infrastructure
# Select "DiscussionForum.Infrastructure" from the dropdown in Package Manager Console

# Create initial migration (if not exists)
Add-Migration InitialCreate

# Apply migrations to create the database
Update-Database
```

This will:
- Create the `DiscussionForumDB` database
- Create all necessary tables
- Seed initial data (categories, admin user, sample topics)

### 6. Run the Application

1. Ensure `DiscussionForum.Web` is set as the startup project (right-click → Set as Startup Project)
2. Press **F5** or click the **Run** button
3. The application will open in your browser at `https://localhost:5001`

## 👤 Default Admin Credentials

After seeding, you can log in with:

- **Username**: `admin`
- **Email**: `admin@forum.com`
- **Password**: `Admin@123`

## 📱 Usage Guide

### For Regular Users

1. **Register**: Click "Register" in the navigation bar
2. **Browse**: Explore categories and topics
3. **Create Topics**: Navigate to a category and click "New Topic"
4. **Reply to Topics**: Open a topic and use the reply form at the bottom
5. **Vote**: Upvote or downvote posts using the arrow buttons
6. **Search**: Use the search bar in the navigation to find topics

### For Administrators

1. **Access Admin Panel**: Click "Admin" in the navigation (only visible to admins)
2. **Manage Categories**: Create, edit, or delete categories
3. **Manage Users**: View all users, ban/unban users
4. **Moderate Topics**: Pin, unpin, lock, or unlock topics
5. **View Statistics**: See total users, topics, and categories

## 🗂️ Project Structure

```
DiscussionForum/
├── DiscussionForum.Domain/
│   ├── Entities/           # Domain entities
│   ├── Enums/             # Enumerations
│   └── Interfaces/        # Repository interfaces
│
├── DiscussionForum.Application/
│   ├── DTOs/              # Data Transfer Objects
│   ├── Interfaces/        # Service interfaces
│   ├── Services/          # Service implementations
│   └── ViewModels/        # MVC ViewModels
│
├── DiscussionForum.Infrastructure/
│   ├── Data/              # DbContext and seeding
│   └── Repositories/      # Repository implementations
│
└── DiscussionForum.Web/
    ├── Controllers/       # MVC Controllers
    ├── Views/            # Razor Views
    ├── Models/           # View-specific models
    ├── wwwroot/          # Static files
    └── Program.cs        # Application startup
```

## 🔧 Configuration

### Identity Settings

Password requirements can be configured in `appsettings.json`:

```json
{
  "IdentitySettings": {
    "PasswordRequireDigit": true,
    "PasswordRequiredLength": 6,
    "PasswordRequireNonAlphanumeric": false,
    "PasswordRequireUppercase": true,
    "PasswordRequireLowercase": true
  }
}
```

### Database Connection

Update `appsettings.json` with your SQL Server connection details:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Connection_String_Here"
  }
}
```

## 🐛 Troubleshooting

### Database Connection Errors

If you encounter connection errors:
1. Verify SQL Server is running
2. Check the connection string in `appsettings.json`
3. Ensure you have permissions to create databases
4. Try using SQL Server Authentication instead of Windows Authentication

### Migration Errors

If migrations fail:
```powershell
# Remove all migrations
Remove-Migration

# Create a fresh migration
Add-Migration InitialCreate

# Apply to database
Update-Database
```

### Port Already in Use

If port 5001 is in use, modify `Properties/launchSettings.json`:
```json
{
  "applicationUrl": "https://localhost:5002;http://localhost:5003"
}
```

## 🧪 Testing

After setup:
1. Register a new user account
2. Create a topic in any category
3. Reply to the topic
4. Test voting on posts
5. Login as admin and access the admin panel
6. Test search functionality

## 📝 Sample Data

The application seeds with:
- **5 Categories**: General Discussion, Technology, Gaming, Sports, Entertainment
- **1 Admin User**: admin@forum.com
- **3 Sample Topics** with posts
- **Sample Posts** in topics

## 🚢 Deployment

### To IIS

1. Publish the Web project (right-click → Publish)
2. Choose IIS deployment target
3. Configure connection strings for production
4. Ensure .NET 8 Hosting Bundle is installed on server

### To Azure

1. Create an Azure App Service
2. Create an Azure SQL Database
3. Update connection strings
4. Publish from Visual Studio or use CI/CD

## 🤝 Contributing

This is a portfolio/demonstration project. Feel free to fork and modify for your needs.

## 📄 License

This project is open source and available for educational purposes.

## 📧 Contact

For questions or issues, please open an issue in the repository.

## 🎯 Success Criteria

✅ Solution opens in Visual Studio 2022 without errors  
✅ Database creates successfully with `Update-Database` command  
✅ Application runs on `https://localhost:5001`  
✅ Users can register, login, logout  
✅ Users can create topics and reply to posts  
✅ Voting system works  
✅ Admin can manage categories and users  
✅ UI is modern, beautiful, and responsive  
✅ No hardcoded connection strings (use appsettings.json)

## 🎨 Features Showcase

### Home Page
- Statistics dashboard
- Category listing with icons
- Recent topics feed

### Topic Discussion
- Threaded conversations
- Vote system
- Rich post formatting
- Moderation tools

### Admin Dashboard
- User management
- Category management
- Forum statistics
- Quick actions

## 🔒 Security Features

- CSRF protection
- XSS prevention through Razor encoding
- Password hashing with ASP.NET Identity
- Role-based authorization
- SQL injection prevention through EF Core parameterization

## 📚 Learning Resources

This project demonstrates:
- Onion Architecture / Clean Architecture
- Repository Pattern
- Unit of Work Pattern
- Dependency Injection
- Entity Framework Core
- ASP.NET Core Identity
- MVC Pattern
- Async/Await programming

---

**Built with ❤️ using ASP.NET Core 8.0 MVC**
