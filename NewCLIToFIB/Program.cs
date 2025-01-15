﻿
using System.CommandLine;

var optionCommand = new Option<FileInfo>("--output","the path output");

var bundle = new Command("bundle", "bundle code to opposite the single page");

bundle.AddOption(optionCommand);

bundle.SetHandler((output) =>
{
    try
    {
        File.Create(output.FullName);
        Console.WriteLine("succsess!! open a new file");
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine("ERROR: the path invalid! check this.");
    }
},optionCommand);

var rootCommand = new RootCommand("this opposite many page code to signel code");

rootCommand.AddCommand(bundle);

await rootCommand.InvokeAsync(args);