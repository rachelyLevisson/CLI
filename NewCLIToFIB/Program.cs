
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

Console.WriteLine("the name option!!");
foreach (var option in bundle.Options)
{
    Console.WriteLine(option.Name);
}
Console.WriteLine();
Console.WriteLine("come!!");



bundle.SetHandler((output, language, note, remove, autho) =>
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
        if (language == "all" || Enum.TryParse(language, true, out Elanguage lang))
        {
            try
            {
                FileStream fs = File.Create(output.FullName);
                string[] file = Directory.GetFiles(Environment.CurrentDirectory);
                Console.WriteLine("lenght: " + file.Length);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    //    האם הקיש את האופציה ומכילה TRUE
                    if (note)
                    {
                        Console.WriteLine("i enter to note!!");
                        writer.WriteLine("# " + Environment.CurrentDirectory);
                    }
                    if (!string.IsNullOrEmpty(autho))
                    {
                        Console.WriteLine("i enter to author!!");
                        writer.WriteLine("#" + autho);
                    }
                    foreach (var item in file)
                    {
                        Console.WriteLine(item);
                        writer.WriteLine("succssess!! towrite");
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
                    Console.WriteLine("come hear!!");
                    //using (StreamWriter writer2 = new StreamWriter(fs))
                    //{
                        Console.WriteLine("i enter to remove!!");
                    using (StreamReader sRemove = new StreamReader(fs))
                    {
                        Console.WriteLine("problem?");
                        foreach (var item in sRemove.ReadLine())
                        {
                            if (item.ToString() == "")
                                Console.WriteLine("delete");
                        }
                    }
                   // }
                }
                //delete a empty line!!
                fs.Close();
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
            catch(ArgumentException e)
            {
                Console.WriteLine("Error: in delete line");
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
        Console.WriteLine("hihi");
        Console.WriteLine(e.Message);
    }
}, outputOption, languageOption, noteOptions, removeEmptyLinesOption, authorOption);

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
await rootCommand.InvokeAsync(args);