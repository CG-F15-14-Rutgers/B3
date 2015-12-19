using UnityEngine;


using System.Collections;

public class option3 : MonoBehaviour {

	public Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();
	}
	void OnMouseEnter() {
		rend.material.color = Color.red;
	}
	
	void OnMouseExit() {
		rend.material.color = Color.white;
	}
	
	void OnMouseUp() {
		Application.LoadLevel (3);
	}
}
