using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;


public class Chapter3root : MonoBehaviour {

	public GameObject MainCharactor;
	public GameObject InteractiveChactorP1;
	public GameObject InteractiveChactorP2;
	public GameObject InteractiveChactorP3;
	public GameObject InteractiveChactorP4;
	public GameObject Camera0;
	public GameObject Camera1;
	public GameObject Camera2;
	public GameObject Camera3;
	
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
		Camera0.SetActive (false);
		Camera1.SetActive (false);
		Camera2.SetActive (false);
		Camera3.SetActive (true);
		behaviorAgent = new BehaviorAgent (this.BuildRootTree ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	void OnGUI(){
		GUIStyle style = new GUIStyle ();
		style.richText = true;
		style.normal.textColor = Color.white;
		style.fontSize = 24;
		GUILayout.BeginArea (new Rect (0, 10, 600, 400));
		GUILayout.BeginVertical ("box");
		GUILayout.Label ("Interaction use ethan press U !",style);
		GUILayout.Label ("Interaction use Dainel press T !",style);
		GUILayout.Label ("Interaction use the girl press Y !",style);
		GUILayout.Label ("Press R To Reset to choose another charactor!",style);
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if ( Input.GetKeyDown(KeyCode.R) == true) {
			Application.LoadLevel(Application.loadedLevel);
		}
		if (Input.GetKeyDown(KeyCode.T) == true) {
			paticipanter = InteractiveChactorP1;
			behaviorAgent = new BehaviorAgent (this.ST_StopFight());
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
			Camera0.SetActive (false);
			Camera1.SetActive (true);
			Camera2.SetActive (false);
			Camera3.SetActive (false);
		}
		
		if (Input.GetKeyDown(KeyCode.Y) == true) {
			paticipanter = InteractiveChactorP2;
			behaviorAgent = new BehaviorAgent (this.ST_StopFight());
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
			Camera0.SetActive (false);
			Camera1.SetActive (false);
			Camera2.SetActive (true);
			Camera3.SetActive (false);
		}
		
		if (Input.GetKeyDown(KeyCode.U) == true) {
			paticipanter = MainCharactor;
			behaviorAgent = new BehaviorAgent (this.ST_StopFight());
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
			Camera0.SetActive (true);
			Camera1.SetActive (false);
			Camera2.SetActive (false);
			Camera3.SetActive (false);
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


	protected Node ST_StopFight(){

		Val<Vector3> position = Val.V (() => StopFightPos.position);
		Val<Vector3> face = Val.V (() => InteractiveChactorP3.transform.position);
		return new Sequence(
			paticipanter.GetComponent<BehaviorMecanim>().Node_GoTo(position),
			new LeafWait(2000),
			new Sequence (
			InteractiveChactorP3.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("ATTACK", false), InteractiveChactorP4.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("HIT", false),
			//new LeafWait(1000),
			InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT",false), InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT",false)
			),  
			InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_OrientTowards(position),
			InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_OrientTowards(position),
			paticipanter.GetComponent<BehaviorMecanim>().Node_OrientTowards(face), 
			InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGONE", true),
			InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGTWO", true),
			paticipanter.GetComponent<BehaviorMecanim>().Node_HandAnimation("CLAP", true),
			new LeafWait(4000),
			InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGONE", false),
			InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKINGTWO", false),
			paticipanter.GetComponent<BehaviorMecanim>().Node_HandAnimation("CLAP", false),
			InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DUCK", true),
			InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DUCK", true),
			new LeafWait(4000),
			InteractiveChactorP3.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DUCK", false),
			InteractiveChactorP4.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DUCK", false),
			ST_EndTree()
			);
	}

	
	protected Node BuildRootTree(){
		return new DecoratorLoop (
			new SequenceParallel(
			ST_Fighter()
			)
			);
		
	}
}
