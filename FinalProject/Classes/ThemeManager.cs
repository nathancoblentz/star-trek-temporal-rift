using System.Drawing;
using System.Windows.Forms;

namespace FinalProject.Classes
{
    public static class ThemeManager
    {
        public static void ApplyTrekTheme(Control root)
        {
            // Root Control
            root.BackColor = Color.Black;
            root.ForeColor = Color.Orange; // Amber
            root.Font = new Font("Consolas", 10F, FontStyle.Regular);

            // Recursive helper
            void ThemeControl(Control control)
            {
                control.BackColor = Color.Black;
                control.ForeColor = Color.Orange;
                control.Font = new Font("Consolas", 10F, FontStyle.Regular);

                if (control is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = Color.Orange;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 255, 165, 0); // Dim amber
                }
                else if (control is TextBox || control is RichTextBox || control is ListBox)
                {
                    control.BackColor = Color.Black;
                    control.ForeColor = Color.Orange;
                    control.Font = new Font("Consolas", 10F, FontStyle.Regular);
                }
                else if (control is Label lbl)
                {
                     control.BackColor = Color.Black;
                     control.ForeColor = Color.Orange;
                     control.Font = new Font("Consolas", 10F, FontStyle.Regular);
                }

                foreach (Control child in control.Controls)
                {
                    ThemeControl(child);
                }
            }

            foreach (Control c in root.Controls)
            {
                ThemeControl(c);
            }
        }
    }
}
