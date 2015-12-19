using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;


public class RootTree : MonoBehaviour {

	public InteractionSystem interactionSystem_p1; // Reference to the InteractionSystem of the character
	public InteractionSystem interactionSystem_p2;
	public Behaviour P1;

	public bool interrupt; // Can we interrupt an interaction of an effector?
	
	// The interaction objects
	public InteractionObject benchMain, benchHands, phone;
	
	private bool Sit_f;
	private bool Pick_up;
	private bool isSitting;

	private MeetBehavior ST_meet;
	private ConversationBehavior ST_conversation;
	private GoodBeyBehavior ST_leave;
	private BehaviorAgent behaviorAgent;
	private int lines;
	public GameObject Camera0;
	public GameObject Camera1;
	public GameObject Camera2;

	public Texture CharactorP1;
	public Texture CharactorP2;
	public Texture CharactorP3;
	public Texture Phone;

	// GUI for calling the interactions
	void OnGUI() {
		GUIStyle style = new GUIStyle ();
		style.richText = true;
		style.normal.textColor = Color.white;
		style.fontSize = 50;

		GUILayout.BeginArea (new Rect (0, 600, Screen.width, 400));
		switch(lines){
				case 1:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (CharactorP1);
				GUILayout.Label ("Hi Eva, What'up",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
				case 2:
				GUILayout.BeginHorizontal ("box");
				GUILayout.FlexibleSpace ();
				GUILayout.Label ("Hi, nothing much",style);
				GUILayout.Label (CharactorP2);
				GUILayout.EndHorizontal ();
				break;
				case 3:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (CharactorP1);
				GUILayout.Label ("What are u doing here?",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
				case 4:
				GUILayout.BeginHorizontal ("box");
				GUILayout.FlexibleSpace ();
				GUILayout.Label ("I skip the graphic class to have a drink, why are you here?",style);
				GUILayout.Label (CharactorP2);
				GUILayout.EndHorizontal ();
				break;
				case 5:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (CharactorP1);
				GUILayout.Label ("Watching the fight",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
				case 6:
				GUILayout.BeginHorizontal ("box");
				GUILayout.FlexibleSpace ();
				GUILayout.Label ("Have you submitted B3 assignment?",style);
				GUILayout.Label (CharactorP2);
				GUILayout.EndHorizontal ();
				break;
				case 7:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (CharactorP1);
				GUILayout.Label ("The assignment is too hard to do, my teammate will handle that.",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
				case 8:
				GUILayout.BeginHorizontal ("box");
				GUILayout.FlexibleSpace ();
				GUILayout.Label ("Do you know how to interupt the behavior tree?",style);
				GUILayout.Label (CharactorP2);
				GUILayout.EndHorizontal ();
				break;
				case 9:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (CharactorP1);
				GUILayout.Label ("I dont know anything.",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
				case 10:
				GUILayout.BeginHorizontal ("box");
				GUILayout.FlexibleSpace ();
				GUILayout.Label ("I have to go back to code. See you",style);
				GUILayout.Label (CharactorP2);
				GUILayout.EndHorizontal ();
				break;
				case 11:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (CharactorP1);
				GUILayout.Label ("See you", style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
				case 12:
				GUILayout.BeginHorizontal ("box");
				GUILayout.FlexibleSpace ();
				GUILayout.Label ("Bye",style);
				GUILayout.Label (CharactorP2);
				GUILayout.EndHorizontal ();
				break;
				case 13:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (CharactorP1);
				GUILayout.Label ("Bye",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
			
				case 14:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (CharactorP1);
				GUILayout.Label ("Whos phone here?",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
			
				case 15:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (CharactorP1);
				GUILayout.Label ("I find a iphone6s plus.",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
				case 16:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (Phone);
				GUILayout.Label ("Ring Ring Ring....",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
				case 17:
				GUILayout.BeginHorizontal ("box");
				GUILayout.Label (CharactorP1);
				GUILayout.Label ("Hey man. I find you phone",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
				case 18:
				GUILayout.BeginHorizontal ("box");
				GUILayout.FlexibleSpace ();
				GUILayout.Label ("Thank you. i will come to pick up. Where r u?",style);
				GUILayout.Label (CharactorP3);
				GUILayout.EndHorizontal ();
				break;
				case 19:
				GUILayout.BeginHorizontal ("box");
			    GUILayout.Label (CharactorP1);
				GUILayout.Label ("I am at Plaza, see you later",style);
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				break;
				case 0:
				
				break;
		
		}
		GUILayout.EndArea();
	}
		
	// Use this for initialization
	void Start () {

		Sit_f = false;
		Pick_up = false;
		ST_meet= this.GetComponent<MeetBehavior> ();
		ST_conversation = this.GetComponent<ConversationBehavior> ();
		ST_leave = this.GetComponent<GoodBeyBehavior> ();
		behaviorAgent = new BehaviorAgent (this.BuildRootTree());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();


	}

	void FixedUpdate(){
		// While seated
	
		   if (!Sit_f) {
				interactionSystem_p1.ResumeAll();
			}
			
		// This is a multiple-effector interaction
		if (Sit_f) {
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.Body, benchMain, interrupt);
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.LeftThigh, benchMain, interrupt);
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.RightThigh, benchMain, interrupt);
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.LeftFoot, benchMain, interrupt);
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.LeftHand, benchHands, interrupt);
			interactionSystem_p1.StartInteraction(FullBodyBipedEffector.RightHand, benchHands, interrupt);

		}
		

	}

	// Update is called once per frame
	void Update () { 


		Debug.Log (Time.realtimeSinceStartup);
		lines = 0;

		MyLines ();
		if (Time.realtimeSinceStartup < 10.0f) {
			Camera0.SetActive (true);
			Camera1.SetActive (false);
			Camera2.SetActive (false);
		}
		if (Time.realtimeSinceStartup > 17.0f) {
			Camera0.SetActive (false);
			Camera1.SetActive (true);
			Camera2.SetActive (false);
		}

		if (Time.realtimeSinceStartup > 55.0f) {	
			Camera0.SetActive (false);
			Camera1.SetActive (false);
			Camera2.SetActive (true);
		}
		if (Time.realtimeSinceStartup > 65.0f) {
			Sit_f = true;
			Pick_up = true;
		}
			
		if (Time.realtimeSinceStartup > 70.0f) {
			Pick_up = false;
		}

		if (!Pick_up) {
			interactionSystem_p2.ResumeAll();
		}
		
		if (Pick_up) {
			interactionSystem_p2.StartInteraction(FullBodyBipedEffector.RightHand, phone, true);
		}
	}

	void MyLines(){
		lines = 0;
		if (Time.realtimeSinceStartup > 9.0f)
			lines = 1;
		if (Time.realtimeSinceStartup > 10.0f)
			lines = 2;
		if (Time.realtimeSinceStartup > 18.0f)
			lines = 3;
		if (Time.realtimeSinceStartup > 21.0f)
			lines = 4;
		if (Time.realtimeSinceStartup > 24.0f)
			lines = 5;
		if (Time.realtimeSinceStartup > 27.0f)
			lines = 6;
		if (Time.realtimeSinceStartup > 30.0f)
			lines = 7;
		if (Time.realtimeSinceStartup > 33.0f)
			lines = 8;
		if (Time.realtimeSinceStartup > 36.0f)
			lines = 9;
		if (Time.realtimeSinceStartup > 39.0f)
			lines = 10;
		if (Time.realtimeSinceStartup > 42.0f)
			lines = 11;
		if (Time.realtimeSinceStartup > 50.0f)
			lines = 12;
		if (Time.realtimeSinceStartup > 52.0f)
			lines = 13;
		if (Time.realtimeSinceStartup > 55.0f)
			lines = 0;
		if (Time.realtimeSinceStartup > 67.0f)
			lines = 14;
		if (Time.realtimeSinceStartup > 70.0f)
			lines = 15;
		if (Time.realtimeSinceStartup > 72.0f)
			lines = 16;
		if (Time.realtimeSinceStartup > 80.0f)
			lines = 17;
		if (Time.realtimeSinceStartup > 85.0f)
			lines = 18;
		if (Time.realtimeSinceStartup > 92.0f)
			lines = 19;
	}



	protected Node ST_EndTree(){
		return new DecoratorLoop (
				new Sequence( new LeafWait(2000))
			);
    }

	protected Node ST_TextAndCall(){
		return new Sequence (
				P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("TEXTING", true),
				new LeafWait(3000),
			    P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("TEXTING", false),
			    new LeafWait(3000),
				P1.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("TALKING ON PHONE", true), 
				new LeafWait(3000),
				P1.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("TALKING ON PHONE", false), 
				ST_EndTree()
			);
	}

	protected Node BuildRootTree(){
		return new Sequence (
			ST_meet.Meeting(), new LeafWait(2000),
			ST_conversation.ConversationTree (), new LeafWait(2000),
			ST_leave.LeaveTree(), new LeafWait(10000),this.ST_TextAndCall()
			);
	}
}
