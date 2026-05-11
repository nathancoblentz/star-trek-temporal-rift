/**
 * Room.js — Room class (port of GameLogic/Room.cs)
 */
class Room {
    constructor(x, y, name, description) {
        this.x = x;
        this.y = y;
        this.name = name;
        this.description = description;
        this.roomEnterLogic = null;  // function(form)
        this.roomUILogic = null;     // function(form)
    }
}
