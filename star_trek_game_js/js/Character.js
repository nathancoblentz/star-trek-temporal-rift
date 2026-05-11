/**
 * Character.js — Base character class (port of Classes/Character.cs)
 * 
 * Abstract base for Player and Enemy. Stores stats, abilities,
 * handles damage calculation, temporary buffs/debuffs, and status effects.
 */
class Character {
    constructor(name = '', hp = 10, str = 1, dex = 1, def = 1, wis = 1, abilities = []) {
        this.name = name;
        this.maxHP = hp;
        this.currentHP = hp;
        this.strength = str;
        this.dexterity = dex;
        this.defense = def;
        this.wisdom = wis;
        this.abilities = abilities;

        this.scottyCallCount = 0;
        this.stunnedTurns = 0;

        // Temporary stat mods
        this.tempStrengthMod = 0;
        this.tempDefenseMod = 0;
        this.statusDuration = 0;
    }

    updateStatus() {
        if (this.statusDuration > 0) {
            this.statusDuration--;
            if (this.statusDuration === 0) {
                this.tempStrengthMod = 0;
                this.tempDefenseMod = 0;
            }
        }
    }

    dealDamage(hasWeapon = false) {
        let baseDamage = this.strength + this.tempStrengthMod;
        if (hasWeapon) baseDamage += 45;
        return Math.max(0, baseDamage);
    }

    takeDamage(amount) {
        const effectiveDefense = this.defense + this.tempDefenseMod;
        const damageTaken = Math.max(amount - effectiveDefense, 0);
        this.currentHP -= damageTaken;
        if (this.currentHP < 0) this.currentHP = 0;
        return damageTaken;
    }

    isAlive() {
        return this.currentHP > 0;
    }

    heal(amount) {
        this.currentHP += amount;
        if (this.currentHP > this.maxHP) this.currentHP = this.maxHP;
        return this.currentHP;
    }

    getAttackMessage(damage, targetName) {
        return `${this.name} attacks ${targetName} for ${damage} damage!`;
    }

    getDefeatMessage() {
        return `${this.name} defeated!`;
    }

    getHitMessage(damage) {
        return null; // Default: no special message
    }

    /**
     * Serialize this character to a plain object for JSON save.
     */
    toSaveData() {
        return {
            type: this.constructor.name,
            name: this.name,
            maxHP: this.maxHP,
            currentHP: this.currentHP,
            strength: this.strength,
            dexterity: this.dexterity,
            defense: this.defense,
            wisdom: this.wisdom,
            scottyCallCount: this.scottyCallCount,
            stunnedTurns: this.stunnedTurns,
            abilityNames: this.abilities.map(a => a.constructor.name),
            abilityCooldowns: this.abilities.map(a => a.cooldownRemaining || 0)
        };
    }
}
