/**
 * Abilities.js — All player abilities (port of Players/*.cs abilities + CallScottyAbility.cs)
 * 
 * Each ability implements: name, description, cooldown, cooldownRemaining, activate(), startCooldown(), reduceCooldown()
 * activate() returns a log string or null (null = ability failed, don't consume turn).
 */

// ============ Base helpers ============
class AbilityBase {
    constructor(name, description, cooldown) {
        this.name = name;
        this.description = description;
        this.cooldown = cooldown;
        this.cooldownRemaining = 0;
    }

    startCooldown() {
        this.cooldownRemaining = this.cooldown;
    }

    reduceCooldown() {
        if (this.cooldownRemaining > 0) this.cooldownRemaining--;
    }

    activate(user, target, context) {
        return null; // Override in subclass
    }
}

// ============ Picard Abilities ============

class InspiringSpeech extends AbilityBase {
    constructor() {
        super('Inspiring Speech', 'Bores the enemy into submission with a long speech.', 3);
    }

    activate(user, target, context) {
        if (target instanceof Q) {
            target.takeDamage(9999);
            return `${user.name} begins a long speech about humanity's potential...\n` +
                `Q rolls his eyes. 'Oh, spare me the lecture, Jean-Luc! I'm leaving!' 🙄\n` +
                `He snaps his fingers. 'You are far too boring today.'\n` +
                `Q vanishes! (Victory by Boredom)`;
        }

        target.tempStrengthMod = -2;
        target.tempDefenseMod = -2;
        target.statusDuration = 2;

        return `${user.name} begins a long, impassioned speech about the Federation's values...\n` +
            `${target.name} looks visibly bored and distracted! 😴 (-2 STR, -2 DEF for 1 round)`;
    }
}

class PrecisionStrike extends AbilityBase {
    constructor() {
        super('Precision Strike', 'Deals extra damage ignoring some defense.', 2);
    }

    activate(user, target, context) {
        const dmg = user.strength + 5;
        const taken = target.takeDamage(dmg);
        return `${user.name} uses ${this.name}, dealing ${taken} damage to ${target.name}!`;
    }
}

// ============ Worf Abilities ============

class BatlethSlash extends AbilityBase {
    constructor() {
        super("Bat'leth Slash", "Powerful melee attack with Worf's Bat'leth.", 3);
    }

    activate(user, target, context) {
        const dmg = user.strength + 10;
        const taken = target.takeDamage(dmg);
        return `${user.name} roars, 'Today is a good day to die!' and swings his Bat'leth! ⚔️\n` +
            `Deals ${taken} damage to ${target.name}!`;
    }
}

class WarriorsRoar extends AbilityBase {
    constructor() {
        super("Warrior's Roar", 'Intimidates the enemy, reducing their defense.', 3);
    }

    activate(user, target, context) {
        if (target instanceof Q) {
            target.takeDamage(9999);
            return `${user.name} lets out a primal scream that shakes the cosmos! 🦁\n` +
                `Q covers his ears. 'How barbaric! I shan't stay for this noise.'\n` +
                `Q vanishes! (Victory by Intimidation)`;
        }

        target.tempDefenseMod = -5;
        target.statusDuration = 2;

        return `${user.name} lets out a primal scream! The enemy trembles! 🦁\n` +
            `${target.name}'s defense reduced by 5 for 1 round!`;
    }
}

// ============ Spock Abilities ============

class VulcanNervePinch extends AbilityBase {
    constructor() {
        super('Vulcan Nerve Pinch', 'Instantly incapacitates enemy for one turn.', 3);
    }

    activate(user, target, context) {
        const dmg = user.strength + 5;
        const taken = target.takeDamage(dmg);
        target.stunnedTurns = 1;

        return `${user.name} calmly reaches out to the enemy's shoulder... 'Sleep.' 🖖\n` +
            `${target.name} collapses momentarily! (Dealt ${taken} damage, Stunned)`;
    }
}

class LogicalCounterattack extends AbilityBase {
    constructor() {
        super('Logical Counterattack', 'Reflects part of incoming damage.', 2);
    }

    activate(user, target, context) {
        if (target instanceof Q) {
            target.takeDamage(9999);
            return `${user.name} raises an eyebrow. 'Your existence is illogical.'\n` +
                `Q frowns. 'You Vulcans are no fun at all. I'm leaving.'\n` +
                `Q vanishes in a puff of logic! (Victory)`;
        }

        const dmg = user.strength + Math.floor(target.strength / 2);
        const taken = target.takeDamage(dmg);
        return `${user.name} calculates the optimal strike vector, using the enemy's momentum.\n` +
            `Logic dictates this result. (Dealt ${taken} damage)`;
    }
}

// ============ Sisko Abilities ============

class SuckerPunch extends AbilityBase {
    constructor() {
        super('Sucker Punch', 'Deals high-damage, risky attack.', 3);
    }

    activate(user, target, context) {
        if (target instanceof Q) {
            target.takeDamage(9999);
            return `${user.name} punches Q right in the face! 👊\n` +
                `Q stumbles back, holding his nose. 'You hit me! Picard never hit me!'\n` +
                `He sneers. 'I'm not staying where I'm not wanted. Goodbye!'\n` +
                `Q vanishes in a flash of light! (Insta-Defeat)`;
        }

        const dmg = user.strength + 10;
        const taken = target.takeDamage(dmg);
        return `${user.name} winds up and delivers a massive hook! 👊\n` +
            `'You're not Q, but that felt good!' (Dealt ${taken} damage)`;
    }
}

class CommandersGambit extends AbilityBase {
    constructor() {
        super("Commander's Gambit", 'High-risk high-reward strike.', 4);
    }

    activate(user, target, context) {
        const dmg = user.strength * 2;
        const taken = target.takeDamage(dmg);

        // Self-damage (ignoring defense)
        user.currentHP -= 5;
        if (user.currentHP < 0) user.currentHP = 0;

        return `${user.name} executes a risky maneuver! The Defiant would be proud! 🚀\n` +
            `Deals ${taken} damage, but takes 5 recoil damage!`;
    }
}

// ============ Acquired Ability: Call Scotty ============

class CallScottyAbility extends AbilityBase {
    constructor() {
        super('Call Scotty', 'Beam all Tribbles into deep space! 🌌', 2);
    }

    activate(user, target, context) {
        // Verify we are fighting Tribbles
        const fightingTribbles = context.enemies.some(e => e instanceof Tribble);
        if (!fightingTribbles) {
            context.appendLog("Scotty: 'I can't get a lock on those signals, Captain! It only works on Tribbles!' 🚫");
            return null;
        }

        user.scottyCallCount++;
        context.appendLog("You tap your Combadge... 'Scotty, beam them up!'");

        if (user.scottyCallCount === 1) {
            context.appendLog("Scotty: 'I canna do it Captain! The interference is too thick!' ⚠️");
            return ' ';
        } else if (user.scottyCallCount === 2) {
            context.appendLog("Scotty: 'I'm givin' her all she's got, but the transporters are overheating!' 🔥");
            return ' ';
        }

        // Third time's the charm
        context.appendLog("Scotty: 'Aye Captain! Locking on now!'");
        context.appendLog('⚡ A shimmering transporter beam engulfs the room! ⚡');

        for (const enemy of context.enemies) {
            if (enemy.isAlive()) {
                enemy.currentHP = 0;
                context.appendLog(`${enemy.name} dematerializes into deep space! 🌌`);
            }
        }

        context.appendLog('Combat ended. Thanks Scotty!');
        context.endCombat();
        return ' ';
    }
}

// ============ Ability Factory (for save/load) ============

function createAbilityByName(name) {
    switch (name) {
        case 'InspiringSpeech':       return new InspiringSpeech();
        case 'PrecisionStrike':      return new PrecisionStrike();
        case 'BatlethSlash':         return new BatlethSlash();
        case 'WarriorsRoar':         return new WarriorsRoar();
        case 'VulcanNervePinch':     return new VulcanNervePinch();
        case 'LogicalCounterattack': return new LogicalCounterattack();
        case 'SuckerPunch':          return new SuckerPunch();
        case 'CommandersGambit':     return new CommandersGambit();
        case 'CallScottyAbility':    return new CallScottyAbility();
        default: return null;
    }
}
