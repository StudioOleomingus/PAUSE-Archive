using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OleoStoryGenerator {
	
	[System.Serializable]
	public class OleoStoryNodeConnections {

		public string id;
		public float anchorPositionX;
		public float anchorPositionY;

		public OleoStoryNodeConnections (string id, float anchorPositionX, float anchorPositionY) {
			this.id = id; this.anchorPositionX = anchorPositionX; this.anchorPositionY = anchorPositionY;
		}
	}
}