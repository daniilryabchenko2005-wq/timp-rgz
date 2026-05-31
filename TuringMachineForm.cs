using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TuringMachineSimulator
{
    public partial class TuringMachineForm : Form
    {
        private TuringMachine _machine;
        private TuringMachineXmlService _xmlService;
        private List<TextBox> _tapeCells;
        private System.Windows.Forms.Timer _autoTimer;

        // Dark theme colors
        private static readonly Color BgColor = Color.FromArgb(30, 30, 30);
        private static readonly Color PanelColor = Color.FromArgb(45, 45, 48);
        private static readonly Color AccentColor = Color.FromArgb(0, 122, 204);
        private static readonly Color TextColor = Color.White;
        private static readonly Color CellColor = Color.FromArgb(60, 60, 60);
        private static readonly Color HeadColor = Color.FromArgb(255, 215, 0);
        private static readonly Color HeadTextColor = Color.Black;
        private static readonly Color AcceptedColor = Color.FromArgb(0, 180, 0);
        private static readonly Color ErrorColor = Color.FromArgb(220, 50, 50);
        private static readonly Font MonoFont = new Font("Consolas", 9f);
        private static readonly Font MonoFontBold = new Font("Consolas", 9f, FontStyle.Bold);
        private static readonly Font TitleFont = new Font("Consolas", 13f, FontStyle.Bold);
        private static readonly Font SubFont = new Font("Consolas", 8f);

        public TuringMachineForm()
        {
            _machine = new TuringMachine();
            _xmlService = new TuringMachineXmlService();
            _tapeCells = new List<TextBox>();
            InitializeComponent();
            SetupAutoTimer();
            AddSaveLoadButtons();
            LoadExample();
            UpdateUI();
        }

        private void SetupAutoTimer()
        {
            _autoTimer = new System.Windows.Forms.Timer();
            _autoTimer.Interval = 150;
            _autoTimer.Tick += (s, e) =>
            {
                bool cont = _machine.Step();
                UpdateTapeDisplay();
                UpdateStatusDisplay();
                if (!cont)
                {
                    _autoTimer.Stop();
                    btnRunAll.Text = "▶▶ Выполнить";
                    ShowMachineResult();
                }
            };
        }

        private void AddSaveLoadButtons()
        {

            btnSave = new Button
            {
                Text = "💾 Сохранить",
                Location = new Point(678, 8),
                Size = new Size(120, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                Font = MonoFontBold
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += btnSave_Click;

            btnLoad = new Button
            {
                Text = "📂 Открыть",
                Location = new Point(807, 8),
                Size = new Size(120, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                Font = MonoFontBold
            };
            btnLoad.FlatAppearance.BorderSize = 0;
            btnLoad.Click += btnLoad_Click;

            panelButtons.Controls.Add(btnSave);
            panelButtons.Controls.Add(btnLoad);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_autoTimer.Enabled)
            {
                _autoTimer.Stop();
                btnRunAll.Text = "▶▶ Выполнить";
            }
            _xmlService.SaveToXml(_machine);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (_autoTimer.Enabled)
            {
                _autoTimer.Stop();
                btnRunAll.Text = "▶▶ Выполнить";
            }

            if (_xmlService.LoadFromXml(_machine, out string error))
            {
                txtInitState.Text = _machine.InitialState ?? "q0";
                txtAcceptState.Text = _machine.AcceptState ?? "qf";
                UpdateTapeDisplay();
                UpdateStatusDisplay();
                RefreshRulesGrid();
                MessageBox.Show("Файл успешно загружен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Ошибка загрузки: {error}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuildTapeDisplay()
        {
            panelTape.Controls.Clear();
            _tapeCells.Clear();

            int cellW = 36, cellH = 36, gap = 2;
            int totalW = TuringMachine.TapeSize * (cellW + gap);
            panelTape.AutoScrollMinSize = new Size(totalW, cellH + 10);

            for (int i = 0; i < TuringMachine.TapeSize; i++)
            {
                var tb = new TextBox
                {
                    Width = cellW,
                    Height = cellH,
                    Location = new Point(i * (cellW + gap) + 4, 8),
                    MaxLength = 1,
                    TextAlign = HorizontalAlignment.Center,
                    BackColor = CellColor,
                    ForeColor = TextColor,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = MonoFontBold,
                    Tag = i
                };
                int idx = i;
                tb.TextChanged += (s, e) =>
                {
                    if (tb.Text.Length == 1)
                        _machine.Tape[idx] = tb.Text[0];
                    else if (tb.Text.Length == 0)
                        _machine.Tape[idx] = '_';
                };
                panelTape.Controls.Add(tb);
                _tapeCells.Add(tb);
            }
        }

        private void UpdateTapeDisplay()
        {
            for (int i = 0; i < TuringMachine.TapeSize; i++)
            {
                var tb = _tapeCells[i];
                tb.TextChanged -= TapeCell_TextChanged;

                tb.Text = _machine.Tape[i].ToString();

                if (i == _machine.HeadPosition)
                {
                    tb.BackColor = HeadColor;
                    tb.ForeColor = HeadTextColor;
                }
                else
                {
                    tb.BackColor = CellColor;
                    tb.ForeColor = TextColor;
                }

                tb.TextChanged += TapeCell_TextChanged;
            }

            int cellW = 38;
            int scrollX = Math.Max(0, _machine.HeadPosition * cellW - panelTape.Width / 2);
            panelTape.AutoScrollPosition = new Point(scrollX, 0);
        }

        private void TapeCell_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            int idx = (int)tb.Tag;
            _machine.Tape[idx] = tb.Text.Length == 1 ? tb.Text[0] : '_';
        }

        private void RefreshRulesGrid()
        {
            dgvRules.Rows.Clear();
            foreach (var r in _machine.Rules)
                dgvRules.Rows.Add(r.CurrentState, r.ReadSymbol.ToString(),
                                  r.NewState, r.WriteSymbol.ToString(), r.Direction.ToString());
        }

        private void UpdateStatusDisplay()
        {
            lblCurrentState.Text = $"Состояние: {_machine.CurrentState}";
            lblHeadPos.Text = $"Позиция: {_machine.HeadPosition}";
            lblStepCount.Text = $"Такт: {_machine.StepCount}";

            switch (_machine.Status)
            {
                case MachineStatus.Accepted:
                    lblStatus.Text = "✔ ПРИНЯТО";
                    lblStatus.ForeColor = AcceptedColor;
                    break;
                case MachineStatus.Halted:
                    lblStatus.Text = "■ ОСТАНОВЛЕНО";
                    lblStatus.ForeColor = ErrorColor;
                    break;
                case MachineStatus.Error:
                    lblStatus.Text = "✖ ОШИБКА";
                    lblStatus.ForeColor = ErrorColor;
                    break;
                case MachineStatus.Running:
                    lblStatus.Text = "► РАБОТАЕТ";
                    lblStatus.ForeColor = AccentColor;
                    break;
                default:
                    lblStatus.Text = "● ГОТОВО";
                    lblStatus.ForeColor = TextColor;
                    break;
            }
        }

        private void UpdateUI()
        {
            UpdateTapeDisplay();
            UpdateStatusDisplay();
            RefreshRulesGrid();

            txtAlphabet.Text = "0,1,_";
            txtStates.Text = "q0,q1,qf";
            txtInitState.Text = _machine.InitialState ?? "q0";
            txtAcceptState.Text = _machine.AcceptState ?? "qf";
        }

        private void ShowMachineResult()
        {
            if (_machine.Status == MachineStatus.Accepted)
            {
                string tape = _machine.GetTapeString();
                MessageBox.Show($"Машина завершила работу успешно!\n\nЛента: {tape}\nТактов: {_machine.StepCount}",
                    "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (_machine.Status == MachineStatus.Halted || _machine.Status == MachineStatus.Error)
            {
                MessageBox.Show(_machine.LastError, "Остановка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnStep_Click(object sender, EventArgs e)
        {

            if (_machine.Status != MachineStatus.Running)
            {
                ApplyConfigToMachine();
            }

            bool cont = _machine.Step();
            UpdateTapeDisplay();
            UpdateStatusDisplay();

            if (!cont)
            {
                ShowMachineResult();
            }
        }

        private void btnRunAll_Click(object sender, EventArgs e)
        {
            if (_autoTimer.Enabled)
            {
                _autoTimer.Stop();
                btnRunAll.Text = "▶▶ Выполнить";
                return;
            }
            ApplyConfigToMachine();
            btnRunAll.Text = "⏸ Пауза";
            _autoTimer.Start();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _autoTimer.Stop();
            btnRunAll.Text = "▶▶ Выполнить";
            ApplyConfigToMachine();
            _machine.Reset();
            UpdateTapeDisplay();
            UpdateStatusDisplay();
        }

        private void btnClearTape_Click(object sender, EventArgs e)
        {
            _autoTimer.Stop();
            btnRunAll.Text = "▶▶ Выполнить";
            _machine.ClearTape();
            UpdateTapeDisplay();
            UpdateStatusDisplay();
        }

        private void btnAddRule_Click(object sender, EventArgs e)
        {
            string cs = txtRuleState.Text.Trim();
            string rs = txtRuleRead.Text.Trim();
            string ns = txtRuleNewState.Text.Trim();
            string ws = txtRuleWrite.Text.Trim();
            string dir = txtRuleDir.Text.Trim().ToUpper();

            if (cs == "" || rs.Length != 1 || ns == "" || ws.Length != 1 ||
                (dir != "L" && dir != "R" && dir != "N"))
            {
                MessageBox.Show("Заполните все поля правила корректно.\nНаправление: L, R или N",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _machine.AddRule(new Rule(cs, rs[0], ns, ws[0], dir[0]));
            RefreshRulesGrid();
        }

        private void btnDeleteRule_Click(object sender, EventArgs e)
        {
            if (dgvRules.SelectedRows.Count == 0) return;
            int idx = dgvRules.SelectedRows[0].Index;
            if (idx >= 0 && idx < _machine.Rules.Count)
            {
                _machine.RemoveRule(_machine.Rules[idx]);
                RefreshRulesGrid();
            }
        }

        private void btnLoadExample_Click(object sender, EventArgs e)
        {
            _autoTimer.Stop();
            btnRunAll.Text = "Выполнить";
            LoadExample();
            UpdateUI();
        }

        private void ApplyConfigToMachine()
        {
            string init = txtInitState.Text.Trim();
            string accept = txtAcceptState.Text.Trim();
            if (init == "")
                init = "q0";
            if (accept == "")
                accept = "qf";
            _machine.Configure(init, accept);
        }

        private void LoadExample()
        {
            _machine.ClearRules();

            _machine.AddRule(new Rule("q0", '0', "q0", '0', 'R'));
            _machine.AddRule(new Rule("q0", '1', "q0", '1', 'R'));
            _machine.AddRule(new Rule("q0", '_', "q1", '_', 'L'));

            _machine.AddRule(new Rule("q1", '0', "qf", '1', 'N'));
            _machine.AddRule(new Rule("q1", '1', "q1", '0', 'L'));
            _machine.AddRule(new Rule("q1", '_', "qf", '1', 'N'));

            _machine.Configure("q0", "qf");
            _machine.SetTape("10111111", 0);
            _machine.SaveCurrentTapeAsInitial();

            txtAlphabet.Text = "0,1,_";
            txtStates.Text = "q0,q1,qf";
            txtInitState.Text = "q0";
            txtAcceptState.Text = "qf";
        }
    }
}
