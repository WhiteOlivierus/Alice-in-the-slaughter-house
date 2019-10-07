using UnityEngine;

public static class Vector3Extensions
{
    public static void EditX(this Vector3 vector, float value)
    {
        vector.EditVector('x', value);
    }
    public static void EditY(this Vector3 vector, float value)
    {
        vector.EditVector('y', value);
    }
    public static void EditZ(this Vector3 vector, float value)
    {
        vector.EditVector('z', value);
    }

    private static void EditVector(this Vector3 v, char axis, float value)
    {
        switch (axis)
        {
            case 'x':
                v = new Vector3(value, v.y, v.z);
                break;
            case 'y':
                v = new Vector3(v.x, value, v.z);
                break;
            case 'z':
                v = new Vector3(v.x, v.y, value);
                break;
            default:
                break;
        }
    }
}
