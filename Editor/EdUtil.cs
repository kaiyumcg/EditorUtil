using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KEditorUtil
{
    public static class EdUtil
    {
        public static string AssetsRelativePath(string absolutePath)
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
    }
}