using UnityEngine;
using System.Collections;

public class ActivateRain : MonoBehaviour {

	public GameObject rain;
	public GameObject Stick;
	public bool User_Interaction_state;
	private bool state = false;

	// Use this for initialization
	void Start () {
		//gameObject.SetActive (false);
		rain.SetActive (state);
		Stick.SetActive (state);
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			print ("Space is held down");
			state = !state;
			rain.SetActive (state);
			Stick.SetActive (state);

		} 
		if (state == true) {
			User_Interaction_state = true;
		} else {
			User_Interaction_state = false;
		}

	}
}
