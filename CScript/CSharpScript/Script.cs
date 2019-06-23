//Copyright (c) 2019 plasma_effect
//This source code is under MIT License. See ./LICENSE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace CSharpScript
{
    public static class Script
    {
        public static void Run(string target, ExecuteInformation information)
        {
            string source;
            using (var stream = new StreamReader(target))
            {
                source = stream.ReadToEnd();
            }
            var stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.RunAsync(source, globals: new PredefinedFunctions(
                    Console.ReadLine,
                    Console.Write,
                    Console.WriteLine,
                    Console.Error.WriteLine)).Wait();
                stopwatch.Stop();
                if (information.Time)
                {
                    Console.WriteLine($"実行時間 {TimeString(stopwatch.ElapsedMilliseconds)}");
                }
            }
            catch (Exception exp)
            {
                stopwatch.Stop();
                Console.Error.WriteLine(exp.Message);
                if (information.Time)
                {
                    Console.Error.WriteLine($"異常終了しました: 実行時間 {TimeString(stopwatch.ElapsedMilliseconds)}");
                }
                else
                {
                    Console.Error.WriteLine("異常終了しました");
                }
            }
        }

        public static string TimeString(long time)
        {
            return $"{time / 1000}.{time % 1000:000}sec";
        }
    }
}
