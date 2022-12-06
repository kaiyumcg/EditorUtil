using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEditor.Build;

namespace EditorUtil
{
    public static class KaiRepoUtil
    {
        static readonly List<string> AllKaiyumAssemblies = new List<string> { "Vortex", "KTaskManager", "KSaveDataManager",
            "UnityExt", "UIF", "KAction", "AttributeExt", "KTween", "Neuron", "KData", "UITween", "KTaskGraph", "KTagSystem",
        "Adrenaline", "KPoolManager", "KEditorUtil" };

        [InitializeOnLoadMethod]
        private static void OnInitialized()
        {
            AssemblyReloadEvents.afterAssemblyReload += () =>
            {
                var asmList = CompilationPipeline.GetAssemblies();
                var currentKaiyumLibs = new List<string>();
                foreach (var a in asmList)
                {
                    if (AllKaiyumAssemblies.Contains(a.name))
                    {
                        currentKaiyumLibs.Add(a.name);
                    }
                }
                SetKaiyumLibSymbols(currentKaiyumLibs);
            };
        }

        static void SetKaiyumLibSymbols(List<string> currentKaiyumRepoAssemblies)
        {
            string[] currentSymbolsArrays;
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            var namedBuildTarget = UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget, out currentSymbolsArrays);

            List<string> currentDefines = new List<string>();
            currentDefines.AddRange(currentSymbolsArrays);

            List<string> kaiDefines = ConvertToDefines(currentKaiyumRepoAssemblies);

            bool anyMissing = HasAnyMissingKaiDefine(kaiDefines, currentDefines);
            bool invalid = HasAnyInvalidKaiDefine(kaiDefines, currentDefines);
            if (anyMissing || invalid)
            {
                RemoveAllKaiDefines(ref currentDefines);
                currentDefines.AddRange(kaiDefines);
                PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, currentDefines.ToArray());
            }

            List<string> ConvertToDefines(List<string> assemblies)
            {
                var result = new List<string>();
                foreach (var asmbl in assemblies)
                {
                    result.Add("_2K_" + asmbl + "_FOREVER_");
                }
                return result;
            }

            //Has any define symbols(corresponding to kaiyum libs) that is not included in the player setting, yet the library is included in the project
            bool HasAnyMissingKaiDefine(List<string> kaiDefines, List<string> currentDefinesInPlayerSetting)
            {
                bool missing = false;
                foreach (var assembly in kaiDefines)
                {
                    if (currentDefinesInPlayerSetting.Contains(assembly) == false)
                    {
                        missing = true;
                        break;
                    }
                }
                return missing;
            }

            //Has any define symbols(corresponding to kaiyum libs) that is included in the player setting but the corresponding kaiyum library is missing from the project!
            bool HasAnyInvalidKaiDefine(List<string> kaiDefines, List<string> currentDefinesInPlayerSetting)
            {
                var invalid = false;
                var allPossibleKaiDefines = ConvertToDefines(AllKaiyumAssemblies);
                foreach (var assembly in currentDefinesInPlayerSetting)
                {
                    if (allPossibleKaiDefines.Contains(assembly) == false) { continue; }

                    if (kaiDefines.Contains(assembly) == false)
                    {
                        invalid = true;
                        break;
                    }
                }
                return invalid;
            }

            void RemoveAllKaiDefines(ref List<string> currentDefinesInPlayerSetting)
            {
                var result = new List<string>();
                var allPossibleKaiDefines = ConvertToDefines(AllKaiyumAssemblies);
                foreach (var assembly in currentDefinesInPlayerSetting)
                {
                    if (allPossibleKaiDefines.Contains(assembly) == false) 
                    { 
                        result.Add(assembly);
                    }
                }
                currentDefinesInPlayerSetting = result;
            }
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