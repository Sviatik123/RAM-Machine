using System;

namespace RAMMachine
{
    class JumpCommand : ICommand
    {
        public CommandType Type { get; set; }
        public string Label { get; set; }

        public void Parse(string line)
        {
            string[] data = line.Split();
            Type = (CommandType)Enum.Parse(typeof(CommandType), data[0]);
            Label = data[1];
        }
        public override string ToString()
        {
            return $"Type: {Type}, Label: {Label}";
        }
    }

}
