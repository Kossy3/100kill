using UnityEngine;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor.AssetImporters;

[ScriptedImporter(1, "mid")]
public class MidiImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        using (var stream = new FileStream(ctx.assetPath, FileMode.Open, FileAccess.Read))
        using (var reader = new BinaryReader(stream)) {
            Midi mid = (Midi)ScriptableObject.CreateInstance("Midi");
            mid.load(reader);
            ctx.AddObjectToAsset("Main", mid);
            ctx.SetMainObject(mid);
        }
    }
}
#endif
