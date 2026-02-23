using System.IO;
using System.Linq;
using UnityEditor;

namespace Editor
{
    public sealed class CscRspPostprocessor : AssetPostprocessor
    {
        private const string
            TablesFolder = "Assets/Features/Localization/Data/Tables",
            CscRspPath = "Assets/csc.rsp",
            SharedTableDataEnding = "Shared Data.asset",
            StringTableEnding = "_en.asset",
            AdditionalFileLineFormat = "/additionalfile:\"{0}\"";

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var allPaths = importedAssets
                .Concat(deletedAssets)
                .Concat(movedAssets)
                .Concat(movedFromAssetPaths);

            if (!allPaths.Any(p => p.StartsWith(TablesFolder)))
                return;

            RegenerateCscRsp();
        }

        [MenuItem("Tools/Localization/Regenerate csc.rsp")]
        private static void RegenerateCscRsp()
        {
            var files = Directory.GetFiles(TablesFolder, "*.asset")
                .Select(path => path.Replace('\\', '/'))
                .Where(path => path.EndsWith(StringTableEnding) || path.EndsWith(SharedTableDataEnding))
                .OrderBy(path => path)
                .ToArray();

            var lines = files.Select(file => string.Format(AdditionalFileLineFormat, file));
            var content = string.Join("\n", lines) + "\n";

            var existing = File.Exists(CscRspPath) ? File.ReadAllText(CscRspPath) : "";
            if (content == existing)
                return;

            File.WriteAllText(CscRspPath, content);
            AssetDatabase.ImportAsset(CscRspPath);
        }
    }
}