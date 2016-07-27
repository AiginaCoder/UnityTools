
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace AC {
	// <summary>Scene遷移を文字列禁止でおこなうクラス</summary>
	public class SceneTransition {
		public enum SceneID {
aaa
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
{SceneID.aaa, "aaa"}
		};

		public void LoadScene(SceneID sceneID) {
			UnityEngine.SceneManagement.SceneManager.LoadScene(m_sceneNameDictionary[sceneID]);
		}
	}
}