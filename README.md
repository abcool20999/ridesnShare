# RideSharing App README

Welcome to the RideSharing App! This .NET application is designed to facilitate cheaper rides for users by allowing them to share rides with other passengers. It includes several controllers and features to manage passengers, drivers, trips, and bookings.

## Controllers

### PassengerController

- Manages passengers and their details.
- Allows for CRUD operations on passengers.
- Powers the ability to create, read, update, and delete passengers.

### DriverController

- Manages drivers and their details.
- Allows for CRUD operations on drivers.
- Provides validation for driver usernames and passwords.

### TripsController

- Handles trips and trip-related operations.
- Allows for creating, reading, and deleting trips.
- Includes a search feature for the dashboard to enable passengers to search for trips.

### BookingsController

- Manages bookings between passengers and drivers.
- Enables passengers to create, read, and update bookings.
- Shows bookings for drivers and passengers.

## Additional Features

- **Personalized Views**: Implement personalized views for ListofBookingsDrivers to show bookings for drivers and ListBookingsForTrip to display trips that passengers have booked.
- **ListOfBookings Table**: Used to show bookings for passengers.
- **ListOfTrips Table**: Displays the list of trips for drivers.

## Usage

To run the RideSharing App locally:

1. Clone this repository.
2. Open the solution file in Visual Studio.
3. Configure the database connection in the appropriate configuration file.
4. Build the solution.
5. Start the application.
6. Access the app through the specified URL in your browser.

