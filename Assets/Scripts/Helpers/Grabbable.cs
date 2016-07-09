/* File: Grabbable.cs
 * Description: #DESCRIPTION#
 * How to use: #INSTRUCTIONS#
*/

using UnityEngine;
using System.Collections;
using Interlude;

namespace Interlude {
	public class Grabbable : MonoBehaviour {

		public bool isGrabbed = false;
		public bool isObjectNetworked = true;

	    public int GrabberIndex { get; set; }

		Rigidbody grabbedRb;

	//	void Start()
	//	{
	//		grabbedRb = GetComponent<Rigidbody> ();
	//	}
	//
	//	[PunRPC]
	//	void enableGravity()
	//	{
	//		grabbedRb.useGravity = true;
	//	}
	//
	//	[PunRPC]
	//	void disableGravity()
	//	{
	//		grabbedRb.useGravity = false;
	//	}
	}
}
