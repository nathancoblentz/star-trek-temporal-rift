namespace FinalProject
{
    partial class StartForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnResume;
        private Button btnNewGame;

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
            btnResume = new Button();
            btnNewGame = new Button();
            SuspendLayout();
            // 
            // btnResume
            // 
            btnResume.Location = new Point(12, 24);
            btnResume.Margin = new Padding(3, 4, 3, 4);
            btnResume.Name = "btnResume";
            btnResume.Size = new Size(279, 31);
            btnResume.TabIndex = 0;
            btnResume.Text = "Resume Game";
            btnResume.UseVisualStyleBackColor = true;
            // 
            // btnNewGame
            // 
            btnNewGame.Location = new Point(12, 63);
            btnNewGame.Margin = new Padding(3, 4, 3, 4);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(279, 31);
            btnNewGame.TabIndex = 1;
            btnNewGame.Text = "Start A New Game";
            btnNewGame.UseVisualStyleBackColor = true;
            // 
            // StartForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(303, 129);
            Controls.Add(btnNewGame);
            Controls.Add(btnResume);
            Margin = new Padding(3, 4, 3, 4);
            Name = "StartForm";
            Text = "Welcome to the Adventure";
            ResumeLayout(false);
        }
    }
}
