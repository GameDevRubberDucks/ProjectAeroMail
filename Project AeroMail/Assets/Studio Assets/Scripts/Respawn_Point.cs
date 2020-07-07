using UnityEngine;

[ExecuteInEditMode]
public class Respawn_Point : MonoBehaviour
{
    public Vector3 Position
    {
        get => transform.position;
    }

    public Quaternion Rotation
    {
        get => transform.rotation;
    }
}
