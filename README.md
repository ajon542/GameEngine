# GameEngine
The purpose of this project is to create a game engine in C# utilizing some of the technologies
found on the open source websites. Some of the technologies include WPF, OpenTK and AvalonDock.

## Current Goal
Integrate AvalonDock to the project and make dockable windows based on the Window menu item.

## TODO
(not in any particular order)
- Create dockable windows for SceneView, GameView, HierarchyView, ConsoleOutputView etc
- Move around the scene
- Place objects in the scene
- Scene hierarchy
- Add scripts to game objects
- Enter "play mode"
- Use the wiki to keep track of goals and acheivements

## Previous Goals

-------------------------------------------------------------------------------------------
One HierarchyView to be able to add a GameObject, update the HierarchyViewModel.
The HierarchyViewModel to communicate with the SceneViewModel to notify of an added GameObject.
The SceneViewModel to notify the SceneView that a GameObject has been added.
The SceneView to render the newly added object.

I have basically achieved this goal of being able to have the SceneViewModel notifying
of an added GameObject. The SceneView now has a notification of the scene list and can
render the scene list. Now I have to think about the design of the view model and view
interaction a little more. I'm thinking the view model needs to keep a description of
the scene and the view needs to render the scene based on this description. The next
task is to come up with a simple scene description so the view can render it.

-------------------------------------------------------------------------------------------