using Mono.Cecil;

namespace FEZStripGen
{
    internal class ModuleLoader
    {
        private string workingDirectory;
        public ModuleLoader(string workingDir)
        {
            workingDirectory = workingDir;
        }

        public bool ModuleExists(string name)
        {
            return File.Exists(Path.Combine(workingDirectory, name));
        }

        public ModuleDefinition LoadModule(string name)
        {
            var path = Path.Combine(workingDirectory, name);

            DefaultAssemblyResolver asmResolver = new DefaultAssemblyResolver();
            asmResolver.AddSearchDirectory(Path.GetDirectoryName(path));
            var readerParams = new ReaderParameters()
            {
                AssemblyResolver = asmResolver
            };

            return ModuleDefinition.ReadModule(path, readerParams);
        }

        public void SaveModule(ModuleDefinition module, string name)
        {
            var path = Path.Combine(workingDirectory, name);

            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? "");

            module.Write(path);
        }
    }
}
