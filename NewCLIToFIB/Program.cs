
using System.CommandLine;

var optionCommand = new Option<FileInfo>("--output","the path output");

var languageOption = new Option<List<string>>("--language", "the language the code or all");

var noteOptions = new Option<FileAttributes>("--note", "The source code is in the comment in the file");

var sortOption = new Option<SortedList<Stream>("--sort","Order of copying the code files");

var removeEmptyLinesOption = new Option<List<StreamReader>>("--remove-empty-lines", "");

var bundle = new Command("bundle", "bundle code to opposite the single page");

bundle.AddOption(optionCommand);
bundle.AddOption(languageOption);
bundle.AddOption(noteOptions);
bundle.AddOption(sortOption);
bundle.AddOption(removeEmptyLinesOption);

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