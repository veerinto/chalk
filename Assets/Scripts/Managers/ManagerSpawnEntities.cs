/* File: ManagerSpawnEntities.cs
 * Description: a singleton that manages the existence of itself (because it's a singleton) and all non-singleton objects in the game.
 * How to use: put script on a root-level GameObject in your scene.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VeerChalk;


namespace VeerChalk {
	public sealed class ManagerSpawnEntities : Photon.PunBehaviour {

		[SerializeField]
		GameObject[] arrayEntities;

		// TODO: expose this to ManagerNetwork
		public Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

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


		void Start() {
		}


		// TEMP
		public void StartDefault() {
			foreach (GameObject go in arrayEntities) {
				if (go.name == "Camera") {
					StartCoroutine (SpawnEntity (go));
				} else {
					StartCoroutine (SpawnEntity (go, new Vector3 (Random.Range ((int)-2, (int)2), 0, 0), Quaternion.identity, true, false));
				}
			}
		}
			

		public IEnumerator SpawnEntity(GameObject prefab) {
			GameObject goEntity = (GameObject)Instantiate (prefab);
			goEntity.transform.parent = this.transform;

			// Adds entity to public dictionary called entities
			Entity entity = new Entity (goEntity);
			entities.Add (goEntity.GetInstanceID(), entity);

			yield return null;
		}

		public IEnumerator SpawnEntity(GameObject prefab, Vector3 position, Quaternion rotation) {
			GameObject goEntity = (GameObject)Instantiate (prefab, position, rotation);
			goEntity.transform.parent = this.transform;

			// Adds entity to public dictionary called entities
			Entity entity = new Entity (goEntity);
			entities.Add (goEntity.GetInstanceID(), entity);

			yield return null;
		}

		public IEnumerator SpawnEntity(GameObject prefab, Vector3 position, Quaternion rotation, bool _isNetworked, bool _isPlayer) {
			GameObject goEntity = (GameObject)Instantiate (prefab, position, rotation);
			goEntity.transform.parent = this.transform;

			// Adds entity to public dictionary called entities
			Entity entity = new Entity (goEntity, _isNetworked, _isPlayer);
			entities.Add (goEntity.GetInstanceID(), entity);

			yield return null;
		}
			

	}

	public sealed class Entity {
		public bool isPlayer { get; private set; }
		public bool isNetworked { get; private set; }
		public int instanceID { get; set; }
		public GameObject gameObject;

		public Entity(GameObject _gameObject) {
			this.gameObject = _gameObject;
			this.isNetworked = false;
			this.isPlayer = false;
		}

		public Entity(GameObject _gameObject, bool _isNetworked) {
			this.gameObject = _gameObject;
			this.isNetworked = _isNetworked;
			this.isPlayer = false;
		}

		public Entity(GameObject _gameObject, bool _isNetworked, bool _isPlayer) {
			this.gameObject = _gameObject;
			this.isNetworked = _isNetworked;
			this.isPlayer = _isPlayer;
		}
	}
		
}
