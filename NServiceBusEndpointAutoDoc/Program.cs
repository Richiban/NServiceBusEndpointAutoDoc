using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NServiceBusEndpointAutoDoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var parsedArgs = Args.Parse(args);

            if(parsedArgs == null)
            {
                DisplayHelp();
                return;
            }

            var assemblyPath = args[0];

            var nsbBuilder = NServiceBusReadmeBuilder.LoadFrom(assemblyPath);

            var readmeBuilder = nsbBuilder.CreateReadme();

            var readmeContents = readmeBuilder.ToString();

            GetOutput(parsedArgs)(readmeContents);

            if (Debugger.IsAttached) Debugger.Break();
        }

        private static Action<string> GetOutput(Args args)
        {
            if (args.OutputFile != null)
            {
                if(args.ClobberOutputFile)
                {
                    return s => File.WriteAllText(args.OutputFile, s);
                }
                else
                {
                    return s => File.AppendAllText(args.OutputFile, s);
                }
            }

            return Console.WriteLine;
        }

        private static void DisplayHelp()
        {
            Console.WriteLine($"Usage: ");
            Console.WriteLine($"       {nameof(NServiceBusEndpointAutoDoc)}.exe <assemblyPath>");
            Console.WriteLine($"       {nameof(NServiceBusEndpointAutoDoc)}.exe <assemblyPath> <outputFile> [--clobber]");

            if (Debugger.IsAttached) Debugger.Break();
        }

        private class Args
        {
            private Args() { }
            public string AssemblyPath { get; private set; }
            public string OutputFile { get; private set; }
            public bool ClobberOutputFile { get; private set; }

            public static Args Parse(string[] rawArgs)
            {
                var ret = new Args();

                var argPool = rawArgs.ToList();

                if(argPool.Contains("--clobber"))
                {
                    argPool.Remove("--clobber");
                    ret.ClobberOutputFile = true;
                }

                ret.AssemblyPath = argPool.SingleOrDefault(x => x.StartsWith("/assemblyPath:"));

                if (ret.AssemblyPath != null)
                {
                    argPool.Remove(ret.AssemblyPath);
                    ret.AssemblyPath = ret.AssemblyPath.Substring("/assemblyPath:".Length);
                }

                ret.OutputFile = argPool.SingleOrDefault(x => x.StartsWith("/outputFile:"));

                if (ret.OutputFile != null)
                {
                    argPool.Remove(ret.OutputFile);
                    ret.OutputFile = ret.OutputFile.Substring("/outputFile:".Length);
                }

                if(ret.AssemblyPath == null)
                {
                    ret.AssemblyPath = argPool.FirstOrDefault();

                    if (ret.AssemblyPath != null)
                    {
                        argPool.Remove(ret.AssemblyPath);
                    }
                }

                ret.OutputFile = argPool.FirstOrDefault();

                if(ret.OutputFile != null)
                {
                    argPool.Remove(ret.OutputFile);
                }

                if(argPool.Any())
                {
                    return null;
                }

                if(ret.OutputFile == null && ret.ClobberOutputFile)
                {
                    return null;
                }

                return ret;
            }
        }
    }
}