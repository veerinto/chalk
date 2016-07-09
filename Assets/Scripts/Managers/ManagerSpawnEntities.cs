/* File: ManagerSpawnEntities.cs
 * Description: a singleton that manages the existence of itself (because it's a singleton) and all non-singleton objects in the game.
 * How to use: put script on a root-level GameObject in your scene.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Interlude;


namespace VeerChalk {
	public sealed class ManagerSpawnEntities : MonoBehaviour {

		//// <singleton>
		// Boilerplate for singleton pattern
		public static ManagerSpawnEntities instance = null;

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
		GameObject prefabPlayer;

		// TODO: expose to ManagerNetwork
		public Dictionary<int, GameObject> entities = new Dictionary<int, GameObject>();

		void Start() {
			StartCoroutine (SpawnEntity (prefabPlayer));
		}

		IEnumerator SpawnEntity(GameObject _prefabEntity) {
			GameObject entity = (GameObject)Instantiate (_prefabEntity);
			entity.transform.parent = this.transform;
			entities.Add (entity.GetInstanceID(), entity);

			yield return null;
		}

		IEnumerator SpawnEntity(GameObject _prefabEntity, Vector3 position, Quaternion rotation) {
			GameObject entity = (GameObject)Instantiate (_prefabEntity, position, rotation);
			entity.transform.parent = this.transform;
			entities.Add (entity.GetInstanceID(), entity);

			yield return null;
		}
			

	}
		
}
