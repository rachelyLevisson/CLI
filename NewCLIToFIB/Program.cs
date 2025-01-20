
using System.CommandLine;
using System.Linq;

var outputOption = new Option<FileInfo>("--output", "the path output") { IsRequired = true };
outputOption.AddAlias("-o");

var languageOption = new Option<string>("--language", "the language the code or all") { IsRequired = true };
languageOption.AddAlias("-l");

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
        if (string.IsNullOrEmpty(output.FullName))
        {
            Console.WriteLine("ERROR: the path is required");
            return;
        }

        if (File.Exists(output.FullName))
        {
            Console.WriteLine("ERROR: Output file already exists! Choose a different path.");
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


                    if (autho != null)
                    {
                        if (!string.IsNullOrWhiteSpace(autho))
                        {
                            writer.WriteLine("#" + autho);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Author field is empty!");
                        }
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
                }

                if (remove)
                {
                    var lines = File.ReadAllLines(path);
                    var nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                    File.WriteAllLines(path, nonEmptyLines);
                }
            }

            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("ERROR: The specified directory does not exist!");
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: An I/O error occurred.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"ERROR: Argument exception occurred: {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: An unexpected error occurred.");
            }
        }
        else
            Console.WriteLine("ERROR: Only code files of the selected languages are allowed!");
    }
    catch (NullReferenceException)
    {
        Console.WriteLine("ERROR: Path not provided!");
    }
}, outputOption, languageOption, noteOptions, sortOption, removeEmptyLinesOption, authorOption);


var create_rspCommand = new Command("create_rsp", "Create a response file with a prepared command");

create_rspCommand.SetHandler(() =>
{
    using (StreamWriter writer = new StreamWriter("res.rsp", true))
    {
        writer.WriteLine(bundle);
        Console.Write("enter the language to need or press all  ");
      //  languageOption.Name = Console.ReadLine();
        writer.WriteLine($"-l  {Console.ReadLine()} ");

        Console.Write("press path to signale file  ");
        writer.WriteLine($"-o {Console.ReadLine()} ");

        Console.Write("Do you want the write source code? y/n  ");
        char answer = char.Parse(Console.ReadLine());
        if (answer == 'y')
            writer.WriteLine("-n ");

        Console.Write("Do you want the file is sort? y/n  ");
        answer = char.Parse(Console.ReadLine());
        if (answer == 'y')
        {
            Console.Write("enter abc or type the abc is defulte  ");
            string answerSorc = Console.ReadLine();
            writer.WriteLine($"-o {answerSorc} ");
        }

        Console.Write("Do you want the remove the empy line? y/n  ");
        answer = char.Parse(Console.ReadLine());
        if (answer == 'y')
            writer.WriteLine("-rem ");

        Console.Write("Do you want teh write authoe? y/n  ");
        answer = char.Parse(Console.ReadLine());
        if (answer == 'y')
        {
            Console.Write("enter the name  ");
            string answerName = Console.ReadLine();
             writer.WriteLine("-a " + answerName);
        }
    }
});


var rootCommand = new RootCommand("this opposite many page code to signel code");
rootCommand.AddCommand(bundle);
rootCommand.AddCommand(create_rspCommand);

await rootCommand.InvokeAsync(args);