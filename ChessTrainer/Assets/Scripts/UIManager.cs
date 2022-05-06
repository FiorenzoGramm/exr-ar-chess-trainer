using UnityEngine;

public class UIManager : MonoBehaviour
{
    void Update()
    {
        transform.parent.transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z), Vector3.up);
        transform.parent.transform.Rotate(Vector3.up, 180);
    }
}
