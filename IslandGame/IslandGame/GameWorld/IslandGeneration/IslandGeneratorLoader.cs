﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace IslandGame.GameWorld
{
    static class IslandGeneratorLoader
    {
        static string lastCompiledCode = "";
        static CompilerResults compResults;

         



        public static bool updateAndReturnIfNewCodeIsReady()
        {
            try
            {
                string currentSource = getCode();
                if (currentSource.Equals(lastCompiledCode))
                {
                    return false;
                }
                else//code has changed
                {
                    bool compiledSuccessfully = compileGeneratorCodeAndReturnIfSuccessful(currentSource);

                    return compiledSuccessfully;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public static IslandGenerator getGenerator()
        {
            IslandGenerator instance = (IslandGenerator)compResults.CompiledAssembly.CreateInstance("IslandGame.GameWorld.TestGenerator");
            return instance;
        }

        private static bool compileGeneratorCodeAndReturnIfSuccessful(string source)
        {
            Dictionary<string, string> providerOptions = new Dictionary<string, string>
                {
                    {"CompilerVersion", "v3.5"}
                };
            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);
            CompilerParameters compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };
            string location = Assembly.GetExecutingAssembly().Location;
            compilerParams.ReferencedAssemblies.Add(location);
            compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.dll");
            
            
            CompilerResults newCompResults = provider.CompileAssemblyFromSource(compilerParams, source);

            if (newCompResults.Errors.Count != 0)
            {
                foreach (CompilerError error in newCompResults.Errors)
                {
                    Console.WriteLine("Line {0},{1}\t: {2}\n ERROR IN DYNC",
                           error.Line, error.Column, error.ErrorText);
                }
                return false;

            }
            else //compilation was successful!
            {
                compResults = newCompResults;
                lastCompiledCode = source;
                return true;
            }
        }

        private static string getCode()
            {
            System.IO.StreamReader myFile =
               new System.IO.StreamReader(@"C:\Users\Public\CubeStudio\GENERATIONCODE\IslandGeneration.cs");
            string source = myFile.ReadToEnd();

            myFile.Close();
            return source;
        }
    }
}
