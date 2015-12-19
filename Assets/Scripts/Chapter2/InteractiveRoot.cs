using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;


public class InteractiveRoot : MonoBehaviour {

	public GameObject MainCharactor;
	public GameObject InteractiveChactorP1;
	public GameObject InteractiveChactorP2;
	public GameObject InteractiveChactorP3;
	public GameObject InteractiveChactorP4;

	public Transform WanderingPos1;
	public Transform WanderingPos2;
	public Transform ConversationPos1;
	public Transform ConversationPos2;
	public Transform Sitpos;
	public Transform StopFightPos;

	private BehaviorAgent behaviorAgent;
	private GameObject paticipanter;
	private bool interact_war;
	private bool interact_girl;
	private bool interact_phone;
	private bool interact_reset;

	// Use this for initialization
	void Start () {
		
		behaviorAgent = new BehaviorAgent (this.BuildRootTree ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}

	void OnGUI(){
		GUIStyle style = new GUIStyle ();
		style.richText = true;
		style.normal.textColor = Color.white;
		GUILayout.BeginArea (new Rect (0, 10, 300, 400));
		GUILayout.BeginVertical ("box");
		GUILayout.Label ("Interaction with girl press U !",style);
		GUILayout.Label ("Interaction with Crowd press T !",style);
		GUILayout.Label ("Interaction with man press Y !",style);
		GUILayout.Label ("Get stoke press R!",style);
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if ( Input.GetKeyDown(KeyCode.R) == true) {
			Application.LoadLevel(Application.loadedLevel);
		}
		if (Input.GetKeyDown(KeyCode.T) == true) {
			behaviorAgent = new BehaviorAgent (this.ST_StopFight());
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
		}

		if (Input.GetKeyDown(KeyCode.Y) == true) {
			behaviorAgent = new BehaviorAgent (this.ST_AskPhoneBack());
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
		}

		if (Input.GetKeyDown(KeyCode.U) == true) {
			behaviorAgent = new BehaviorAgent (this.ST_AskDance());
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
		}


	}

	protected Node ST_EndTree(){
		return new DecoratorLoop (
			new Sequence( new LeafWait(2000))
			);
	}

	protected Node ST_Fighting(){
		//Debug.Log ("Fighting");
		return new Sequence (InteractiveChactorP3.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("ATTACK", true), InteractiveChactorP4.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("HIT", true));
	}

	protected Node ST_Fighter(){
		//Debug.Log ("Start Fighting");
		Val<Vector3> P3position = Val.V (() => InteractiveChactorP3.transform.position); /* Charator face to each other */
		Val<Vector3> P4position = Val.V (() => InteractiveChactorP4.transform.position);
		return 
				new Sequence (
					              InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_OrientTowards(P4position),
								  InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_OrientTowards(P3position),
					              InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT",true), 
								  InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT",true),
								  ST_EndTree()
			);
	}

	protected Node ST_GirlSit(){
		Val<Vector3> position = Val.V (() => Sitpos.position);
		return new Sequence (
			InteractiveChactorP2.GetComponent<BehaviorMecanim>().Node_OrientTowards(position),
			InteractiveChactorP2.GetComponent<BehaviorMecanim>().Node_SitDown(),  ST_EndTree()
			);
	}

	protected Node ST_ManWandering(){
		Val<Vector3> position1 = Val.V (() => WanderingPos1.position);
		Val<Vector3> position2 = Val.V (() => WanderingPos2.position);

		return 
			new Sequence (
			       
			InteractiveChactorP1.GetComponent<BehaviorMecanim>().Node_GoTo(position1),
			InteractiveChactorP1.GetComponent<BehaviorMecanim>().Node_GoTo(position2),
			ST_EndTree()
			);
	}

	protected Node ST_AskDance(){
		Val<Vector3> position = Val.V (() => Sitpos.position);
		Val<Vector3> face = Val.V (() => InteractiveChactorP2.transform.position);
		return new Sequence(
			MainCharactor.GetComponent<BehaviorMecanim>().Node_GoTo(position),
			InteractiveChactorP2.GetComponent<BehaviorMecanim>().Node_StandUp(),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_OrientTowards(face),
			InteractiveChactorP2.GetComponent<BehaviorMecanim>().Node_OrientTowards(position),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_BodyAnimation("GANASTYLE", true),
			new LeafWait(2000),
			InteractiveChactorP2.GetComponent<BehaviorMecanim>().Node_BodyAnimation("GANASTYLE", true),
			new LeafWait(5000),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_BodyAnimation("GANASTYLE", false),
			new LeafWait(2000),
			InteractiveChactorP2.GetComponent<BehaviorMecanim>().Node_BodyAnimation("GANASTYLE", false),
			ST_EndTree()
			);
	}

	protected Node ST_StopFight(){
		Val<Vector3> position = Val.V (() => StopFightPos.position);
		Val<Vector3> face = Val.V (() => InteractiveChactorP3.transform.position);
		return new Sequence(
			MainCharactor.GetComponent<BehaviorMecanim>().Node_GoTo(position),
			new LeafWait(2000),
			new Sequence (
			 InteractiveChactorP3.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("ATTACK", false), InteractiveChactorP4.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("HIT", false),
			//new LeafWait(1000),
			 InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT",false), InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT",false)
			),  
			InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_OrientTowards(position),
			InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_OrientTowards(position),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_OrientTowards(face), 
			InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGONE", true),
			InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGTWO", true),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_HandAnimation("CLAP", true),
			new LeafWait(4000),
			InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGONE", false),
			InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGTWO", false),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_HandAnimation("CLAP", false),
			ST_EndTree()
			);
	}

	protected Node ST_AskPhoneBack(){
		Val<Vector3> position1 = Val.V (() => ConversationPos1.position);
		Val<Vector3> position2 = Val.V (() => ConversationPos2.position);
		return new Sequence(
			MainCharactor.GetComponent<BehaviorMecanim>().Node_GoTo(position2),
			InteractiveChactorP1.GetComponent<BehaviorMecanim>().Node_GoTo(position1),
			new LeafWait(2000),
			InteractiveChactorP1.GetComponent<BehaviorMecanim>().Node_OrientTowards(position2),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_OrientTowards(position1), 
			InteractiveChactorP1.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGONE", true),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGTWO", true),
			new LeafWait(4000),
			InteractiveChactorP1.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGONE", false),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGTWO", false),
			InteractiveChactorP1.GetComponent<BehaviorMecanim>().Node_HandAnimation("CLAP", true),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_HandAnimation("CHEER", true),
			new LeafWait(2000),
			MainCharactor.GetComponent<BehaviorMecanim>().Node_HandAnimation("CHEER", false),
			InteractiveChactorP1.GetComponent<BehaviorMecanim>().Node_HandAnimation("CLAP", false),
			ST_EndTree()
			);
	}

	protected Node BuildRootTree(){
		return new DecoratorLoop (
				   new SequenceParallel(
					ST_Fighter(), ST_GirlSit(), ST_ManWandering()
					)
				);

	}

}
