
using System;
using System.Globalization;
using System.IO;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = File.ReadAllText("resurses/input.txt");
            new Md(path);
        }
    }
};
