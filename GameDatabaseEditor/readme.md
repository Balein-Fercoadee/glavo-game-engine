# Game Database Editor

Game Database Editor is a .NET MAUI application for creating and editing game
databases for the Glavo Game Engine's text-based games. It provides a graphical
alternative to editing `.gge` database files by hand.

## Current capabilities

- Start a new in-memory database with the engine's built-in words, or open an
  existing `.gge` file using the Windows file picker.
- View and edit the database name, description, starting room, and treasure room.
- View rooms, words, items, messages, and actions.
- Edit an existing room by double-clicking it or selecting it and choosing
  **Edit Room**. Change its name, description, and exits; **Save** applies the
  changes to the in-memory room, while **Cancel** discards them.
- Remove a selected room from the in-memory database.

## Current limitations

The **Save Database** button does not yet write changes to a file. Room editor
**Save** only updates the in-memory database, so changes are lost when the
database is replaced or the application closes. Adding rooms and managing words,
items, messages, and actions are not yet implemented.

See the [repository README](../README.md) for requirements and Windows
publishing instructions.
