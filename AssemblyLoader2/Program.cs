using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace AssemblyLoader2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("A various implementations for C# binary in memory loaders");
                Console.WriteLine("Usage:");
                Console.WriteLine("AssemblyLoader.exe <FILE.EXE> \"<param1 param2 paramX>\"");
                return;
            }
            var file = args[0];
            Byte[] fileBytes = File.ReadAllBytes(file);

            var entry = args[1];

            string[] fileArgs = { };
            if (args.Length > 2)
            {
                fileArgs = args[2].Split(' ');
            }

            Console.WriteLine("ExecuteAssemblyLoad2\n");
            ExecuteAssemblyLoad2(fileBytes, entry, fileArgs);

        }


        public static void ExecuteAssemblyLoad2(Byte[] assemblyBytes,string entry, string[] param)
        {
            Console.WriteLine("[*] entry = " + entry);
            Console.WriteLine("[*] Using Assembly.Load 2:");

            // Load the assembly
            Assembly assembly = Assembly.Load(assemblyBytes);
            // Find all the types (Namespaces and classes) containing the methods 
            foreach (var type in assembly.GetTypes())
            {
                if (type.ToString() == entry) { 
                Console.WriteLine($"[*] Loaded Type '{type}'");
                // Get the parameters
                object[] parameters = new object[] { param };
                    // Console.WriteLine(parameters);

                    // Enumerate (and try to load) all methods in this type (doesn't include 'Main' method)
                    foreach (MethodInfo method in type.GetMethods())
                    {
                          Console.WriteLine($"  [*] Loading Method '{method.Name}'");
                        //  Console.WriteLine($"  [*] Enumerating Method Parameters:");
                        // Enumerate the method's parameters
                        foreach (var mp in method.GetParameters())
                    //    {
                    //          Console.WriteLine($"    - parameter type: '{mp}'");
                    //    }
                        try
                        {
                            // Create an instance of the type using a constructor
                            object instance = Activator.CreateInstance(type);
                            // Invoke the method with its parameters
                            method.Invoke(instance, parameters);
                        }
                        catch (Exception e)
                        {
                            //   Console.WriteLine($"[!] Ops, wrong method name '{method.Name}'");
                            Console.WriteLine(e);
                        }
                    }
                }
            }
        }

    }
}
