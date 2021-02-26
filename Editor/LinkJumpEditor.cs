using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;


namespace org.ftxtool.lib.editor
{
    public static class LinkJumpEditor
    {
        private static void AddScheme(PlistElementArray schemes, string scheme)
        {
            if (!schemes.values.Exists(i => i.AsString().Equals(scheme, StringComparison.Ordinal)))
            {
                schemes.AddString(scheme);
            }
        }
        
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget != BuildTarget.iOS)
                return;

            var schemeList = LinkJumpSchemeListEditor.Load();
            if (schemeList == null)
            {
                return;
            }

            var infoPath = Path.Combine(path, "Info.plist");
            if (!File.Exists(infoPath))
            {
                throw new FileNotFoundException($"Not found info plist at: {infoPath}", "Info.plist");
            }
            
            var infoDoc = new PlistDocument();
            infoDoc.ReadFromFile(infoPath);

            var root = infoDoc.root;
            var schemes = root.values.TryGetValue("LSApplicationQueriesSchemes", out var queriesSchemes)
                ? queriesSchemes.AsArray()
                : root.CreateArray("LSApplicationQueriesSchemes");

            foreach (var scheme in schemeList.Schemes)
            {
                AddScheme(schemes, scheme);
            }
            
            infoDoc.WriteToFile(infoPath);
        }
    }
}
