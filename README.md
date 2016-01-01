# GameEngine
The purpose of this project is to create a game engine in C# utilizing some of the technologies
found on the open source websites. Some of the technologies include WPF, OpenTK and AvalonDock.

The game engine should provide a clear user interface to manipulate game objects in a scene
hierarchy. It should be possible to save and load the scene. However, employing the latest and
greatest rendering techniques may not be one of the highest priorities of this project.

## Current Goal
Make adding game objects to the scene more intuitive. This will most likely involve improving
the batching of objects to be rendered and the use of the scene camera.

In order to achieve this goal, I have been experimenting with how a game developer may use the
game engine. I have been creating a number of different scenes to do things such as texturing,
mouse input etc to try and find some of the repetitive actions. There are many so far, and by
creating more scenes, I will gain a better understanding of what needs to be pulled out and made
simpler.

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
Goal 2
Integrate AvalonDock to the project and make dockable windows based on the Window menu item.
This is still a slow work in progress as I have become side-tracked in creating a better scene
manager and overall game engine architecture.

Goal 1
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