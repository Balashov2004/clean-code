using System;
using System.IO;
using Markdown.Interfaces;

namespace Markdown;

public static class WorkWithFile
{
    public static string Reader(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch(Exception e)
        {
            Console.WriteLine("Ошибка чтения: " + e.Message);
            throw;
        }
    }

    public static void Writer(string path, string content)
    {
        try
        {
            File.WriteAllText(path, content);
        }
        catch(Exception e)
        {
            Console.WriteLine("Ошибка записи: " + e.Message);
            throw;
        }
    }
}