using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KEditorUtil
{
    public static class EdUtil
    {
        public static string ExAssetsRelativePath(this string absolutePath)
        {
            if (absolutePath.StartsWith(Application.dataPath))
            {
                return "Assets" + absolutePath.Substring(Application.dataPath.Length);
            }
            else
            {
                throw new System.ArgumentException("Full path does not contain the current project's Assets folder", "absolutePath");
            }
        }
        public static bool ExIsValid(this string input)
        {
            return !string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input);
        }
        public static bool ExIsItModelFile(this GameObject gameObject)
        {
            var assetPath = AssetDatabase.GetAssetPath(gameObject);
            return assetPath.ExIsValid();
        }
    }
}