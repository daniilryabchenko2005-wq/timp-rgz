using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachineSimulator
{
    public enum MachineStatus
    {
        Ready,
        Running,
        Halted,
        Accepted,
        Error
    }

    public class TuringMachine
    {
        public const int TapeSize = 50;
        public const int MaxSteps = 1000;

        public char[] Tape { get; private set; }
        public int HeadPosition
        {
            get => _headPosition;
            private set => _headPosition = value;
        }
        public string CurrentState { get; private set; }
        public string InitialState { get; private set; }
        public string AcceptState { get; private set; }
        public List<Rule> Rules { get; private set; }
        public int StepCount { get; private set; }
        public MachineStatus Status { get; private set; }
        public string LastError { get; private set; }

        private char[] _initialTape;
        private int _initialHeadPosition;
        private int _headPosition;

        public TuringMachine()
        {
            Tape = new char[TapeSize];
            _initialTape = new char[TapeSize];
            Rules = new List<Rule>();
            ClearTape();
        }

        public void Configure(string initialState, string acceptState)
        {
            InitialState = initialState;
            AcceptState = acceptState;
            CurrentState = initialState;
            Status = MachineStatus.Ready;
            StepCount = 0;
            LastError = string.Empty;
        }

        public void SetTape(string content, int headStart = 0)
        {
            ClearTape();
            for (int i = 0; i < content.Length && i < TapeSize; i++)
                Tape[i] = content[i];

            HeadPosition = headStart;
            Array.Copy(Tape, _initialTape, TapeSize);
            _initialHeadPosition = headStart;
        }

        public void ClearTape()
        {
            for (int i = 0; i < TapeSize; i++)
                Tape[i] = '_';
            HeadPosition = 0;
        }

        public void Reset()
        {
            Array.Copy(_initialTape, Tape, TapeSize);
            HeadPosition = _initialHeadPosition;
            CurrentState = InitialState;
            StepCount = 0;
            Status = MachineStatus.Ready;
            LastError = string.Empty;
        }

        public void SaveCurrentTapeAsInitial()
        {
            Array.Copy(Tape, _initialTape, TapeSize);
            _initialHeadPosition = HeadPosition;
        }

        public bool Step()
        {
            if (Status == MachineStatus.Error)
                return false;

            if (CurrentState == AcceptState)
            {
                Status = MachineStatus.Accepted;
                return false;
            }

            if (StepCount >= MaxSteps)
            {
                Status = MachineStatus.Error;
                LastError = $"Превышен лимит шагов ({MaxSteps}). Возможен бесконечный цикл.";
                return false;
            }

            char symbol = Tape[HeadPosition];
            Rule rule = FindRule(CurrentState, symbol);

            if (rule == null)
            {
                if (CurrentState == AcceptState)
                {
                    Status = MachineStatus.Accepted;
                }
                else
                {
                    Status = MachineStatus.Halted;
                    LastError = $"Правило не найдено для состояния '{CurrentState}' и символа '{symbol}'.";
                }
                return false;
            }

            Tape[HeadPosition] = rule.WriteSymbol;
            CurrentState = rule.NewState;

            if (rule.Direction == 'R')
            {
                if (HeadPosition < TapeSize - 1)
                    HeadPosition++;
            }
            else if (rule.Direction == 'L')
            {
                if (HeadPosition > 0)
                    HeadPosition--;
            }

            StepCount++;

            if (CurrentState == AcceptState)
            {
                Status = MachineStatus.Accepted;
                return false;
            }

            Status = MachineStatus.Running;
            return true;
        }

        private Rule FindRule(string state, char symbol)
        {
            return Rules.FirstOrDefault(r => r.CurrentState == state && r.ReadSymbol == symbol);
        }

        public void AddRule(Rule rule) => Rules.Add(rule);
        public void RemoveRule(Rule rule) => Rules.Remove(rule);
        public void ClearRules() => Rules.Clear();

        public string GetTapeString()
        {
            string result = new string(Tape);
            return result.TrimEnd('_');
        }
    }
}
