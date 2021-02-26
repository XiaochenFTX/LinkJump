using System.IO;
using UnityEditor;
using UnityEngine;

namespace org.ftxtool.lib.editor
{
    public static class LinkJumpSchemeListEditor
    {
        private const string FileName = "QueriesSchemeList";

        private static string FolderPath()
        {
            return Path.Combine("Assets", "LinkJump", "Editor");
        }

        private static string FilePath()
        {
            var path = FolderPath();
            return Path.Combine(path, FileName + ".asset");
        }

        [MenuItem("ftxtool/Build Queries Scheme List")]
        public static void Create()
        {
            var path = FolderPath();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var assetPath = FilePath();

            LinkJumpSchemeList scriptableObject;
            if (File.Exists(assetPath))
            {
                scriptableObject = AssetDatabase.LoadAssetAtPath<LinkJumpSchemeList>(assetPath);
            }
            else
            {
                scriptableObject = ScriptableObject.CreateInstance<LinkJumpSchemeList>();
                AssetDatabase.CreateAsset(scriptableObject, assetPath);
            }

            Selection.activeObject = scriptableObject;
        }

        public static LinkJumpSchemeList Load()
        {
            var assetPath = FilePath();

            return !File.Exists(assetPath) ? null : AssetDatabase.LoadAssetAtPath<LinkJumpSchemeList>(assetPath);
        }
    }
}