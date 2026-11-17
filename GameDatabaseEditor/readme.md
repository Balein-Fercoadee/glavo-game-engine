# Game Database Editor

Game Database Editor is a .NET MAUI application for creating and editing game
databases for the Glavo Game Engine's text-based games. It provides a graphical
alternative to editing `.gge` database files by hand.

## Current capabilities

- Start a new in-memory database with the engine's built-in words, or open an
  existing `.gge` file using the Windows file picker.
- Save the database and its changes to a selected existing `.gge` file using the
  Windows file picker.
- View and edit the database name, description, starting room, and treasure room.
- View rooms, words, items, messages, and actions.
- Add, edit, and remove rooms, items, and messages. New objects receive the
  lowest unused ID of their type. To edit an existing object, double-click it
  or select it and choose **Edit**. **Save** applies the changes to the
  in-memory database; **Cancel** discards them. Use **Save Database** to write
  changes to a file.
- Edit a room's name, description, exits, and contained items. The item picker
  excludes items already in the room. Removing an item from a room does not
  remove it from the database.
- Remove a selected room from the in-memory database unless an action refers
  to it. When removal is allowed, exits and the starting and treasure room
  settings that refer to it are cleared.
- Edit an item's name, short name, description, takeable status, and treasure
  status. Treasure items are highlighted in gold in the main and room item
  lists.
- Edit a message's text using a multiline field.
- Prevent removal of items referenced by rooms or actions, and rooms and
  messages referenced by action commands. The editor identifies the references
  that must be removed first.

## Current limitations

Adding, editing, and removing words and actions is not yet implemented. The
file picker is configured for Windows.

See the [repository README](../readme.md) for requirements and Windows
publishing instructions.
