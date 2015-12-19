using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree_oldversion : MonoBehaviour
{
	public Transform wander1;
	public Transform ConversePosition;
	public Transform PhonePosition;
	public Transform SitPosition;
	public Transform DancePosition;
	public Transform BenchPosition;
	public Transform SleepBenchPosition;
	public Transform ShelterPosition;
	public Transform PickUp;
	public GameObject participant_A;
	public GameObject participant_B;
	public GameObject participant_C;

	private bool User_Interaction_stat;
	private ActivateRain _otherScript;
	private BehaviorAgent behaviorAgent;
	private GameObject participant;
	private bool old_stat;
	private bool new_stat;

	void Awake()
	{
		_otherScript = gameObject.GetComponent<ActivateRain> ();
	}
	// Use this for initialization


	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		old_stat = User_Interaction_stat;
		new_stat = _otherScript.User_Interaction_state;
		User_Interaction_stat = new_stat;
		//BehaviorManager.Instance.Register (behaviorAgent);
		//print (User_Interaction_stat);
		/*if (User_Interaction_stat == true) {
			behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
		}*/

	}

	protected Node ST_PlayerA()
	{
		participant = participant_A;

			
		if (User_Interaction_stat == false) {
			return	new DecoratorLoop (
				new SequenceShuffle (
					this.ST_ApproachAndRandom (this.wander1),
					this.ST_ApproachAndConverse (this.ConversePosition,participant_B),
					this.ST_ApproachAndSit (this.SitPosition, this.BenchPosition)));
		} else {
			return	
				new SequenceShuffle (
					this.ST_ApproachAndWait (this.ShelterPosition));
		}
	}

	protected Node ST_PlayerB()
	{
		participant = participant_B;
		
		
		if (User_Interaction_stat == false) {
			return	new DecoratorLoop (
				new SequenceShuffle (
				this.ST_ApproachAndCall (this.PhonePosition),
				this.ST_ApproachAndConverse (this.ConversePosition, participant_A),
				this.ST_ApproachAndDance(this.DancePosition,"GANASTYLE")));
		} else {
			return	
				new SequenceShuffle (
					this.ST_ApproachAndWait (this.ShelterPosition));
		}
	}

	protected Node ST_PlayerC()
	{
		participant = participant_C;

		
		if (User_Interaction_stat == false) {
			return	new DecoratorLoop (
				new SequenceShuffle (
				this.ST_ApproachAndWait (this.PickUp),
				this.ST_Chase (participant_A)
				));
		} else {
			return	
				new SequenceShuffle (
					this.ST_ApproachAndWait (this.ShelterPosition));
		}
	}



	
	protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

	protected Node ST_ApproachAndRandom(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000),ST_RandomGesture());
	}

	protected Node ST_ApproachAndDance(Transform target, string name)
	{	
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000), this.ST_Dance(name));
	}

	protected Node ST_Chase(GameObject tar)
	{
		Val<Vector3> position = Val.V (tar.GetComponent<Transform> ().position);
		return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(100));
	}

	protected Node ST_ApproachAndConverse(Transform target, GameObject obj)
	{	
		Val<Vector3> position = Val.V (() => target.position);
		Val<Vector3> tar = Val.V (obj.GetComponent<Transform> ().position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(tar),this.ST_Wait(obj,position),new LeafWait (2000));
	}

	protected Node ST_ApproachAndCall(Transform target)
	{	
		Val<Vector3> position = Val.V (() => target.position);

		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait (2000),this.ST_TalkPhone());
	}

	protected Node ST_Wait(GameObject obj, Val<Vector3> position)
	{
	
			if (Is_Near (obj.GetComponent<Transform> ().position, position.Value) == true){
				return new LeafWait (200);
			}
		return new LeafWait (10000);
	}

	protected bool Is_Near(Vector3 P1, Vector3 P2){
		float diffx = P2.x - P1.x;
		float diffz = P2.z - P1.z;
		float distance = diffx * diffx + diffz * diffz;
		return distance < 4 * 4;
	}

	protected Node ST_ApproachAndSit(Transform target, Transform obj)
	{	
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000),this.ST_Sit(obj));
	}

	protected Node ST_ApproachAndSleep(Transform target, Transform obj)
	{	
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000),this.ST_Sleep(obj));
	}

	protected Node ST_Dance(string name)
	{
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation(name, true), new LeafWait(12000), participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation(name, false),new LeafWait(1000));
	}

	protected Node ST_Sit(Transform obj)
	{	
		return new Sequence ( this.ST_FaceOpsiteX(obj),participant.GetComponent<BehaviorMecanim>().Node_SitDown(), new LeafWait (3000),participant.GetComponent<BehaviorMecanim>().Node_StandUp(), new LeafWait (1000));
	}

	protected Node ST_Sleep(Transform obj)
	{	
		return new Sequence ( this.ST_FaceOpsiteZ(obj),participant.GetComponent<BehaviorMecanim>().Node_LyingDown(), new LeafWait (3000),participant.GetComponent<BehaviorMecanim>().Node_StandUp_Sleep(), new LeafWait (1000));
	}

	protected Node ST_FaceOpsiteX(Transform target)
	{		
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence (participant.GetComponent<BehaviorMecanim> ().ST_TurnToBackX (position), new LeafWait (1000));
	}

	protected Node ST_FaceOpsiteZ(Transform target)
	{		
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence (participant.GetComponent<BehaviorMecanim> ().ST_TurnToBackZ (position), new LeafWait (1000));
	}

	protected Node ST_RandomGesture()
	{
		return new SelectorShuffle (this.ST_Fight(),this.ST_Duck());
	}

	protected Node ST_TalkPhone()
	{
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKING ON PHONE", true), new LeafWait(2000), participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKING ON PHONE", false),new LeafWait(1000));
	}

	protected Node ST_Duck()
	{
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DUCK", true), new LeafWait(2000), participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DUCK", false),new LeafWait(1000));
	}

	protected Node ST_Fight()
	{
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT", true), new LeafWait(2000), participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT", false),new LeafWait(1000));
	}




	/*protected Node ST_EyeContectAndConverse()
	{

	}*/

	protected Node BuildTreeRoot()
	{
		return
			new DecoratorLoop(
				new SequenceParallel(
					this.ST_PlayerA(),
					this.ST_PlayerB(),
					this.ST_PlayerC()
					));
	}
}
