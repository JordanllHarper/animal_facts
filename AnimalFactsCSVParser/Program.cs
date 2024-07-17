using AnimalFactsApi.Model;

namespace AnimalFactsCSVParser;

static class Program
{
    private static List<AnimalFact> ParseAnimalFacts(string filepath) =>
        File.ReadAllLines(filepath).Select(line =>
        {
            var contents = line.Split(',');
            Console.WriteLine(contents);
            Console.WriteLine(contents[0]);
            // 5 attributes = 0 - 4
            return new AnimalFact(contents[0], contents[1], contents[2], contents[3], contents[4]);
        }).ToList();

    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine($"Parsing: {args.First()}");
            var filename = args.First();
            var facts = ParseAnimalFacts(filename);
            // hand off to some database
            foreach (var fact in facts)
            {
                Console.WriteLine(fact);
            }
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("No provided file path: please provide the file path you want to parse.");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("No provided file path: please provide the file path you want to parse.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Something went wrong: {e}");
        }
    }
}