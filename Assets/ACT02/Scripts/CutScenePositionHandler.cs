using UnityEngine;

public class CutScenePositionHandler : MonoBehaviour {

    public static CutScenePositionHandler instance;

    public Cutscenedata[] cutscenedata;

    [System.Serializable]
    public class Cutscenedata
    {
        public string value;
        public Transform player;
        public Transform playerCamera;
        public Vector3 playerPosition;
        public float playerRotationY;
        public Vector3 playerCameraRotation;
        public GameObject[] activateObjects;
    }

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    public void HandlePosition (string cutscenevalue) {
		for(int i = 0; i < cutscenedata.Length; i++)
        {
            if (cutscenevalue == cutscenedata[i].value)
            {
                Debug.Log("found cutscene value "+cutscenevalue);
                cutscenedata[i].player.position = cutscenedata[i].playerPosition;
                cutscenedata[i].player.rotation = Quaternion.Euler(cutscenedata[i].player.eulerAngles.x, cutscenedata[i].playerRotationY, cutscenedata[i].player.eulerAngles.z);
                cutscenedata[i].playerCamera.localRotation = Quaternion.Euler(cutscenedata[i].playerCameraRotation.x, cutscenedata[i].playerCameraRotation.y, cutscenedata[i].playerCameraRotation.z);
                cutscenedata[i].player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().M_MouseLook.Init(cutscenedata[i].player, cutscenedata[i].playerCamera);
                foreach(GameObject obj in cutscenedata[i].activateObjects)
                {
                    obj.SetActive(true);
                }
                break;
            }
        }
	}
}
