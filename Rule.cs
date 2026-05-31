namespace TuringMachineSimulator
{
    public class Rule
    {
        public string CurrentState { get; set; }
        public char ReadSymbol { get; set; }
        public string NewState { get; set; }
        public char WriteSymbol { get; set; }
        // 'L', 'R', 'N'.
        public char Direction { get; set; }

        public Rule(string currentState, char readSymbol,
                    string newState, char writeSymbol, char direction)
        {
            CurrentState = currentState;
            ReadSymbol = readSymbol;
            NewState = newState;
            WriteSymbol = writeSymbol;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"({CurrentState}, {ReadSymbol}) → ({NewState}, {WriteSymbol}, {Direction})";
        }
    }
}
