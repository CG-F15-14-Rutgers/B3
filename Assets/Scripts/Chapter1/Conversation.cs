using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class Conversation : MonoBehaviour {

	public Behaviour P1;
	public Behaviour P2;

	
	public Node Converse(string scenceName){

		switch (scenceName.ToUpper()){
			case "NORMAL":
				return 
				new Sequence( 	  
				             P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("CLAP", true), 
				             P2.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("TALKINGTWO", true), new LeafWait (2000),				       
				             P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("CLAP", false), 
				             P2.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("TALKINGTWO", false), new LeafWait (2000),

				             P1.gameObject.GetComponent<BehaviorMecanim>().Node_FaceAnimation("ROAR", true), 
				             P2.gameObject.GetComponent<BehaviorMecanim>().Node_HandAnimation("SURPRISED", true), new LeafWait(4000),
				             P1.gameObject.GetComponent<BehaviorMecanim>().Node_FaceAnimation("ROAR", false),  
				             P2.gameObject.GetComponent<BehaviorMecanim>().Node_HandAnimation("SURPRISED", false), new LeafWait(4000),

				             //P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("HEADSHAKETHINK", true),  
				             P2.gameObject.GetComponent<BehaviorMecanim> ().Node_FaceAnimation ("HEADNOD", true), new LeafWait (2000),
				             //P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("HEADSHAKETHINK", false), 
				             P2.gameObject.GetComponent<BehaviorMecanim> ().Node_FaceAnimation ("HEADNOD", false), new LeafWait (2000),

				             //P1.gameObject.GetComponent<BehaviorMecanim>().Node_HandAnimation("SHOCK", true),  
				             P2.gameObject.GetComponent<BehaviorMecanim>().Node_HandAnimation("CRY", true), new LeafWait(2000),
				             //P1.gameObject.GetComponent<BehaviorMecanim>().Node_HandAnimation("SHOCK", false),
				             P2.gameObject.GetComponent<BehaviorMecanim>().Node_HandAnimation("CRY", false), new LeafWait(2000),

				             P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("CLAP", true), 
				             //P1.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("TALKINGTWO", true), 
				             P2.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("TALKINGONE", true), new LeafWait (2000),
				             P1.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("CLAP", false), 
				             //P1.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("TALKINGTWO", false),
				             P2.gameObject.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("TALKINGONE", false), new LeafWait (2000)

				             //this.TalkingShufle()
 
				            );
				break;
			case "":
				return
					new LeafWait(1000);
				break;
		}
		
		return
			new LeafWait(1000);
	}



}
