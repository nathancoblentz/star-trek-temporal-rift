/**
 * GameState.js — Persistent game state (port of GameLogic/GameState.cs)
 * 
 * Uses localStorage instead of JSON file I/O.
 */
class GameState {
    constructor() {
        this.playerX = 0;
        this.playerY = 0;
        this.inventory = [];
        this.actionsTaken = [];
        this.hasSeenStartMessage = false;
        this.storedQHealth = -1;
        this.playerData = null; // serialized player
    }

    save() {
        const data = {
            playerX: this.playerX,
            playerY: this.playerY,
            inventory: this.inventory,
            actionsTaken: this.actionsTaken,
            hasSeenStartMessage: this.hasSeenStartMessage,
            storedQHealth: this.storedQHealth,
            playerData: this.playerData
        };
        localStorage.setItem('starTrekGameState', JSON.stringify(data));
    }

    static load() {
        const json = localStorage.getItem('starTrekGameState');
        if (!json) return null;

        try {
            const data = JSON.parse(json);
            const state = new GameState();
            state.playerX = data.playerX || 0;
            state.playerY = data.playerY || 0;
            state.inventory = data.inventory || [];
            state.actionsTaken = data.actionsTaken || [];
            state.hasSeenStartMessage = data.hasSeenStartMessage || false;
            state.storedQHealth = data.storedQHealth || -1;
            state.playerData = data.playerData || null;
            return state;
        } catch (e) {
            console.error('Failed to load game state:', e);
            return null;
        }
    }

    static clear() {
        localStorage.removeItem('starTrekGameState');
    }

    static exists() {
        return localStorage.getItem('starTrekGameState') !== null;
    }
}
