namespace GameForm
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveGame = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exit = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.GasBar = new System.Windows.Forms.ProgressBar();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(489, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGame,
            this.toolStripSeparator1,
            this.saveGame,
            this.loadGame,
            this.toolStripSeparator2,
            this.exit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "Fájl";
            // 
            // newGame
            // 
            this.newGame.Name = "newGame";
            this.newGame.Size = new System.Drawing.Size(151, 22);
            this.newGame.Text = "Új játék";
            this.newGame.Click += new System.EventHandler(this.newGame_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(148, 6);
            // 
            // saveGame
            // 
            this.saveGame.Name = "saveGame";
            this.saveGame.Size = new System.Drawing.Size(151, 22);
            this.saveGame.Text = "Játék mentése";
            this.saveGame.Click += new System.EventHandler(this.saveGame_Click);
            // 
            // loadGame
            // 
            this.loadGame.Name = "loadGame";
            this.loadGame.Size = new System.Drawing.Size(151, 22);
            this.loadGame.Text = "Játék betöltése";
            this.loadGame.Click += new System.EventHandler(this.loadGame_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(148, 6);
            // 
            // exit
            // 
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(151, 22);
            this.exit.Text = "Kilépés";
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(346, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Üzemanyagszint:";
            // 
            // GasBar
            // 
            this.GasBar.Location = new System.Drawing.Point(308, 150);
            this.GasBar.Name = "GasBar";
            this.GasBar.Size = new System.Drawing.Size(169, 23);
            this.GasBar.Step = 1;
            this.GasBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.GasBar.TabIndex = 9;
            this.GasBar.Value = 20;
            // 
            // GameForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(489, 206);
            this.Controls.Add(this.GasBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SpeedUp!";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newGame;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem saveGame;
        private ToolStripMenuItem loadGame;
        private ToolStripSeparator toolStripSeparator2;
        private Label label1;
        private ProgressBar GasBar;
        private ToolStripMenuItem exit;
    }
}