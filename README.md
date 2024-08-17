# Excel Reader App
### DATABASE
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)

![Screenshot 2024-07-25 210216](https://github.com/user-attachments/assets/2f0ed897-1dc2-44ab-a217-a0f3a70330e7)

##### Relations: 
-   **Users to UploadedFiles**: One-to-Many (One user can upload multiple files)
-   **Users to Devices**: One-to-Many (One user can have multiple devices)
-   **Users to UserRoles**: One-to-Many (One user can have multiple roles)
-   **Roles to UserRoles**: One-to-Many (One role can be assigned to multiple users)
-   **UploadedFiles to Devices**: One-to-Many (One uploaded file can be associated with multiple devices)
-   **Devices to DeviceSensors**: One-to-Many (One device can have multiple device sensors)
-   **UploadedFiles to DeviceSensors**: One-to-Many (One uploaded file can be associated with multiple device sensors)
-   **Sensors to DeviceSensors**: One-to-Many (One sensor can be associated with multiple device sensors)
<hr>

### BACKEND
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)  ![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens) ![Rider](https://img.shields.io/badge/Rider-000000.svg?style=for-the-badge&logo=Rider&logoColor=white&color=black&labelColor=crimson) ![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)   

##### Features:
- User Login & Registration & Update
- Role Based Authorization with JWT
- Entitiy Framework Core with SQL Server
- Excel File Upload & Read & Delete

##### TO DO:
- History
- ~~Deatiled Filtering~~
<hr>

### FRONTEND
![React](https://img.shields.io/badge/react-%2320232a.svg?style=for-the-badge&logo=react&logoColor=%2361DAFB) ![NodeJS](https://img.shields.io/badge/node.js-6DA55F?style=for-the-badge&logo=node.js&logoColor=white)

##### Features: 
- Login & Register Page
- Detailed Charts on Dashboard
- User Update Page
- File Upload Page
#### TO DO:
- ~~Detailed Filtering~~
- ~~Print Charts to PDF~~
- Sidenav History 
