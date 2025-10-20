# Crypto Mining Profitability Calculator ⛏️💰

A Windows Forms desktop application built with **VB.NET** and **MySQL** that calculates the potential profitability of cryptocurrency mining. Users can input various parameters like hardware cost, hashrate, power consumption, and electricity cost to estimate daily and monthly profits.

![Mining Calculator Screenshot](image.jpg)

---

## 📖 About The Project

This project was developed as a college assignment to create a practical tool for cryptocurrency miners. It provides a user-friendly interface to perform complex profitability calculations, helping users make informed decisions about their mining operations. The application securely stores user data and calculation history in a MySQL database.

---

## ✨ Features

-   **🔐 Secure User Authentication:** Separate forms for user registration and login.
-   **📈 Dynamic Profit Calculation:** Takes the following inputs to calculate profit:
    -   Selected Cryptocurrency (e.g., Bitcoin)
    -   Hardware Costs ($)
    -   Hashrate (MH/s)
    -   Power Consumption (W)
    -   Electricity Cost ($/kWh)
-   **📊 Profit Display:** Clearly shows calculated **Daily Profit** and **Monthly Profit**.
-   **📜 Calculation History:** Users can view a history of their past calculations.
-   **📄 Print to PDF:** Option to print and save calculation results as a PDF file.
-   ** sleek, modern UI with a stock market theme.

---

## 🛠️ Tech Stack

-   **Front-End:** **Visual Basic .NET (.NET Framework)** - Windows Forms
-   **Back-End:** **MySQL**
-   **IDE:** **Visual Studio 2022**
-   **Key Libraries:** `Newtonsoft.Json` for data handling, `MySql.Data` for database connection.

![Tech Badges](https://img.shields.io/badge/VB.NET-512BD4?style=for-the-badge&logo=visualbasic&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white)

---

## 🚀 Getting Started

To get a local copy up and running, follow these detailed steps.

### Prerequisites

1.  **Visual Studio 2022**
    -   Make sure the **.NET desktop development** workload is installed.
2.  **MySQL Server & Workbench**
    -   Download and install from the official [MySQL website](https://dev.mysql.com/downloads/).
    -   During installation, remember your `root` password.
3.  **MySQL Connector/NET**
    -   This is required for VB.NET to communicate with MySQL. You can install it via the NuGet Package Manager in Visual Studio[15].

### Installation & Setup

1.  **Clone the Repository**
    ```
    git clone https://github.com/your-username/crypto-mining-calculator.git
    ```

2.  **Database Setup**
    -   Open **MySQL Workbench** and connect to your local server.
    -   Create a new schema (database) for the project. Let's name it `crypto_calc_db`.
      ```
      CREATE SCHEMA `crypto_calc_db`;
      ```
    -   Run the provided `database_schema.sql` file (you will need to create this file) to set up the necessary tables (`users`, `calculations`, etc.).

3.  **Configure the Application**
    -   Open the project solution (`.sln` file) in **Visual Studio 2022**.
    -   Locate the module or class where the database connection string is defined (e.g., a file named `DBConnect.vb` or similar).
    -   Update the connection string with your MySQL server details:
      ```
      ' Example Connection String
      Dim connStr As String = "server=127.0.0.1;user=root;password=YOUR_PASSWORD;database=crypto_calc_db;"
      ```
      Replace `YOUR_PASSWORD` with the MySQL root password you set during installation.

4.  **Install NuGet Packages**
    -   Right-click on the project in the Solution Explorer and select "Manage NuGet Packages...".
    -   Go to the "Browse" tab and search for the following packages, then install them:
        -   `MySql.Data`
        -   `Newtonsoft.Json`

5.  **Build and Run**
    -   Build the solution by pressing `Ctrl+Shift+B`.
    -   Run the project by pressing `F5` or clicking the "Start" button. The login form should appear.

---

## ✍️ Author

**Mohammed Abubakker Siddiqui**

-   **LinkedIn:** [your-linkedin-profile](https://www.linkedin.com/in/your-profile-url)
-   **GitHub:** [@your-username](https://github.com/your-username)
-   **Email:** your.email@example.com

---

## 🙏 Acknowledgments

*   This project was developed as part of my coursework at KJU.
*   Thanks to the open-source community for providing the necessary tools and libraries.
