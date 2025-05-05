# Travel Agency Database – APBD Assignment

This repository contains the SQL script to initialize the database for the travel agency REST API.

---

## Database Schema

The script creates the following tables:
- **Client**
- **Trip**
- **Country**
- **Country_Trip** (junction table)
- **Client_Trip** (junction table)

All primary and foreign keys are set according to the assignment's ERD.

---

## How to Initialize the Database

1. **Open your SQL client** (DataGrip, SSMS, Azure Data Studio, etc.).
2. **Connect to your SQL Server instance** (see below for connection details).
3. **Run the `script.sql` file** to create tables and insert sample data.

---

## Connecting to the Database

### PJATK School Database

- **Connection String:**
  ```
  Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True
  ```
- **Instructions:**
  - Make sure you are connected to the university network or VPN.
  - Use your student credentials for authentication.
  - For detailed steps, see the official guide:  
    [MSSQL in DataGrip on macOS (PJATK)](https://bss.pja.edu.pl/en/docs/students/macos/mssql_datagrip/)

### Local SQL Server (Docker)

- **Run SQL Server in Docker:**
  ```bash
  docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
  ```
- **Connection String:**
  ```
  Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=YourStrong!Passw0rd;Trust Server Certificate=True
  ```
- **Instructions:**
  - Connect using your SQL client with the above credentials.
  - Run the `script.sql` to initialize the database.

- **Reference:**  
  [Running Microsoft SQL Server in Docker](https://medium.com/@analyticscodeexplained/running-microsoft-sql-server-in-docker-a8dfdd246e45)

---

## Sample Data

The script inserts sample data for:
- 10 countries
- 10 trips
- 10 clients
- Trip-country and client-trip relationships

---

## References

- [PJATK DataGrip/MSSQL Guide](https://bss.pja.edu.pl/en/docs/students/macos/mssql_datagrip/)
- [Running Microsoft SQL Server in Docker](https://medium.com/@analyticscodeexplained/running-microsoft-sql-server-in-docker-a8dfdd246e45)

---

