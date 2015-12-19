using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;


	/// <summary>
	/// Conversation Event
	/// </summary>
	public class ConversationBehavior : MonoBehaviour {


		public Behaviour P1;
		public Behaviour P2;
		public Transform ConversationPos_P1;
	    public Transform ConversationPos_P2;

		//private BehaviorAgent behaviorAgent;
		//private GameObject p;
		private Conversation sb_t;


		protected Node ApproachAndOrient(){
	        Val<Vector3> P2_pos = Val.V (() => this.ConversationPos_P2.position);
		    Val<Vector3> P1_pos = Val.V (() => this.ConversationPos_P1.position);
			Val<Vector3> P2position = Val.V (() => this.P2.transform.position); /* Charator face to each other */
			Val<Vector3> P1position = Val.V (() => this.P1.transform.position);
			
			return new Sequence (
					P1.gameObject.GetComponent<BehaviorMecanim>().Node_GoTo(P1_pos),
					P2.gameObject.GetComponent<BehaviorMecanim>().Node_GoTo(P2_pos),
					new Sequence (
						P1.gameObject.GetComponent<BehaviorMecanim>().Node_OrientTowards(P2position),
						P2.gameObject.GetComponent<BehaviorMecanim>().Node_OrientTowards(P1position)
						)
					);
		}

		public Node ConversationTree(){
			sb_t = gameObject.GetComponent<Conversation> ();
			return new Sequence (
				this.ApproachAndOrient(),
				sb_t.Converse("NORMAL")
			);
		}

		
	}

