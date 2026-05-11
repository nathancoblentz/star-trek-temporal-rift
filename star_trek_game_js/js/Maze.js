/**
 * Maze.js — Map builder (port of GameLogic/Maze.cs)
 * 
 * Builds the same 8-room grid with identical room logic.
 * Room coordinates match the C# version exactly.
 */
class Maze {
    constructor() {
        this.rooms = {};
        this.buildMap();
    }

    key(x, y) {
        return `${x},${y}`;
    }

    getRoom(x, y) {
        return this.rooms[this.key(x, y)] || null;
    }

    buildMap() {
        // ===== [0,0] Transporter Room (Start) =====
        const r00 = new Room(0, 0, 'Transporter Room', '');
        r00.roomEnterLogic = (form) => {
            if (!form.gameState.hasSeenStartMessage) {
                form.appendLog('You materialize in the Transporter Room of a derelict starship.');
                form.appendLog('The air is stale, and emergency lighting flickers.');
                form.appendLog('MISSION: Retrieve the Time Crystal from the Captain\'s Locker to save the timeline! ⏳');
                form.gameState.hasSeenStartMessage = true;
                form.gameState.save();
            } else {
                form.appendLog('You are back in the Transporter Room.');
            }

            if (form.gameState.actionsTaken.includes('Defeated Tribbles')) {
                form.appendLog('The bio-hazard seal to the Engineering Core is lifted.');
            } else if (form.gameState.actionsTaken.includes('Engaged override')) {
                form.appendLog('The forcefield to the Engineering Core is deactivated by manual override.');
            } else {
                form.appendLog('The door to the Engineering Core (Left) is sealed by a bio-hazard containment protocol.');
                form.appendLog('Sensors detect a Tribble infestation in the Cargo Bay.');
            }
        };
        r00.roomUILogic = (form) => {
            form.hideActionButton();
            if (form.gameState.actionsTaken.includes('Defeated Tribbles') ||
                form.gameState.actionsTaken.includes('Engaged override')) {
                form.unlockDirection('Left');
            } else {
                form.lockDirection('Left');
            }
        };
        this.rooms[this.key(0, 0)] = r00;

        // ===== [-1,0] Engineering Core =====
        const rn10 = new Room(-1, 0, 'Engineering Core', '');
        rn10.roomEnterLogic = (form) => {
            if (form.gameState.actionsTaken.includes('RoutedPower'))
                form.appendLog('The Warp Core hums softly. Sensor arrays are operating at maximum efficiency.');
        };
        rn10.roomUILogic = (form) => {
            if (!form.gameState.actionsTaken.includes('RoutedPower')) {
                form.appendLog('You stand on the catwalk above the Warp Core. It\'s dark and silent.');
                form.appendLog('The main sensor array power coupling is disconnected.');
                form.showActionButton('Route Power to Sensors');
            } else {
                form.hideActionButton();
                if (!form.gameState.actionsTaken.includes('ProbeBuffApplied')) {
                    form.currentPlayer.defense += 2;
                    form.gameState.actionsTaken.push('ProbeBuffApplied');
                    form.appendLog('Power restored! Sensors come online, predicting enemy movements. (+2 Defense)');
                }
            }
        };
        this.rooms[this.key(-1, 0)] = rn10;

        // ===== [1,0] Captain's Ready Room =====
        const r10 = new Room(1, 0, "Captain's Ready Room", 'You see the Captain\'s personal locker.');
        r10.roomEnterLogic = (form) => {
            if (!form.gameState.actionsTaken.includes('Defeated Bat')) {
                form.appendLog('A screeching Space Bat drops from the ceiling!');
                form.setMovementEnabled(false);
                form.startCombat([new Bat()]);
            }
        };
        r10.roomUILogic = (form) => {
            if (form.gameState.inventory.includes('Keycard'))
                form.showActionButton('Access Locker');
            else {
                form.hideActionButton();
                form.appendLog('The locker is sealed with a Level 10 Command Encryption. The Time Crystal must be inside!');
            }
        };
        this.rooms[this.key(1, 0)] = r10;

        // ===== [-1,-1] Sickbay =====
        const rn1n1 = new Room(-1, -1, 'Sickbay', 'You enter the medical bay.');
        rn1n1.roomEnterLogic = (form) => {
            if (!form.gameState.actionsTaken.includes('Defeated Romulan')) {
                form.appendLog('The air shimmers... a Romulan Scout decloaks!');
                form.setMovementEnabled(false);
                form.startCombat([new RomulanScout()]);
            } else {
                form.appendLog('The Sickbay is quiet.');
                if (!form.gameState.inventory.includes('Phaser')) {
                    form.appendLog('The defeated Romulan dropped a Phaser!');
                }
            }
        };
        rn1n1.roomUILogic = (form) => {
            if (form.gameState.actionsTaken.includes('Defeated Romulan') &&
                !form.gameState.inventory.includes('Phaser')) {
                form.showActionButton('Take Phaser');
            } else {
                form.hideActionButton();
            }
        };
        this.rooms[this.key(-1, -1)] = rn1n1;

        // ===== [0,-1] Communications Array =====
        const r0n1 = new Room(0, -1, 'Communications Array', 'Subspace relays line the walls.');
        r0n1.roomEnterLogic = (form) => {
            if (!form.gameState.inventory.includes('Combadge')) {
                form.appendLog("You enter the Communications Array. It's silent.");
                form.appendLog('In the corner, you find a dead Redshirt. 💀');
                form.appendLog('He is clutching his Combadge.');
                form.showActionButton('Take Combadge');
            } else {
                form.appendLog('You are in the Communications Array. The Redshirt is still dead.');
                form.hideActionButton();
            }
        };
        r0n1.roomUILogic = (form) => {
            if (!form.gameState.inventory.includes('Combadge'))
                form.showActionButton('Take Combadge');
            else if (form.gameState.inventory.includes('Phaser') &&
                     !form.gameState.actionsTaken.includes('CutDoor'))
                form.showActionButton('Cut Blast Door');
            else
                form.hideActionButton();

            if (!form.gameState.actionsTaken.includes('CutDoor')) {
                form.lockDirection('Down');
                form.appendLog('The blast door to the Bridge (Down) is sealed. You\'ll have to find another way around.');
            } else {
                form.unlockDirection('Down');
                form.appendLog('The blast door has been cut open with a Phaser.');
            }
        };
        this.rooms[this.key(0, -1)] = r0n1;

        // ===== [-1,-2] Infested Cargo Bay =====
        const rn1n2 = new Room(-1, -2, 'Infested Cargo Bay', '');
        rn1n2.roomEnterLogic = (form) => {
            if (form.gameState.actionsTaken.includes('Defeated Tribbles')) {
                form.appendLog('The cargo bay is quiet. The Tribbles are gone.');
            } else {
                form.appendLog('Sensors indicate lifeforms ahead.');
                form.appendLog('You hear a soft chirping sound... You see something adorable.');
                form.setMovementEnabled(false);
                const enemies = EnemyFactory.createTribbleEncounter();
                if (enemies.length > 0) {
                    form.startCombat(enemies);
                }
            }
        };
        rn1n2.roomUILogic = (form) => { form.hideActionButton(); };
        this.rooms[this.key(-1, -2)] = rn1n2;

        // ===== [0,-2] Bridge =====
        const r0n2 = new Room(0, -2, 'Bridge', 'The main bridge of the ship.');
        r0n2.roomEnterLogic = (form) => {
            if (!form.gameState.actionsTaken.includes('Defeated Q')) {
                form.appendLog("Q is sitting in the Captain's chair! 'Mon capitaine!'");
                form.setMovementEnabled(false);

                const q = new Q();
                if (form.gameState.storedQHealth > 0)
                    q.currentHP = form.gameState.storedQHealth;

                form.startCombat([q]);
            } else {
                form.appendLog('The Bridge is empty. Q has left... for now.');
                form.appendLog('The door to the Security Office (Right) is unlocked.');
            }
        };
        r0n2.roomUILogic = (form) => {
            form.hideActionButton();
            if (!form.gameState.actionsTaken.includes('Defeated Q')) {
                form.lockDirection('Right');
            } else {
                form.unlockDirection('Right');
                form.appendLog('The door to the Security Office is unlocked.');
            }
        };
        this.rooms[this.key(0, -2)] = r0n2;

        // ===== [1,-2] Security Office =====
        const r1n2 = new Room(1, -2, 'Security Office', 'You see a security desk.');
        r1n2.roomEnterLogic = (form) => {};
        r1n2.roomUILogic = (form) => {
            if (!form.gameState.inventory.includes('Keycard'))
                form.showActionButton('Take Keycard');
            else {
                form.hideActionButton();
                form.appendLog('The security desk is empty.');
            }
        };
        this.rooms[this.key(1, -2)] = r1n2;

        // ===== [0,1] Auxiliary Control =====
        const r01 = new Room(0, 1, 'Auxiliary Control', 'You see a manual override console.');
        r01.roomEnterLogic = (form) => {
            form.appendLog("The Emergency Medical Hologram is here. 'I am monitoring the override systems.'");
        };
        r01.roomUILogic = (form) => {
            form.showActionButton('Engage Override');
        };
        this.rooms[this.key(0, 1)] = r01;
    }
}
