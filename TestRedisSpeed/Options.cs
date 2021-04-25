using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace TestRedisSpeed
{

    internal class Options
    {
        private int _season;

        [Option('v', "verbose", HelpText = "Print query progress", Default = false)]
        public bool Verbose { get; set; }

        [Option("debug", HelpText = "Debug data", Default = false)]
        public bool Debug { get; set; }
        [Option("host", HelpText = "host ip", Default="172.16.4.4")] // private Ip - change to your own
        public string Host { get; set; }
        [Option("port", HelpText = "Port number", Default = 6377)]
        public int Port { get; set; }

        [Option('n', "number-of-tests", HelpText = "Number of tests", Default = 100)]
        public int NumberOfTests { get; set; }

        [Option('t', "thread", HelpText = "No of threads", Default = 1)]
        public int Threads { get; set; }

        [Option('c',"command", HelpText="Command to send", Default = null)]
        public string Command { get; set; }

        [Option( "db", HelpText = "Database number", Default = null)]
        public int DbNumber { get; set; }
    }

    [Verb("bets", HelpText = "Convert bet stats to default")]
    internal class BetOptions : Options
    {
        [Option("id", HelpText = "convert for this id (from A)")]
        public int Id { get; set; }
    }

}