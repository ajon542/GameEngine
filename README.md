# GameEngine
The purpose of this project is to create a game engine in C# as well as exploring some of the
rendering techniques available through OpenTK / OpenGL.

## Current Status
The game engine is under heavy development and is currently in a prototyping or experimental stage.
It essentially contains a bunch of example scenes demonstrating different aspects of the game engine.

## Current Goal
Fix some of the bugs and TODO's

## TODO
(not in any particular order)
- Selectable and movable 3D objects
- Vertex attributes
- Batching
- Make the scene selectable through the UI
- Move the game specific stuff out of the core
- Complete some of the examples I started
- Create dockable windows for SceneView, GameView, HierarchyView, ConsoleOutputView etc
- Scene hierarchy
- Add scripts to game objects
- Enter "play mode"
- Use the wiki to keep track of goals and acheivements

## Previous Goals

-------------------------------------------------------------------------------------------
Goal 4
Now that I have implemented basic .obj file loading. We need a way to switch the shader based
on the type of data that was loaded. For example, a .obj file may contain only vertices and faces,
while another may contain vertices, normals, uvs and faces.
This may include switching the type of shader used based on the parameters available or
generating things like normals.

Material files need to be loaded along with the .obj file, so I need to figure out what will be
needed here and pass these settings along to the shader.

Goal 3
Make adding game objects to the scene more intuitive. This will most likely involve improving
the batching of objects to be rendered and the use of the scene camera.

In order to achieve this goal, I have been experimenting with how a game developer may use the
game engine. I have been creating a number of different scenes to do things such as texturing,
mouse input etc to try and find some of the repetitive actions. There are many so far, and by
creating more scenes, I will gain a better understanding of what needs to be pulled out and made
simpler.

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