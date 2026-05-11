/*
 * Module: Maze.cs
 * Purpose: Defines the game map and room logic.
 * Functionality:
 *  - Creates the grid of Rooms (Start, Engineering, Bridge, etc.).
 *  - Defines room-specific logic (descriptions, items, events).
 *  - Handles room entry events (e.g., spawning enemies, unlocking doors).
 *  - Provides methods to retrieve room data based on coordinates.
 */
using System;
using System.Collections.Generic;
using FinalProject.Classes;
using FinalProject;


namespace FinalProject.GameLogic
{
    public class Maze
    {
        private Dictionary<(int, int), Room> rooms;

        public Maze()
        {
            rooms = new Dictionary<(int, int), Room>();
            BuildMap();            
        }

        private void BuildMap()
        {
            // Start Room [0,0] -> Transporter Room
            rooms[(0, 0)] = new Room(0, 0, "Transporter Room", "")
            {
                RoomEnterLogic = form =>
                {
                    // Only show the start message once per game
                    if (!form.gameState.HasSeenStartMessage)
                    {
                        form.AppendLog("You materialize in the Transporter Room of a derelict starship.\n");
                        form.AppendLog("The air is stale, and emergency lighting flickers.");
                        form.AppendLog("MISSION: Retrieve the Time Crystal from the Captain's Locker to save the timeline! ⏳");
                        form.gameState.HasSeenStartMessage = true;
                        form.gameState.Save("gamestate.json");
                    } else
                    {
                        form.AppendLog("You are back in the Transporter Room.");
                    }

                    if (form.gameState.ActionsTaken.Contains("Defeated Tribbles"))
                    {
                        form.AppendLog("The bio-hazard seal to the Engineering Core is lifted.");
                    }
                    else if (form.gameState.ActionsTaken.Contains("Engaged override"))
                    {
                        form.AppendLog("The forcefield to the Engineering Core is deactivated by manual override.");
                    }
                    else
                    {
                        form.AppendLog("The door to the Engineering Core (Left) is sealed by a bio-hazard containment protocol.");
                        form.AppendLog("Sensors detect a Tribble infestation in the Cargo Bay.");
                    }
                },

                RoomUILogic = form =>
                {
                    form.HideActionButton();
                    
                    // Unlock if Tribbles defeated OR Override engaged
                    if (form.gameState.ActionsTaken.Contains("Defeated Tribbles") || form.gameState.ActionsTaken.Contains("Engaged override"))
                    {
                        form.UnlockDirection("Left");
                    }
                    else
                    {
                        form.LockDirection("Left");
                    }
                }
            };

            // Well Room [-1,0] -> Engineering Core
            rooms[(-1, 0)] = new Room(-1, 0, "Engineering Core", "")
            {
                RoomEnterLogic = form =>
                {
                    if (form.gameState.ActionsTaken.Contains("RoutedPower"))
                        form.AppendLog("The Warp Core hums softly. Sensor arrays are operating at maximum efficiency.");
                },
                RoomUILogic = form =>
                {
                    if (!form.gameState.ActionsTaken.Contains("RoutedPower"))
                    {
                        form.AppendLog("You stand on the catwalk above the Warp Core. It's dark and silent.");
                        form.AppendLog("The main sensor array power coupling is disconnected.");
                        form.ShowActionButton("Route Power to Sensors");
                    }
                    else
                    {
                        form.HideActionButton();
                        if (!form.gameState.ActionsTaken.Contains("ProbeBuffApplied"))
                        {
                            form.CurrentPlayer.Defense += 2;
                            form.gameState.ActionsTaken.Add("ProbeBuffApplied");
                            form.AppendLog("Power restored! Sensors come online, predicting enemy movements. (+2 Defense)");
                        }
                    }
                }
            };

            // Treasure Room [1,0] -> Captain's Ready Room
            rooms[(1, 0)] = new Room(1, 0, "Captain's Ready Room", "You see the Captain's personal locker.")
            {
                RoomEnterLogic = form => 
                {
                    if (!form.gameState.ActionsTaken.Contains("Defeated Bat"))
                    {
                        form.AppendLog("A screeching Space Bat drops from the ceiling!");
                        form.SetMovementEnabled(false);
                        var enemies = new List<Enemy> { new Bat() };
                        form.StartCombat(enemies);
                    } 
                    
                },
                RoomUILogic = form =>
                {
                    if (form.gameState.Inventory.Contains("Keycard"))
                        form.ShowActionButton("Access Locker");
                    else
                    {
                        form.HideActionButton();
                        form.AppendLog("The locker is sealed with a Level 10 Command Encryption. The Time Crystal must be inside!");
                    }
                }
            };

            // Old Woman Room [-1,-1] -> Sickbay
            rooms[(-1, -1)] = new Room(-1, -1, "Sickbay", "You enter the medical bay.")
            {
                RoomEnterLogic = form =>
                {
                    if (!form.gameState.ActionsTaken.Contains("Defeated Romulan"))
                    {
                        form.AppendLog("The air shimmers... a Romulan Scout decloaks!");
                        form.SetMovementEnabled(false);
                        var enemies = new List<Enemy> { new RomulanScout() };
                        form.StartCombat(enemies);
                    }
                    else
                    {
                        form.AppendLog("The Sickbay is quiet.");
                        if (!form.gameState.Inventory.Contains("Phaser"))
                        {
                            form.AppendLog("The defeated Romulan dropped a Phaser!");
                        }
                    }
                },
                RoomUILogic = form =>
                {
                    if (form.gameState.ActionsTaken.Contains("Defeated Romulan") && !form.gameState.Inventory.Contains("Phaser"))
                    {
                        form.ShowActionButton("Take Phaser");
                    }
                    else
                    {
                        form.HideActionButton();
                    }
                }
            };

            // Com Badge Room [0,-1] -> Communications Array
            rooms[(0, -1)] = new Room(0, -1, "Communications Array", "Subspace relays line the walls.")
            {
                RoomEnterLogic = form =>
                {
                    if (!form.gameState.Inventory.Contains("Combadge"))
                    {
                        form.AppendLog("You enter the Communications Array. It's silent.");
                        form.AppendLog("In the corner, you find a dead Redshirt. 💀");
                        form.AppendLog("He is clutching his Combadge.");
                        form.ShowActionButton("Take Combadge");
                    }
                    else
                    {
                        form.AppendLog("You are in the Communications Array. The Redshirt is still dead.");
                        form.HideActionButton();
                    }
                },
                RoomUILogic = form =>
                {
                    if (!form.gameState.Inventory.Contains("Combadge"))
                        form.ShowActionButton("Take Combadge");
                    else if (form.gameState.Inventory.Contains("Phaser") && !form.gameState.ActionsTaken.Contains("CutDoor"))
                        form.ShowActionButton("Cut Blast Door");
                    else
                        form.HideActionButton();
                    
                    if (!form.gameState.ActionsTaken.Contains("CutDoor"))
                    {
                        form.LockDirection("Down");
                        form.AppendLog("The blast door to the Bridge (Down) is sealed. You'll have to find another way around.");
                    }
                    else
                    {
                        form.UnlockDirection("Down");
                        form.AppendLog("The blast door has been cut open with a Phaser.");
                    }
                }
            };

            // Bad Feeling Room [-1,-2] -> Infested Cargo Bay
            rooms[(-1, -2)] = new Room(-1, -2, "Infested Cargo Bay", "")
            {
                RoomEnterLogic = form =>
                {
                    if (form.gameState.ActionsTaken.Contains("Defeated Tribbles"))
                    {
                        form.AppendLog("The cargo bay is quiet. The Tribbles are gone.");
                    }
                    else
                    {
                        form.AppendLog("Sensors indicate lifeforms ahead.");
                        form.AppendLog("You hear a soft chirping sound... You see something adorable.");
                        if (form.CurrentPlayer != null)
                        {
                            form.SetMovementEnabled(false);

                            // Spawn tribbles
                            var enemies = EnemyFactory.CreateTribbleEncounter();
                            if (enemies.Count > 0)
                            {
                                form.StartCombat(enemies);
                            }
                        }
                    }
                },
                RoomUILogic = form => { form.HideActionButton(); }
            };

            // Q Room [0,-2] -> Bridge
            rooms[(0, -2)] = new Room(0, -2, "Bridge", "The main bridge of the ship.")
            {
                RoomEnterLogic = form =>
                {
                    if (!form.gameState.ActionsTaken.Contains("Defeated Q"))
                    {
                        form.AppendLog("Q is sitting in the Captain's chair! 'Mon capitaine!'");
                        form.SetMovementEnabled(false);
                        
                        var q = new Q();
                        if (form.gameState.StoredQHealth > 0)
                            q.CurrentHP = form.gameState.StoredQHealth;

                        var enemies = new List<Enemy> { q };
                        form.StartCombat(enemies);
                    }
                    else
                    {
                         form.AppendLog("The Bridge is empty. Q has left... for now.");
                         form.AppendLog("The door to the Security Office (Right) is unlocked.");
                    }
                },
                RoomUILogic = form => 
                { 
                    form.HideActionButton();
                    if (!form.gameState.ActionsTaken.Contains("Defeated Q"))
                    {
                        form.LockDirection("Right");
                    }
                    else
                    {
                        form.UnlockDirection("Right");
                        form.AppendLog("The door to the Security Office is unlocked.");
                    }
                }
            };

            // Key Room [1,-2] -> Security Office
            rooms[(1, -2)] = new Room(1, -2, "Security Office", "You see a security desk.")
            {
                RoomEnterLogic = form => 
                {
                },
                RoomUILogic = form =>
                {
                    if (!form.gameState.Inventory.Contains("Keycard"))
                        form.ShowActionButton("Take Keycard");
                    else
                    {
                        form.HideActionButton();
                        form.AppendLog("The security desk is empty.");
                    }
                }
            };

            // Lever Room [0,1] -> Auxiliary Control
            rooms[(0, 1)] = new Room(0, 1, "Auxiliary Control", "You see a manual override console.")
            {
                RoomEnterLogic = form => 
                {
                    form.AppendLog("The Emergency Medical Hologram is here. 'I am monitoring the override systems.'");
                },
                RoomUILogic = form =>
                {
                    form.ShowActionButton("Engage Override");
                }
            };
        }

        public Room? GetRoom(int x, int y)
        {
            rooms.TryGetValue((x, y), out var room);
            return room;
        }
    
        
    }
}
