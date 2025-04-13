# IMDB Explorer - WPF Application

![IMDB Explorer](https://img.shields.io/badge/PROG%202500-Windows%20Programming-blue)
![Development Status](https://img.shields.io/badge/Status-Completed-success)

## Project Overview

IMDB Explorer is a Windows Presentation Foundation (WPF) application developed as the final project for PROG 2500 - Windows Programming. The application allows users to browse and explore movie data from the IMDB database using a modern, intuitive interface.

## Features

- **Multiple Data Views**: Navigate through at least three different views of IMDB data
- **Advanced Filtering**: Utilize LINQ queries to filter movie data by various criteria
- **Related Data Display**: View relationships between movies, actors, directors, and other entities
- **Responsive Design**: Clean and modern UI with proper XAML implementation
- **Data Persistence**: Entity Framework integration with SQL Server database

## Technical Implementation

- Built using C# and WPF (.NET Framework)
- Implements the MVVM (Model-View-ViewModel) architectural pattern
- Entity Framework for database access and management
- Unit tests using MSTest framework to ensure code quality and reliability
- Installer package for easy deployment

## Screenshots

*[Screenshots will be added here upon completion of the UI design]*

## Getting Started

### Prerequisites

- Windows OS (Windows 10 or higher recommended)
- .NET Framework 4.8 or higher
- SQL Server (2019 or higher)
- Visual Studio 2022 (recommended for development)

### Installation

1. Clone this repository
   ```
   git clone https://github.com/Iasminv/IMDB_app.git
   ```

2. Restore the IMDB database from the backup file (located in the `Database` folder)

3. Update the connection string in `App.config` to point to your SQL Server instance

4. Build and run the application using Visual Studio, or install using the provided MSI installer

## Database Schema

The application uses the IMDB database with the following main entities:
- Movies
- Actors
- Directors
- Genres
- Ratings
- User Reviews

## Project Structure

```
Solution 'IMDB'/
├── IMDB/                        # Main application project
│   ├── Commands/                # Command implementations
│   │   └── RelayCommands.cs     # MVVM command implementation
│   ├── Data/                    # Data access
│   │   └── ImdbContext.cs       # Entity Framework context
│   ├── Models/                  # Data models
│   │   ├── Generated/           # Auto-generated model classes
│   │   ├── Episode.cs
│   │   ├── Genre.cs
│   │   ├── Name.cs
│   │   ├── Principal.cs
│   │   ├── Rating.cs
│   │   ├── Title.cs
│   │   └── TitleAlias.cs
│   ├── ViewModels/              # MVVM ViewModels
│   │   └── MainViewModel.cs
│   ├── Views/                   # XAML Views
│   │   ├── App.xaml
│   │   ├── AssemblyInfo.cs
│   │   ├── MainWindow.xaml
│   │   └── MainWindow.xaml.cs
│   └── Dependencies/            # External dependencies
└── IMDB_Tests/                  # Unit test project
    ├── Dependencies/            # Test dependencies
    │   ├── Analyzers/
    │   ├── Frameworks/          # Using MSTest
    │   ├── Packages/
    │   └── Projects/
    ├── MSTestSettings.cs
    └── Tests.cs
```

## Development Team

- Iasmin Veronez
- Mason Liao
- Hope Kasheke
- Sloan Corey

## Acknowledgments

- Geoff Gillespie | PROG2500 Teacher

## License

This project is developed for educational purposes as part of PROG 2500 - Windows Programming course.
