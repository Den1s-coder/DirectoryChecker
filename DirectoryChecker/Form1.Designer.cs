using System.Windows.Forms;
using System.Drawing;

namespace DirectoryChecker
{
    partial class Form1 : Form  // Додано успадкування від Form
    {
        private System.ComponentModel.IContainer components = null;

        // Оголошуємо елементи керування як public, щоб вони були доступні з Form1.cs
        public Button btnAddFolder;
        public Button btnRemoveFolder;
        public ListBox lstLogs;
        public ListBox lstFolders;

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
            this.components = new System.ComponentModel.Container();

            // Ініціалізація компонентів
            this.btnAddFolder = new Button();
            this.btnRemoveFolder = new Button();
            this.lstLogs = new ListBox();
            this.lstFolders = new ListBox();

            // Налаштування форми
            this.Text = "Моніторинг папок";
            this.Size = new Size(800, 600);

            // Налаштування кнопки додавання
            this.btnAddFolder.Location = new Point(10, 10);
            this.btnAddFolder.Size = new Size(100, 23);
            this.btnAddFolder.Text = "Додати папку";
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);

            // Налаштування кнопки видалення
            this.btnRemoveFolder.Location = new Point(120, 10);
            this.btnRemoveFolder.Size = new Size(100, 23);
            this.btnRemoveFolder.Text = "Видалити папку";
            this.btnRemoveFolder.Click += new System.EventHandler(this.btnRemoveFolder_Click);

            // Налаштування списку папок
            this.lstFolders.Location = new Point(10, 40);
            this.lstFolders.Size = new Size(300, 200);

            // Налаштування списку логів
            this.lstLogs.Location = new Point(10, 250);
            this.lstLogs.Size = new Size(770, 300);

            // Додавання контролів на форму
            this.Controls.AddRange(new Control[] {
                this.btnAddFolder,
                this.btnRemoveFolder,
                this.lstFolders,
                this.lstLogs
            });
        }
    }
}
