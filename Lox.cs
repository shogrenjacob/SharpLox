using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

namespace SharpLox;

public class Lox
{
    static bool hadError = false;
    
    public static void Main(string[] args)
    {
        try
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: SharpLox <filename>");
                Environment.Exit(1);
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Lox.Constructor(): " + e.Message);
        }
    }

    private static void RunFile(string filename)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(filename);
            Run(new string(Encoding.Default.GetChars(bytes)));
            
            if (hadError) { Environment.Exit(2); }
        }
        catch (IOException e)
        {
            Console.WriteLine("Lox.RunFile(): " + e.Message);
        }
    }

    private static void RunPrompt()
    {
        try
        {
            while (true)
            {
                Console.Write("> ");
                string? line = Console.ReadLine();
                
                // Use control + D to exit, sends a null to console.
                if (line == null)
                {
                    break;
                }

                Run(line);
                hadError = false;
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Lox.RunPrompt(): " + e.Message);
        }
    }

    private static void Run(string source)
    {
        string[] tokens = source.Split(' ');

        foreach (string token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    public static void Error(int line, string message)
    {
        report(line, "", message);
    }

    private static void report(int line, string where, string message)
    {
        Console.Error.WriteLine("[ Line " + line + " ] Error " + where + ": " + message);
        hadError = true;
    }
}