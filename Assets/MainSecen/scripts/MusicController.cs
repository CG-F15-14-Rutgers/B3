using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	private static bool created = false;
	public GameObject music;

	void Awake() {
		if (!created) {
			// this is the first instance - make it persist
			DontDestroyOnLoad(this.gameObject);
			created = true;
		} else {
			// this must be a duplicate from a scene reload - DESTROY!
			Destroy(this.gameObject);
		} 
	}
}
