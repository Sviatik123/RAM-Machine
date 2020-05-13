using System.Collections.Generic;
using System.IO;
using System;

namespace RAMMachine
{
    class Machine
    {
        public List<int> InputString { get; set; } = new List<int>();
        public List<int> OutputString { get; set; } = new List<int>();
        public int CommandCounter { get; set; }
        public Dictionary<int, int> Registers { get; set; } = new Dictionary<int, int>();
        public List<ICommand> Commands { get; set; } = new List<ICommand>();
        public int InputStringPosition { get; set; }
        public Dictionary<string, int> Labels { get; set; } = new Dictionary<string, int>();

        public void ParseCommands(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string[] lines = sr.ReadToEnd().Replace("\r", "").Replace("\n\n", "\n").Split('\n');
                int rowDif = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(":"))
                    {
                        Labels[lines[i].Substring(0, lines[i].Length - 1)] = i - rowDif;
                        rowDif++;
                    }
                    else
                    {
                        if (lines[i].Contains("JUMP") || lines[i].Contains("JZERO") || lines[i].Contains("JGTZ"))
                        {
                            JumpCommand command = new JumpCommand();
                            command.Parse(lines[i]);
                            Commands.Add(command);
                        }
                        else
                        {
                            Command command = new Command();
                            command.Parse(lines[i]);
                            Commands.Add(command);
                        }                   
                    }
                }
            }
        }
        public void Run(string inputString)
        {
            ResetMachine();
            FormInputString(inputString);
            bool isEnd = false;
            while (!isEnd)
            {
                switch (Commands[CommandCounter].Type)
                {
                    case CommandType.READ:
                        switch (((Command)Commands[CommandCounter]).OpType)
                        {
                            case OperandType.Register:
                                Registers[((Command)Commands[CommandCounter]).Operand] = InputString[InputStringPosition];
                                InputStringPosition++;
                                break;
                            case OperandType.Address:
                            case OperandType.Number:
                            default:
                                throw new Exception("Invalid Command");
                        }
                        break;
                    case CommandType.WRITE:
                        switch (((Command)Commands[CommandCounter]).OpType)
                        {
                            case OperandType.Number:
                                OutputString.Add(((Command)Commands[CommandCounter]).Operand);
                                break;
                            case OperandType.Register:
                                OutputString.Add(Registers[((Command)Commands[CommandCounter]).Operand]);
                                break;
                            case OperandType.Address:
                                OutputString.Add(Registers[Registers[((Command)Commands[CommandCounter]).Operand]]);
                                break;
                            default:
                                throw new Exception("Invalid Command");
                        }
                        break;
                    case CommandType.LOAD:
                        switch (((Command)Commands[CommandCounter]).OpType)
                        {
                            case OperandType.Number:
                                Registers[0] = ((Command)Commands[CommandCounter]).Operand;
                                break;
                            case OperandType.Register:
                                Registers[0] = Registers[((Command)Commands[CommandCounter]).Operand];
                                break;
                            case OperandType.Address:
                                Registers[0] = Registers[Registers[((Command)Commands[CommandCounter]).Operand]];
                                break;
                            default:
                                throw new Exception("Invalid Command");
                        }
                        break;
                    case CommandType.STORE:
                        switch (((Command)Commands[CommandCounter]).OpType)
                        {
                            case OperandType.Register:
                                Registers[((Command)Commands[CommandCounter]).Operand] = Registers[0];
                                break;
                            case OperandType.Address:
                                Registers[Registers[((Command)Commands[CommandCounter]).Operand]] = Registers[0];
                                break;
                            case OperandType.Number:
                            default:
                                throw new Exception("Invalid Command");
                        }
                        break;
                    case CommandType.ADD:
                        switch (((Command)Commands[CommandCounter]).OpType)
                        {
                            case OperandType.Number:
                                Registers[0] += ((Command)Commands[CommandCounter]).Operand;
                                break;
                            case OperandType.Register:
                                Registers[0] += Registers[((Command)Commands[CommandCounter]).Operand];
                                break;
                            case OperandType.Address:
                                Registers[0] += Registers[Registers[((Command)Commands[CommandCounter]).Operand]];
                                break;
                            default:
                                throw new Exception("Invalid Command");
                        }
                        break;
                    case CommandType.SUB:
                        switch (((Command)Commands[CommandCounter]).OpType)
                        {
                            case OperandType.Number:
                                Registers[0] -= ((Command)Commands[CommandCounter]).Operand;
                                break;
                            case OperandType.Register:
                                Registers[0] -= Registers[((Command)Commands[CommandCounter]).Operand];
                                break;
                            case OperandType.Address:
                                Registers[0] -= Registers[Registers[((Command)Commands[CommandCounter]).Operand]];
                                break;
                            default:
                                throw new Exception("Invalid Command");
                        }
                        break;
                    case CommandType.MULT:
                        switch (((Command)Commands[CommandCounter]).OpType)
                        {
                            case OperandType.Number:
                                Registers[0] *= ((Command)Commands[CommandCounter]).Operand;
                                break;
                            case OperandType.Register:
                                Registers[0] *= Registers[((Command)Commands[CommandCounter]).Operand];
                                break;
                            case OperandType.Address:
                                Registers[0] *= Registers[Registers[((Command)Commands[CommandCounter]).Operand]];
                                break;
                            default:
                                throw new Exception("Invalid Command");
                        }
                        break;
                    case CommandType.DIV:
                        switch (((Command)Commands[CommandCounter]).OpType)
                        {
                            case OperandType.Number:
                                Registers[0] /= ((Command)Commands[CommandCounter]).Operand;
                                break;
                            case OperandType.Register:
                                Registers[0] /= Registers[((Command)Commands[CommandCounter]).Operand];
                                break;
                            case OperandType.Address:
                                Registers[0] /= Registers[Registers[((Command)Commands[CommandCounter]).Operand]];
                                break;
                            default:
                                throw new Exception("Invalid Command");
                        }
                        break;
                    case CommandType.JUMP:
                        CommandCounter = Labels[((JumpCommand)Commands[CommandCounter]).Label] - 1;
                        break;
                    case CommandType.JZERO:
                        if(Registers[0] == 0)
                        {
                            CommandCounter = Labels[((JumpCommand)Commands[CommandCounter]).Label] - 1;
                        }
                        break;
                    case CommandType.JGTZ:
                        if (Registers[0] > 0)
                        {
                            CommandCounter = Labels[((JumpCommand)Commands[CommandCounter]).Label] - 1;
                        }
                        break;
                    case CommandType.HALT:
                        isEnd = true;
                        break;
                    default:
                        throw new Exception("Invalid Command");
                }
                CommandCounter++;
            }
        }

        private void FormInputString(string inputString)
        {
            string[] elements = inputString.Split();
            foreach (var el in elements)
            {
                InputString.Add(int.Parse(el));
            }
        }
        public void PrintResult()
        {
            foreach (var el in OutputString)
            {
                Console.Write(el + " ");
            }
            Console.WriteLine("\n");
        }
        
        public void PrintRegisters()
        {
            foreach (var item in Registers)
            {
                Console.WriteLine($"{item.Key} - {item.Value}");
            }
            Console.WriteLine("\n");
        }

        private void ResetMachine()
        {
            InputString.Clear();
            OutputString.Clear();
            CommandCounter = 0;
            Registers.Clear();
            InputStringPosition = 0;
        }
    }
}
