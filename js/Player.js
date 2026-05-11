/**
 * Player.js — Player subclasses (port of Players/*.cs)
 * 
 * Each player has unique stats and two starting abilities.
 * Player is a thin wrapper; the subclasses define identity.
 */
class Player extends Character {
    constructor(name, hp, str, dex, def, wis, abilities) {
        super(name, hp, str, dex, def, wis, abilities);
    }
}

class Picard extends Player {
    constructor() {
        super('Jean-Luc Picard', 120, 10, 12, 14, 18, [
            new InspiringSpeech(),
            new PrecisionStrike()
        ]);
    }
}

class Worf extends Player {
    constructor() {
        super('Worf', 150, 18, 12, 16, 10, [
            new BatlethSlash(),
            new WarriorsRoar()
        ]);
    }
}

class Spock extends Player {
    constructor() {
        super('Spock', 130, 14, 14, 12, 20, [
            new VulcanNervePinch(),
            new LogicalCounterattack()
        ]);
    }
}

class Sisko extends Player {
    constructor() {
        super('Benjamin Sisko', 140, 16, 12, 15, 14, [
            new SuckerPunch(),
            new CommandersGambit()
        ]);
    }
}

/**
 * Factory: create a Player from a key string (used for save/load and selection).
 */
function createPlayer(key) {
    switch (key) {
        case 'Picard': return new Picard();
        case 'Worf':   return new Worf();
        case 'Spock':  return new Spock();
        case 'Sisko':  return new Sisko();
        default:       return new Picard();
    }
}

/**
 * Restore a Player from saved data, including HP and ability cooldowns.
 */
function restorePlayer(data) {
    const player = createPlayer(data.type);
    player.currentHP = data.currentHP;
    player.maxHP = data.maxHP;
    player.strength = data.strength;
    player.dexterity = data.dexterity;
    player.defense = data.defense;
    player.wisdom = data.wisdom;
    player.scottyCallCount = data.scottyCallCount || 0;

    // Restore additional abilities (e.g., CallScotty from Combadge)
    if (data.abilityNames) {
        const existing = player.abilities.map(a => a.constructor.name);
        data.abilityNames.forEach((name, i) => {
            if (!existing.includes(name)) {
                const ability = createAbilityByName(name);
                if (ability) player.abilities.push(ability);
            }
        });
        // Restore cooldowns
        if (data.abilityCooldowns) {
            player.abilities.forEach((a, i) => {
                if (data.abilityCooldowns[i] !== undefined) {
                    a.cooldownRemaining = data.abilityCooldowns[i];
                }
            });
        }
    }

    return player;
}
