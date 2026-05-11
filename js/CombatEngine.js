/**
 * CombatEngine.js — Turn-based combat (port of GameLogic/CombatEngine.cs)
 * 
 * Uses async/await with delay() to replicate C#'s Task.Delay() pacing.
 */
class CombatEngine {
    constructor(form, player, enemies) {
        this.form = form;
        this.player = player;
        this.enemies = enemies;
        this.playerTurn = true;
        this.combatAborted = false;
        this.pendingEnemies = [];
    }

    get activeEnemy() {
        return this.enemies.find(e => e.isAlive()) || null;
    }

    get currentEnemy() {
        return this.activeEnemy;
    }

    get aliveEnemyCount() {
        return this.enemies.filter(e => e.isAlive()).length;
    }

    // ============ Combat Flow ============

    startCombat() {
        this.form.appendLog(`⚔️ Combat started! ${this.player.name} vs ${this.enemies.length} enemies!`);
        this.form.showCombatUI();
        this.form.updatePlayerAbilitiesList();
        this.playerTurnStart();
    }

    playerTurnStart() {
        this.playerTurn = true;
        this.player.updateStatus();
        this.form.appendLog("➡️ Your turn! Press Attack or use an Ability.");
    }

    endCombat() {
        // Check what we defeated and add to actions
        if (this.enemies.some(e => e instanceof Tribble)) {
            if (!this.form.gameState.actionsTaken.includes('Defeated Tribbles'))
                this.form.gameState.actionsTaken.push('Defeated Tribbles');
        }
        if (this.enemies.some(e => e instanceof Bat)) {
            if (!this.form.gameState.actionsTaken.includes('Defeated Bat'))
                this.form.gameState.actionsTaken.push('Defeated Bat');
        }
        if (this.enemies.some(e => e instanceof Q)) {
            if (!this.form.gameState.actionsTaken.includes('Defeated Q'))
                this.form.gameState.actionsTaken.push('Defeated Q');
        }
        if (this.enemies.some(e => e instanceof BorgDrone)) {
            if (!this.form.gameState.actionsTaken.includes('Defeated Borg'))
                this.form.gameState.actionsTaken.push('Defeated Borg');
        }
        if (this.enemies.some(e => e instanceof RomulanScout)) {
            if (!this.form.gameState.actionsTaken.includes('Defeated Romulan'))
                this.form.gameState.actionsTaken.push('Defeated Romulan');
        }

        this.form.appendLog('⚔️ Combat ended.');
        this.form.returnToExplorationMode();
    }

    // ============ Player Actions ============

    async playerAttack() {
        const target = this.activeEnemy;
        if (!this.playerTurn || !target) return;

        this.playerTurn = false;

        const damage = this.player.dealDamage(this.form.hasWeaponInInventory());
        const finalDamage = target.takeDamage(damage);

        await delay(500);

        const hitMsg = target.getHitMessage(finalDamage);
        if (hitMsg) {
            this.form.appendLog(hitMsg);
        } else {
            this.form.appendLog(this.player.getAttackMessage(finalDamage, target.name));
        }

        this.updateUI();

        if (!target.isAlive()) {
            await delay(500);
            this.form.appendLog(target.getDefeatMessage());
        }

        if (await this.checkVictory()) return;

        await this.enemyTurn();
    }

    async playerUseAbility(ability) {
        if (!this.playerTurn || !this.activeEnemy) return;

        if (ability.cooldownRemaining > 0) {
            this.form.appendLog(`${ability.name} is on cooldown (${ability.cooldownRemaining} turns left).`);
            return;
        }

        this.playerTurn = false;

        const log = ability.activate(this.player, this.activeEnemy, this);

        if (log === null) {
            this.playerTurn = true;
            return;
        }

        if (log && log.trim().length > 0) {
            this.form.appendLog(log);
            await delay(2000);
        }

        ability.startCooldown();
        this.updateUI();

        if (await this.checkVictory()) return;

        await this.enemyTurn();
    }

    // ============ Enemy Logic ============

    spawnEnemy(enemy) {
        this.pendingEnemies.push(enemy);
    }

    async enemyTurn() {
        const aliveSnapshot = this.enemies.filter(e => e.isAlive());

        for (const enemy of aliveSnapshot) {
            enemy.onTurnStart(this);

            if (this.combatAborted) return;

            if (enemy.stunnedTurns > 0) {
                enemy.stunnedTurns--;
                this.form.appendLog(`${enemy.name} is stunned and cannot act! 💫`);
                continue;
            }

            await delay(500);
            if (this.combatAborted) return;

            this.form.appendLog(`➡️ ${enemy.name}'s turn!`);

            const dmg = enemy.dealDamage();
            const taken = this.player.takeDamage(dmg);

            await delay(500);
            if (this.combatAborted) return;

            this.form.appendLog(`${enemy.name} attacks ${this.player.name} for ${taken} damage!`);

            if (!this.player.isAlive()) {
                await delay(500);
                if (enemy instanceof Tribble)
                    this.form.appendLog(`${this.player.name} has suffocated from being overwhelmed by Tribbles. 💀`);
                else
                    this.form.appendLog(`${this.player.name} has fallen... 💀`);

                await delay(2000);
                this.form.gameOver();
                return;
            }
        }

        // Add spawned enemies
        if (this.pendingEnemies.length > 0) {
            this.enemies.push(...this.pendingEnemies);
            this.pendingEnemies = [];
        }

        // Reduce cooldowns
        this.player.abilities.forEach(a => a.reduceCooldown());

        this.updateUI();
        this.playerTurn = true;
        await delay(500);

        if (this.combatAborted) return;

        this.form.appendLog("➡️ Your turn! Press Attack or use an Ability.");
    }

    // ============ Victory ============

    async checkVictory() {
        if (!this.enemies.some(e => e.isAlive())) {
            await delay(500);

            if (this.player instanceof Picard) {
                this.form.appendLog("Picard gently explains that humans prefer not to be attacked. 🤝");
                this.form.appendLog("The enemy, persuaded by his logic and diplomacy, stands down.");
            } else if (this.player instanceof Sisko) {
                this.form.appendLog("Sisko dusts off his uniform. 'You bet against the Sisko, you lose.' ⚾");
                this.form.appendLog("The Dominion—err, the enemy—retreats.");
            } else if (this.player instanceof Spock) {
                this.form.appendLog("Spock raises an eyebrow. 'The statistical probability of your defeat was 99.7%.' 🖖");
                this.form.appendLog("Live long and prosper.");
            } else if (this.player instanceof Worf) {
                this.form.appendLog("Worf roars in triumph! 'Qapla'! A glorious victory!' ⚔️");
                this.form.appendLog("He glares at the defeated foe. 'You fought... adequately.'");
            } else {
                this.form.appendLog("Victory! All enemies defeated.");
            }

            this.endCombat();
            return true;
        }
        return false;
    }

    // ============ Helpers ============

    appendLog(message) {
        this.form.appendLog(message);
    }

    updateUI() {
        this.form.updatePlayerUI();
        const displayEnemy = this.activeEnemy || this.enemies[0];
        this.form.updateEnemyUI(displayEnemy);
    }

    // ============ Special Events ============

    async teleportPlayer() {
        this.combatAborted = true;

        // Save Q's health
        if (this.activeEnemy instanceof Q) {
            this.form.gameState.storedQHealth = this.activeEnemy.currentHP;
        }

        this.form.appendLog("Q snaps his fingers! The world dissolves... 🌀");
        await delay(1000);

        let targetX, targetY;

        if (!this.form.gameState.actionsTaken.includes('Defeated Tribbles')) {
            this.form.appendLog("Q laughs. 'I think you need some furry companions!' 🧶");
            targetX = -1;
            targetY = -2;
        } else {
            const rooms = [
                [0, 0], [-1, 0], [1, 0],
                [-1, -1], [0, -1],
                [-1, -2], [1, -2],
                [0, 1]
            ];
            const pick = rooms[Math.floor(Math.random() * rooms.length)];
            targetX = pick[0];
            targetY = pick[1];
        }

        this.form.returnToExplorationMode();
        this.form.teleportTo(targetX, targetY);

        this.form.appendLog("You materialize in a different part of the ship...");
        this.form.gameState.save();
    }
}

// Utility
function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
