using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class Triggel : MonoBehaviour {

	private InteractionSystem interactionSystem;
	[SerializeField] InteractionObject interactionObject; // The object to interact to
	[SerializeField] FullBodyBipedEffector[] effectors; // The effectors to interact with
	
	void Awake() {
		interactionSystem = GetComponent<InteractionSystem>();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(){
		print ("Enter the trigger");
		foreach (FullBodyBipedEffector e in effectors) {
			interactionSystem.StartInteraction(e, interactionObject, true);
		}
	}
}
