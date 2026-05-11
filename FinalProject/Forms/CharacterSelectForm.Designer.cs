namespace FinalProject
{
    partial class CharacterSelectForm
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cmbPlayers;
        private Label lblName;
        private Label lblPlayerStrength;
        private Label lblPlayerDexterity;
        private Label lblPlayerDefense;
        private Label lblPlayerWisdom;
        private Label lblPlayerMaxHP;
        private ListBox lbAbilities;
        private Button btnStartGame;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            cmbPlayers = new ComboBox();
            lblName = new Label();
            lblPlayerStrength = new Label();
            lblPlayerDexterity = new Label();
            lblPlayerDefense = new Label();
            lblPlayerWisdom = new Label();
            lblPlayerMaxHP = new Label();
            lbAbilities = new ListBox();
            btnStartGame = new Button();
            SuspendLayout();
            // 
            // cmbPlayers
            // 
            cmbPlayers.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlayers.Location = new Point(20, 20);
            cmbPlayers.Name = "cmbPlayers";
            cmbPlayers.Size = new Size(200, 28);
            cmbPlayers.TabIndex = 0;
            // 
            // lblName
            // 
            lblName.Location = new Point(20, 60);
            lblName.Name = "lblName";
            lblName.Size = new Size(200, 23);
            lblName.TabIndex = 1;
            // 
            // lblPlayerStrength
            // 
            lblPlayerStrength.Location = new Point(20, 90);
            lblPlayerStrength.Name = "lblPlayerStrength";
            lblPlayerStrength.Size = new Size(200, 23);
            lblPlayerStrength.TabIndex = 2;
            // 
            // lblPlayerDexterity
            // 
            lblPlayerDexterity.Location = new Point(20, 120);
            lblPlayerDexterity.Name = "lblPlayerDexterity";
            lblPlayerDexterity.Size = new Size(200, 23);
            lblPlayerDexterity.TabIndex = 3;
            // 
            // lblPlayerDefense
            // 
            lblPlayerDefense.Location = new Point(20, 150);
            lblPlayerDefense.Name = "lblPlayerDefense";
            lblPlayerDefense.Size = new Size(200, 23);
            lblPlayerDefense.TabIndex = 4;
            // 
            // lblPlayerWisdom
            // 
            lblPlayerWisdom.Location = new Point(20, 180);
            lblPlayerWisdom.Name = "lblPlayerWisdom";
            lblPlayerWisdom.Size = new Size(200, 23);
            lblPlayerWisdom.TabIndex = 5;
            // 
            // lblPlayerMaxHP
            // 
            lblPlayerMaxHP.Location = new Point(20, 210);
            lblPlayerMaxHP.Name = "lblPlayerMaxHP";
            lblPlayerMaxHP.Size = new Size(200, 23);
            lblPlayerMaxHP.TabIndex = 6;
            // 
            // lbAbilities
            // 
            lbAbilities.Location = new Point(250, 20);
            lbAbilities.Name = "lbAbilities";
            lbAbilities.Size = new Size(200, 144);
            lbAbilities.TabIndex = 7;
            // 
            // btnStartGame
            // 
            btnStartGame.Location = new Point(20, 250);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(120, 38);
            btnStartGame.TabIndex = 8;
            btnStartGame.Text = "Start Game";
            // 
            // CharacterSelectForm
            // 
            ClientSize = new Size(480, 300);
            Controls.Add(cmbPlayers);
            Controls.Add(lblName);
            Controls.Add(lblPlayerStrength);
            Controls.Add(lblPlayerDexterity);
            Controls.Add(lblPlayerDefense);
            Controls.Add(lblPlayerWisdom);
            Controls.Add(lblPlayerMaxHP);
            Controls.Add(lbAbilities);
            Controls.Add(btnStartGame);
            Name = "CharacterSelectForm";
            Text = "Select Your Player";
            ResumeLayout(false);
        }
    }
}
