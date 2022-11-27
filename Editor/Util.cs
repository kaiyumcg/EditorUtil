using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace EditorUtil
{
    public static class Util
    {
        static readonly string[] kaiyumAsmList = { };

        [InitializeOnLoadMethod]
        private static void OnInitialized()
        {
            if (EditorPrefs.GetBool("HasNew"))
            {
                EditorPrefs.DeleteKey("HasNew");
               
            }
            AssemblyReloadEvents.afterAssemblyReload += () =>
            {
                var asmList = CompilationPipeline.GetAssemblies();
                foreach (var a in asmList)
                {
                    Debug.Log(a.name);
                }
            };
        }

        public static AssemblyName[] GetAssemblies()
        {
            //Assembly currentAssembly = typeof(/*enter the class name which contains the assembly references*/).Assembly;
            //AssemblyName[] referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            //return referencedAssemblies;
            return null;
        }
    }
}