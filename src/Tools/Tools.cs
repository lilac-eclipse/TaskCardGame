namespace Tools;

internal static class Tools
{
    [STAThread]
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please specify a tool to run.");
            return;
        }

        switch (args[0].ToLower())
        {
            case "proj2clip":
                ProjToClipboard.Run(args[1..]);
                break;
            default:
                Console.WriteLine($"Unknown tool: {args[0]}");
                break;
        }
    }
}
