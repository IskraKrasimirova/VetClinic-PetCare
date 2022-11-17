# 🐶 🐱 VetClinic PetCare
VetClinic PetCare is a Web application for making medical booking of your pets. It is my defense project for ASP.NET Core course at SoftUni.
## 📝 Overview
### Guest user
- Can see all information about departments, doctors and services that the vet clinic offers but he can't make an appointment for his pet.
- First he have to register (with full name, phone number, email and password).
### Clent (pet owner)- registered user, logged in
- Can login with email and password of the application or use Facebook login.
- Can add, edit details of his pets and delete them.
- Can make an appointment for a doctor and service of the department his pet needs.
- Can see all pets he has added, the past and upcomming appointments he has made, all doctors' prescriptions for his pets.
- Can cancel an upcomming appointment.
- Can download a prescription if he wants and needs it.
### Doctor
- Can see all pets added in the vet clinic with the history of their diseases, tratments and prescriptions.
- Can see information of pets owners and edit pet data if there are mistakes and inaccuracies.
- Can manage his work shedule :
     - add prescriptions for past appointments
     - cancel an upcomming appointment
     - send cancelation email to pet owner
 ### Admin
 - Only Admin can add, edit and delete information about departments, doctors and services the vet clinic offers.
 ## 📌 Accounts
 ### Client:
   - Email: client@client.com
   - Password: 123456
 ### Admin:
   - Email: admin@petcare.com
   - Password: admin12
 ### Doctor:
   - Email: {doctorFamily}@petcare.com
      - (For example: petkov@petcare.com)
   - Password: 123456
 ## 🔨 Built with:
 - ASP.NET Core 6.0
 - ASP.NET Core areas
 - Entity Framework Core 6.0
 - MSSQL Server
 - Bootstrap
 - Font Awesome
 - HTML&CSS
 - jQuery and JS
 - Cloudinary
 - SendGrid
 - nUnit
 - Moq
 - Fluent Assertions
 - Coverlet
