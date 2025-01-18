
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
        //    האם הכניס ניתוב כל שהוא אם לא יכניס ערך ברירת מחדל
        //if (string.IsNullOrEmpty(output.ToString()))
        //{
        //    Console.WriteLine("ERROR: --output option is required!");
        //    output = new FileInfo("text.txt");
        //}
        //      האם השפה שהוקשה נמצאת ברשימה
        if (language == "all" || Enum.TryParse(language, true, out Elanguage lang))
        {
            try
            {
                File.Create(output.FullName);
                string[] file = Directory.GetFiles(Environment.CurrentDirectory);
                Console.WriteLine("lenght: "+file.Length);
                foreach (var item in file)
                {
                    Console.WriteLine(item);
                }
                //    האם הקיש את האופציה ומכילה TRUE
                if (note)
                {
                    Console.WriteLine("i enter to note!!");
                    File.AppendText("the write me😜💥");
                    Console.WriteLine("i write");
                    File.AppendText("#" + Environment.CurrentDirectory);
                }
                //האם הקיש את האופציה ומכילה TRUE
                if (remove)
                {
                    Console.WriteLine("i enter to remove!!");

                    //delete a empty line!!
                }
                if (string.IsNullOrEmpty(autho.ToString()))
                {
                    Console.WriteLine("i enter to author!!");
                    File.OpenWrite("#" + autho.ToString());
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
            }
            Console.WriteLine("sucssess!!! yesh! yesh! ♥️🎁💥🍔😜");
        }
        else
        {
            Console.WriteLine("ERROR: Only code files of the selected languages!");
        }
        Console.WriteLine("succsess!! 😆😂😃🙌🏻");
    }
    catch (NullReferenceException e)
    {
        Console.WriteLine("hihi");
        Console.WriteLine(e.Message);
    }
    #region //
    //  }
    //catch (IOException e)
    //{
    //    Console.WriteLine("wo no vivivivi........🥹😞😭");
    //}
    //catch (NullReferenceException ne) { Console.WriteLine("errorr ooooffffff"+ne.Message); }
    #endregion
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