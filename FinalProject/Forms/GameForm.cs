// Final Project for CPT-230-W48 C# Programming I
// Jonathan Coblentz

// Classes: Hero, Villain, Character, Theme Manager (for Star Trek aesthetic)
// Subclasses:
//      Players: choose between Spock, Benjamin Sisko, Worf and Jean Luc-Picard, each with their own abilities and flavor dialogue
//      Enemies:  Bat - A basic easy fight
//                RomulanScout - Another easy fight, and drops a Phaser
//                BorgDrone (not implimented),
//                Tribbles - Tribbles have a 'Cute' defense for the first 4 rounds, they will "multiply" which means their strength
//                and hit points increase exponentially.  They can easily overwhelm the player, but can be beamed out by Scotty
//                Q - performs a few random attacks, teleports Player to another room in the game; if player hasn't fought the Tribbles yet,
//                he sends you there first.  Each player has a unique ability that chases Q away and Q has a snarky response unique to each player.                
// Interfaces:    IAbility, ICombatContext, ICombatant (built from IAttackable and IAttackableTarget)
// Interactive elements:
//      Override console (unlocks a door)
//      Route Power to Sensors (boosts defense)
//      Phaser - boosts damage and blasts through a blocked door
//      Combadge - Calls Scotty.  After two unsuccessful attempts (with a Scotty catchphrase!),
//      it beams the Tribbles into space ending teh combat sequence.
//      KeyCard - opens the captain's locker so the player can take the Time Crystal, ending the gameplay.
// Extra:
//      - Inventory and "Actions taken" log to track the player's progress in the game.  Both are tracked in the GameState object
// which writes to a JSON file so the Player can resume progress if they quit the game.
//      - Player Abilities - They utilize the IAbility interface:
//          Picard: Inspiring Speech, Precision Strike
//          Word: Bat'leth Slash, Warrior's Roar (Worf),
//          Spock: Vulcan Nerve Pinch, Logical Counterattack
//          Sisko: Sucker Punch, Commander's Gambit (Sisko)
//


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinalProject.Classes;
using FinalProject.GameLogic;
using FinalProject.Interfaces;

namespace FinalProject
{
    public partial class GameForm : Form
    {
        #region Fields & Properties

        public Player CurrentPlayer { get; set; }

        public GameState gameState; // Save data

        private Maze maze; // Build the maze

        private int playerX; // Player coordinates
        private int playerY;

        public CombatEngine CurrentCombat { get; set; }

        private Character CurrentEnemy => CurrentCombat?.CurrentEnemy;

        private bool isGameOver = false;

        public bool ShouldSerializeCurrentPlayer() => false;
        public bool ShouldSerializeCurrentCombat() => false;

        #endregion

        #region Constructors

        // --- Default constructor (for new player) ---
        public GameForm(Player chosenPlayer)
        {
            InitializeComponent();
            ThemeManager.ApplyTrekTheme(this);

            // assign selected player
            CurrentPlayer = chosenPlayer ?? throw new ArgumentNullException(nameof(chosenPlayer));

            // new game state
            gameState = new GameState
            {
                CurrentPlayer = CurrentPlayer,
                PlayerX = playerX,
                PlayerY = playerY
            };

            maze = new Maze();

            // Start with enemy UI hidden
            ShowMovementUI();

            // Populate UI
            UpdatePlayerUI();
            RefreshInventoryList();
            RefreshActionsTakenList();
            EnterRoom();
        }

        // --- Constructor for resuming a saved game ---
        public GameForm(GameState loadedState)
        {
            InitializeComponent();
            ThemeManager.ApplyTrekTheme(this);

            gameState = loadedState ?? new GameState();
            CurrentPlayer = gameState.CurrentPlayer ?? CurrentPlayer;
            playerX = gameState.PlayerX;
            playerY = gameState.PlayerY;

            maze = new Maze();

            // Start with enemy UI hidden until combat starts
            ShowMovementUI();

            UpdatePlayerUI();
            RefreshInventoryList();
            RefreshActionsTakenList();
            EnterRoom();
        }

        #endregion

        #region Core Logic

        public void StartCombat(List<Enemy> enemies)
        {
            CurrentCombat = new CombatEngine(this, CurrentPlayer, enemies);
            CurrentCombat.StartCombat();

            ShowCombatUI();
            btnFight.Visible = true;
            btnFight.Enabled = true;
        }

        private void EnterRoom()
        {
            UpdateMovementButtons();

            txtX.Text = playerX.ToString();
            txtY.Text = playerY.ToString();

            var room = maze.GetRoom(playerX, playerY);
            if (room != null)
            {
                AppendLog($"\r\n=== {room.Name} ===");
                AppendLog(room.Description);

                // Room logic may show/hide action button or do other things
                room.RoomEnterLogic?.Invoke(this);
                room.RoomUILogic?.Invoke(this);
            }
        }

        public void LoadRoom()
        {
            UpdateMovementButtons();

            txtX.Text = playerX.ToString();
            txtY.Text = playerY.ToString();

            maze.GetRoom(playerX, playerY)?.RoomUILogic?.Invoke(this);
        }

        public void TeleportTo(int x, int y)
        {
            playerX = x;
            playerY = y;
            gameState.PlayerX = x;
            gameState.PlayerY = y;
            EnterRoom();
        }

        #endregion

        #region Movement Logic

        private void MovePlayer(int dx, int dy)
        {
            int newX = playerX + dx;
            int newY = playerY + dy;

            var room = maze.GetRoom(newX, newY);
            if (room == null)
            {
                AppendLog("You can't go that way.");
                return;
            }

            playerX = newX;
            playerY = newY;

            if (gameState == null) gameState = new GameState();

            gameState.PlayerX = playerX;
            gameState.PlayerY = playerY;
            gameState.CurrentPlayer = CurrentPlayer;
            gameState.Save("gamestate.json");

            EnterRoom();
        }

        public void SetMovementEnabled(bool enabled)
        {
            if (enabled)
            {
                UpdateMovementButtons();
            }
            else
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
                btnLeft.Enabled = false;
                btnRight.Enabled = false;
            }
        }

        public void UpdateMovementButtons()
        {
            btnUp.Enabled = maze.GetRoom(playerX, playerY + 1) != null;
            btnDown.Enabled = maze.GetRoom(playerX, playerY - 1) != null;
            btnRight.Enabled = maze.GetRoom(playerX + 1, playerY) != null;
            btnLeft.Enabled = maze.GetRoom(playerX - 1, playerY) != null;
        }

        public void EnableMovementButtons()
        {
            UpdateMovementButtons();
        }

        public void LockDirection(string direction)
        {
            switch (direction)
            {
                case "Left": btnLeft.Enabled = false; break;
                case "Right": btnRight.Enabled = false; break;
                case "Up": btnUp.Enabled = false; break;
                case "Down": btnDown.Enabled = false; break;
            }
        }

        public void UnlockDirection(string direction)
        {
            switch (direction)
            {
                case "Left": btnLeft.Enabled = true; break;
                case "Right": btnRight.Enabled = true; break;
                case "Up": btnUp.Enabled = true; break;
                case "Down": btnDown.Enabled = true; break;
            }
        }

        #endregion

        #region UI Management

        public void UpdatePlayerUI()
        {
            if (CurrentPlayer == null) return;

            // Ensure progress bar max is set before assigning value
            pbPlayerHealth.Maximum = Math.Max(1, CurrentPlayer.MaxHP);
            pbPlayerHealth.Value = Math.Max(0, Math.Min(CurrentPlayer.CurrentHP, pbPlayerHealth.Maximum));

            lblPlayerName.Text = CurrentPlayer.Name ?? "Player";
            lblPlayerMaxHP.Text = $"{CurrentPlayer.CurrentHP}/{CurrentPlayer.MaxHP}";
            lblPlayerStrength.Text = $"Strength: {CurrentPlayer.Strength}";
            lblPlayerDexterity.Text = $"Dexterity: {CurrentPlayer.Dexterity}";
            lblPlayerDefense.Text = $"Defense: {CurrentPlayer.Defense}";
            lblPlayerWisdom.Text = $"Wisdom: {CurrentPlayer.Wisdom}";

            UpdatePlayerAbilitiesList();
        }

        public void UpdateEnemyUI(Character enemy)
        {
            if (enemy == null)
            {
                ShowMovementUI();
                return;
            }

            ShowCombatUI();
            pbEnemyHealth.Maximum = Math.Max(1, enemy.MaxHP);
            pbEnemyHealth.Value = Math.Max(0, Math.Min(enemy.CurrentHP, pbEnemyHealth.Maximum));

            lblEnemyName.Text = enemy.Name ?? "Enemy";
            lblEnemyMaxHP.Text = $"{enemy.CurrentHP}/{enemy.MaxHP}";
            lblEnemyStrength.Text = $"Strength: {enemy.Strength}";
            lblEnemyDexterity.Text = $"Dexterity: {enemy.Dexterity}";
            lblEnemyDefense.Text = $"Defense: {enemy.Defense}";
            lblEnemyWisdom.Text = $"Wisdom: {enemy.Wisdom}";

            lbEnemySpecialAbilities.Items.Clear();
            if (enemy.Abilities != null)
            {
                lbEnemySpecialAbilities.DisplayMember = "Name";
                foreach (var a in enemy.Abilities)
                    lbEnemySpecialAbilities.Items.Add(a);
            }
        }

        public void ShowCombatUI()
        {
            pnlMovement.Visible = false;
            pnlCombat.Visible = true;

            // Show Enemy Stats
            pnlEnemyStats.Visible = true;
            pbEnemyHealth.Visible = true;
            pBoxEnemy.Visible = true;
        }

        public void ShowMovementUI()
        {
            pnlMovement.Visible = true;
            pnlCombat.Visible = false;

            // Hide Enemy Stats
            pnlEnemyStats.Visible = false;
            pbEnemyHealth.Visible = false;
            pBoxEnemy.Visible = false;

            // Clear enemy display content
            lblEnemyName.Text = "EnemyName";
            lblEnemyMaxHP.Text = "Max HP: ";
            lblEnemyStrength.Text = "Strength: ";
            lblEnemyDexterity.Text = "Dexterity: ";
            lblEnemyDefense.Text = "Defense: ";
            lblEnemyWisdom.Text = "Wisdom: ";
            lbEnemySpecialAbilities.Items.Clear();
        }

        public void ReturnToExplorationMode()
        {
            HideActionButton();
            ShowMovementUI();
            SetMovementEnabled(true);
            RefreshActionsTakenList();
            LoadRoom();
        }

        public bool HasWeaponInInventory()
        {
            return gameState != null && (gameState.Inventory.Contains("Phaser"));
        }

        private void RefreshInventoryList()
        {
            lbInventory.Items.Clear();
            if (gameState == null) return;
            foreach (var item in gameState.Inventory)
                lbInventory.Items.Add(item);
        }

        private void RefreshActionsTakenList()
        {
            lbActionsTaken.Items.Clear();
            if (gameState == null) return;
            foreach (var action in gameState.ActionsTaken)
                lbActionsTaken.Items.Add(action);
        }

        public void AppendLog(string message)
        {
            if (this.IsDisposed || txtGameLog.IsDisposed) return;
            txtGameLog.AppendText("\r\n" + message + "\n\n");
        }

        public void ShowActionButton(string text)
        {
            btnAction.Visible = true;
            btnAction.Text = text;
        }

        public void HideActionButton()
        {
            btnAction.Visible = false;
        }

        public void UpdatePlayerAbilitiesList()
        {
            lbPlayerSpecialAbilities.Items.Clear();
            if (CurrentPlayer?.Abilities == null) return;

            foreach (var ability in CurrentPlayer.Abilities)
            {
                string text = ability.CooldownRemaining > 0
                    ? $"{ability.Name} (CD: {ability.CooldownRemaining})"
                    : ability.Name;

                lbPlayerSpecialAbilities.Items.Add(ability);
                lbPlayerSpecialAbilities.DisplayMember = "Name";
            }
        }

        #endregion

        #region Event Handlers

        private async void btnFight_Click(object sender, EventArgs e)
        {
            if (CurrentCombat == null) return;
            await CurrentCombat.PlayerAttack();
        }

        private async void btnUseAbility_Click(object sender, EventArgs e)
        {
            if (lbPlayerSpecialAbilities.SelectedItem is IAbility ability)
            {
                if (CurrentCombat == null) return;
                await CurrentCombat.PlayerUseAbility(ability);
            }
        }

        protected void btnUp_Click(object sender, EventArgs e) => MovePlayer(0, 1);
        protected void btnDown_Click(object sender, EventArgs e) => MovePlayer(0, -1);
        protected void btnLeft_Click(object sender, EventArgs e) => MovePlayer(-1, 0);
        protected void btnRight_Click(object sender, EventArgs e) => MovePlayer(1, 0);

        private void btnAction_Click(object sender, EventArgs e)
        {
            var room = maze.GetRoom(playerX, playerY);
            if (room == null) return;

            string actionText = btnAction.Text;

            switch (actionText)
            {
                case "Route Power to Sensors":
                    if (!gameState.ActionsTaken.Contains("RoutedPower"))
                        gameState.ActionsTaken.Add("RoutedPower");
                    AppendLog("You reroute auxiliary power to the sensor array. The console lights up.");
                    break;

                case "Take Phaser":
                    if (!gameState.Inventory.Contains("Phaser"))
                        gameState.Inventory.Add("Phaser");
                    AppendLog("You take the Type-2 Phaser.");
                    break;

                case "Cut Blast Door":
                    if (!gameState.ActionsTaken.Contains("CutDoor"))
                        gameState.ActionsTaken.Add("CutDoor");
                    AppendLog("You set the Phaser to maximum and slice through the blast door locks. 💥");
                    AppendLog("The door slides open with a groan.");
                    break;

                case "Engage Override":
                    if (!gameState.ActionsTaken.Contains("Engaged override"))
                    {
                        gameState.ActionsTaken.Add("Engaged override");
                        AppendLog("You engage the manual override. The forcefields deactivate.");
                    }
                    else AppendLog("Override already engaged.");
                    break;

                case "Take Keycard":
                    if (!gameState.Inventory.Contains("Keycard"))
                        gameState.Inventory.Add("Keycard");
                    AppendLog("You pick up the Security Keycard.");
                    break;

                case "Take Tricorder":
                    if (!gameState.Inventory.Contains("Tricorder"))
                        gameState.Inventory.Add("Tricorder");
                    AppendLog("You pick up the standard issue Tricorder.");
                    break;

                case "Access Locker":
                    GameWin();
                    break;

                case "Scan Entity":
                    AppendLog("You scan the entity. It appears to be a silicon-based lifeform.");
                    break;

                case "Take Combadge":
                    if (!gameState.Inventory.Contains("Combadge"))
                    {
                        gameState.Inventory.Add("Combadge");
                        AppendLog("You take the Combadge from the dead Redshirt.");
                        AppendLog("You feel a strange connection to the ship's engineer...");

                        // Grant the ability
                        CurrentPlayer.Abilities.Add(new FinalProject.Players.CallScottyAbility());
                        AppendLog("Ability Acquired: Call Scotty! 🖖");

                        HideActionButton();
                        UpdatePlayerUI();
                    }
                    break;

                default:
                    AppendLog($"Action '{actionText}' not handled.");
                    break;
            }

            // Save and refresh UI
            gameState.Save("gamestate.json");
            LoadRoom();
            RefreshInventoryList();
            RefreshActionsTakenList();
        }

        private void btnStartOver_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to start over? This will erase all game progress.",
                "Confirm Start Over",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
                ResetGame();
        }

        private void btnSaveAndQuit_Click(object sender, EventArgs e)
        {
            gameState.Save("gamestate.json");
            Application.Exit();
        }

        #endregion

        #region Game State Management

        private void ResetGame()
        {
            // Reset in-memory state
            gameState = new GameState();

            // Persist empty/new state
            gameState.Save("gamestate.json");

            // Close current game form (the caller will open CharacterSelectForm)
            this.Close();

            // Open the Character Select screen
            var selectForm = new CharacterSelectForm();
            selectForm.Show();
        }

        public void gameOver()
        {
            if (isGameOver) return;
            isGameOver = true;

            var result = MessageBox.Show(
                "Try again",
                "Try again?",
                MessageBoxButtons.YesNo
                );

            if (result == DialogResult.Yes)
            {
                ResetGame();
            }
            else
            {
                this.Close();
            }
        }

        public void GameWin()
        {
            if (isGameOver) return;
            isGameOver = true;

            MessageBox.Show(
                "You open the locker and find the shimmering Time Crystal! 💎\r\n" +
                "With this artifact, you can restore the timeline and save the ship!\r\n\r\n" +
                "MISSION ACCOMPLISHED!",
                "Victory!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            var result = MessageBox.Show(
                "Play again?",
                "Victory!",
                MessageBoxButtons.YesNo
            );

            if (result == DialogResult.Yes)
            {
                ResetGame();
            }
            else
            {
                this.Close();
            }
        }

        #endregion


    }
}
