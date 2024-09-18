using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 pos = new Vector3(0, 5, -7);
    void Update()
    {
        this.gameObject.transform.position = player.transform.position + pos;
        
    }
}
