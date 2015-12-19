using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class MeetBehavior : MonoBehaviour {

	public Behaviour P1;
	public Behaviour P2;
	public Transform startPosition_P1;
	public Transform startPosition_P2;
	//private BehaviorAgent behaviorAgent;




	protected Node gotoStartPostion(){
		Val<Vector3> P2position = Val.V (() => this.startPosition_P2.position); /* Charator face to each other */
		Val<Vector3> P1position = Val.V (() => this.startPosition_P1.position);
		return new Sequence ( P1.gameObject.GetComponent<BehaviorMecanim>().Node_GoTo(P1position),
		                      P2.gameObject.GetComponent<BehaviorMecanim>().Node_GoTo(P2position));
	}

	protected Node OrientedAndGreat(){
		Val<Vector3> P2position = Val.V (() => this.P2.transform.position); /* Charator face to each other */
		Val<Vector3> P1position = Val.V (() => this.P1.transform.position);

		return new Sequence (
			P1.gameObject.GetComponent<BehaviorMecanim> ().Node_OrientTowards (P2position),
			P2.gameObject.GetComponent<BehaviorMecanim> ().Node_OrientTowards (P1position),
			   
			P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("WAVE", true), 
			P2.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("WAVE", true),

			P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("WAVE", false), new LeafWait (1000),
			P2.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("WAVE", false), new LeafWait (1000)
		);
	
	}

 	public Node Meeting(){
		return new Sequence (
			 gotoStartPostion (),
			 OrientedAndGreat ()
		);
	}

}
