# osu! Database Manager

A simple command-line tool for managing your osu! database (osu!.db) file. This tool allows you to perform various operations on your osu! database safely, with automatic backup creation.

## Features

- Delete user information (username, permissions, unlock status)
- Delete all beatmaps from the database

## Requirements

- Windows operating system
- .NET runtime
- osu! game installed (to access the database file)

## Usage

1. Run the application
2. Select your osu!.db file when prompted (usually located in `%localappdata%/osu!`)
3. Choose from the available options:
   - Delete user information
   - Delete all beatmaps
   - Save changes
   - Exit

## Dependencies

- OsuParsers - for reading and writing osu! database files

## Note

This tool modifies your osu! database file. While it creates backups automatically, it's recommended to manually backup your osu!.db file before using this tool.

## License

This project is open source and available under the MIT License. 