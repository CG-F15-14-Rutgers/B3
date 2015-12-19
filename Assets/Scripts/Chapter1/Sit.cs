using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;

public class Sit : MonoBehaviour {

	public InteractionSystem interactionSystem_p1; // Reference to the InteractionSystem of the character
	public InteractionSystem interactionSystem_p2;
	
	public bool interrupt; // Can we interrupt an interaction of an effector?
	
	// The interaction objects
	public InteractionObject benchMain, benchHands, phone;

	private bool Sit_f;
	private bool Pick_up;
	private bool isSitting;

	
	// GUI for calling the interactions
	void OnGUI() {
		interrupt = GUILayout.Toggle(interrupt, "Interrupt");
		
		// While seated
		if (isSitting) {
			
			if (!interactionSystem_p1.inInteraction && !Sit_f) {
				interactionSystem_p1.ResumeAll();
				
				isSitting = false;
			}
			
			return;
		}
		
		// This is a multiple-effector interaction
		if (!interactionSystem_p1.inInteraction && Sit_f) {
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.Body, benchMain, interrupt);
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.LeftThigh, benchMain, interrupt);
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.RightThigh, benchMain, interrupt);
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.LeftFoot, benchMain, interrupt);
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.LeftHand, benchHands, interrupt);
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.RightHand, benchHands, interrupt);
			
			isSitting = true;
		}

		if (!interactionSystem_p2.inInteraction && !Pick_up) {
			interactionSystem_p2.ResumeAll();
		}

		if (!interactionSystem_p2.inInteraction && Pick_up) {
			interactionSystem_p2.StartInteraction(FullBodyBipedEffector.RightHand, phone, true);
		}

	}

	void Update (){
		Debug.Log (Time.realtimeSinceStartup);
		Sit_f = false;
		Pick_up = false;
		if (Time.realtimeSinceStartup > 7.0f) {
			Sit_f = true;
			Pick_up = true;
		}	
		if (Time.realtimeSinceStartup > 20.0f) {
			Sit_f = false;
			Pick_up = false;
		}
	}
}
