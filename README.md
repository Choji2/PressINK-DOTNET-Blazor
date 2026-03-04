# PressINK - Network Printer Management & Monitoring System

A sophisticated ASP.NET 8 Blazor application designed for real-time monitoring and management of network printers across enterprise environments. Built with a focus on enterprise reliability, LDAP authentication, and comprehensive printer status tracking.

## ⚠️ Important Prerequisites

**This project is NOT a plug-and-play solution.** It requires significant manual configuration and customization for your environment:

- **Network Printers Only**: Designed specifically for network-connected printers (tested with Lexmark multifunction devices)
- **Manual Configuration Required**: Database schema, LDAP servers, API endpoints, and printer inventory must be manually configured
- **Status API Integration**: Requires manual implementation of status APIs for your specific printer models
- **LDAP Integration**: Requires connection to your organization's LDAP/Active Directory infrastructure
- **Database Setup**: MySQL database creation and schema population needed

---

## 📋 Features

### Core Capabilities
- **Real-time Printer Monitoring**: Live status tracking for networked multifunction printers
- **Health Monitoring**: Network connectivity checks via ICMP ping
- **Status API Integration**: Automated data collection from printer status endpoints
- **LDAP Authentication**: Enterprise authentication using Active Directory/LDAP
- **Role-Based Access Control**: Two-tier authorization (Administrator/Standard User)
- **Responsive UI**: MudBlazor Material Design components with local storage support

### Additional Components
- **API Form Templates**: Customizable form generation for printer data
- **Real-time Statistics**: Live printer status dashboard
- **User Management**: Admin and standard user roles
- **Error Tracking**: Comprehensive logging via Serilog
- **Data Export**: Support for template-based API data collection

---

## 🛠️ Tech Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| Framework | ASP.NET Core | 8.0 |
| UI Framework | Blazor Server-side | 8.0 |
| Database | MySQL | Via Entity Framework Core 8.0 |
| Authentication | JWT + LDAP | Custom implementation |
| UI Components | MudBlazor | 6.8.0 |
| Logging | Serilog | 4.3.0 |
| Storage | Blazored LocalStorage | 4.5.0 |

---

## 📋 System Requirements

- **.NET 8.0 SDK** or later
- **MySQL 8.0** or later (or compatible MySQL variant)
- **Active Directory/LDAP Server** (for authentication)
- **Network Access** to printer IP addresses (multicast and unicast)
- **Printer Status APIs** accessible from application server
- **Admin rights** to configure application on hosting server

---

## ⚙️ Configuration Required

### 1. **Database Configuration**
Create a MySQL database and update connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "ConnectionPressINK": "Server=[YOUR_SERVER];Database=PressINK;User=[USER];Password=[PASSWORD];port=3306;"
  }
}
```

### 2. **LDAP Server Configuration** (Manual)
Edit `SD.cs` with your organization's LDAP settings:

```csharp
public const string LDAP_String = "OU=YourOU,DC=YourDomain,DC=com";
public const string LDAP_Engineer = "Your_Engineer_Group";
public const string LDAP_Sales = "Your_Sales_Group";

public List<string> LDAP
{
    get { return new() { "ldap://your-server1.com", "ldap://your-server2.com" }; }
}
```

### 3. **Printer Status API Endpoints** (Manual Implementation)
Configure API server addresses in `SD.cs`:

```csharp
public const string APIServer1 = "printer-api-1.yourdomain.com";
public const string APIServer2 = "printer-api-2.yourdomain.com";
public const string APIPort1 = "5443";
public const string APIPort2 = "6443";
public const string REQ_Header = "Your_API_Header_Name";
public const string VALID = "Your_API_Security_Token";
```

### 4. **JWT Bearer Token Configuration**
Update JWT settings in `appsettings.json`:

```json
{
  "JwtSettings": {
    "Issuer": "PressINK",
    "Audience": "PressINK",
    "SecretKey": "[BASE64_ENCODED_SECRET]",
    "TokenName": "PressINK_AUTH_Token"
  }
}
```

### 5. **Printer Inventory Database Population** (Manual)
Populate the `PRINTER_MAIN` table with your printer inventory:

| Field | Type | Required | Example |
|-------|------|----------|---------|
| HOSTNAMEv4 | varchar(15) | Yes | 192.168.1.100 |
| QUE | varchar(50) | Yes | PrinterName_Q |
| ASSET_NUMBER | varchar(50) | Yes | AST-12345 |
| LOCATION | varchar(20) | Yes | Building-A-Floor2 |
| MODEL_ID | varchar(10) | Yes | Lexmark_5255 |
| PLANT_ID | varchar(5) | Yes | P001 |
| CATEGORY_ID | varchar(5) | Yes | MFP |

### 6. **Data Protection Keys**
The `ProtectKeys/` directory contains XML-based data protection keys. Ensure these are properly copied to your deployment environment.

---

## 📦 Installation

### Step 1: Clone Repository
```powershell
git clone https://github.com/yourusername/PressINK-DOTNET-Blazor.git
cd PressINK-DOTNET-Blazor/PressINK-Server-App
```

### Step 2: Restore Dependencies
```powershell
dotnet restore
```

### Step 3: Complete Configuration (See Configuration Section Above)
Manually configure all items in the configuration section before proceeding.

### Step 4: Create/Apply Database
```powershell
# Create database
dotnet ef database create

# Apply migrations
dotnet ef database update
```

### Step 5: Build Application
```powershell
dotnet build
```

### Step 6: Run Application
```powershell
# Development
dotnet run

# Production
dotnet publish -c Release
```

The application will be available at `https://localhost:7224` (development) or your configured hosting address.

---

## 📂 Project Structure

```
PressINK-Server-App/
├── Data/
│   ├── API_Forms/          # Form context and schemas
│   ├── Auth/               # Authentication database context
│   └── PrinterDBContexts/  # Printer data context
├── Model/
│   ├── API Data/           # API request/response models
│   ├── API_Form Models/    # Template attribute models
│   ├── Auth. Models/       # LDAP and authentication models
│   ├── Printer DB Models/  # PRINTER_MAIN and related schemas
│   └── PrinterAPIModels/   # API response models (Lexmark, etc.)
├── Services/
│   ├── APIServices.cs      # Background service for API polling
│   ├── PingServices.cs     # Network connectivity monitoring
│   ├── PrinterService.cs   # Printer data access layer
│   ├── APIInjectServices.cs# Dependency injection for API
│   └── Auth.Services/      # LDAP and JWT authentication
├── Pages/
│   ├── Admin/              # Administrator pages
│   ├── Info Views/         # Information/detail pages
│   ├── TemplateViews/      # Template management pages
│   ├── LiveStats.razor     # Real-time statistics dashboard
│   └── PrinterLanding.razor# Main printer listing page
├── Shared/
│   ├── MainLayout.razor    # Primary layout component
│   ├── NavMenu.razor       # Navigation menu
│   └── Auth/               # Authentication components
├── Migrations/             # Entity Framework migrations
├── Certs/                  # SSL certificates (if needed)
├── ProtectKeys/            # Data protection key store
└── wwwroot/                # Static assets (CSS, images)
```

---

## 🔑 Key Services

### APIServices (Background Service)
- Polls printer status APIs at configurable intervals
- Aggregates API responses with printer database records
- Handles error states and null responses
- Thread-safe printer data collection

### PingServices (Background Service)
- Monitors network connectivity to all printers
- Performs ICMP echo requests (ping)
- Tracks response times and availability
- Runs continuously for real-time health monitoring

### PrinterService (Data Access Layer)
Provides methods for:
- Retrieving printers by ID, hostname, asset number, plant, or model
- Fetching printer attributes and configurations
- Template management for UI generation

### Authentication Services
- **LdapAuthenticationService**: Validates user credentials against LDAP/AD
- **AuthenticationHandler**: JWT token generation and validation
- Implements role-based authorization (SuperUser vs. Standard)

---

## 📊 Database Schema Overview

### Core Tables
- **PRINTER_MAIN**: Master printer inventory
- **PrinterAttributes**: Configurable printer data fields
- **AdminDB**: User/authentication records
- **API_Forms**: API request/response templates

### Supporting Tables
Status tracking, model definitions, and form templates are automatically managed via Entity Framework Core.

---

## 🔐 Authentication & Authorization

The application implements a two-tier authentication system:

1. **LDAP/Active Directory Authentication**
   - User credentials validated against your organization's directory
   - Automatic group membership detection (Engineer, Sales, etc.)

2. **JWT Token-Based Authorization**
   - Token issued upon successful LDAP login
   - Token stored in session for subsequent API requests
   - Two authorization policies:
     - `RequireAdministratorRoleORStandard`: Full access
     - `RequireAuthenticatedUser`: Basic authenticated access

3. **Role-Managed Access**
   - **SuperUser**: Full system access, configuration management
   - **Standard**: View-only or limited operational access

---

## ⚠️ Manual Customizations Required

### Status API Models
The application includes model scaffolding for Lexmark MFP devices. For other printer models, you must:

1. Create new model classes in `Model/PrinterAPIModels/{ManufacturerName}/`
2. Implement `IPrinter` interface with your device's response schema
3. Update `APIServices.cs` to instantiate your new model type based on printer type
4. Map API response fields to your model properties

**Example Structure:**
```csharp
// Create custom model
public class CanonIR : IPrinter { /* ... */ }

// Update APIServices
if (printer.MODEL_ID.Contains("Canon")) {
    stats = JsonConvert.DeserializeObject<CanonIR>(apiResponse);
}
```

### Database Tables
Add custom printer attributes by:
1. Creating new columns in the printer schema via EF Core migrations
2. Adding properties to `PRINTER_MAIN` model
3. Populating `Model_Attributes` for dynamic UI generation

### API Polling Intervals
Adjust polling frequency in `Program.cs` where services are registered, or implement configurable intervals via `appsettings.json`.

---

## 🚀 Deploying to Production

### IIS Deployment
Update `SD.cs` with your IIS virtual directory:
```csharp
public const string IIS_NAV = "/press-ink";
```

### Docker (Custom Implementation Needed)
Create a `Dockerfile` containerizing the application with:
- .NET 8 runtime
- MySQL client libraries
- LDAP/SSL certificates properly mounted

### HTTPS Configuration
1. Place SSL certificates in `Certs/` directory
2. Configure in `Program.cs` or appsettings
3. Update connection strings to use HTTPS endpoints

---

## 📝 Logging

Logs are generated by Serilog to both console and rolling file outputs:
- **File Location**: `logs/PressLogs-{DATE}.txt`
- **Minimum Level**: Warning (configurable in Program.cs)
- **Daily Rolling**: New file created each day

Monitor logs for:
- LDAP authentication failures
- API call timeouts or errors
- Database connection issues
- Printer connectivity changes

---

## 🤝 Contributing

This is an enterprise application with specific use cases. If contributing:

1. **Model-Specific Code**: Keep printer model integrations isolated in their own folders
2. **Configuration**: Never commit hardcoded credentials; use `appsettings.{Environment}.json`
3. **Testing**: Test LDAP authentication and API connectivity thoroughly
4. **Documentation**: Update configuration sections if adding new manual setup steps

---

## 📄 License

--

---

## 📞 Support & Documentation

### Known Limitations
- **Single Manufacturer Focus**: Currently optimized for Lexmark devices; extending to other manufacturers requires custom model implementation
- **No Multi-Site Replication**: Designed for single-site deployments
- **Manual Inventory**: Printer inventory must be manually entered into the database

### Getting Help
- Review `appsettings.Development.json` for configuration examples
- Check `logs/` directory for application error details
- Validate database connectivity before investigating API issues
- Ensure network firewall rules allow printer communication

### Future Enhancements
- Bulk printer import via CSV/Excel
- Additional printer manufacturer support
- Multi-site federation
- Automated backup and archiving of printer stats

---

## 🔧 Troubleshooting

### Application Won't Start
- Verify MySQL connection string is correct
- Ensure database exists and is accessible
- Check .NET 8 SDK is installed: `dotnet --version`

### LDAP Authentication Fails
- Validate LDAP server addresses in `SD.cs`
- Verify network connectivity to AD/LDAP servers
- Check `logs/` for detailed authentication errors
- Confirm user exists in configured OU

### Printers Not Appearing
- Populate `PRINTER_MAIN` table with printer inventory
- Verify printer IP addresses are reachable
- Check API server endpoints are accessible
- Review logs for API timeout errors

### API Calls Timeout
- Increase `Timeout` constant in `SD.cs` (currently 20 seconds)
- Verify network connectivity to API servers
- Check if API endpoints require authentication headers

---

**Last Updated**: March 2026  
**Framework Version**: .NET 8.0  
**Blazor Hosting Model**: Server-side
