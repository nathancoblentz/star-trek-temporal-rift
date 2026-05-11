/**
 * Enemies.js — All enemy subclasses + EnemyFactory (port of Enemies/*.cs)
 */

// ============ Bat ============
class Bat extends Enemy {
    constructor() {
        super('Space Bat', 30, 8, 15, 2, 5);
    }

    onTurnStart(engine) {
        if (Math.random() < 0.3) {
            engine.appendLog(`${this.name} screeches loudly! 🦇`);
        }
    }
}

// ============ Romulan Scout ============
class RomulanScout extends Enemy {
    constructor() {
        super('Romulan Scout', 78, 20, 12, 7, 10);
    }

    onTurnStart(engine) {
        if (Math.random() < 0.2) {
            engine.appendLog('The Romulan Scout flickers in and out of cloak! (+3 Dexterity)');
            this.dexterity += 3;
        }
    }
}

// ============ Borg Drone ============
class BorgDrone extends Enemy {
    constructor() {
        super('Borg Drone', 40, 8, 5, 10, 10);
    }

    onTurnStart(engine) {
        if (Math.random() < 0.2) {
            engine.appendLog('The Borg Drone adapts to your attacks! (+2 Defense)');
            this.defense += 2;
        }
    }
}

// ============ Q ============
class Q extends Enemy {
    constructor() {
        super('Q', 500, 15, 20, 20, 99);
    }

    onTurnStart(engine) {
        const roll = Math.floor(Math.random() * 5) + 1;
        switch (roll) {
            case 1:
                engine.appendLog("Q snaps his fingers and a mariachi band appears! 🎺");
                engine.appendLog("The trumpet blast hurts your ears! (10 Sonic Damage)");
                engine.form.currentPlayer.takeDamage(10);
                break;
            case 2:
                engine.appendLog("Q changes your uniform into Robin Hood cosplay. 'Fetching!'");
                break;
            case 3:
                engine.appendLog("Q yawns. 'Is this the best humanity has to offer?'");
                break;
            case 4:
                engine.appendLog("Q smiles mischievously. 'Let's see how you fare elsewhere.'");
                engine.teleportPlayer();
                break;
            case 5:
                engine.appendLog("Q offers you a cigar. It explodes in your face! 💥");
                engine.appendLog("That really hurt! (40 Damage)");
                engine.form.currentPlayer.takeDamage(40);
                break;
        }
    }

    getDefeatMessage() {
        return "Q laughs. 'A temporary setback, mon capitaine!' He vanishes in a flash of light. ✨";
    }
}

// ============ Tribble ============
class Tribble extends Enemy {
    constructor(generation = 1) {
        super(`Tribble (Gen ${generation})`,
            10 + (generation * 5),   // HP
            2 + (generation * 3),    // STR
            2, 6, 1
        );
        this.roundsAlive = 0;
        this.threatened = false;
        this.generation = generation;
    }

    get cuteDefenseBonus() {
        return this.roundsAlive < 4 ? 100 : 0;
    }

    onTurnStart(engine) {
        this.roundsAlive++;

        // Swarm Logic
        const swarmCount = engine.aliveEnemyCount;
        if (swarmCount > 1) {
            this.strength += swarmCount;
            engine.appendLog(`${this.name} gains +${swarmCount} Strength from the swarm! 🐀`);
        }

        // 90% chance to breed if threatened
        if (this.threatened && Math.random() < 0.9) {
            const newTribble = new Tribble(this.generation + 1);
            engine.appendLog(`A Tribble multiplies! (Gen ${newTribble.generation}) 🐾`);
            engine.spawnEnemy(newTribble);
        }
    }

    takeDamage(dmg) {
        this.threatened = true;
        const effectiveDmg = Math.max(dmg - this.cuteDefenseBonus, 0);
        if (effectiveDmg <= 0) return 0;
        return super.takeDamage(effectiveDmg);
    }

    getDefeatMessage() {
        return `${this.name} squeaks and pops! 💥`;
    }

    getHitMessage(damage) {
        if (damage === 0) {
            return "You hesitate... you can't attack something so cute! 💖 (0 damage)";
        }
        return null;
    }
}

// ============ Enemy Factory ============
const EnemyFactory = {
    createTribbleEncounter() {
        const count = Math.floor(Math.random() * 3) + 1; // 1-3
        const list = [];
        for (let i = 0; i < count; i++) {
            list.push(new Tribble());
        }
        return list;
    }
};
