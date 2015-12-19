using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree : MonoBehaviour
{
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;
	public GameObject participant;
	public GameObject police;

	private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.R) == true) {
			//behaviorAgent.StopBehavior ();
			behaviorAgent = new BehaviorAgent (this.ST_ApproachAndWait(this.wander1));
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
		}



		if (Input.GetKeyDown (KeyCode.T) == true) {
			//behaviorAgent.StopBehavior ();
			behaviorAgent = new BehaviorAgent (this.ST_ApproachAndWait(this.wander2));
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
		}

		if (Input.GetKeyDown (KeyCode.Y) == true) {
			//behaviorAgent.StopBehavior ();
			behaviorAgent = new BehaviorAgent (this.ST_ApproachAndWait(this.wander3));
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
		}
	
	}

	protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), 
		                    	new DecoratorLoop(new LeafWait(10000)));
	}


	protected Node BuildTreeRoot()
	{

		Node root = new DecoratorLoop (
						new Sequence(
						this.ST_ApproachAndWait(this.wander1),
						this.ST_ApproachAndWait(this.wander3)));


		return root;
	}
}
