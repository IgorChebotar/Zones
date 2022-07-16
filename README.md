# Zones
Trigger zones and collider visualization for Unity projects.

**Author:** Igor-Valerii Chebotar
**Email:**  igor.valerii.chebotar@gmail.com


## Requirements
* [Sirenix - Odin Inspector](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041#description "* Sirenix - Odin Inspector")
* [Simple Man - Utilities](https://github.com/IgorChebotar/Utilities)

## Quick start
1. Add the standard zone to scene by right click inside the 'Hierachy' window -> Zones -> Event zone.
2. This zone will react on any object got inside. Use events 'On object entered' and 'On object came out'. 
3. Done! 

## Custom zones
Here's how to create a new custom zone that will only respond to the "_CharacterLocomotion_" component and disable the ability to jump. Example and first person character asset you can found in 'Demo' package.
1. Create new C# class and inherit it from the 'Zone' class. Don't forget about 'using'. Add '_SimpleMan.Zones_>' and '_SimpleMan.FirstPersonCharacter_' namespaces.
2. Put your target component class in triangle brackets
```C#
using UnityEngine;
using SimpleMan.Zones;
using SimpleMan.FirstPersonCharacter;

public class NoJumpZone : Zone<CharacterLocomotion>
{
    
}
```

3. Override '_ValidObjectEnteredHandler_' and '_ValidObjectCameOutHandler_' methods.
4. Those methods will be called after object with component '_CharacterLocomotion_'  entered / came out from zone. *You can also override '_CanBeRegistered_' method for creating custom registration condition: for example if your zone should react only on character with '_Health_' parameter value less then 50. All registered objects you can get by '_RegisteredObjects_' property, if you need to make operations with multiple objects inside the zone.
5. Argument '_collider_' is collider that was detected by zone. Argument '_component_' is target component on the game object. In our case component is '_CharacterLocomotion_'. 
6. Make operations with '_component_' argument inside this methods.
7. Done!

```C#
using UnityEngine;
using SimpleMan.Zones;
using SimpleMan.FirstPersonCharacter;

public class NoJumpZone : Zone<CharacterLocomotion>
{
    //Object with 'CharacterLocomotion' component entered in zone
    protected override void ValidObjectEnteredHandler(Collider collider, CharacterLocomotion component)
    {
        //Forbid jump ability
        component.JumpForceMultiplier = 0;
    }

    //Object with 'CharacterLocomotion' came out from zone
    protected override void ValidObjectCameOutHandler(Collider collider, CharacterLocomotion component)
    {
        //Allow jump ability again
        component.JumpForceMultiplier = 1;
    }
}
```

## How to add custom zone to the hierarchy create menu? 
1. Save your custom zone as prefab on any folder.
2. Go to Edit -> Project settings -> Zones
3. Add your prefab to the list
4. **ALERT! Save all your unsaved scripts. Make sure, that you have't compilation errors before click on the 'Apply' button.** 
5. Click 'Apply' button.
6. Wait for end of compilation process.
7. Done! 


## Zone<T> component
Base component of all zones. Inherit your custom zone classes from it.

#### Properties
| Property name | Description                    |
| ------------- | ------------------------------ |
| UseTags |   Enable tag checking toggle     |
| AllowedTags |   Objects with this tags only will be registered|
| RegisteredObjects | Readonly list of objects inside this zone|

#### Events
| Event name | Description                    |
| ------------- | ------------------------------ |
| OnObjectEntered |Called on object was entered in zone|
| OnObjectCameOut |Called on object came out from zone|

#### Methods
| Function name | Description                    |
| ------------- | ------------------------------ |
| ValidObjectEnteredHandler |Called on object was entered in zone|
| ValidObjectCameOutHandler |Called on object came out from zone|
| CanBeRegistered |Override it if you need to make a custom registration condition|

## Visualizer
Draws collider borders and labels. Can be used without zone component.

#### Properties
| Property name | Description                    |
| ------------- | ------------------------------ |
| DrawShape |Visualize collider borders toggle|
| WireShape |Draw collider borders by lines|
| DrawLabel |Draw label toggle|
| LabelSize |Size of the label text|
| LabelOffset |Vertical label text position offset|
| LabelSizeDependsOnScale |Should label change size with game object scale toggle|
| ShapeColor |Color of the collider borders|
| LabelColor |Label text color|
| ColliderTarget |Cached target collider|

## Collider switcher
Gives ability to change collider on the game object by one click in the inspector.

#### Properties
| Property name | Description                    |
| ------------- | ------------------------------ |
| CurrentCollider |Collider type|

