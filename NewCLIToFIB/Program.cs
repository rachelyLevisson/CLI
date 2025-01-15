
using System.CommandLine;

var outputOption = new Option<FileInfo>("--output","the path output");

var languageOption = new Option<string>("--language", "the language the code or all");

var noteOptions = new Option<bool>("--note", "The source code is in the comment in the file");

var sortOption = new Option<string>("--sort","Order of copying the code files");

var removeEmptyLinesOption = new Option<bool>("--remove-empty-lines", "Delete empty rows");

var authorOption = new Option<string>("--author", "Register the name of the file creator");

var bundle = new Command("bundle", "bundle code to opposite the single page");

bundle.AddOption(languageOption);
bundle.AddOption(outputOption);
bundle.AddOption(noteOptions);
bundle.AddOption(sortOption);
bundle.AddOption(removeEmptyLinesOption);
bundle.AddOption(authorOption);

Console.WriteLine("the name option!!");
foreach (var option in bundle.Options)
{
    Console.WriteLine(option.Name);
}
Console.WriteLine();

bundle.SetHandler((output, language, note ,remove, autho) =>
{
    try
    {
        if (string.IsNullOrEmpty(language))
        {
            Console.WriteLine("ERROR: --language option is required!");
            return;
        }

        if (string.IsNullOrEmpty(output.ToString()))
        {
            Console.WriteLine("ERROR: --output option is required!");
            return;
        }

        if (language == "all" || Enum.TryParse(language, true, out Elanguage lang))
        {
            File.Create(output.FullName);
            if (note)
            {
                File.OpenWrite("//" + Environment.CurrentDirectory);
            }
            if (remove)
            {
                //delete a empty line!!
            }
            File.OpenWrite("//" + autho.ToString());
        }
        else
        {
            Console.WriteLine("ERROR: Only code files of the selected languages!");
        }
        Console.WriteLine("succsess!!");
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine("ERROR: the path invalid! check this.");
        ex.Data.Clear();
    }
}, outputOption, languageOption, noteOptions, removeEmptyLinesOption, authorOption);



//var create_rspCommand = new Command("create-rsp", "Create a response file with a prepared command");

//create_rspCommand.SetHandler(() =>
//{
//    Console.WriteLine("create_rspCommand new!!!");
//});

var rootCommand = new RootCommand("this opposite many page code to signel code");
rootCommand.AddCommand(bundle);


//rootCommand.AddCommand(create_rspCommand);
Console.WriteLine("Arguments count: " + args.Length);
foreach (var arg in args)
{
    Console.WriteLine("Argument: " + arg);
}

await rootCommand.InvokeAsync(args);