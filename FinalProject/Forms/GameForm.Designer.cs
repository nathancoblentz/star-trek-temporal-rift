namespace FinalProject
{
    partial class GameForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            btnUp = new Button();
            btnDown = new Button();
            btnLeft = new Button();
            btnRight = new Button();
            btnAction = new Button();
            txtGameLog = new TextBox();
            lblX = new Label();
            lblY = new Label();
            lbInventory = new ListBox();
            lblInventory = new Label();
            txtX = new TextBox();
            txtY = new TextBox();
            lbActionsTaken = new ListBox();
            lblActionsTaken = new Label();
            btnStartOver = new Button();
            btnSaveAndQuit = new Button();
            pnlMovement = new Panel();
            pbPlayerHealth = new ProgressBar();
            pnlPlayerStats = new Panel();
            lblPlayerWisdom = new Label();
            lblPlayerDefense = new Label();
            lblPlayerDexterity = new Label();
            lblPlayerStrength = new Label();
            lblPlayerMaxHP = new Label();
            lblPlayerName = new Label();
            lblPlayerSpecialAbilities = new Label();
            lbPlayerSpecialAbilities = new ListBox();
            pnlEnemyStats = new Panel();
            lblEnemyWisdom = new Label();
            lblEnemyDefense = new Label();
            lblEnemyDexterity = new Label();
            lblEnemyStrength = new Label();
            lblEnemyMaxHP = new Label();
            lblEnemyName = new Label();
            lblEnemySpecialAbilities = new Label();
            lbEnemySpecialAbilities = new ListBox();
            pbEnemyHealth = new ProgressBar();
            pBoxPlayer = new PictureBox();
            pBoxEnemy = new PictureBox();
            btnFight = new Button();
            btnUseAbility = new Button();
            pnlCombat = new Panel();
            pnlMovement.SuspendLayout();
            pnlPlayerStats.SuspendLayout();
            pnlEnemyStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pBoxPlayer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pBoxEnemy).BeginInit();
            pnlCombat.SuspendLayout();
            SuspendLayout();
            // 
            // btnUp
            // 
            btnUp.Location = new Point(99, 12);
            btnUp.Name = "btnUp";
            btnUp.Size = new Size(75, 53);
            btnUp.TabIndex = 0;
            btnUp.Text = "Up";
            btnUp.UseVisualStyleBackColor = true;
            btnUp.Click += btnUp_Click;
            // 
            // btnDown
            // 
            btnDown.Location = new Point(99, 116);
            btnDown.Name = "btnDown";
            btnDown.Size = new Size(75, 53);
            btnDown.TabIndex = 1;
            btnDown.Text = "Down";
            btnDown.UseVisualStyleBackColor = true;
            btnDown.Click += btnDown_Click;
            // 
            // btnLeft
            // 
            btnLeft.Location = new Point(5, 71);
            btnLeft.Name = "btnLeft";
            btnLeft.Size = new Size(88, 53);
            btnLeft.TabIndex = 2;
            btnLeft.Text = "Left";
            btnLeft.UseVisualStyleBackColor = true;
            btnLeft.Click += btnLeft_Click;
            // 
            // btnRight
            // 
            btnRight.Location = new Point(180, 71);
            btnRight.Name = "btnRight";
            btnRight.Size = new Size(75, 53);
            btnRight.TabIndex = 3;
            btnRight.Text = "Right";
            btnRight.UseVisualStyleBackColor = true;
            btnRight.Click += btnRight_Click;
            // 
            // btnAction
            // 
            btnAction.Location = new Point(292, 53);
            btnAction.Name = "btnAction";
            btnAction.Size = new Size(125, 88);
            btnAction.TabIndex = 4;
            btnAction.Text = "Action";
            btnAction.UseVisualStyleBackColor = true;
            btnAction.Click += btnAction_Click;
            // 
            // txtGameLog
            // 
            txtGameLog.Location = new Point(293, 64);
            txtGameLog.Multiline = true;
            txtGameLog.Name = "txtGameLog";
            txtGameLog.ReadOnly = true;
            txtGameLog.ScrollBars = ScrollBars.Vertical;
            txtGameLog.Size = new Size(475, 331);
            txtGameLog.TabIndex = 10;
            // 
            // lblX
            // 
            lblX.AutoSize = true;
            lblX.Location = new Point(293, 12);
            lblX.Name = "lblX";
            lblX.Size = new Size(21, 20);
            lblX.TabIndex = 1;
            lblX.Text = "X:";
            // 
            // lblY
            // 
            lblY.AutoSize = true;
            lblY.Location = new Point(377, 15);
            lblY.Name = "lblY";
            lblY.Size = new Size(20, 20);
            lblY.TabIndex = 0;
            lblY.Text = "Y:";
            // 
            // lbInventory
            // 
            lbInventory.FormattingEnabled = true;
            lbInventory.Location = new Point(796, 306);
            lbInventory.Name = "lbInventory";
            lbInventory.Size = new Size(200, 104);
            lbInventory.TabIndex = 40;
            // 
            // lblInventory
            // 
            lblInventory.AutoSize = true;
            lblInventory.Location = new Point(796, 283);
            lblInventory.Name = "lblInventory";
            lblInventory.Size = new Size(73, 20);
            lblInventory.TabIndex = 62;
            lblInventory.Text = "Inventory:";
            // 
            // txtX
            // 
            txtX.Location = new Point(320, 12);
            txtX.Name = "txtX";
            txtX.ReadOnly = true;
            txtX.Size = new Size(49, 27);
            txtX.TabIndex = 60;
            // 
            // txtY
            // 
            txtY.Location = new Point(403, 12);
            txtY.Name = "txtY";
            txtY.ReadOnly = true;
            txtY.Size = new Size(49, 27);
            txtY.TabIndex = 61;
            // 
            // lbActionsTaken
            // 
            lbActionsTaken.FormattingEnabled = true;
            lbActionsTaken.Location = new Point(796, 101);
            lbActionsTaken.Name = "lbActionsTaken";
            lbActionsTaken.Size = new Size(200, 164);
            lbActionsTaken.TabIndex = 45;
            // 
            // lblActionsTaken
            // 
            lblActionsTaken.Location = new Point(796, 75);
            lblActionsTaken.Name = "lblActionsTaken";
            lblActionsTaken.Size = new Size(143, 23);
            lblActionsTaken.TabIndex = 44;
            lblActionsTaken.Text = "Actions Taken:";
            // 
            // btnStartOver
            // 
            btnStartOver.Location = new Point(796, 430);
            btnStartOver.Name = "btnStartOver";
            btnStartOver.Size = new Size(200, 59);
            btnStartOver.TabIndex = 50;
            btnStartOver.Text = "Start Over";
            btnStartOver.UseVisualStyleBackColor = true;
            btnStartOver.Click += btnStartOver_Click;
            // 
            // btnSaveAndQuit
            // 
            btnSaveAndQuit.Location = new Point(796, 495);
            btnSaveAndQuit.Name = "btnSaveAndQuit";
            btnSaveAndQuit.Size = new Size(200, 73);
            btnSaveAndQuit.TabIndex = 51;
            btnSaveAndQuit.Text = "Save && Quit";
            btnSaveAndQuit.UseVisualStyleBackColor = true;
            btnSaveAndQuit.Click += btnSaveAndQuit_Click;
            // 
            // pnlMovement
            // 
            pnlMovement.Controls.Add(btnAction);
            pnlMovement.Controls.Add(btnUp);
            pnlMovement.Controls.Add(btnDown);
            pnlMovement.Controls.Add(btnLeft);
            pnlMovement.Controls.Add(btnRight);
            pnlMovement.Location = new Point(293, 409);
            pnlMovement.Name = "pnlMovement";
            pnlMovement.Size = new Size(463, 183);
            pnlMovement.TabIndex = 30;
            // 
            // pbPlayerHealth
            // 
            pbPlayerHealth.Location = new Point(0, 0);
            pbPlayerHealth.Name = "pbPlayerHealth";
            pbPlayerHealth.Size = new Size(100, 23);
            pbPlayerHealth.TabIndex = 29;
            // 
            // pnlPlayerStats
            // 
            pnlPlayerStats.Controls.Add(lblPlayerWisdom);
            pnlPlayerStats.Controls.Add(lblPlayerDefense);
            pnlPlayerStats.Controls.Add(lblPlayerDexterity);
            pnlPlayerStats.Controls.Add(lblPlayerStrength);
            pnlPlayerStats.Controls.Add(lblPlayerMaxHP);
            pnlPlayerStats.Controls.Add(lblPlayerName);
            pnlPlayerStats.Controls.Add(lblPlayerSpecialAbilities);
            pnlPlayerStats.Controls.Add(lbPlayerSpecialAbilities);
            pnlPlayerStats.Controls.Add(pbPlayerHealth);
            pnlPlayerStats.Location = new Point(12, 12);
            pnlPlayerStats.Name = "pnlPlayerStats";
            pnlPlayerStats.Size = new Size(259, 383);
            pnlPlayerStats.TabIndex = 20;
            // 
            // lblPlayerWisdom
            // 
            lblPlayerWisdom.AutoSize = true;
            lblPlayerWisdom.Location = new Point(2, 168);
            lblPlayerWisdom.Name = "lblPlayerWisdom";
            lblPlayerWisdom.Size = new Size(71, 20);
            lblPlayerWisdom.TabIndex = 28;
            lblPlayerWisdom.Text = "Wisdom: ";
            // 
            // lblPlayerDefense
            // 
            lblPlayerDefense.AutoSize = true;
            lblPlayerDefense.Location = new Point(2, 148);
            lblPlayerDefense.Name = "lblPlayerDefense";
            lblPlayerDefense.Size = new Size(70, 20);
            lblPlayerDefense.TabIndex = 27;
            lblPlayerDefense.Text = "Defense: ";
            // 
            // lblPlayerDexterity
            // 
            lblPlayerDexterity.AutoSize = true;
            lblPlayerDexterity.Location = new Point(3, 128);
            lblPlayerDexterity.Name = "lblPlayerDexterity";
            lblPlayerDexterity.Size = new Size(76, 20);
            lblPlayerDexterity.TabIndex = 26;
            lblPlayerDexterity.Text = "Dexterity: ";
            // 
            // lblPlayerStrength
            // 
            lblPlayerStrength.AutoSize = true;
            lblPlayerStrength.Location = new Point(3, 108);
            lblPlayerStrength.Name = "lblPlayerStrength";
            lblPlayerStrength.Size = new Size(72, 20);
            lblPlayerStrength.TabIndex = 25;
            lblPlayerStrength.Text = "Strength: ";
            // 
            // lblPlayerMaxHP
            // 
            lblPlayerMaxHP.AutoSize = true;
            lblPlayerMaxHP.Location = new Point(7, 43);
            lblPlayerMaxHP.Name = "lblPlayerMaxHP";
            lblPlayerMaxHP.Size = new Size(67, 20);
            lblPlayerMaxHP.TabIndex = 24;
            lblPlayerMaxHP.Text = "Max HP: ";
            // 
            // lblPlayerName
            // 
            lblPlayerName.AutoSize = true;
            lblPlayerName.Location = new Point(3, 74);
            lblPlayerName.Name = "lblPlayerName";
            lblPlayerName.Size = new Size(89, 20);
            lblPlayerName.TabIndex = 23;
            lblPlayerName.Text = "PlayerName";
            // 
            // lblPlayerSpecialAbilities
            // 
            lblPlayerSpecialAbilities.AutoSize = true;
            lblPlayerSpecialAbilities.Location = new Point(3, 201);
            lblPlayerSpecialAbilities.Name = "lblPlayerSpecialAbilities";
            lblPlayerSpecialAbilities.Size = new Size(118, 20);
            lblPlayerSpecialAbilities.TabIndex = 22;
            lblPlayerSpecialAbilities.Text = "Special Abilities:";
            // 
            // lbPlayerSpecialAbilities
            // 
            lbPlayerSpecialAbilities.FormattingEnabled = true;
            lbPlayerSpecialAbilities.Location = new Point(3, 225);
            lbPlayerSpecialAbilities.Margin = new Padding(3, 4, 3, 4);
            lbPlayerSpecialAbilities.Name = "lbPlayerSpecialAbilities";
            lbPlayerSpecialAbilities.Size = new Size(249, 124);
            lbPlayerSpecialAbilities.TabIndex = 21;
            // 
            // pnlEnemyStats
            // 
            pnlEnemyStats.Controls.Add(lblEnemyWisdom);
            pnlEnemyStats.Controls.Add(lblEnemyDefense);
            pnlEnemyStats.Controls.Add(lblEnemyDexterity);
            pnlEnemyStats.Controls.Add(lblEnemyStrength);
            pnlEnemyStats.Controls.Add(lblEnemyMaxHP);
            pnlEnemyStats.Controls.Add(lblEnemyName);
            pnlEnemyStats.Controls.Add(lblEnemySpecialAbilities);
            pnlEnemyStats.Controls.Add(lbEnemySpecialAbilities);
            pnlEnemyStats.Controls.Add(pbEnemyHealth);
            pnlEnemyStats.Controls.Add(pBoxPlayer);
            pnlEnemyStats.Controls.Add(pBoxEnemy);
            pnlEnemyStats.Location = new Point(1031, 46);
            pnlEnemyStats.Margin = new Padding(3, 4, 3, 4);
            pnlEnemyStats.Name = "pnlEnemyStats";
            pnlEnemyStats.Size = new Size(200, 546);
            pnlEnemyStats.TabIndex = 29;
            // 
            // lblEnemyWisdom
            // 
            lblEnemyWisdom.AutoSize = true;
            lblEnemyWisdom.Location = new Point(6, 231);
            lblEnemyWisdom.Name = "lblEnemyWisdom";
            lblEnemyWisdom.Size = new Size(71, 20);
            lblEnemyWisdom.TabIndex = 28;
            lblEnemyWisdom.Text = "Wisdom: ";
            // 
            // lblEnemyDefense
            // 
            lblEnemyDefense.AutoSize = true;
            lblEnemyDefense.Location = new Point(6, 211);
            lblEnemyDefense.Name = "lblEnemyDefense";
            lblEnemyDefense.Size = new Size(70, 20);
            lblEnemyDefense.TabIndex = 27;
            lblEnemyDefense.Text = "Defense: ";
            // 
            // lblEnemyDexterity
            // 
            lblEnemyDexterity.AutoSize = true;
            lblEnemyDexterity.Location = new Point(7, 191);
            lblEnemyDexterity.Name = "lblEnemyDexterity";
            lblEnemyDexterity.Size = new Size(76, 20);
            lblEnemyDexterity.TabIndex = 26;
            lblEnemyDexterity.Text = "Dexterity: ";
            // 
            // lblEnemyStrength
            // 
            lblEnemyStrength.AutoSize = true;
            lblEnemyStrength.Location = new Point(7, 171);
            lblEnemyStrength.Name = "lblEnemyStrength";
            lblEnemyStrength.Size = new Size(72, 20);
            lblEnemyStrength.TabIndex = 25;
            lblEnemyStrength.Text = "Strength: ";
            // 
            // lblEnemyMaxHP
            // 
            lblEnemyMaxHP.AutoSize = true;
            lblEnemyMaxHP.Location = new Point(7, 43);
            lblEnemyMaxHP.Name = "lblEnemyMaxHP";
            lblEnemyMaxHP.Size = new Size(67, 20);
            lblEnemyMaxHP.TabIndex = 24;
            lblEnemyMaxHP.Text = "Max HP: ";
            // 
            // lblEnemyName
            // 
            lblEnemyName.AutoSize = true;
            lblEnemyName.Location = new Point(7, 137);
            lblEnemyName.Name = "lblEnemyName";
            lblEnemyName.Size = new Size(93, 20);
            lblEnemyName.TabIndex = 23;
            lblEnemyName.Text = "EnemyName";
            // 
            // lblEnemySpecialAbilities
            // 
            lblEnemySpecialAbilities.AutoSize = true;
            lblEnemySpecialAbilities.Location = new Point(7, 260);
            lblEnemySpecialAbilities.Name = "lblEnemySpecialAbilities";
            lblEnemySpecialAbilities.Size = new Size(118, 20);
            lblEnemySpecialAbilities.TabIndex = 22;
            lblEnemySpecialAbilities.Text = "Special Abilities:";
            // 
            // lbEnemySpecialAbilities
            // 
            lbEnemySpecialAbilities.FormattingEnabled = true;
            lbEnemySpecialAbilities.Location = new Point(7, 288);
            lbEnemySpecialAbilities.Margin = new Padding(3, 4, 3, 4);
            lbEnemySpecialAbilities.Name = "lbEnemySpecialAbilities";
            lbEnemySpecialAbilities.Size = new Size(161, 124);
            lbEnemySpecialAbilities.TabIndex = 21;
            // 
            // pbEnemyHealth
            // 
            pbEnemyHealth.Location = new Point(7, 24);
            pbEnemyHealth.Margin = new Padding(3, 4, 3, 4);
            pbEnemyHealth.Name = "pbEnemyHealth";
            pbEnemyHealth.Size = new Size(114, 19);
            pbEnemyHealth.TabIndex = 19;
            pbEnemyHealth.Visible = false;
            // 
            // pBoxPlayer
            // 
            pBoxPlayer.Location = new Point(105, 550);
            pBoxPlayer.Margin = new Padding(3, 4, 3, 4);
            pBoxPlayer.Name = "pBoxPlayer";
            pBoxPlayer.Size = new Size(16, 33);
            pBoxPlayer.TabIndex = 20;
            pBoxPlayer.TabStop = false;
            // 
            // pBoxEnemy
            // 
            pBoxEnemy.Location = new Point(159, 550);
            pBoxEnemy.Margin = new Padding(3, 4, 3, 4);
            pBoxEnemy.Name = "pBoxEnemy";
            pBoxEnemy.Size = new Size(20, 33);
            pBoxEnemy.TabIndex = 20;
            pBoxEnemy.TabStop = false;
            pBoxEnemy.Visible = false;
            // 
            // btnFight
            // 
            btnFight.Location = new Point(31, 15);
            btnFight.Name = "btnFight";
            btnFight.Size = new Size(198, 65);
            btnFight.TabIndex = 5;
            btnFight.Text = "Fight";
            btnFight.UseVisualStyleBackColor = true;
            btnFight.Click += btnFight_Click;
            // 
            // btnUseAbility
            // 
            btnUseAbility.Location = new Point(31, 86);
            btnUseAbility.Name = "btnUseAbility";
            btnUseAbility.Size = new Size(198, 73);
            btnUseAbility.TabIndex = 6;
            btnUseAbility.Text = "Ability";
            btnUseAbility.UseVisualStyleBackColor = true;
            btnUseAbility.Click += btnUseAbility_Click;
            // 
            // pnlCombat
            // 
            pnlCombat.Controls.Add(btnUseAbility);
            pnlCombat.Controls.Add(btnFight);
            pnlCombat.Location = new Point(12, 409);
            pnlCombat.Name = "pnlCombat";
            pnlCombat.Size = new Size(259, 192);
            pnlCombat.TabIndex = 31;
            pnlCombat.Visible = false;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1243, 623);
            Controls.Add(lblY);
            Controls.Add(lblX);
            Controls.Add(txtY);
            Controls.Add(txtX);
            Controls.Add(btnStartOver);
            Controls.Add(btnSaveAndQuit);
            Controls.Add(lblInventory);
            Controls.Add(lbInventory);
            Controls.Add(txtGameLog);
            Controls.Add(lbActionsTaken);
            Controls.Add(lblActionsTaken);
            Controls.Add(pnlEnemyStats);
            Controls.Add(pnlPlayerStats);
            Controls.Add(pnlMovement);
            Controls.Add(pnlCombat);
            Name = "GameForm";
            Text = "      ";
            pnlMovement.ResumeLayout(false);
            pnlPlayerStats.ResumeLayout(false);
            pnlPlayerStats.PerformLayout();
            pnlEnemyStats.ResumeLayout(false);
            pnlEnemyStats.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pBoxPlayer).EndInit();
            ((System.ComponentModel.ISupportInitialize)pBoxEnemy).EndInit();
            pnlCombat.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private Button btnUp;
        private Button btnDown;
        private Button btnLeft;
        private Button btnRight;
        private Button btnAction;
        private TextBox txtGameLog;
        private Label lblX;
        private Label lblY;
        private ListBox lbInventory;
        private Label lblInventory;
        private TextBox txtX;
        private TextBox txtY;
        private ListBox lbActionsTaken;
        private Label lblActionsTaken;
        private Button btnStartOver;
        private Button btnSaveAndQuit;
        private Panel pnlMovement;
        private Panel pnlPlayerStats;
        private ProgressBar pbPlayerHealth;
        private Label lblPlayerName;
        private Label lblPlayerSpecialAbilities;
        private ListBox lbPlayerSpecialAbilities;
        private Label lblPlayerWisdom;
        private Label lblPlayerDefense;
        private Label lblPlayerDexterity;
        private Label lblPlayerStrength;
        private Label lblPlayerMaxHP;
        private Panel pnlEnemyStats;
        private Label lblEnemyWisdom;
        private Label lblEnemyDefense;
        private Label lblEnemyDexterity;
        private Label lblEnemyStrength;
        private Label lblEnemyMaxHP;
        private Label lblEnemyName;
        private Label lblEnemySpecialAbilities;
        private ListBox lbEnemySpecialAbilities;
        private ProgressBar pbEnemyHealth;
        internal Button btnFight;
        private Button btnUseAbility;
        private Panel pnlCombat;
        private PictureBox pBoxPlayer;
        private PictureBox pBoxEnemy;
    }
}
