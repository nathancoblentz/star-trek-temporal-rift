/*
 * Module: CombatEngine.cs
 * Purpose: Manages the turn-based combat system.
 * Functionality:
 *  - Handles player attacks and ability usage.
 *  - Manages enemy turns, including AI logic (attacks, breeding, etc.).
 *  - Coordinates combat flow (start, turns, end).
 *  - Updates the UI with combat logs and status changes.
 */

using FinalProject.Classes;
using FinalProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.GameLogic
{

    /// Manages the turn-based combat encounters in the game.
    /// Handles turns, damage calculation, victory conditions, and UI updates.

    public class CombatEngine : ICombatContext
    {
        #region Fields & Properties

        public GameForm Form { get; }
        private Character Player { get; }
        

        /// List of enemies currently in the combat encounter.
        public List<Enemy> Enemies { get; set; }

        private Enemy ActiveEnemy => Enemies.FirstOrDefault(e => e.IsAlive());
        public Enemy CurrentEnemy => ActiveEnemy;
        public int AliveEnemyCount => Enemies.Count(e => e.IsAlive());

        private bool playerTurn = true;
        private bool combatAborted = false;
        private List<Enemy> pendingEnemies = new List<Enemy>();

        #endregion

        #region Constructor

        public CombatEngine(GameForm form, Character player, List<Enemy> enemies)
        {
            Form = form;
            Player = player;
            Enemies = enemies;
        }

        public CombatEngine() { }

        #endregion

        #region Combat Flow


        /// Initializes the combat encounter and sets up the UI.

        public void StartCombat()
        {
            Form.AppendLog($"⚔️ Combat started! {Player.Name} vs {Enemies.Count} enemies!");
            Form.ShowCombatUI();
            Form.UpdatePlayerAbilitiesList();
            PlayerTurn();
        }

        /// <summary>
        /// Prepares the player's turn.
        /// </summary>
        private void PlayerTurn()
        {
            playerTurn = true;
            Player.UpdateStatus();
            Form.AppendLog("➡️ Your turn! Press Fight or use Ability.");
        }

        /// <summary>
        /// Ends the combat encounter and returns to exploration mode.
        /// Checks for specific enemy defeats to update game state flags.
        /// </summary>
        public void EndCombat()
        {
            // Check if we defeated Tribbles
            if (Enemies.Any(e => e is Tribble))
            {
                 if (!Form.gameState.ActionsTaken.Contains("Defeated Tribbles"))
                 {
                     Form.gameState.ActionsTaken.Add("Defeated Tribbles");
                 }
            }

            // Check if we defeated Bat
            if (Enemies.Any(e => e is Bat))
            {
                 if (!Form.gameState.ActionsTaken.Contains("Defeated Bat"))
                 {
                     Form.gameState.ActionsTaken.Add("Defeated Bat");
                 }
            }

            // Check if we defeated Q
            if (Enemies.Any(e => e is Q))
            {
                 if (!Form.gameState.ActionsTaken.Contains("Defeated Q"))
                 {
                     Form.gameState.ActionsTaken.Add("Defeated Q");
                 }
            }

            // Check if we defeated Borg
            if (Enemies.Any(e => e is BorgDrone))
            {
                 if (!Form.gameState.ActionsTaken.Contains("Defeated Borg"))
                 {
                     Form.gameState.ActionsTaken.Add("Defeated Borg");
                 }
            }

            // Check if we defeated Romulan
            if (Enemies.Any(e => e is RomulanScout))
            {
                 if (!Form.gameState.ActionsTaken.Contains("Defeated Romulan"))
                 {
                     Form.gameState.ActionsTaken.Add("Defeated Romulan");
                 }
            }

            Form.AppendLog("⚔️ Combat ended.");

            Form.ReturnToExplorationMode();
        }

        #endregion

        #region Player Actions

        /// <summary>
        /// Executes the player's basic attack against the active enemy.
        /// </summary>
        public async Task PlayerAttack()
        {
            var target = ActiveEnemy;
            if (!playerTurn || target == null) return;
            
            playerTurn = false; // Prevent re-entrancy immediately

            int damage = Player.DealDamage(Form.HasWeaponInInventory());
            int finalDamage = target.TakeDamage(damage);

            await Task.Delay(500); // Delay before log
            
            // Check for specific hit message (e.g. Tribble cuteness)
            string hitMsg = target.GetHitMessage(finalDamage);
            if (!string.IsNullOrEmpty(hitMsg))
            {
                Form.AppendLog(hitMsg);
            }
            else
            {
                Form.AppendLog(Player.GetAttackMessage(finalDamage, target.Name));
            }
            
            UpdateUI();

            if (!target.IsAlive())
            {
                await Task.Delay(500);
                Form.AppendLog(target.GetDefeatMessage());
            }

            // Check for victory
            if (await CheckVictory()) return;

            // playerTurn is already false, so we just proceed to EnemyTurn
            await EnemyTurn();
        }

        /// <summary>
        /// Executes a special ability used by the player.
        /// </summary>
        /// <param name="ability">The ability to activate.</param>
        public async Task PlayerUseAbility(IAbility ability)
        {
            if (!playerTurn || ActiveEnemy == null) return;

            if (ability.CooldownRemaining > 0)
            {
                Form.AppendLog($"{ability.Name} is on cooldown ({ability.CooldownRemaining} turns left).");
                return;
            }
            
            playerTurn = false; // Prevent re-entrancy immediately

            // Pass 'this' as the context
            string log = ability.Activate(Player, ActiveEnemy, this);
            
            // If the ability returns null, it means it failed/aborted (e.g. wrong target).
            // We must restore playerTurn to true so they can try again or do something else.
            if (log == null) 
            {
                playerTurn = true;
                return;
            }

            // If the ability returns a log, show it.
            if (!string.IsNullOrEmpty(log))
            {
                Form.AppendLog(log);
                await Task.Delay(2000);
            }

            ability.StartCooldown();
            UpdateUI();

            // Check if combat ended
            if (!Enemies.Any(e => e.IsAlive()))
            {
                // Proceed to CheckVictory instead of returning
            }
            
            // Check for victory
            if (await CheckVictory()) return;

            // playerTurn is already false, so we just proceed to EnemyTurn
            await EnemyTurn();
        }

        #endregion

        #region Enemy Logic

        /// <summary>
        /// Queues an enemy to be spawned at the end of the turn.
        /// </summary>
        /// <param name="enemy">The enemy to spawn.</param>
        public void SpawnEnemy(Enemy enemy)
        {
            pendingEnemies.Add(enemy);
        }

        /// <summary>
        /// Processes the turn for all active enemies.
        /// Handles AI actions, attacks, and status effects.
        /// </summary>
        private async Task EnemyTurn()
        {
            // Use ToList() to create a snapshot and avoid "Collection modified" exception
            foreach (var enemy in Enemies.Where(e => e.IsAlive()).ToList())
            {
                // Trigger turn start (aging, breeding, etc.)
                enemy.OnTurnStart(this);

                if (combatAborted) return;

                if (enemy.StunnedTurns > 0)
                {
                    enemy.StunnedTurns--;
                    Form.AppendLog($"{enemy.Name} is stunned and cannot act! 💫");
                    continue;
                }

                await Task.Delay(500);
                
                if (combatAborted) return;

                Form.AppendLog($"➡️ {enemy.Name}'s turn!");

                int dmg = enemy.DealDamage();
                int taken = Player.TakeDamage(dmg);

                await Task.Delay(500);
                if (combatAborted) return;

                Form.AppendLog($"{enemy.Name} attacks {Player.Name} for {taken} damage!");
                if (!Player.IsAlive())
                {
                    await Task.Delay(500);
                    if (enemy is Tribble)
                        Form.AppendLog($"{Player.Name} has suffocated from being overwhelmed by Tribbles. 💀");
                    else
                        Form.AppendLog($"{Player.Name} has fallen... 💀");

                    await Task.Delay(2000);
                    Form.gameOver();
                    return;
                }
            }

            // Add any new enemies spawned during the turn
            if (pendingEnemies.Any())
            {
                Enemies.AddRange(pendingEnemies);
                pendingEnemies.Clear();
            }

            // Reduce all cooldowns
            Player.Abilities.ForEach(a => a.ReduceCooldown());

            UpdateUI();
            playerTurn = true;
            await Task.Delay(500);
            
            if (combatAborted) return;

            Form.AppendLog("➡️ Your turn! Press Fight or use Ability.");
        }

        #endregion

        #region Victory Conditions

        /// <summary>
        /// Checks if all enemies are defeated and handles victory logic.
        /// </summary>
        /// <returns>True if victory is achieved, false otherwise.</returns>
        private async Task<bool> CheckVictory()
        {
            if (!Enemies.Any(e => e.IsAlive()))
            {
                 await Task.Delay(500);
                 
                 if (Player is FinalProject.Players.Picard)
                 {
                     Form.AppendLog("Picard gently explains that humans prefer not to be attacked. 🤝");
                     Form.AppendLog("The enemy, persuaded by his logic and diplomacy, stands down.");
                 }
                 else if (Player is FinalProject.Players.Sisko)
                 {
                     Form.AppendLog("Sisko dusts off his uniform. 'You bet against the Sisko, you lose.' ⚾");
                     Form.AppendLog("The Dominion—err, the enemy—retreats.");
                 }
                 else if (Player is FinalProject.Players.Spock)
                 {
                     Form.AppendLog("Spock raises an eyebrow. 'The statistical probability of your defeat was 99.7%.' 🖖");
                     Form.AppendLog("Live long and prosper.");
                 }
                 else if (Player is FinalProject.Players.Worf)
                 {
                     Form.AppendLog("Worf roars in triumph! 'Qapla'! A glorious victory!' ⚔️");
                     Form.AppendLog("He glares at the defeated foe. 'You fought... adequately.'");
                 }
                 else
                 {
                     Form.AppendLog("Victory! All enemies defeated.");
                 }

                 EndCombat();
                 return true;
            }
            return false;
        }

        #endregion

        #region UI & Helpers

        public void AppendLog(string message) => Form.AppendLog(message);
        
        public void UpdateUI()
        {
            Form.UpdatePlayerUI();
            // If active enemy is null (all dead), show the first enemy (dead) so UI shows 0 HP
            var displayEnemy = ActiveEnemy ?? Enemies.FirstOrDefault();
            Form.UpdateEnemyUI(displayEnemy);
        }

        #endregion

        #region Special Events

        /// <summary>
        /// Teleports the player to a random room (Q event).
        /// </summary>
        public async void TeleportPlayer()
        {
            combatAborted = true;
            
            // Save Q's health
            if (ActiveEnemy is Q q)
            {
                Form.gameState.StoredQHealth = q.CurrentHP;
            }

            Form.AppendLog("Q snaps his fingers! The world dissolves... 🌀");
            await Task.Delay(1000);

            // Pick random room (excluding Bridge 0,-2)
            int targetX, targetY;

            if (!Form.gameState.ActionsTaken.Contains("Defeated Tribbles"))
            {
                Form.AppendLog("Q laughs. 'I think you need some furry companions!' 🧶");
                targetX = -1;
                targetY = -2;
            }
            else
            {
                var rooms = new List<(int, int)> 
                { 
                    (0, 0), (-1, 0), (1, 0), 
                    (-1, -1), (0, -1), 
                    (-1, -2), (1, -2), 
                    (0, 1) 
                };
                
                var random = new Random();
                (targetX, targetY) = rooms[random.Next(rooms.Count)];
            }

            // End combat cleanly
            Form.ReturnToExplorationMode();
            
            // Move player
            Form.TeleportTo(targetX, targetY);
            
            Form.AppendLog("You materialize in a different part of the ship...");
            Form.gameState.Save("gamestate.json");
        }

        #endregion
    }
}
