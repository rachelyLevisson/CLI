
using System.CommandLine;

var outputOption = new Option<FileInfo>(new[] { "--output", "-o" }, "the path output");

var languageOption = new Option<string>
    (new[] { "--language", "-l" }, "the language the code or all");

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

bundle.AddOption(languageOption);
bundle.AddOption(outputOption);
bundle.AddOption(noteOptions);
bundle.AddOption(sortOption);
bundle.AddOption(removeEmptyLinesOption);
bundle.AddOption(authorOption);


bundle.SetHandler((output, language, note, sort, remove, autho) =>
{
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
                    if (sort == "abc")
                    {
                        file = file.OrderBy(f => Path.GetFileName(f)).ToArray();
                    }
                    //type code
                    else
                    {
                        file = file.OrderBy(f => Path.GetExtension(f)).ToArray();
                    }
                }

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    //    האם הקיש את האופציה ומכילה TRUE
                    if (note)
                    {
                        writer.WriteLine("# " + Environment.CurrentDirectory);
                    }

                    if (!string.IsNullOrEmpty(autho))
                        writer.WriteLine("#" + autho);
                    else
                    {
                        if (autho == "")
                            Console.WriteLine("ERROR: this dont author!!");
                    }

                    foreach (var item in file)
                    {
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

                if (remove)
                {
                    var lines = File.ReadAllLines(path);
                    var nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line.Trim()) && line != "\n").ToArray();
                    File.WriteAllLines(path, nonEmptyLines);
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
        }
        else
            Console.WriteLine("ERROR: Only code files of the selected languages!");
    }
    catch (NullReferenceException e)
    {
        Console.WriteLine("ERROR: not enter the path!!");
        Console.WriteLine(e.Message);
    }
}, outputOption, languageOption, noteOptions, sortOption, removeEmptyLinesOption, authorOption);

var create_rspCommand = new Command("create-rsp", "Create a response file with a prepared command");

create_rspCommand.SetHandler(() =>
{
    string option = "-l ";
    Console.WriteLine("enter the language to need or press all");
    option += Console.ReadLine();

    Console.WriteLine("press path to signale file");
    option += " -o ";
    option += Console.ReadLine();
    option += " ";

    Console.WriteLine("Do you want the write source code? y/n");
    char answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
        option += "-n ";

    Console.WriteLine("Do you want the file is sort? y/n");
    answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
    {
        Console.WriteLine("enter abc or type the abc is defulte");
        string answerSorc = Console.ReadLine();
        option += "-s " + answerSorc;
        option += " ";
    }

    Console.WriteLine("Do you want the remove the empy line? y/n");
    answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
        option += "-rem ";

    Console.WriteLine("Do you want teh write authoe? y/n");
    answer=char.Parse(Console.ReadLine());
    if(answer == 'y')
    {
        Console.WriteLine("enter the name");
        string answerName = Console.ReadLine();
        option += "-a " + answerName;
    }
    option = option.TrimEnd();

    bundle.(option);
});

var rootCommand = new RootCommand("this opposite many page code to signel code");
rootCommand.AddCommand(bundle);


//rootCommand.AddCommand(create_rspCommand);
if ((args[1] == "--language" || args[1] == "-l") && (args[2] == "--output" || args[2] == "-o"))
{
    Console.WriteLine("ERROR!!!");
    return;
}
if (args[args.Length - 1] == "-a" || args[args.Length - 1] == "--author")
{
    Console.WriteLine("ERROR!!!");
    return;
}
await rootCommand.InvokeAsync(args);