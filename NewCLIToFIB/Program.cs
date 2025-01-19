
using System.CommandLine;

var outputOption = new Option<FileInfo>(
 new[] { "--output", "-o" },
  "the path output"
    );


//var languageOption = new Option<string>("--language", "the language the code or all");

var languageOption = new Option<string>(
    new[] { "--language", "-l" }, // Define both the long and short options
    "the language the code or all"
);

var noteOptions = new Option<bool>("--note", "The source code is in the comment in the file");

noteOptions.AddAlias("-n");

var sortOption = new Option<string>("--sort", "Order of copying the code files");

sortOption.AddAlias("-s");
sortOption.SetDefaultValue("abc");

var removeEmptyLinesOption = new Option<bool>("--remove-empty-lines", "Delete empty rows");

removeEmptyLinesOption.AddAlias("-rem");

var authorOption = new Option<string>("--author", "Register the name of the file creator");

authorOption.AddAlias("-a");

var bundle = new Command("bundle", "bundle code to opposite the single page");



if (languageOption.Name == null | languageOption.Name == "")
{
    Console.WriteLine("Error: invalid!! enter language!!!");
    return;
}

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
Console.WriteLine("come!!");



bundle.SetHandler((output, language, note, sort, remove, autho) =>
{
    Console.WriteLine("come to  SetHandler!!");
    try
    {
        //      האם הקיש בכלל את האפשרות של שפה
        if (string.IsNullOrEmpty(language))
        {
            Console.WriteLine("ERROR: --language option is required!");
            return;
        }
        if (output.Name[0] == '-')
        {
            Console.WriteLine("ERROR: --output option is required!");
            return;
        }

        Console.WriteLine("over?");
        if (language == "all" || Enum.TryParse(language, true, out Elanguage lang))
        {
            try
            {
                string path = output.FullName;
                using (FileStream fs = File.Create(path)) { }
                string[] file = Directory.GetFiles(Environment.CurrentDirectory);

                //sort
                if (!string.IsNullOrEmpty(sort))
                {
                    if(sort == "abc")
                    {
                        file = file.OrderBy(f => Path.GetFileName(f)).ToArray();
                    }
                    //type code
                    else
                    {
                        file = file.OrderBy(f=> Path.GetExtension(f)).ToArray();
                    }
                }
                

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    //    האם הקיש את האופציה ומכילה TRUE
                    if (note)
                    {
                        Console.WriteLine("i enter to note!!");
                        writer.WriteLine("# " + Environment.CurrentDirectory);
                        writer.WriteLine();
                    }

                    if (!string.IsNullOrEmpty(autho))
                    {
                        if (autho == "")
                            Console.WriteLine("ERRORRRRRRRR");
                        Console.WriteLine("i enter to author!!");
                        writer.WriteLine("#" + autho);
                    }
                    else
                    Console.WriteLine("ERROR: this dont author!!");
                    foreach (var item in file)
                    {
                        Console.WriteLine(item);
                        writer.WriteLine("succssess!! towrite");
                        writer.WriteLine(item);
                        if (item != output.FullName)
                        {
                            using (StreamReader sr = new StreamReader(item))
                            {
                                writer.WriteLine(sr.ReadToEnd());
                            }
                        }
                    }
                    //האם הקיש את האופציה ומכילה TRUE
                }
                Console.WriteLine("come?");

                if (remove)
                {
                    Console.WriteLine("i enter to remove!!");
                    var lines = File.ReadAllLines(path);

                    // Filter out empty lines
                    var nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

                    // Write the non-empty lines back to the file
                    File.WriteAllLines(path, nonEmptyLines);
                    Console.WriteLine("i do this!!");
                }
            }

            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("ERROR: the path invalid! check this.");
                ex.Data.Clear();
            }
            catch (IOException e)
            {
                Console.WriteLine("Worng!!");
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Error: in delete line");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            { 
                Console.WriteLine("come this");
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("finally!");
            }
            Console.WriteLine("sucssess!!! yesh! yesh! ♥️🎁💥🍔😜");
        }
        else
            Console.WriteLine("ERROR: Only code files of the selected languages!");
        Console.WriteLine("succsess!! 😆😂😃🙌🏻");
    }
    catch (NullReferenceException e)
    {
        Console.WriteLine("ERROR: not enter the path!!");
        Console.WriteLine(e.Message);
    }
}, outputOption, languageOption, noteOptions,sortOption, removeEmptyLinesOption, authorOption);

#region //
//var create_rspCommand = new Command("create-rsp", "Create a response file with a prepared command");

//create_rspCommand.SetHandler(() =>
//{
//    Console.WriteLine("create_rspCommand new!!!");
//});
#endregion

var rootCommand = new RootCommand("this opposite many page code to signel code");
rootCommand.AddCommand(bundle);


//rootCommand.AddCommand(create_rspCommand);
Console.WriteLine("the Arguments count: " + args.Length);
foreach (var arg in args)
{
    Console.WriteLine("ArgumentTo: " + arg);
}
Console.WriteLine("finishhhh!!");
if ((args[1]== "--language" || args[1]== "-l") &&( args[2] == "--output" || args[2] == "-o"))
{
    Console.WriteLine("ERROR!!!");
    return;
}
if (args[args.Length-1] == "-a" || args[args.Length-1]== "--author")
{
    Console.WriteLine("ERROR!!!");
    return;
}
await rootCommand.InvokeAsync(args);