# Ecologisch Verantwoord Vervoer

Onze Opdrachtgever is CGI, opgericht in 1976, is een van de grootste dienstverleners ter wereld op het gebied van informatietechnologie en bedrijfsprocessen.

De opdracht is dat wij een app moeten ontwikkelen voor CGI.\
Deze app moet ervoor zorgen dat de werknemers van het bedrijf ecologisch verantwoord vervoer gebruiken om naar hun werk te gaan. \
De werknemers kunnen invoeren de afstand die wordt afgelegd en wel vervoerstype ze gebruiken, waarbij de persoon die het meest ecologisch verantwoord naar het werk komt een prijs krijgt.

## Requirements

To work on this project, you'll need to have the following tools installed:

- [SQL Server Management Studio](https://aka.ms/ssmsfullsetup) (to run the Microsoft SQL database)
- Visual Studio (to run the project)
- Visual Studio Code (optional - you can also code in Visual Studio)

## Installation

1. Clone the repository.
2. Open the project in Visual Studio.
3. Build the project.
4. Open SQL Server Management Studio.
5. Connect to the local instance of the MsSqlLocalDB server.
6. Run the scripts in the `Database` folder to create the necessary tables and data.

## Usage

1. Run the project in Visual Studio.
2. Navigate to `http://localhost:3000` in your web browser.

## Features

List the features of your project here.

## Running the Database Script in SQL Server Management Studio

To create the necessary tables and data for your project's Microsoft SQL database, follow these steps:

1. Open SQL Server Management Studio.
2. Connect to the local instance of the MsSqlLocalDB server by clicking on the "Connect" button and selecting "Database Engine".
3. In the "Server name" field, type "(LocalDB)\MSSQLLocalDB".
4. In the "Authentication" field, select "Windows Authentication".
5. Click the "Connect" button to connect to the server.
6. In the "Object Explorer" pane on the left-hand side, expand the "Databases" folder.
7. Right-click on the "Databases" folder and select "New Database".
8. In the "New Database" dialog box, enter a name for your database and click the "OK" button.
9. In the "Object Explorer" pane, expand your new database and right-click on the "Tables" folder.
10. Select "New Query" to open a new query window.
11. In the query window, copy and paste the SQL script from the `Database` folder in your project into the query editor.
12. Click the "Execute" button (or press F5) to run the script.
13. Verify that the tables were created successfully by expanding the "Tables" folder in the "Object Explorer" pane.

## Credits

- Joost Raemakers
- Billy Hofland
- Kubilay Karabulut
- Rowan van der Weel
- Stijn van den Hurk
- Jonathan Kat