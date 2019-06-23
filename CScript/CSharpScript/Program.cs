//Copyright (c) 2019 plasma_effect
//This source code is under MIT License. See ./LICENSE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRegex;

namespace CSharpScript
{
    static class Program
    {
        static IRegex fullCommandRegex = Predefined.String.Create("--") + (+Atomic.Create(_ => true));
        static IRegex shortCommandRegex = Predefined.String.Create("-") + (+Atomic.Create(_ => true));
        static string CheckFullCommand(string str)
        {
            if (fullCommandRegex.Match(str))
            {
                return str.Substring(2);
            }
            else
            {
                return null;
            }
        }
        static string CheckShortCommand(string str)
        {
            if (shortCommandRegex.Match(str))
            {
                return str.Substring(1);
            }
            else
            {
                return null;
            }
        }
        static class FullCommands
        {
            static public bool Help(ExecuteInformation information)
            {
                Console.WriteLine(Resource1.HelpInformation);
                return true;
            }
            static public bool Version(ExecuteInformation information)
            {
                Console.WriteLine(Resource1.VersionInformation);
                return true;
            }
            static public bool Time(ExecuteInformation information)
            {
                information.Time = true;
                return false;
            }
            static public bool Function(ExecuteInformation information)
            {
                Console.WriteLine(Resource1.FunctionsInformation);
                return true;
            }
        }
        static SortedDictionary<string, Func<ExecuteInformation, bool>> fullCommands;
        static SortedDictionary<string, Action<ExecuteInformation, string>> shortCommands;
        static Program()
        {
            fullCommands = new SortedDictionary<string, Func<ExecuteInformation, bool>>
            {
                ["version"] = FullCommands.Version,
                ["help"] = FullCommands.Help,
                ["time"] = FullCommands.Time,
                ["function"] = FullCommands.Function
            };
            shortCommands = new SortedDictionary<string, Action<ExecuteInformation, string>>();
        }
        static void Main(string[] args)
        {
            var information = new ExecuteInformation
            {
                Time = false
            };
            string target = null;
            for (var i = 0; i < args.Length; ++i)
            {
                if (CheckFullCommand(args[i]) is string fullCommand)
                {
                    if (fullCommands.TryGetValue(fullCommand, out var func))
                    {
                        if(func(information))
                        {
                            return;
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine($"--{fullCommand} is not command");
                        return;
                    }
                }
                else if (CheckShortCommand(args[i]) is string shortCommand)
                {
                    if (shortCommands.TryGetValue(shortCommand, out var func))
                    {
                        ++i;
                        if (i < args.Length)
                        {
                            func(information, args[i]);
                        }
                        else
                        {
                            Console.Error.WriteLine($"-{shortCommand} have no argument");
                            return;
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine($"-{shortCommand} is not command");
                    }
                }
                else
                {
                    target = args[i];
                }
            }
            if (target is null)
            {
                Console.Error.WriteLine("CScript: fatal error: no input files");
                return;
            }
            Script.Run(target, information);
        }
    }
}
