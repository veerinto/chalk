/* File: IManagerSynced.cs
 * Description: #DESCRIPTION#
 * How to use: #INSTRUCTIONS#
*/

using UnityEngine;
using System.Collections;
using Interlude;

// TODO: integrate this with your managers
namespace VeerChalk {
	public interface IManagerSynced<T> {
		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
	}
}
