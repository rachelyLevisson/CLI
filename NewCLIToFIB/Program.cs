
using System.CommandLine;

var bundle = new Command("bundle", "bundle code to opposite the single page");

bundle.SetHandler(() =>
{
    Console.WriteLine("succses!!");
});

var rootCommand = new RootCommand("this opposite many page code to signel code");

rootCommand.AddCommand(bundle);

rootCommand.InvokeAsync(args);