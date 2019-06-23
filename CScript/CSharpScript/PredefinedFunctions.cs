//Copyright (c) 2019 plasma_effect
//This source code is under MIT License. See ./LICENSE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSharpScript
{
    public class PredefinedFunctions
    {
        Func<string> input;
        Action<string> cout;
        Action<string> coutEndl;
        Action<string> message;

        public PredefinedFunctions(Func<string> input, Action<string> cout, Action<string> coutEndl, Action<string> message)
        {
            this.input = input;
            this.cout = cout;
            this.coutEndl = coutEndl;
            this.message = message;
        }
        #region output
        public void Write(object obj)
        {
            this.cout(obj.ToString());
        }
        public void WriteLine()
        {
            this.coutEndl("");
        }
        public void WriteLine(string str)
        {
            this.coutEndl(str);
        }
        public void WriteLine(object obj)
        {
            this.coutEndl(obj.ToString());
        }
        public void WriteLine<T>(IEnumerable<T> ts)
        {
            using (var ite = ts.GetEnumerator())
            {
                if (ite.MoveNext())
                {
                    Write(ite.Current.ToString());
                    while (ite.MoveNext())
                    {
                        Write(" ");
                        Write(ite.Current.ToString());
                    }
                }
            }
            this.coutEndl("");
        }
        public void ErrorMesssage(string str)
        {
            this.message(str);
        }
        #endregion
        #region input
        public string ReadLine()
        {
            return this.input();
        }
        public T[] ReadArray<T>(Func<string, T> parser)
        {
            return this.input().Split(' ').Select(parser).ToArray();
        }
        public string FileRead(string filename)
        {
            using (var stream = new StreamReader(filename))
            {
                return stream.ReadToEnd();
            }
        }
        #endregion
        #region utility
        public IEnumerable<int> Range(int min, int max, int step = 1)
        {
            for (var i = min; i < max; i += step)
            {
                yield return i;
            }
        }
        public IEnumerable<long> Range(long min, long max, long step = 1)
        {
            for (var i = min; i < max; i += step)
            {
                yield return i;
            }
        }
        public IEnumerable<int> Range(int max)
        {
            return Range(default, max);
        }
        public IEnumerable<long> Range(long max)
        {
            return Range(default, max);
        }
        #endregion
    }
}