
using System.CommandLine;

var outputOption = new Option<FileInfo>("--output", "the path output");

var languageOption = new Option<string>("--language", "the language the code or all");

var noteOptions = new Option<bool>("--note", "The source code is in the comment in the file");

var sortOption = new Option<string>("--sort", "Order of copying the code files");

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





bundle.SetHandler((output, language, note, remove, autho) =>
{
    //האם הקיש בכלל את האפשרות של שפה
    if (string.IsNullOrEmpty(language))
    {
        Console.WriteLine("ERROR: --language option is required!");
        return;
    }

    // האם הכניס ניתוב כל שהוא אם לא יכניס ערך ברירת מחדל
    if (string.IsNullOrEmpty(output.ToString()))
    {
        Console.WriteLine("ERROR: --output option is required!");
    }
    else
        output = new FileInfo("text.txt");

    //האם השפה שהוקשה נמצאת ברשימה
    if (language == "all" || Enum.TryParse(language, true, out Elanguage lang))
    {
        try
        {
            File.Create(output.FullName);
            string[] file = Directory.GetFiles(Environment.CurrentDirectory);
            foreach (var item in file)
            {
                File.WriteAllText(output.ToString(),File.ReadAllText(item));
            }
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine("ERROR: the path invalid! check this.");
            ex.Data.Clear();
        }
        //האם הקיש את האופציה ומכילה TRUE
        if (note)
        {
            File.OpenWrite("#" + Environment.CurrentDirectory);
        }
        //האם הקיש את האופציה ומכילה TRUE
        if (remove)
        {
            //delete a empty line!!
        }
        if (string.IsNullOrEmpty(autho.ToString()))
            File.OpenWrite("#" + autho.ToString());
    }
    else
    {
        Console.WriteLine("ERROR: Only code files of the selected languages!");
    }
    Console.WriteLine("succsess!!");
}, outputOption, languageOption, noteOptions, removeEmptyLinesOption, authorOption);



var create_rspCommand = new Command("create-rsp", "Create a response file with a prepared command");

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