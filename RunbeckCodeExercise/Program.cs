using System;
using System.IO;
using System.Linq;
using System.Text;

namespace RunbeckCodeExercise
{
    /// <summary>
    /// Runbeck Code Exercise assignment by Dincer Birduzer.
    /// This Console Application process a delimited text file. The file will have a header row, then one row per record. The records may be comma-separated or tab-separated.
    /// The application then produce two output files. One file will contain the records with the correct number of fields. The second will contain the records with the incorrect number of fields. 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method for the application.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("\n\n     Runbeck Code Exercise \n\n");

            // 1.	Where is the file located?
            Console.Write("1.	Please enter file location: ");
            string inputFilePath = Console.ReadLine();

            while (!File.Exists(inputFilePath) || string.IsNullOrWhiteSpace(inputFilePath))
            {
                Console.Write($"\t Specified file <{inputFilePath}> not exist, Please provide correct file location, or [ X ] for exit: ");
                inputFilePath = Console.ReadLine();

                if (inputFilePath.Equals("x", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting..");
                    return;
                }
            }

            // 2.	Is the file format CSV (comma-separated values) or TSV (tab-separated values)?
            Console.Write("2.	Please specify file format CSV (comma-separated values) or TSV (tab-separated values) [ C | T ]: ");
            char fileFormat = char.ToUpper(Console.ReadKey(true).KeyChar);

            while (!"CTX".Contains(fileFormat))
            {
                Console.WriteLine("\n\t Please enter [ C ] for CSV, [ T ] for TSV, or [ X ] for exit: ");
                fileFormat = char.ToUpper(Console.ReadKey().KeyChar);

                if (fileFormat.Equals('X'))
                {
                    Console.WriteLine("Exiting..");
                    return;
                }
            }

            // 3.	How many fields should each record contain?
            Console.Write("\n3.	Please enter fields counts for each record: ");
            string fieldCount = Console.ReadLine();

            while (!int.TryParse(fieldCount, out int result))
            {
                Console.WriteLine($"\n\t Specified field counts {fieldCount} is not correct. Please enter fields counts for each record, or [ X ] for exit:");
                fieldCount = Console.ReadLine();

                if (fieldCount.Equals('X'))
                {
                    Console.WriteLine("Exiting..");
                    return;
                }

            }

            ProcessFile(inputFilePath, fileFormat, Convert.ToInt32(fieldCount));

            Console.WriteLine($"File has been processed. Please check <{Path.GetDirectoryName(inputFilePath)}> location.");
            Console.ReadLine();

        }

        /// <summary>
        /// This method reads input file and process output files.
        /// </summary>
        /// <param name="inputFilePath">The input file.</param>
        /// <param name="fileFormat">File format CSV [C] or TSV [T]</param>
        /// <param name="fieldCount">fields count for each record</param>
        static private void ProcessFile(string inputFilePath, char fileFormat, int fieldCount)
        {
            var correctRecords = new StringBuilder();
            var incorrectRecords = new StringBuilder();
            int correctCount = 0;
            int incorrectCount = 0;

            try
            {
                // get delimeter based on file format.
                char delimeter = fileFormat == 'C' ? ',' : '\t';

                // Read the input file
                using (StreamReader sr = new StreamReader(inputFilePath))
                {
                    // header line
                    string line = sr.ReadLine();

                    while ((line = sr.ReadLine()) != null)
                    {
                        var fields = line.Split(delimeter);
                                                
                        if (fields.Count() == fieldCount)
                        {
                            // Get Correctly formatted records
                            correctRecords.AppendLine(line);
                            correctCount++;
                        }
                        else
                        {
                            // Get Incorrectly formatted records
                            incorrectRecords.AppendLine(line);
                            incorrectCount++;
                        }
                    }
                }

                // Create file which contains the records(if any) with the correct number of fields
                if (correctCount > 0)
                {
                    string fileName = "CorrectRecords" + (fileFormat == 'C' ? ".csv" : ".txt");
                    var filePath = Path.Combine(
                        Path.GetDirectoryName(inputFilePath),
                        fileName
                        );

                    File.WriteAllText(filePath, correctRecords.ToString());
                }

                // Create file which contains the records(if any) with the incorrect number of fields. 
                if (incorrectCount > 0)
                {
                    string fileName = "IncorrectRecords" + (fileFormat == 'C' ? ".csv" : ".txt");
                    var filePath = Path.Combine(
                        Path.GetDirectoryName(inputFilePath),
                        fileName
                        );

                    File.WriteAllText(filePath, incorrectRecords.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error happened while processing files:");
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}
