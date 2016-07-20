using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CreateSceneTransitionManagerScriptEditor {
	private const string kItemName = "ItoShin/Create/SceneTransitionManagerScript";
	private const string PATH       = "Assets/SceneTransition.cs";        // ファイルパス

	private static readonly string FILENAME                     = Path.GetFileName(PATH);                   // ファイル名(拡張子あり)
	private static readonly string FILENAME_WITHOUT_EXTENSION   = Path.GetFileNameWithoutExtension(PATH);   // ファイル名(拡張子なし)

	[MenuItem(CreateSceneTransitionManagerScriptEditor.kItemName)]
	static void OnSelect()
	{
		bool confirmCreateScript = EditorUtility.DisplayDialog ("title", "やる？", "ok", "no");
		if (confirmCreateScript) {
			CreateScript ();
		}
	}

	static public void CreateScript() {
		var builder = new StringBuilder();

		builder.Append(@"
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace AC {
	// <summary>Scene遷移を文字列禁止でおこなうクラス</summary>
	public class SceneTransition {
		public enum SceneID {
");
		// シーン名のenumを宣言
		var sceneNames = UnityEditor.EditorBuildSettings.scenes
			.Select (scene => Path.GetFileNameWithoutExtension (scene.path))
			.Distinct ()
			.ToArray();

		int sceneNamesLength = sceneNames.Length;
		for(var i = 0; i < sceneNamesLength; i++){
			// 最後のシーン名ならば
			if (i == sceneNamesLength - 1) {
				builder.AppendFormat ("{0}", sceneNames[i]);
			} else {
				builder.AppendFormat ("{0},", sceneNames[i]);	
			}
		}
		builder.Append(@"
		}
		private SceneTransition(){}
		private static SceneTransition m_instance;

		public static SceneTransition Instance {
			get {
				if(SceneTransition.m_instance == null) {
					SceneTransition.m_instance = new SceneTransition();
				}
				return SceneTransition.m_instance;
			}
		}

		private Dictionary<SceneID, string> m_sceneNameDictionary = new Dictionary<SceneID, string> {
");
		for(var i = 0; i < sceneNamesLength; i++){
			// 最後のシーン名ならば
			if (i == sceneNamesLength - 1) {
				builder.AppendFormat (@"{{SceneID.{0}, ""{0}""}}", sceneNames[i]);
			} else {
				builder.AppendFormat (@"{{SceneID.{0}, ""{0}""}},", sceneNames[i]);
			}
		}

		builder.Append(@"
		};

		public void LoadScene(SceneID sceneID) {
			UnityEngine.SceneManagement.SceneManager.LoadScene(m_sceneNameDictionary[sceneID]);
		}
	}
}");


		var directoryName = Path.GetDirectoryName(PATH);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}

		File.WriteAllText(PATH, builder.ToString(), Encoding.UTF8);
		AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
	}

	/// <summary>
	/// クラスを作成できるかどうかを取得します
	/// </summary>
	[MenuItem(CreateSceneTransitionManagerScriptEditor.kItemName, true)]
	static public bool CanCreate()
	{
		return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
	}
}