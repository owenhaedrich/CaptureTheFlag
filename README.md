# W26 Design Week Template

This project serves as a template for this design week challenge.

## 1. Multi-Display Rendering

Each of the Viswall's monitors are identical. The Viswall is a 5x2 matrix of monitors, though each 2x2 set at either end is used as a single monitor. Below is information about the displays.

![viswall](https://github.com/MohawkRaphaelT/w26-design-week-template/blob/main/docs/img/viswall.png)

- **Aspect Ratio**:
  - 16 by 9
- **Resolution**:
  - 4k @ 3840x by 2160y
  - 2K @ 1920x by 1080y
- **Physical Dimensions**: (roughly)
  - Each individual panel: 4x2.5 feet
  - Each logical panel (2x2): 8x5 feet.

### 1.1 Display Setup

Unity can output up to 8 monitors simultaneously. One `Camera` component can project to one display.

https://docs.unity3d.com/6000.0/Documentation/Manual/MultiDisplay.html

In Unity, you must create multiple Game windows and select which display output to view.

![editor-displays-info](https://github.com/MohawkRaphaelT/w26-design-week-template/blob/main/docs/img/editor-displays-info.png)

You must run a bit of code to activate the monitor manually when playing from a build. The project provides the [ConfigureDisplays.cs](https://github.com/MohawkRaphaelT/w26-design-week-template/blob/main/DW%20W26%20Unity/Assets/Scripts/ConfigureDisplays.cs) script and game object to do this on scene `Start`. 

If the rendered windows don't start where you want, you should set things up in you display settings so that Monitor 1 is the left window, and Monitor 2 is the right window. You could potentially hold down `CTRL+Shift+Windows` and then press arrow keys to move the game windows around, too.

## 2. Controllers

The Viswall will be set up with a tower PC connected to 6 game controllers simultaneously (XBOX and/or PS5, though any traditional controller will do). You do not need to use all controllers for your game. However, the game must remain a multiplayer game.

We encourage you to use as few buttons, triggers, and/or control stick as possible to keep button the game accessible and remove issues related to button mapping across different controllers.

### 2.1 Input System

This project template uses Unity's newer Input System rather than the legacy Input Manager. To boil it down, an action map is defined for the project. Each action map contains schemes (profiles) for sets of actions, eg. Player, UI. Rather than directly poll an input device for its physical buttons, the action is abstract (eg. Move, Jump) and is tied to one or more of input types (controller stick, face button). Then, in code, you can check for the action being triggered. Unity then abstracts a large number of inputs that can be tied to the action. So even though you may, for instance, use different brands of controllers, they can all "Jump" using the west face button across all traditional controllers.

Input System manual: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.18/manual/index.html

### 2.2 Automatic Player Drop-In via Player Input Manager

The project is set up so that when a new input device is registered (when a button is pressed) it spawns in a Player prefab into the scene. This is managed via Unity's `PlayerInputManager` currently on the "Player Input Manager" object in the sample scene. The script [PlayerSpawn](https://github.com/MohawkRaphaelT/w26-design-week-template/blob/main/DW%20W26%20Unity/Assets/Scripts/PlayerSpawn.cs) attached to this object is called when the [PlayerInputManager](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.18/api/UnityEngine.InputSystem.PlayerInputManager.html) registers a new input device. `PlayerSpawn`'s function `OnPlayerJoined` is "magically" called by virtue of being attached to the same game object and handles assigning the information such as the `PlayerInput` (action map / input device) to the player object. You can change the messaging method by changing the top drop-down on `PlayerInputManager`.

![player input manager](https://github.com/MohawkRaphaelT/w26-design-week-template/blob/main/docs/img/player%20input%20manager.png)

`PlayerInputManager` can only clone one prefab set in the Inspector. If you want different roles for players, consider spawning in an object that then spawns in the correct prefab and uses the correct action map. 

![player prefab](https://github.com/MohawkRaphaelT/w26-design-week-template/blob/main/docs/img/player%20prefab.png)

The [PlayerController](https://github.com/MohawkRaphaelT/w26-design-week-template/blob/main/DW%20W26%20Unity/Assets/Scripts/PlayerController.cs) script manages the player prefabs. It has a few functions called by `PlayerSpawn` as the player is being spawned, such as assigning the input device, player number, and player color.

### 2.3 Getting Input

The newer Input System has a lot of ways to get input. I tried to provide the solution that seems most straightforward and similar enough to the legacy input manager. 

First, you need to define the possible actions and tie them to types of input. You can find the asset in your Assets database or via `Edit > Player Settings > Input System Package`.

![action map](https://github.com/MohawkRaphaelT/w26-design-week-template/blob/main/docs/img/action%20map.png)

Actions maps are the collections of actions for a logical unit (eg. player, UI), and each action is driven by a type of input, such as gamepad stick or keyboard button.

In your scripts, you need to get the instance of `PlayerInput` from `PlayerInputManager`. You could check input indirectly without the `PlayerInput` instance but it seems to poll all controllers. How to do this is best explain by looking at the above section 2.2 and related scripts.

Once you have the `PlayerInput`, you can find actions within the scheme.

```C#
// Find the references to the "Move" and "Jump" actions inside the
// player input's action map.
InputAction InputActionMove = playerInput.actions.FindAction("Player/Move");
InputAction InputActionJump = playerInput.actions.FindAction("Player/Jump");
```

The type `InputAction` is an abstraction on the opposite end of the action map. The directive `/` is how you can navigate the action map in absolute terms.

Once you have the `InputAction`, you can interpret it according to the configuration. For instance, you can interpret the `InputAction` as a binary input (button) or as a 2D input (stick axes).

```C#
// MOVE: Read the "Move" action value, which is a 2D vector
Vector2 moveValue = InputActionMove.ReadValue<Vector2>();

// JUMP: Read the "Jump" action state, which is a boolean value
if (InputActionJump.WasPressedThisFrame()) { /* ... */ }
```

