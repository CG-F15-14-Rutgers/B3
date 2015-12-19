using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

[RequireComponent(typeof(InteractionSystem))]

public class PickUpScript : MonoBehaviour {


	public Transform pickupPosition;
	public Transform runPosition;
	private BehaviorAgent behaviorAgent;
	public GameObject participant;




	// Use this for initialization
	void Start () {
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	protected Node ST_Player()
	{

			return	new DecoratorLoop (
				new Sequence (
					this.ST_ApproachAndWait (this.pickupPosition),
					//this.ST_GrondPickUpLeft ()
					new LeafWait(1000)
					));

	}

	protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

	protected Node ST_GrondPickUpLeft()
	{

		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("PICKUPLEFT", true), new LeafWait(1000));
	}

	protected Node BuildTreeRoot()
	{
		return
			new DecoratorLoop(
				new Sequence(
					
					this.ST_Player()
					
				));
	}
}
