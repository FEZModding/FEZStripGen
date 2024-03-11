using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FEZStripGen
{
    internal static class Stripper
    {
        public static void Strip(ModuleDefinition module)
        {
            foreach (var type in module.Types)
            {
                StripType(type);
            }
        }

        private static void StripType(TypeDefinition type)
        {
            RemoveMethodBodiesFromType(type);

            foreach (TypeDefinition nested in type.NestedTypes)
            {
                StripType(nested);
            }
        }

        private static void RemoveMethodBodiesFromType(TypeDefinition type)
        {
            foreach (MethodDefinition method in type.Methods)
            {
                if (method.HasBody && method.Body.Instructions.Count != 0)
                {
                    method.Body = new MethodBody(method);
                }
            }
        }
    }
}
