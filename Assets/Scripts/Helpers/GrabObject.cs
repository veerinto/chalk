/* File: GrabObject.cs
 * Description: #DESCRIPTION#
 * How to use: #INSTRUCTIONS#
*/

using UnityEngine;
using System.Collections;
using Interlude;

namespace Interlude {
	[RequireComponent(typeof(Rigidbody))]
	public class GrabObject : MonoBehaviour
	{
		private Rigidbody attachPoint;

		bool usedGravity = false;

		SteamVR_TrackedObject trackedObj;
		FixedJoint joint;

	//	public PhotonView grabbedPhotonView;

		void Awake()
		{
			trackedObj = GetComponentInParent<SteamVR_TrackedObject>();
		}

		void Start() {
			attachPoint = GetComponent<Rigidbody> ();
			attachPoint.useGravity = false;
		}

		void FixedUpdate()
		{
			var device = SteamVR_Controller.Input((int)trackedObj.index);	

			if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
			{
				releaseObject ();
			}
		}

		void OnTriggerStay(Collider other) {
			if (other.attachedRigidbody)
			{			
				grabObject (other.gameObject);
			}
		}

		void grabObject(GameObject objectToGrab) {
			if (objectToGrab.tag == "Grabbable"
				&& objectToGrab.GetComponent<Grabbable> () == null) {
				objectToGrab.AddComponent<Grabbable> ();
			}

			var device = SteamVR_Controller.Input((int)trackedObj.index);

			Grabbable grabbedObject = objectToGrab.GetComponent<Grabbable>();
			if (grabbedObject)
			{
				if (joint == null && !grabbedObject.isGrabbed
						&& device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
	//				grabbedPhotonView = objectToGrab.GetComponent<PhotonView> ();
					attachObject (objectToGrab);
					grabbedObject.isGrabbed = true;
					grabbedObject.GrabberIndex = (int)trackedObj.index;
				} 
				if (!joint) {
					device.TriggerHapticPulse (600);
				}
			}
		}

		public bool attachObject(GameObject objectToAttach)
		{
			if (joint == null) {

				usedGravity = objectToAttach.GetComponent<Rigidbody> ().useGravity;
				objectToAttach.GetComponent<Rigidbody> ().useGravity = false;
	//			grabbedPhotonView.RPC ("disableGravity", PhotonTargets.All);

				joint = objectToAttach.AddComponent<FixedJoint> ();
				attachPoint.gameObject.SetActive (true);
				joint.connectedBody = attachPoint;

				return true;
			} else {
				return false;
			}
		}

		void releaseObject() {
			var device = SteamVR_Controller.Input((int)trackedObj.index);

			if (joint) {
				GameObject grabbedObject = joint.gameObject;
				Rigidbody grabbedObjectRb = grabbedObject.GetComponent<Rigidbody> ();

				Debug.Log(device.velocity);

				grabbedObjectRb.useGravity = usedGravity;
	//			if(usedGravity)	grabbedPhotonView.RPC ("enableGravity", PhotonTargets.All);
				grabbedObjectRb.isKinematic = false;

				Object.DestroyImmediate(joint);
				joint = null;

				Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
				if (origin) {
					grabbedObjectRb.velocity = origin.TransformVector (device.velocity);
					grabbedObjectRb.angularVelocity = origin.TransformVector (device.angularVelocity);
				} else {
					grabbedObjectRb.velocity = device.velocity;
					grabbedObjectRb.angularVelocity = device.angularVelocity;
				}


				grabbedObject.GetComponent<Grabbable> ().isGrabbed = false;

				grabbedObjectRb.maxAngularVelocity = grabbedObjectRb.angularVelocity.magnitude;
			}		
		}

		public bool isHoldingSomething()
		{
			return (joint != null);
		}

		public IEnumerator triggerFeedback(ushort hapticForce)	{
			var device = SteamVR_Controller.Input((int)trackedObj.index);	

			for (int i=0; i < 3; i++){
				device.TriggerHapticPulse (hapticForce);
				yield return null;
			}
		}
	}
}
