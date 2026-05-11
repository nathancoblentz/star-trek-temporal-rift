using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinalProject;

// Game logic is split into two processes
// - RoomEnterLogic describes what you see in the room or whether or not an enemy appears
// - RoomUILogic determines what UI elements appear based on what has already happened in the game.  
//      - for example, the 'Open Locker' only appears if the keycard is in your inventory.

namespace FinalProject.GameLogic
{
    public class Room
    {
        public int X { get; } // player coordinates
        public int Y { get; }
        public string Name { get; } // room attributes
        public string Description { get; }

        public Action<GameForm>? RoomEnterLogic { get; set; } // run the Room Enter logic
        public Action<GameForm>? RoomUILogic { get; set; } // run the Room UI logic

        public Room(int x, int y, string name, string desc)
        {
            X = x; Y = y; Name = name; Description = desc;
        }
    }
}

