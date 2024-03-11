using System.Reflection;

namespace FEZStripGen
{
    internal class Program
    {
        static readonly string[] AssembliesToStrip = {
            "FEZ.exe",
            "FezEngine.dll",
            "FNA.dll",
            "Common.dll",
            "EasyStorage.dll",
            "ContentSerialization.dll",
            "XnaWordWrapCore.dll"
        };

        static string StrippedAssembliesOutDir = "Stripped";

        static void Main(string[] args)
        {
            var loader = new ModuleLoader(GetWorkingDir(args));

            foreach (var assemblyName in AssembliesToStrip)
            {
                if (!loader.ModuleExists(assemblyName))
                {
                    Console.WriteLine($"Could not find assembly to strip: {assemblyName}");
                    continue;
                }

                var module = loader.LoadModule(assemblyName);

                Console.WriteLine($"Stripping {assemblyName}...");

                Stripper.Strip(module);

                loader.SaveModule(module, Path.Combine(StrippedAssembliesOutDir, assemblyName));
            }
            Console.WriteLine($"Done!");
        }


        private static string GetWorkingDir(string[] args)
        {
            var workingDir = Directory.GetCurrentDirectory();
            if (args.Length > 0)
            {
                if (!Directory.Exists(args[0]))
                {
                    throw new DirectoryNotFoundException();
                }
                else
                {
                    workingDir = args[0];
                }
            }

            return workingDir;
        }
    }
}
