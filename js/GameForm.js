/**
 * GameForm.js — Main game controller (port of Forms/GameForm.cs)
 * 
 * Replaces WinForms UI manipulation with DOM operations.
 * This is the central orchestrator: movement, actions, combat delegation, UI updates.
 */
class GameForm {
    constructor(player, gameState = null) {
        this.currentPlayer = player;
        this.gameState = gameState || new GameState();
        this.maze = new Maze();
        this.playerX = this.gameState.playerX;
        this.playerY = this.gameState.playerY;
        this.currentCombat = null;
        this.isGameOver = false;
        this.selectedAbilityIndex = -1;

        // Cache DOM references
        this.els = {
            gameLog:        document.getElementById('txtGameLog'),
            playerName:     document.getElementById('lblPlayerName'),
            playerHP:       document.getElementById('pbPlayerHealth'),
            playerMaxHP:    document.getElementById('lblPlayerMaxHP'),
            playerStr:      document.getElementById('lblPlayerStrength'),
            playerDex:      document.getElementById('lblPlayerDexterity'),
            playerDef:      document.getElementById('lblPlayerDefense'),
            playerWis:      document.getElementById('lblPlayerWisdom'),
            playerAbilities:document.getElementById('lbPlayerAbilities'),
            inventory:      document.getElementById('lbInventory'),
            actionsTaken:   document.getElementById('lbActionsTaken'),
            enemyPanel:     document.getElementById('pnlEnemyStats'),
            enemyName:      document.getElementById('lblEnemyName'),
            enemyHP:        document.getElementById('pbEnemyHealth'),
            enemyMaxHP:     document.getElementById('lblEnemyMaxHP'),
            enemyStr:       document.getElementById('lblEnemyStrength'),
            enemyDex:       document.getElementById('lblEnemyDexterity'),
            enemyDef:       document.getElementById('lblEnemyDefense'),
            enemyWis:       document.getElementById('lblEnemyWisdom'),
            enemyAbilities: document.getElementById('lbEnemyAbilities'),
            pnlMovement:    document.getElementById('pnlMovement'),
            pnlCombat:      document.getElementById('pnlCombat'),
            btnUp:          document.getElementById('btnUp'),
            btnDown:        document.getElementById('btnDown'),
            btnLeft:        document.getElementById('btnLeft'),
            btnRight:       document.getElementById('btnRight'),
            btnAction:      document.getElementById('btnAction'),
            btnFight:       document.getElementById('btnFight'),
            btnUseAbility:  document.getElementById('btnUseAbility'),
            txtX:           document.getElementById('txtX'),
            txtY:           document.getElementById('txtY')
        };

        this.bindEvents();
        this.showMovementUI();
        this.updatePlayerUI();
        this.refreshInventoryList();
        this.refreshActionsTakenList();
        this.enterRoom();
    }

    // ============ Event Binding ============

    bindEvents() {
        this.els.btnUp.addEventListener('click', () => this.movePlayer(0, 1));
        this.els.btnDown.addEventListener('click', () => this.movePlayer(0, -1));
        this.els.btnLeft.addEventListener('click', () => this.movePlayer(-1, 0));
        this.els.btnRight.addEventListener('click', () => this.movePlayer(1, 0));

        this.els.btnFight.addEventListener('click', () => {
            if (this.currentCombat) this.currentCombat.playerAttack();
        });

        this.els.btnUseAbility.addEventListener('click', () => {
            if (this.currentCombat && this.selectedAbilityIndex >= 0) {
                const ability = this.currentPlayer.abilities[this.selectedAbilityIndex];
                if (ability) this.currentCombat.playerUseAbility(ability);
            } else {
                this.appendLog('Select an ability first.');
            }
        });

        this.els.btnAction.addEventListener('click', () => this.handleAction());

        document.getElementById('btnSaveQuit').addEventListener('click', () => {
            this.saveGame();
            this.appendLog('Game saved.');
            // Switch to start screen
            switchScreen('screen-start');
            checkResumeButton();
        });

        document.getElementById('btnStartOver').addEventListener('click', () => {
            if (confirm('Are you sure you want to start over? This will erase all game progress.')) {
                this.resetGame();
            }
        });

        // Modal play-again button
        document.getElementById('modalBtnYes').addEventListener('click', () => {
            bootstrap.Modal.getInstance(document.getElementById('gameModal')).hide();
            this.resetGame();
        });

        document.getElementById('modalBtnNo').addEventListener('click', () => {
            switchScreen('screen-start');
            checkResumeButton();
        });
    }

    // ============ Core Logic ============

    startCombat(enemies) {
        this.currentCombat = new CombatEngine(this, this.currentPlayer, enemies);
        this.currentCombat.startCombat();
        this.showCombatUI();
    }

    enterRoom() {
        this.updateMovementButtons();
        this.els.txtX.textContent = this.playerX;
        this.els.txtY.textContent = this.playerY;

        const room = this.maze.getRoom(this.playerX, this.playerY);
        if (room) {
            this.appendLog(`\n=== ${room.name} ===`, 'heading');
            if (room.description) this.appendLog(room.description);
            if (room.roomEnterLogic) room.roomEnterLogic(this);
            if (room.roomUILogic) room.roomUILogic(this);
        }
    }

    loadRoom() {
        this.updateMovementButtons();
        this.els.txtX.textContent = this.playerX;
        this.els.txtY.textContent = this.playerY;

        const room = this.maze.getRoom(this.playerX, this.playerY);
        if (room && room.roomUILogic) room.roomUILogic(this);
    }

    teleportTo(x, y) {
        this.playerX = x;
        this.playerY = y;
        this.gameState.playerX = x;
        this.gameState.playerY = y;
        this.enterRoom();
    }

    // ============ Movement ============

    movePlayer(dx, dy) {
        const newX = this.playerX + dx;
        const newY = this.playerY + dy;

        const room = this.maze.getRoom(newX, newY);
        if (!room) {
            this.appendLog("You can't go that way.");
            return;
        }

        this.playerX = newX;
        this.playerY = newY;

        this.gameState.playerX = this.playerX;
        this.gameState.playerY = this.playerY;
        this.saveGame();

        this.enterRoom();
    }

    setMovementEnabled(enabled) {
        if (enabled) {
            this.updateMovementButtons();
        } else {
            this.els.btnUp.disabled = true;
            this.els.btnDown.disabled = true;
            this.els.btnLeft.disabled = true;
            this.els.btnRight.disabled = true;
        }
    }

    updateMovementButtons() {
        this.els.btnUp.disabled    = !this.maze.getRoom(this.playerX, this.playerY + 1);
        this.els.btnDown.disabled  = !this.maze.getRoom(this.playerX, this.playerY - 1);
        this.els.btnRight.disabled = !this.maze.getRoom(this.playerX + 1, this.playerY);
        this.els.btnLeft.disabled  = !this.maze.getRoom(this.playerX - 1, this.playerY);
    }

    lockDirection(dir) {
        switch (dir) {
            case 'Left':  this.els.btnLeft.disabled = true; break;
            case 'Right': this.els.btnRight.disabled = true; break;
            case 'Up':    this.els.btnUp.disabled = true; break;
            case 'Down':  this.els.btnDown.disabled = true; break;
        }
    }

    unlockDirection(dir) {
        switch (dir) {
            case 'Left':  this.els.btnLeft.disabled = false; break;
            case 'Right': this.els.btnRight.disabled = false; break;
            case 'Up':    this.els.btnUp.disabled = false; break;
            case 'Down':  this.els.btnDown.disabled = false; break;
        }
    }

    // ============ Actions ============

    handleAction() {
        const actionText = this.els.btnAction.textContent;

        switch (actionText) {
            case 'Route Power to Sensors':
                if (!this.gameState.actionsTaken.includes('RoutedPower'))
                    this.gameState.actionsTaken.push('RoutedPower');
                this.appendLog('You reroute auxiliary power to the sensor array. The console lights up.');
                break;

            case 'Take Phaser':
                if (!this.gameState.inventory.includes('Phaser'))
                    this.gameState.inventory.push('Phaser');
                this.appendLog('You take the Type-2 Phaser.');
                break;

            case 'Cut Blast Door':
                if (!this.gameState.actionsTaken.includes('CutDoor'))
                    this.gameState.actionsTaken.push('CutDoor');
                this.appendLog('You set the Phaser to maximum and slice through the blast door locks. 💥');
                this.appendLog('The door slides open with a groan.');
                break;

            case 'Engage Override':
                if (!this.gameState.actionsTaken.includes('Engaged override')) {
                    this.gameState.actionsTaken.push('Engaged override');
                    this.appendLog('You engage the manual override. The forcefields deactivate.');
                } else {
                    this.appendLog('Override already engaged.');
                }
                break;

            case 'Take Keycard':
                if (!this.gameState.inventory.includes('Keycard'))
                    this.gameState.inventory.push('Keycard');
                this.appendLog('You pick up the Security Keycard.');
                break;

            case 'Take Tricorder':
                if (!this.gameState.inventory.includes('Tricorder'))
                    this.gameState.inventory.push('Tricorder');
                this.appendLog('You pick up the standard issue Tricorder.');
                break;

            case 'Access Locker':
                this.gameWin();
                return; // Don't continue to save/refresh

            case 'Scan Entity':
                this.appendLog('You scan the entity. It appears to be a silicon-based lifeform.');
                break;

            case 'Take Combadge':
                if (!this.gameState.inventory.includes('Combadge')) {
                    this.gameState.inventory.push('Combadge');
                    this.appendLog('You take the Combadge from the dead Redshirt.');
                    this.appendLog("You feel a strange connection to the ship's engineer...");
                    this.currentPlayer.abilities.push(new CallScottyAbility());
                    this.appendLog('Ability Acquired: Call Scotty! 🖖');
                    this.hideActionButton();
                    this.updatePlayerUI();
                }
                break;

            default:
                this.appendLog(`Action '${actionText}' not handled.`);
                break;
        }

        this.saveGame();
        this.loadRoom();
        this.refreshInventoryList();
        this.refreshActionsTakenList();
    }

    // ============ UI Management ============

    updatePlayerUI() {
        if (!this.currentPlayer) return;
        const p = this.currentPlayer;

        const pct = Math.max(0, Math.min(100, (p.currentHP / Math.max(1, p.maxHP)) * 100));
        this.els.playerHP.style.width = pct + '%';
        this.els.playerName.textContent = p.name;
        this.els.playerMaxHP.textContent = `${p.currentHP}/${p.maxHP}`;
        this.els.playerStr.textContent = `STR: ${p.strength}`;
        this.els.playerDex.textContent = `DEX: ${p.dexterity}`;
        this.els.playerDef.textContent = `DEF: ${p.defense}`;
        this.els.playerWis.textContent = `WIS: ${p.wisdom}`;

        this.updatePlayerAbilitiesList();
    }

    updateEnemyUI(enemy) {
        if (!enemy) {
            this.showMovementUI();
            return;
        }

        this.showCombatUI();
        const pct = Math.max(0, Math.min(100, (enemy.currentHP / Math.max(1, enemy.maxHP)) * 100));
        this.els.enemyHP.style.width = pct + '%';
        this.els.enemyName.textContent = enemy.name;
        this.els.enemyMaxHP.textContent = `${enemy.currentHP}/${enemy.maxHP}`;
        this.els.enemyStr.textContent = `STR: ${enemy.strength}`;
        this.els.enemyDex.textContent = `DEX: ${enemy.dexterity}`;
        this.els.enemyDef.textContent = `DEF: ${enemy.defense}`;
        this.els.enemyWis.textContent = `WIS: ${enemy.wisdom}`;

        // Enemy abilities
        this.els.enemyAbilities.innerHTML = '';
        if (enemy.abilities) {
            enemy.abilities.forEach(a => {
                const li = document.createElement('li');
                li.textContent = a.name;
                this.els.enemyAbilities.appendChild(li);
            });
        }
    }

    updatePlayerAbilitiesList() {
        this.els.playerAbilities.innerHTML = '';
        if (!this.currentPlayer || !this.currentPlayer.abilities) return;

        this.currentPlayer.abilities.forEach((ability, i) => {
            const li = document.createElement('li');
            const onCooldown = ability.cooldownRemaining > 0;
            li.textContent = onCooldown
                ? `${ability.name} (CD: ${ability.cooldownRemaining})`
                : ability.name;
            if (onCooldown) li.classList.add('on-cooldown');
            if (i === this.selectedAbilityIndex) li.classList.add('selected');

            li.addEventListener('click', () => {
                this.selectedAbilityIndex = i;
                this.updatePlayerAbilitiesList(); // re-render to show selection
            });

            this.els.playerAbilities.appendChild(li);
        });
    }

    showCombatUI() {
        this.els.pnlMovement.classList.add('d-none');
        this.els.pnlCombat.classList.remove('d-none');
        this.els.enemyPanel.classList.remove('d-none');
    }

    showMovementUI() {
        this.els.pnlMovement.classList.remove('d-none');
        this.els.pnlCombat.classList.add('d-none');
        this.els.enemyPanel.classList.add('d-none');

        // Clear enemy display
        this.els.enemyName.textContent = '—';
        this.els.enemyMaxHP.textContent = '—';
        this.els.enemyStr.textContent = 'STR: —';
        this.els.enemyDex.textContent = 'DEX: —';
        this.els.enemyDef.textContent = 'DEF: —';
        this.els.enemyWis.textContent = 'WIS: —';
        this.els.enemyAbilities.innerHTML = '';
    }

    returnToExplorationMode() {
        this.hideActionButton();
        this.showMovementUI();
        this.setMovementEnabled(true);
        this.refreshActionsTakenList();
        this.loadRoom();
    }

    showActionButton(text) {
        this.els.btnAction.textContent = text;
        this.els.btnAction.classList.remove('d-none');
    }

    hideActionButton() {
        this.els.btnAction.classList.add('d-none');
    }

    hasWeaponInInventory() {
        return this.gameState.inventory.includes('Phaser');
    }

    refreshInventoryList() {
        this.els.inventory.innerHTML = '';
        this.gameState.inventory.forEach(item => {
            const li = document.createElement('li');
            li.textContent = item;
            this.els.inventory.appendChild(li);
        });
    }

    refreshActionsTakenList() {
        this.els.actionsTaken.innerHTML = '';
        this.gameState.actionsTaken.forEach(action => {
            const li = document.createElement('li');
            li.textContent = action;
            this.els.actionsTaken.appendChild(li);
        });
    }

    appendLog(message, type = '') {
        const div = document.createElement('div');
        div.classList.add('log-entry');
        if (type === 'heading') div.classList.add('log-heading');
        div.textContent = message;
        this.els.gameLog.appendChild(div);
        this.els.gameLog.scrollTop = this.els.gameLog.scrollHeight;
    }

    // ============ Game State Management ============

    saveGame() {
        this.gameState.playerX = this.playerX;
        this.gameState.playerY = this.playerY;
        this.gameState.playerData = this.currentPlayer.toSaveData();
        this.gameState.save();
    }

    gameOver() {
        if (this.isGameOver) return;
        this.isGameOver = true;

        document.getElementById('modalTitle').textContent = 'Defeat';
        document.getElementById('modalBody').textContent = 'Your mission has failed. Try again?';
        const modal = new bootstrap.Modal(document.getElementById('gameModal'));
        modal.show();
    }

    gameWin() {
        if (this.isGameOver) return;
        this.isGameOver = true;

        this.appendLog('You open the locker and find the shimmering Time Crystal! 💎');
        this.appendLog('With this artifact, you can restore the timeline and save the ship!');
        this.appendLog('MISSION ACCOMPLISHED!');

        document.getElementById('modalTitle').textContent = '🎉 Victory!';
        document.getElementById('modalBody').textContent =
            'You found the Time Crystal and saved the timeline! Play again?';
        const modal = new bootstrap.Modal(document.getElementById('gameModal'));
        modal.show();
    }

    resetGame() {
        GameState.clear();
        switchScreen('screen-select');
    }
}
