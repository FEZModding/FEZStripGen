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

            RemoveUncommonTypes(module, new[]{
                "FezGame.Components.MockAchievement",
                "FezGame.Services.MockUser",
                "FezGame.Tools.ErrorDialog",
            });
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

        private static void RemoveUncommonTypes(ModuleDefinition module, string[] namesOfTypesToRemove)
        {
            foreach(var type in module.Types)
            {
                // clean content of types that should be removed,
                // so they're not used to generate hooks
                if (namesOfTypesToRemove.Contains(type.FullName))
                {
                    type.Methods.Clear();
                }

                // Some methods will still reference them.
                // Make sure they're removed as well.
                var methodsToRemove = type.Methods.Where(
                    method => method.Parameters.Any(
                        parameter => namesOfTypesToRemove.Contains(parameter.ParameterType.FullName)
                    )
                ).ToList();

                foreach(var method in methodsToRemove)
                {
                    type.Methods.Remove(method);
                }
            }
        }
    }
}
