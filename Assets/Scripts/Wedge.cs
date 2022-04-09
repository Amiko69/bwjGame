using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wedge : MonoBehaviour
{
    private float rope;
    private float radius;

    public void SetRope (float rope)
    {
        transform.localScale = new Vector3 (rope, transform.localScale.y, transform.localScale.z);
        this.rope = rope;
    }

    public void SetRadius (float radius)
    {
        transform.localScale = new Vector3 (transform.localScale.x, radius, transform.localScale.z);
        this.radius = radius;
    }

    public void MoveWedge (Vector3 position)
    {
        transform.position += position;
    }

    public float CalculateWedgeAngle ()
    {
        float angle;
        float hypotenuse;
        float cotangent;
        cotangent = rope / 2;
        hypotenuse = Mathf.Sqrt(Mathf.Pow(radius, 2) + Mathf.Pow(cotangent, 2));
        angle = Mathf.Asin(cotangent / hypotenuse);
        Debug.Log(angle);
        return angle;
    }

    public void RotateWedge (float angle)
    {
        transform.Rotate(new Vector3 (0, angle));
    }
}
