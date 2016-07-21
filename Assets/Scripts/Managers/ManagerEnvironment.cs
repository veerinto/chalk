/* File: ManagerEnvironment.cs
 * Description: a singleton that handles environment singletons.
 * How to use: put script on a root-level GameObject in your scene.
*/

using UnityEngine; 
using System.Collections;
using System.Collections.Generic;
using VeerChalk;


namespace VeerChalk {
	public sealed class ManagerEnvironment : MonoBehaviour {

		[SerializeField]
		private GameObject[] arrayEnvironment;

		// TODO: expose these to ManagerNetwork
		// NOTE: the int here is an arbitrary ID, not an instance ID
		public Dictionary<int, Environment> environments = new Dictionary<int, Environment> ();
		public Environment currentEnvironment { get; set; }


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


		void Start () {
		}
			

		private void GenerateCache() {
			foreach (GameObject go in arrayEnvironment) {
				AddEnvironment (go);
				Debug.Log ("ManagerEnvironment.cs: adding "
					+ go.name + " to list of environments...");
			}
		}

		// TODO: make a mathematically sound implementation of this
		private int GenerateID(GameObject instance) {
			return Mathf.Abs(instance.GetHashCode ());
		}

		// TODO: check against collisions in environment
		public void AddEnvironment (GameObject prefabEnvironment) {
			Environment environment = new Environment (prefabEnvironment);
			environments.Add (GenerateID(environment.gameObject), environment);
		}

		public void AddEnvironment (Environment environment) {
			if (environment.gameObject == null) {
				Debug.Log ("ManagerEnvironment.cs: failed to add environment; no corresponding GameObject found.");
			}

			environments.Add (GenerateID (environment.gameObject), environment);
		}

		public IEnumerator LoadEnvironment(int idEnvironment) {
			if (this.currentEnvironment != null) {
				Destroy (this.currentEnvironment.gameObject);
			}

			Environment environmentNew = environments [idEnvironment];
			this.currentEnvironment.gameObject = (GameObject)Instantiate (environmentNew.gameObject);
			this.currentEnvironment.gameObject.transform.parent = this.transform;

			yield return null;
		}

		public IEnumerator LoadEnvironment(GameObject prefabEnvironment) {
			if (this.currentEnvironment != null) {
				Destroy (this.currentEnvironment.gameObject);
			}

			Environment environmentNew = new Environment (prefabEnvironment);
			this.currentEnvironment = environmentNew;
			this.currentEnvironment.gameObject = (GameObject)Instantiate (environmentNew.gameObject);
			this.currentEnvironment.gameObject.transform.parent = this.transform;

			ManagerEnvironment.instance.AddEnvironment (environmentNew);

			yield return null;
		}

		public IEnumerator UnloadEnvironment(int idEnvironment) {
			yield return null;
		}



	}


	public sealed class Environment {
		public GameObject gameObject { get; set; }
		public int environmentID { get; set; }

		public Environment(GameObject _gameObject) {
			this.gameObject = _gameObject;
		}

	}


}
