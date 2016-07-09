/* File: ManagerDrawLine.cs
 * Description: #DESCRIPTION#
 * How to use: #INSTRUCTIONS#
*/

using UnityEngine;
using System.Collections;
using Interlude;


namespace Interlude {
	public class ManagerDrawLine : MonoBehaviour {

		[SerializeField] SteamVR_TrackedObject trackedObj;
		[SerializeField] Material lineMaterial;
		[SerializeField] float lineWidth = 0.01f;

		private GraphicsLineRenderer lineCurrent;
		private int countClicks;

		void Start () {
		
		}
		
		void Update () {
			if (trackedObj == null) {
				return;
			}
				

			SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
			if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {

				// Creates line object
				GameObject line = new GameObject ();
				line.AddComponent<MeshFilter> ();
				line.AddComponent<MeshRenderer> ();
				lineCurrent = line.AddComponent<GraphicsLineRenderer> ();

				// Adds collider to line object
				line.AddComponent<MeshCollider> ().sharedMesh = lineCurrent.lineMesh;

				// Childs line object to manager
				line.transform.parent = transform;
				line.name = "Line";

				// Sets line object width and material
				lineCurrent.SetWidth (lineWidth);
				lineCurrent.lineMaterial = lineMaterial;

			} else if (device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
				lineCurrent.AddPoint (trackedObj.transform.position);


			}
		}
	}
}
