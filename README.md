# Raw Hierarchy Icons

Quick icon picker for GameObjects, right in the Hierarchy context menu.

Requires **Unity 6000.6** or newer: the package is built on `Unity.Hierarchy`, an API that does not exist in earlier versions.

## What it does

- Adds a **Select Icon** submenu as the first entry of the right-click menu in the Hierarchy.
- Hides the auto icons Unity derives from components, so only the icons you set yourself remain.
- Swaps a folder icon for an opened one while the node is expanded.

The icon itself is stored the standard way, in the object's `m_Icon`. Opening the folder and hiding auto icons happen at draw time only and write nothing to the scene.

## Installation

Package Manager → **Install package from git URL**:

```
https://github.com/i3sn/raw-hierarchy-icons.git
```

Pin a tag unless you want to follow the branch:

```
https://github.com/i3sn/raw-hierarchy-icons.git#v0.1.0
```

## Your own icon set

The list is a constant named `Builtin` in `Editor/HierarchyIconContextMenu.cs`. One line is one menu entry:

```csharp
("Component", "GridLayoutGroup Icon"),
```

The second value is the name of a built-in editor icon. Use the **base name, without the `d_` prefix**: `EditorGUIUtility.IconContent` picks the dark or light variant on its own.

A folder is recognised by the built-in `Folder Icon`, matched by texture name, so the behaviour survives an editor theme change.
