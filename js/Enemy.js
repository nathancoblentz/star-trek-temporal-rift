/**
 * Enemy.js — Enemy base class (port of Classes/Enemy.cs)
 * 
 * Adds onTurnStart hook for AI behavior. Subclasses override this.
 */
class Enemy extends Character {
    constructor(name, hp, str, dex, def, wis, abilities = []) {
        super(name, hp, str, dex, def, wis, abilities);
    }

    getDefense() {
        return this.defense;
    }

    onTurnStart(engine) {
        this.updateStatus();
    }
}
