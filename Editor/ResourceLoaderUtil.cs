using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace KEditorUtil
{
    public static class ProjectResourceUtil
    {
        public static List<T> LoadAssetsFromResourceFolder<T>() where T : UnityEngine.Object
        {
            var result = new List<T>();
            var assets = Resources.LoadAll<T>("");
            result.AddRange(assets);
            return result;
        }
        public static List<T> LoadAssetsFromAssetFolder<T>() where T : UnityEngine.Object
        {
            var result = new List<T>();
            var assets = AssetDatabase.FindAssets("t:" + typeof(T));
            if (assets != null && assets.Length > 0)
            {
                for (int i = 0; i < assets.Length; i++)
                {
                    var guid = assets[i];
                    if (string.IsNullOrEmpty(guid) || string.IsNullOrWhiteSpace(guid)) { continue; }
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (string.IsNullOrEmpty(assetPath) || string.IsNullOrWhiteSpace(assetPath)) { continue; }

                    var obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                    if (obj != null) 
                    {
                        result.Add(obj);
                    }
                }
            }
            return result;
        }
        public static void CreateScriptFromTemplate(string generatedFileName, params string[] uniqueNames)
        {
            var path = "";
            string[] files = Directory.GetFiles("Assets/", "*.txt", SearchOption.AllDirectories);
            if (files != null && files.Length > 0)
            {
                foreach (var f in files)
                {
                    var allCount = 0;
                    var foundCount = 0;
                    if (uniqueNames != null && uniqueNames.Length > 0)
                    {
                        foreach (var fName in uniqueNames)
                        {
                            if (string.IsNullOrEmpty(fName) || string.IsNullOrWhiteSpace(fName)) { continue; }
                            allCount++;
                            if (f.Contains(fName))
                            {
                                foundCount++;
                            }
                        }
                        if (foundCount == allCount)
                        {
                            path = f;
                            break;
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(path) == false)
            {
                ProjectWindowUtil.CreateScriptAssetFromTemplateFile(path, generatedFileName + ".cs");
            }
        }
        public static void LoadAnimations(string absolutePath, ref List<AnimationClip> clips)
        {
            string[] files = Directory.GetFiles(absolutePath);
            if (files != null && files.Length > 0)
            {
                foreach (string f in files)
                {
                    if (f.EndsWith(".fbx"))
                    {
                        var allSubAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(EdUtil.AssetsRelativePath(f));

                        foreach (var asset in allSubAssets)
                        {
                            var animationClip = asset as AnimationClip;

                            if (animationClip != null)
                            {
                                clips.Add(animationClip);
                            }
                        }
                    }
                }
            }

            string[] dirs = Directory.GetDirectories(absolutePath);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    LoadAnimations(dir, ref clips);
                }
            }
        }
        public static void LoadAllUnityAssets<T>(string absolutePath, ref List<T> assets) where T : UnityEngine.Object
        {
            string[] files = Directory.GetFiles(absolutePath);
            if (files != null && files.Length > 0)
            {
                foreach (string f in files)
                {
                    if (f.EndsWith(".asset"))
                    {
                        var allSubAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(EdUtil.AssetsRelativePath(f));

                        foreach (var asset in allSubAssets)
                        {
                            var animationClip = asset as T;

                            if (animationClip != null)
                            {
                                assets.Add(animationClip);
                            }
                        }
                    }
                }
            }

            string[] dirs = Directory.GetDirectories(absolutePath);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    LoadAllUnityAssets(dir, ref assets);
                }
            }
        }

        public static void LoadPrefabsWithConstraints(string absolutePath, ref Dictionary<string, GameObject> urlPrefabPair, System.Type constraintType, bool searchInChildren = false)
        {
            string[] files = Directory.GetFiles(absolutePath);
            if (files != null && files.Length > 0)
            {
                foreach (string f in files)
                {
                    if (f.EndsWith(".prefab"))
                    {
                        var loadedPrefabRoot = AssetDatabase.LoadAssetAtPath<GameObject>(EdUtil.AssetsRelativePath(f)); //PrefabUtility.LoadPrefabContents(f);
                        if (loadedPrefabRoot != null)
                        {
                            var constraint = loadedPrefabRoot.GetComponent(constraintType.GetType());
                            if (constraint == null && searchInChildren)
                            {
                                constraint = loadedPrefabRoot.GetComponentInChildren(constraintType.GetType());
                            }
                            if (constraint != null)
                            {
                                urlPrefabPair.Add(f, loadedPrefabRoot);
                            }
                        }
                    }
                }
            }

            string[] dirs = Directory.GetDirectories(absolutePath);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    LoadPrefabsWithConstraints(dir, ref urlPrefabPair, constraintType, searchInChildren);
                }
            }
        }
        public static void LoadPrefabsWithConstraints(string absolutePath, ref List<GameObject> resultPrefabs, System.Type constraintType, bool searchInChildren = false)
        {
            string[] files = Directory.GetFiles(absolutePath);
            if (files != null && files.Length > 0)
            {
                foreach (string f in files)
                {
                    if (f.EndsWith(".prefab"))
                    {
                        var loadedPrefabRoot = AssetDatabase.LoadAssetAtPath<GameObject>(EdUtil.AssetsRelativePath(f)); //PrefabUtility.LoadPrefabContents(f);
                        if (loadedPrefabRoot != null)
                        {
                            var constraint = loadedPrefabRoot.GetComponent(constraintType.GetType());
                            if (constraint == null && searchInChildren)
                            {
                                constraint = loadedPrefabRoot.GetComponentInChildren(constraintType.GetType());
                            }
                            if (constraint != null)
                            {
                                resultPrefabs.Add(loadedPrefabRoot);
                            }
                        }
                    }
                }
            }

            string[] dirs = Directory.GetDirectories(absolutePath);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    LoadPrefabsWithConstraints(dir, ref resultPrefabs, constraintType, searchInChildren);
                }
            }
        }
        public static void LoadPrefabs(string absolutePath, ref Dictionary<string, GameObject> urlPrefabPair)
        {
            string[] files = Directory.GetFiles(absolutePath);
            if (files != null && files.Length > 0)
            {
                foreach (string f in files)
                {
                    if (f.EndsWith(".prefab"))
                    {
                        var loadedPrefabRoot = AssetDatabase.LoadAssetAtPath<GameObject>(EdUtil.AssetsRelativePath(f)); //PrefabUtility.LoadPrefabContents(f);
                        if (loadedPrefabRoot != null)
                        {
                            urlPrefabPair.Add(f, loadedPrefabRoot);
                        }
                    }
                }
            }

            string[] dirs = Directory.GetDirectories(absolutePath);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    LoadPrefabs(dir, ref urlPrefabPair);
                }
            }
        }
        public static void LoadPrefabs(string absolutePath, ref List<GameObject> prefabs)
        {
            string[] files = Directory.GetFiles(absolutePath);
            if (files != null && files.Length > 0)
            {
                foreach (string f in files)
                {
                    if (f.EndsWith(".prefab"))
                    {
                        var loadedPrefabRoot = AssetDatabase.LoadAssetAtPath<GameObject>(EdUtil.AssetsRelativePath(f)); //PrefabUtility.LoadPrefabContents(f);
                        if (loadedPrefabRoot != null)
                        {
                            prefabs.Add(loadedPrefabRoot);
                        }
                    }
                }
            }

            string[] dirs = Directory.GetDirectories(absolutePath);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    LoadPrefabs(dir, ref prefabs);
                }
            }
        }
    }
}