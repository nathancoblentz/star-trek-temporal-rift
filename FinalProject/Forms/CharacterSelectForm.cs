using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FinalProject.Classes;
/*
 * Module: CharacterSelectForm.cs
 * Purpose: Provides the character selection screen.
 * Functionality:
 *  - Allows the user to choose a character class (e.g., Spock, Sisko, Worf or Picard).
 *  - Displays a brief description of each character.
 *  - Instantiates the selected Player object and launches the main GameForm.
 */
using FinalProject.Players;

namespace FinalProject
{
    public partial class CharacterSelectForm : Form
    {
        private Dictionary<string, Player> playerLookup;

        public CharacterSelectForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            ThemeManager.ApplyTrekTheme(this);
            LoadPlayers();
            SetupUI();
        }

        private void LoadPlayers()
        {
            // Instantiate all available players
            playerLookup = new Dictionary<string, Player>()
            {
                { "Picard", new Picard() },
                { "Worf", new Worf() },
                { "Spock", new Spock() },
                { "Sisko", new Sisko() }
            };
        }

        private void SetupUI()
        {
            // Load names into ComboBox
            cmbPlayers.Items.Clear();
            foreach (var name in playerLookup.Keys)
                cmbPlayers.Items.Add(name);

            // Hook events
            cmbPlayers.SelectedIndexChanged += CmbPlayers_SelectedIndexChanged;
            btnStartGame.Click += BtnStartGame_Click;

            // Default selection
            if (cmbPlayers.Items.Count > 0)
                cmbPlayers.SelectedIndex = 0;
        }

        private void CmbPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbPlayers.SelectedItem?.ToString();
            if (selected == null) return;

            if (!playerLookup.TryGetValue(selected, out Player player))
                return;

            // Update labels
            lblName.Text = $"Name: {player.Name}";
            lblPlayerStrength.Text = $"Strength: {player.Strength}";
            lblPlayerDexterity.Text = $"Dexterity: {player.Dexterity}";
            lblPlayerDefense.Text = $"Defense: {player.Defense}";
            lblPlayerWisdom.Text = $"Wisdom: {player.Wisdom}";
            lblPlayerMaxHP.Text = $"Max HP: {player.MaxHP}";

            // Update abilities list
            lbAbilities.Items.Clear();
            if (player.Abilities != null)
            {
                foreach (var ability in player.Abilities)
                    lbAbilities.Items.Add(ability.Name);
            }
        }

        private void BtnStartGame_Click(object sender, EventArgs e)
        {
            if (cmbPlayers.SelectedItem == null)
            {
                MessageBox.Show("Please select a player.");
                return;
            }

            string selected = cmbPlayers.SelectedItem.ToString();
            Player chosenPlayer = playerLookup[selected];

            GameForm form = new GameForm(chosenPlayer);
            form.Show();
            this.Hide();
        }
    }
}
