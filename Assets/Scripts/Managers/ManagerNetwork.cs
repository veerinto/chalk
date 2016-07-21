/* File: ManagerNetwork.cs
 * Description: Handles all network-related stuff, including spawning all networked Managers
 * How to use: 
 * 1) Put script on a root-level GameObject in your scene.
 * 2) Setup a PhotonView on all objects which have to be synced.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Interlude;
using Photon;
using VeerChalk;


namespace VeerChalk {
	public class ManagerNetwork : Photon.PunBehaviour {

		[SerializeField]
		bool isInDebugMode = false;

		// TODO: abstract these
		// Note that these are from local managers
		[SerializeField]
		Dictionary<int, Environment> environments;
		[SerializeField]
		Dictionary<int, Entity> entities;

		//// <singleton>
		// Boilerplate for singleton pattern
		public static ManagerNetwork instance = null;

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
			// Connects to the PUN servers as the arbitrarily set version "0.1"
			PhotonNetwork.ConnectUsingSettings ("0.1");

			if (isInDebugMode) {
				PhotonNetwork.logLevel = PhotonLogLevel.Full;
			}

		}

		// Displays the current connection status of the game
		void OnGUI() {
			GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString());
		}

		void OnJoinedLobby() {
			PhotonNetwork.JoinRandomRoom ();
		}

		void OnPhotonRandomJoinFailed() {
			PhotonNetwork.CreateRoom (null);
		}

		void OnJoinedRoom() {

			// Logs room name
			string strIsMaster = (PhotonNetwork.isMasterClient) ? "master client" : "slave client";
			Debug.LogError("ManagerNetwork.cs: joining room " +
				PhotonNetwork.room.name + " as " + strIsMaster + "...");



			//// Master clients
			/// TODO: make sure all slave managers are synced with master managers
			/// 	1) inMaster: Attach PhotonView to master managers and set them as observed
			/// 	2) inSlave: Get data from master managers and replace their corresponding local copies


			//// Slave clients

			if (!PhotonNetwork.isMasterClient) {
				ManagerEnvironment.instance.environments = environments;
				ManagerSpawnEntities.instance.entities = entities;

				if (ManagerEnvironment.instance.environments != null &&
				    ManagerSpawnEntities.instance.entities != null) {
					Debug.Log ("ManagerNetwork.cs: sync successful.");
				}
			}

//			// Spawns the environment
//			if (!PhotonNetwork.isMasterClient) {
//				// Loads current environment
//				// TODO: managerEnvironment.instance = [master managerEnvironment singleton]
//				managerEnvironment.LoadEnvironment (managerEnvironment.currentEnvironment.environmentID);
//			}
//				
//			// TODO: try checking something other than isMasterClient
//			// Spawns all entities in room
//			if (!PhotonNetwork.isMasterClient) {
//				foreach (Entity ent in managerSpawnEntities.entities.Values) {
//					Debug.Log (ent.gameObject.name + " isNetworked = " + ent.isNetworked);
//					if (ent.isNetworked) {
//						Debug.Log ("ManagerNetwork.cs: syncing " + ent.gameObject.name + "...");
//
//						// Recovers the prefab name of the instance
//						string entNameOld = ent.gameObject.name;
//						int index = entNameOld.IndexOf ("(Clone)");
//						string entNameNew = (index < 0)
//							? entNameOld
//							: entNameOld.Remove (index, ("(Clone)").Length);
//								
//						PhotonNetwork.Instantiate (
//							entNameNew,
//							ent.gameObject.transform.position, 
//							ent.gameObject.transform.rotation, 
//							0);
//					}
//				}
//			}

		}

		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
			if (stream.isWriting) {
				stream.SendNext (environments);
				stream.SendNext (entities);
			} else {
				environments = (Dictionary<int, Environment>)stream.ReceiveNext ();
				entities = (Dictionary<int, Entity>)stream.ReceiveNext ();
			}
		}
			







	}
}
