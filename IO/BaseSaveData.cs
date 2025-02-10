using UnityEngine;

namespace Skeletom.Essentials.IO {

    [System.Serializable]
	public abstract class BaseSaveData {
		public string version = Application.version;
		public override string ToString() {
			return JsonUtility.ToJson(this);
		}
	}
}