/**
 * app.js — Application entry point (port of Program.cs + StartForm.cs + CharacterSelectForm.cs)
 * 
 * Wires up the three screens: Start → Character Select → Game.
 */

let currentGame = null;

// ============ Screen Management ============

function switchScreen(screenId) {
    document.querySelectorAll('.game-screen').forEach(s => s.classList.remove('active'));
    document.getElementById(screenId).classList.add('active');
}

function checkResumeButton() {
    document.getElementById('btnResume').disabled = !GameState.exists();
}

// ============ Start Screen ============

document.getElementById('btnNewGame').addEventListener('click', () => {
    switchScreen('screen-select');
    updateCharPreview();
});

document.getElementById('btnResume').addEventListener('click', () => {
    const state = GameState.load();
    if (!state || !state.playerData) {
        alert('Saved game is corrupted or missing player data.');
        return;
    }

    const player = restorePlayer(state.playerData);
    switchScreen('screen-game');
    document.getElementById('txtGameLog').innerHTML = '';
    currentGame = new GameForm(player, state);
});

// ============ Character Select ============

const playerDataMap = {
    Picard: { name: 'Jean-Luc Picard', hp: 120, str: 10, dex: 12, def: 14, wis: 18, abilities: ['Inspiring Speech', 'Precision Strike'] },
    Worf:   { name: 'Worf',            hp: 150, str: 18, dex: 12, def: 16, wis: 10, abilities: ["Bat'leth Slash", "Warrior's Roar"] },
    Spock:  { name: 'Spock',           hp: 130, str: 14, dex: 14, def: 12, wis: 20, abilities: ['Vulcan Nerve Pinch', 'Logical Counterattack'] },
    Sisko:  { name: 'Benjamin Sisko',  hp: 140, str: 16, dex: 12, def: 15, wis: 14, abilities: ['Sucker Punch', "Commander's Gambit"] }
};

function updateCharPreview() {
    const key = document.getElementById('cmbPlayers').value;
    const data = playerDataMap[key];
    if (!data) return;

    document.getElementById('prevHP').textContent = data.hp;
    document.getElementById('prevSTR').textContent = data.str;
    document.getElementById('prevDEX').textContent = data.dex;
    document.getElementById('prevDEF').textContent = data.def;
    document.getElementById('prevWIS').textContent = data.wis;

    const abList = document.getElementById('prevAbilities');
    abList.innerHTML = '';
    data.abilities.forEach(a => {
        const li = document.createElement('li');
        li.textContent = `▸ ${a}`;
        abList.appendChild(li);
    });
}

document.getElementById('cmbPlayers').addEventListener('change', updateCharPreview);

document.getElementById('btnStartGame').addEventListener('click', () => {
    const key = document.getElementById('cmbPlayers').value;
    const player = createPlayer(key);

    switchScreen('screen-game');
    document.getElementById('txtGameLog').innerHTML = '';
    currentGame = new GameForm(player);
});

// ============ Init ============

checkResumeButton();
updateCharPreview();
