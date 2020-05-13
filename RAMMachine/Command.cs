using System;

namespace RAMMachine
{
    class Command : ICommand
    {
        public CommandType Type { get; set; }
        public int Operand { get; set; }
        public OperandType OpType { get; set; }
        public Command()
        {

        }
        public Command(CommandType type, int operand, OperandType opType)
        {
            Type = type;
            Operand = operand;
            OpType = opType;
        }
        public void Parse(string line)
        {
            string[] data = line.Replace("\r", "").Split();
            Type = (CommandType)Enum.Parse(typeof(CommandType), data[0]);
            if (data.Length > 1)
            {
                if (data[1].Contains("="))
                {
                    OpType = OperandType.Number;
                    Operand = int.Parse(data[1].Replace("=", ""));
                }
                else if (data[1].Contains("*"))
                {
                    OpType = OperandType.Address;
                    Operand = int.Parse(data[1].Replace("*", ""));
                }
                else
                {
                    OpType = OperandType.Register;
                    Operand = int.Parse(data[1]);
                }
            }
        }
        public override string ToString()
        {
            return $"Type: {Type}, Operand: {Operand}, Operand Type: {OpType}";
        }
    }

}
