# Space Invaders (Unity prototype)

A small Space Invaders-style prototype built in Unity in July 2023, as an early exercise in Unity's 2D physics and scripting in C#.

## What it does

- **Player movement**: the ship moves left and right with the arrow keys (or A/D). It stops at two boundary markers so it can't leave the play area.
- **Shooting**: pressing Space fires a bullet from the ship's gun at a set speed.
- **Hits**: a bullet that collides with an enemy ship destroys both. Bullets that miss expire after 3 seconds.
- **Scene**: a formation of enemy ships above the player.

This is a prototype, not a finished game. Enemies don't move or fire back, and there is no score, lives or win/lose state yet.

## Scripts

All gameplay code is in `Space Invaders Prototype 1/Assets/`:

| Script | Purpose |
| --- | --- |
| `Move2D.cs` | Reads player input, moves the ship and clamps it between the `BottomLeftLimit` and `TopRightLimit` objects |
| `ShootingBehaviour.cs` | Spawns a bullet prefab at `BulletSpawnPoint` on Space and sets its velocity |
| `Bullet.cs` | Destroys the bullet after its lifetime, or destroys the bullet and the object it hits on collision |

## Running it

1. Install **Unity 2021.3.14f1** (other 2021.3 LTS versions should also work) through Unity Hub.
2. Clone this repository.
3. In Unity Hub, choose **Add project from disk** and select the `Space Invaders Prototype 1` folder.
4. Open `Assets/Scenes/SampleScene.unity` and press **Play**.

## Controls

| Key | Action |
| --- | --- |
| Left / Right arrow (or A / D) | Move |
| Space | Shoot |
