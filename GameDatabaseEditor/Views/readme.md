# Generic Editor Popup Overview

THE GGE Database Editor uses a modular system for the display of editors for the various game objects.

One popup is defined with a Title Bar, an area for custom views by game object, and a Save and Cancel section. Views are created for a game object with the various fields that it requires. At runtime, when the user selects ADD or EDIT, the appropriate view is feed into the popup, the popup's properties are set of the object type, and BAM!: You have an customized game object editor.