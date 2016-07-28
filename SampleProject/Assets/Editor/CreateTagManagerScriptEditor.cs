using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

public class CreateTagManagerScriptEditor : MonoBehaviour
{
    private const string kMenuDirectory = "AiginaCoder/Create/MakeTagManagerScript";
    private const string kScriptSavePath = "Assets/Scripts/TagManager.cs"; // 保存先

    [MenuItem (CreateTagManagerScriptEditor.kMenuDirectory)]
    static void OnSelect ()
    {
        bool confirmCreateScript = EditorUtility.DisplayDialog ("確認", "TagManagerクラスを出力します", "OK", "NO");
        if (confirmCreateScript) {
            CreateScript ();
        }
    }

    static public void CreateScript ()
    {
        var builder = new StringBuilder ();

        builder.Append (
@"using System.Collections.Generic;

namespace AC {
    // <summary>TagManagerクラス</summary>
    public class TagManager {
    public enum TagID {
");
        // tagのenumを宣言
        var tagNames = InternalEditorUtility.tags;

        int tagNamesLength = tagNames.Length;
        for (var i = 0; i < tagNamesLength; i++) {
            // 最後のtag名ならば
            if (i == tagNamesLength - 1) {
                builder.AppendFormat ("        {0}", tagNames [i]);
            } else {
                builder.AppendFormat ("        {0},\n", tagNames [i]); 
            }
        }
        builder.Append (@"
    }
    private static TagManager m_instance;

    public static TagManager Instance {
        get {
            if(TagManager.m_instance == null) {
                TagManager.m_instance = new TagManager();
            }
            return TagManager.m_instance;
        }
    }

    private Dictionary<TagID, string> m_tagDictionary = new Dictionary<TagID, string> {
");
        for (var i = 0; i < tagNamesLength; i++) {
            // 最後のtag名ならば
            if (i == tagNamesLength - 1) {
                builder.AppendFormat ("        {{TagID.{0}, \"{0}\"}}", tagNames [i]);
            } else {
                builder.AppendFormat ("        {{TagID.{0}, \"{0}\"}},\n", tagNames [i]);
            }
        }
        builder.Append (@"
    };

    public string GetTagString(TagID tagID) {
        return m_tagDictionary[tagID];
    }
  }
}");


        var directoryName = Path.GetDirectoryName (kScriptSavePath);
        if (!Directory.Exists (directoryName)) {
            Directory.CreateDirectory (directoryName);
        }

        File.WriteAllText (kScriptSavePath, builder.ToString (), Encoding.UTF8);
        AssetDatabase.Refresh (ImportAssetOptions.ImportRecursive);
    }

    /// <summary>
    /// クラスを作成できるかどうかを取得します
    /// </summary>
    [MenuItem (CreateTagManagerScriptEditor.kMenuDirectory, true)]
    static public bool CanCreate ()
    {
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }
}
