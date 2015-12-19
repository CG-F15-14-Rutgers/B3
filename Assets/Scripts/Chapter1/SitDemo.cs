using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;


	
	/// <summary>
	/// Simple demo controller for the InteractionSystem.
	/// </summary>
	public class InteractionDemo : MonoBehaviour {
		
		public InteractionSystem interactionSystem; // Reference to the InteractionSystem of the character
		
		public bool interrupt; // Can we interrupt an interaction of an effector?
		
		// The interaction objects
		public InteractionObject benchMain, benchHands;
		
		private bool isSitting;
		
		// GUI for calling the interactions
		void OnGUI() {
			interrupt = GUILayout.Toggle(interrupt, "Interrupt");
			
			// While seated
			if (isSitting) {
				
				if (!interactionSystem.inInteraction && GUILayout.Button("Stand Up")) {
					interactionSystem.ResumeAll();
					
					isSitting = false;
				}
				
				return;
			}

			// This is a multiple-effector interaction
			if (!interactionSystem.inInteraction && GUILayout.Button("Sit Down")) {
				interactionSystem.StartInteraction(FullBodyBipedEffector.Body, benchMain, interrupt);
				interactionSystem.StartInteraction(FullBodyBipedEffector.LeftThigh, benchMain, interrupt);
				interactionSystem.StartInteraction(FullBodyBipedEffector.RightThigh, benchMain, interrupt);
				interactionSystem.StartInteraction(FullBodyBipedEffector.LeftFoot, benchMain, interrupt);
				
				interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, benchHands, interrupt);
				interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, benchHands, interrupt);
				
				isSitting = true;
			}
		}
	}

