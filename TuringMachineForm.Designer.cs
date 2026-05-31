using System.Drawing;
using System.Windows.Forms;

namespace TuringMachineSimulator
{
    partial class TuringMachineForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private Panel panelHeader;
        private Label lblTitle;
        private Label lblSubtitle;

        private Panel panelTape;
        private Label lblTapeHeader;

        private Panel panelConfig;
        private Label lblAlphabet;
        private TextBox txtAlphabet;
        private Label lblStates;
        private TextBox txtStates;
        private Label lblInitState;
        private TextBox txtInitState;
        private Label lblAcceptState;
        private TextBox txtAcceptState;

        private Panel panelStatus;
        private Label lblCurrentState;
        private Label lblHeadPos;
        private Label lblStepCount;
        private Label lblStatus;

        private Panel panelButtons;
        private Button btnStep;
        private Button btnRunAll;
        private Button btnReset;
        private Button btnClearTape;
        private Button btnLoadExample;
        private Button btnSave;
        private Button btnLoad;

        private Panel panelRules;
        private Label lblRulesHeader;
        private DataGridView dgvRules;
        private Panel panelAddRule;
        private Label lblRuleState;
        private TextBox txtRuleState;
        private Label lblRuleRead;
        private TextBox txtRuleRead;
        private Label lblRuleNewState;
        private TextBox txtRuleNewState;
        private Label lblRuleWrite;
        private TextBox txtRuleWrite;
        private Label lblRuleDir;
        private TextBox txtRuleDir;
        private Button btnAddRule;
        private Button btnDeleteRule;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // ── Form ──────────────────────────────────────────────────
            this.Text = "Разработка программы-имитатора машины Тьюринга";
            this.Size = new Size(1200, 820);
            this.MinimumSize = new Size(1000, 720);
            this.BackColor = BgColor;
            this.ForeColor = TextColor;
            this.Font = MonoFont;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ── Header ────────────────────────────────────────────────
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(20, 20, 20),
                Padding = new Padding(12, 8, 12, 8)
            };

            lblTitle = new Label
            {
                Text = "Разработка программы-имитатора машины Тьюринга",
                Font = TitleFont,
                ForeColor = AccentColor,
                AutoSize = true,
                Location = new Point(12, 8)
            };

            lblSubtitle = new Label
            {
                Text = "Mammut  |  Algorithm  |  Programmer of simple arithmetic operations",
                Font = SubFont,
                ForeColor = Color.FromArgb(160, 160, 160),
                AutoSize = true,
                Location = new Point(14, 38)
            };

            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblSubtitle);

            // ── Tape panel ────────────────────────────────────────────
            lblTapeHeader = new Label
            {
                Text = "ЛЕНТА",
                Font = MonoFontBold,
                ForeColor = AccentColor,
                AutoSize = true,
                Location = new Point(8, 76)
            };

            panelTape = new Panel
            {
                Location = new Point(8, 96),
                Size = new Size(1160, 58),
                BackColor = Color.FromArgb(20, 20, 20),
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            // ── Status panel ──────────────────────────────────────────
            panelStatus = new Panel
            {
                Location = new Point(8, 164),
                Size = new Size(1160, 36),
                BackColor = PanelColor
            };

            lblCurrentState = new Label
            {
                Text = "Состояние: q0",
                Font = MonoFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(8, 8)
            };

            lblHeadPos = new Label
            {
                Text = "Позиция: 0",
                Font = MonoFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(200, 8)
            };

            lblStepCount = new Label
            {
                Text = "Такт: 0",
                Font = MonoFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(360, 8)
            };

            lblStatus = new Label
            {
                Text = "● ГОТОВО",
                Font = MonoFontBold,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(520, 8)
            };

            panelStatus.Controls.AddRange(new Control[]
                { lblCurrentState, lblHeadPos, lblStepCount, lblStatus });

            // ── Buttons panel ─────────────────────────────────────────
            panelButtons = new Panel
            {
                Location = new Point(8, 208),
                Size = new Size(1160, 52),
                BackColor = BgColor
            };

            // Создаём кнопки с уменьшенной шириной (120px)
            btnStep = CreateButton("⏭ Шаг", 0);
            btnRunAll = CreateButton("▶▶ Выполнить", 1);
            btnReset = CreateButton("↺ Сброс", 2);
            btnClearTape = CreateButton("⌫ Очистить", 3);
            btnLoadExample = CreateButton("★ Загрузить пример", 4);
            btnLoadExample.Width = 150;
            btnLoadExample.BackColor = Color.FromArgb(0, 80, 140);

            btnStep.Click += btnStep_Click;
            btnRunAll.Click += btnRunAll_Click;
            btnReset.Click += btnReset_Click;
            btnClearTape.Click += btnClearTape_Click;
            btnLoadExample.Click += btnLoadExample_Click;

            panelButtons.Controls.AddRange(new Control[]
                { btnStep, btnRunAll, btnReset, btnClearTape, btnLoadExample });

            // ── Config panel ──────────────────────────────────────────
            panelConfig = new Panel
            {
                Location = new Point(8, 268),
                Size = new Size(1160, 50),
                BackColor = PanelColor
            };

            int cx = 8;
            lblAlphabet = MakeLabel("Алфавит:", cx, 14); cx += 80;
            txtAlphabet = MakeTextBox(cx, 10, 160); cx += 168;
            lblStates = MakeLabel("Состояния:", cx, 14); cx += 90;
            txtStates = MakeTextBox(cx, 10, 200); cx += 208;
            lblInitState = MakeLabel("Нач.сост.:", cx, 14); cx += 88;
            txtInitState = MakeTextBox(cx, 10, 80); cx += 88;
            lblAcceptState = MakeLabel("Доп.сост.:", cx, 14); cx += 88;
            txtAcceptState = MakeTextBox(cx, 10, 80);

            panelConfig.Controls.AddRange(new Control[]
            {
                lblAlphabet, txtAlphabet, lblStates, txtStates,
                lblInitState, txtInitState, lblAcceptState, txtAcceptState
            });

            // ── Rules panel ───────────────────────────────────────────
            panelRules = new Panel
            {
                Location = new Point(8, 326),
                Size = new Size(1160, 440),
                BackColor = PanelColor
            };

            lblRulesHeader = new Label
            {
                Text = "ТАБЛИЦА ПРАВИЛ ПЕРЕХОДОВ",
                Font = MonoFontBold,
                ForeColor = AccentColor,
                AutoSize = true,
                Location = new Point(8, 6)
            };

            dgvRules = new DataGridView
            {
                Location = new Point(8, 28),
                Size = new Size(820, 400),
                BackgroundColor = Color.FromArgb(40, 40, 40),
                ForeColor = TextColor,
                GridColor = Color.FromArgb(80, 80, 80),
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = MonoFont,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvRules.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 140);
            dgvRules.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            dgvRules.ColumnHeadersDefaultCellStyle.Font = MonoFontBold;
            dgvRules.DefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dgvRules.DefaultCellStyle.ForeColor = TextColor;
            dgvRules.DefaultCellStyle.SelectionBackColor = AccentColor;
            dgvRules.DefaultCellStyle.SelectionForeColor = TextColor;
            dgvRules.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);

            dgvRules.Columns.Add("CurState", "Тек. состояние");
            dgvRules.Columns.Add("ReadSym", "Читаемый символ");
            dgvRules.Columns.Add("NewState", "Новое состояние");
            dgvRules.Columns.Add("WriteSym", "Записываемый символ");
            dgvRules.Columns.Add("Dir", "Направление");

            // Add rule form
            panelAddRule = new Panel
            {
                Location = new Point(836, 28),
                Size = new Size(316, 400),
                BackColor = Color.FromArgb(35, 35, 35)
            };

            int ry = 10;
            lblRuleState = MakeLabel("Тек. состояние:", 8, ry); ry += 22;
            txtRuleState = MakeTextBox(8, ry, 200); ry += 32;
            lblRuleRead = MakeLabel("Читаемый символ:", 8, ry); ry += 22;
            txtRuleRead = MakeTextBox(8, ry, 60); ry += 32;
            lblRuleNewState = MakeLabel("Новое состояние:", 8, ry); ry += 22;
            txtRuleNewState = MakeTextBox(8, ry, 200); ry += 32;

            lblRuleWrite = MakeLabel("Записываемый символ:", 8, ry); ry += 22;
            txtRuleWrite = MakeTextBox(8, ry, 60); ry += 32;

            lblRuleDir = MakeLabel("Направление (L/R/N):", 8, ry); ry += 22;
            txtRuleDir = MakeTextBox(8, ry, 60); ry += 40;

            btnAddRule = new Button
            {
                Text = "+ Добавить правило",
                Location = new Point(8, ry),
                Size = new Size(200, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 100, 0),
                ForeColor = Color.White,
                Font = MonoFontBold
            };
            btnAddRule.FlatAppearance.BorderSize = 0;
            ry += 40;

            btnDeleteRule = new Button
            {
                Text = "🗑 Удалить выбранное",
                Location = new Point(8, ry),
                Size = new Size(200, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(139, 0, 0),
                ForeColor = Color.White,
                Font = MonoFontBold
            };
            btnDeleteRule.FlatAppearance.BorderSize = 0;

            btnAddRule.Click += btnAddRule_Click;
            btnDeleteRule.Click += btnDeleteRule_Click;

            panelAddRule.Controls.AddRange(new Control[]
            {
                lblRuleState, txtRuleState,
                lblRuleRead, txtRuleRead,
                lblRuleNewState, txtRuleNewState,
                lblRuleWrite, txtRuleWrite,
                lblRuleDir, txtRuleDir,
                btnAddRule, btnDeleteRule
            });

            panelRules.Controls.Add(lblRulesHeader);
            panelRules.Controls.Add(dgvRules);
            panelRules.Controls.Add(panelAddRule);

            // ── Добавление всех элементов на форму ────────────────────
            this.Controls.Add(panelRules);
            this.Controls.Add(panelConfig);
            this.Controls.Add(panelButtons);
            this.Controls.Add(panelStatus);
            this.Controls.Add(panelTape);
            this.Controls.Add(lblTapeHeader);
            this.Controls.Add(panelHeader);

            this.ResumeLayout(false);
            this.PerformLayout();

            // Инициализация графики ленты
            BuildTapeDisplay();
        }

        private Button CreateButton(string text, int index)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(8 + (index * 128), 8),
                Size = new Size(120, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                Font = MonoFontBold
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private Label MakeLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.FromArgb(200, 200, 200),
                Font = MonoFont
            };
        }

        private TextBox MakeTextBox(int x, int y, int width)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Width = width,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = MonoFont
            };
        }
    }
}