/* File: ManagerEnvironment.cs
 * Description: a singleton that handles environment assets.
 * How to use: put script on a root-level GameObject in your scene.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Interlude;


namespace VeerChalk {
	public sealed class ManagerEnvironment : MonoBehaviour {

		//// <singleton>
		// Boilerplate for singleton pattern
		public static ManagerEnvironment instance = null;

		// This whole Awake() is singleton boilerplate
		void Awake() {

			if (instance == null) {
				instance = this;
			} else if (instance != null) {
				Destroy (this);
			}

			DontDestroyOnLoad (this);
		}

		//// </singleton>

		[SerializeField]
		private GameObject environmentDefault;

		[SerializeField]
		private GameObject[] arrayEnvironments;

		public Dictionary<int, GameObject> environments = new Dictionary<int, GameObject> ();

		// TODO: test out singleton abstraction on this
		public GameObject environmentCurrent{ get; set; }

		void Start () {
			// Ensures the existence of an environment
			StartCoroutine(LoadEnvironment(environmentDefault));
			}
			

		public IEnumerator LoadEnvironment(GameObject environment) {
			if (environmentCurrent != null) {
				Destroy (environmentCurrent);
			} else if (environmentCurrent == null) {
				environmentCurrent = Instantiate (environment);
				environmentCurrent.transform.parent = this.transform;
			}


			yield return null;
		}

	}
}
