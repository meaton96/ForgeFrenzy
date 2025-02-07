# Forge Frenzy

## Project Overview

Forge Frenzy consists of two games modes: Conveyor Calamity and Ingot Insanity. I imagined the latter as a minigame and developed it in addition to the Conveyor game in case Conveyor Calamity didn’t have quite enough collision in its current state.

## Conveyor Calamity

This game would be a 2D/top down/ or isometric game focused on crafting ingots into items to deliver to customers. It could be adapted to be a level based puzzle game or, my original vision, as a fast paced chaotic crafting game. The final design would force the player to balance quick decision making and reaction speed  with risk vs reward gameplay.

### Gameplay

- Spawn Ingots from the forges, adjust conveyor belts to deliver the ingots to the Smithies. Deliver duplicate type ingots to the same smithy to begin smithing items.
- Avoid different type collisions, these will destroy your ingots
- Avoid same type collisions while on the conveyors, these will create pile ups that need to be cleared
- Balance risk and reward of using higher Reactive ingots to forge faster but risk worse pileups.
- Spawning Ingots (those from the forge, and from collisions) would cost money so having pileups or deleting ingots from collisions would reduce your profit from selling items.

### Controls

- Basic mouse actions
    - Clicking a “Forge,” one of the objects on the left of the screen, will spawn an ingot on that conveyor.
        - Click/Tap up to 5 times in a short period to increase the Reactivity of an Ingot
        - Higher reactivity Ingots will create more items in a smithy, but also lead to larger pileups
    - Clicking on a merger will change its direction (center 2)
    - Clicking on a disabled belt will attempt to re-enable it
    - Right click and drag to use the Broom to sweep away piled up ingots
- Space bar will create a random order on the right side of the screen for demonstration

## Ingot Insanity

Tired of waiting for conveyor belts to deliver your items to your customers? Introducing the Ingot Cannon!

### Gameplay

- Left click to fire an ingot from the ingot cannon at the falling ingots. Collisions will spawn new ingots and the items will be automatically smithed into a finished item and delivered to the customer via gravity.
- Complete orders by creating cascading collisions and quickly crafting items

### Controls

- Left click to fire cannon
- Keyboard 1-4 to select which type of ingot to fire
- Space bar will create a random order for demonstration
