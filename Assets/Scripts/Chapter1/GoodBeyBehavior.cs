using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class GoodBeyBehavior : MonoBehaviour {

	public Behaviour P1;
	public Behaviour P2;
	public Transform LeavePos_P1;
	public Transform LeavePos_P2;
	public Transform LeavePosMid_P1;
	public Transform LeavePosMid_P2;
	public Transform OrientPos;


	private Node ST_STEPBACK(){
		return new Sequence (
			P1.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("STEPBACK", true), 
			P2.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("STEPBACK", true), new LeafWait (2000),
			P1.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("STEPBACK", false),
			P2.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("STEPBACK", false), new LeafWait (2000)
			);
	}

	private Node ST_WAVE(){
		Val<Vector3> P2position = Val.V (() => this.P2.transform.position); /* Charator face to each other */
		Val<Vector3> P1position = Val.V (() => this.P1.transform.position);
		return new Sequence (
			P1.gameObject.GetComponent<BehaviorMecanim> ().Node_OrientTowards (P2position),
			P2.gameObject.GetComponent<BehaviorMecanim> ().Node_OrientTowards (P1position),
			P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("WAVE", true), 
			P2.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("WAVE", true), new LeafWait (2000),
			P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("WAVE", false), 
			P2.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("WAVE", false), new LeafWait (2000)
			);
	}

	private Node ST_Leave(){
		Val<Vector3> P2position = Val.V (() => this.LeavePos_P2.position); /* Charator face to each other */
		Val<Vector3> P1position = Val.V (() => this.LeavePos_P1.position);
		Val<Vector3> Towards = Val.V (() => this.OrientPos.position);
		return new SequenceParallel (
			P1.gameObject.GetComponent<BehaviorMecanim>().Node_GoTo(P1position),
			P2.gameObject.GetComponent<BehaviorMecanim>().Node_GoTo(P2position), new LeafWait(2000)

			);
	}

	private Node ST_Orient(){
		Val<Vector3> Towards = Val.V (() => this.OrientPos.position);
		return new Sequence (
			P2.gameObject.GetComponent<BehaviorMecanim> ().Node_OrientTowards (Towards)
		);
	}

	private Node ST_Mid(){
		Val<Vector3> P2position = Val.V (() => this.LeavePosMid_P2.position); /* Charator face to each other */
		Val<Vector3> P1position = Val.V (() => this.LeavePosMid_P1.position);

		return new SequenceParallel (
			P1.gameObject.GetComponent<BehaviorMecanim>().Node_GoTo(P1position),
			P2.gameObject.GetComponent<BehaviorMecanim>().Node_GoTo(P2position), new LeafWait(1000)

			);
	}

	public Node LeaveTree(){
		return new Sequence (
				this.ST_STEPBACK(), this.ST_Mid(), this.ST_WAVE(), this.ST_Leave(), this.ST_Orient()
			);
	}
}
