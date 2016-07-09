/* File: LoadEnvironment.cs
 * Description: #DESCRIPTION#
 * How to use: #INSTRUCTIONS#
*/

using UnityEngine;
using System.Collections;
using Interlude;


namespace Interlude {
	public class IdentityPhotosphere : MonoBehaviour {

		[SerializeField]
		GameObject managerEnvironment;

		[SerializeField]
		GameObject environment;

		void Start() {

			// Creates a miniature model of the environment to be loaded
			// inside the photosphere
			GameObject model = (GameObject)Instantiate (environment, this.transform.position, this.transform.rotation);
			model.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
			model.transform.parent = this.transform;
		}

		void OnTriggerEnter(Collider hit) {
			if (hit.tag == "Player") {
				managerEnvironment.GetComponent<ManagerEnvironment> ().LoadEnvironment (environment);
			}
		}

	}
}
