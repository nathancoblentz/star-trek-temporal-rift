using FinalProject.GameLogic;
using System;
using System.IO;
using System.Windows.Forms;
using FinalProject.Classes;

namespace FinalProject
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            ThemeManager.ApplyTrekTheme(this);

            // Disable Resume button if no save file exists
            btnResume.Enabled = File.Exists("gamestate.json");

            // Button handlers
            btnResume.Click += btnResume_Click;
            btnNewGame.Click += BtnNewGame_Click;
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            GameState gameState = GameState.Load("gamestate.json");
            if (gameState?.CurrentPlayer == null)
            {
                MessageBox.Show("Saved game is corrupted or missing player data.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GameForm game = new GameForm(gameState); // pass loaded GameState
            game.Show();
            this.Hide();
        }

        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            // Open CharacterSelectForm for new game
            CharacterSelectForm select = new CharacterSelectForm();
            select.Show();
            this.Hide();
        }

    }
}
