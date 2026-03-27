using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset=new Vector3(0,4,-8);
    public float smoothSpeed =8f;

    void LateUpdate()
    {
        if (target==null) return;
        Vector3 desiredPos = target.position-target.forward*5f+Vector3.up*2f;
        transform.position=Vector3.Lerp(transform.position,desiredPos,smoothSpeed*Time.deltaTime);
        transform.LookAt(target.position+Vector3.up*1f);
    }
}
