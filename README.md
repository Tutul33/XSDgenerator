# XSD Generator (WPF Application)

A simple and modern **WPF application** that generates **XSD (XML Schema) files** from SQL Server stored procedures. This tool allows you to connect to a SQL Server database, select a stored procedure, and generate an XSD schema representing the result set.

---

## **Features**

- Connect to any SQL Server database using server, database, username, and password.
- Generate XSD from any stored procedure.
- Choose the folder where the XSD file will be saved.
- Clean, modern interface with easy-to-use controls.

---

## **Prerequisites**

- **Windows 10/11**
- **.NET Framework 4.7.2** (or compatible version installed)
- SQL Server access with permissions to execute stored procedures.

---

## **Installation**

1. Download the installer `.msi` or `.exe` file.
2. Run the installer and follow the on-screen instructions.
3. After installation, launch **XSD Generator** from the Start menu or desktop shortcut.

---

## **Usage**

1. **Enter Database Connection Details**:
   - **Server:** SQL Server instance name.
   - **Database:** Name of your database.
   - **User:** SQL Server username.
   - **Password:** SQL Server password.

2. **Enter Stored Procedure Name**:
   - Type the exact name of the stored procedure you want to generate an XSD for.

3. **Select Folder**:
   - Click **Select Folder** to choose the location where the XSD file will be saved.

4. **Generate XSD**:
   - Click **Generate XSD**.  
   - A `.xsd` file with the same name as the stored procedure will be created in the selected folder.
   - You will see a success message with the file path.

---

## **Example**

- **Stored Procedure Name:** `GetCustomers`  
- **Output:** `GetCustomers.xsd` containing all columns returned by the stored procedure with proper XSD types.

---

## **Supported SQL Data Types**

The application maps SQL data types to XSD types automatically:

| SQL Type               | XSD Type      |
|------------------------|--------------|
| `int`, `long`, `short` | `xs:int`     |
| `string`               | `xs:string`  |
| `DateTime`             | `xs:dateTime`|
| `bool`                 | `xs:boolean` |
| `decimal`, `double`, `float` | `xs:decimal` |
| `byte`                 | `xs:byte`    |

> All columns are marked as `nillable="true"` in the XSD by default.

---

## **Error Handling**

- If the folder is not selected, the app will prompt you to select one.
- Any database connection or stored procedure errors are displayed in a message box.

---

## **Support**

For issues, contact the developer or raise an issue in the repository (if hosted on GitHub).

