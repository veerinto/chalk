/* File: ManagerEnvironment.cs
 * Description: #DESCRIPTION#
 * How to use: #INSTRUCTIONS#
*/

using UnityEngine;
using System.Collections;
using Interlude;


namespace Interlude {
	public class ManagerEnvironment : MonoBehaviour {

		[SerializeField]
		private GameObject environmentDefault;

		// TODO: test out singleton abstraction on this
		public GameObject environmentCurrent{ get; set; }

		void Start () {
			// Ensures the existence of an environment
			if (environmentCurrent == null) {
				environmentCurrent = (GameObject) Instantiate (environmentDefault);
				environmentCurrent.transform.parent = this.transform;
			}
		}

		public void LoadEnvironment(GameObject environment) {
			Destroy (environmentCurrent);
			environmentCurrent = Instantiate (environment);
			environmentCurrent.transform.parent = this.transform;
		}

	}
}
