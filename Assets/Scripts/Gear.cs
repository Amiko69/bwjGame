using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gear : MonoBehaviour
{
    const int TEXTURE_SIZE = 5000;
    public SpriteRenderer spriteRenderer;
    public EdgeCollider2D edgeCollider2D;
    // public Texture2D texture2D;

    private Vector2 center;
    public bool hasChild;

    enum Rotation 
    {
        Right,
        Left
    }
    private Rotation gearRotation;
    
    enum PointType
    {
        Double,
        Single
    }
    PointType currectPointType;

    int currentDistance;
    int currentAngle;

    public int distanceDoublePointMin;
    public int distanceDoublePointMax;
    public int distanceSinglePointMin;
    public int distanceSinglePointMax;
    public int angleMin;
    public int angleMax;

    public List <Vector2> vertices = new List<Vector2>();
    public List <ushort> triangles = new List<ushort>();
    List <Vector2> verticesAux = new List <Vector2>();

    void Update()
    {
        transform.parent.Rotate(0, 0, 0.01f);
    }

    public Gear RandomizeGear()
    {
        CreateSprite();
        vertices.Add(center);
        for (int angle = 0 ; angle <= 360; angle += GenerateNextAngle())
        {
            if (Random.Range(1,3) == 1)
                GenerateNextPoint();
            else
                GenerateNextTwoPoints();

            if (angle != 0)
            {
                GenerateNextTriangle();
            }
        }
        GenerateLastTriangle();

        ChangeSprite();
        EditCollider();

        return this;
    }

    void CreateSprite()
    {
        var texture2D = new Texture2D(TEXTURE_SIZE, TEXTURE_SIZE);
        spriteRenderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
        spriteRenderer.color = Color.black;
        center = new Vector2 (TEXTURE_SIZE / 2, TEXTURE_SIZE / 2);
        transform.localScale *= 2;
    }

    public void DecidePositionFromOtherGear(Gear otherGear)
    {
        float x;
        float y;
        float distanceFromCenterToCenter;
        float randomDirectionAngle;

        if (!otherGear)
        {
            transform.position -= new Vector3 (center.x / 50, center.y / 50, 0);
        }
        else
        {
            distanceFromCenterToCenter = 3000;
            if (Random.Range(0,2) == 0)
            {
                randomDirectionAngle = Random.Range(0, 45);
            }
            else
            {
                randomDirectionAngle = Random.Range(315, 360);
            }
            x = otherGear.transform.parent.position.x / 50 + (distanceFromCenterToCenter / 50 * Mathf.Cos(randomDirectionAngle * Mathf.PI / 180));
            y = otherGear.transform.parent.position.y / 50 + (distanceFromCenterToCenter / 50 * Mathf.Sin(randomDirectionAngle * Mathf.PI / 180));
            transform.position -= new Vector3 (center.x / 50, center.y / 50, 0);
            transform.parent.position = new Vector3 (otherGear.transform.parent.position.x + x, otherGear.transform.parent.position.y + y, 0);
            otherGear.hasChild = true;
        }
    }

    public void DecideRotationFromOtherGear(Gear otherGear)
    {
        if (otherGear.gearRotation == Rotation.Left)
        {
            gearRotation = Rotation.Right;
        }
        else
        {
            gearRotation = Rotation.Left;
        }
    }

    void GenerateNextPoint()
    {
        float x;
        float y;

        currectPointType = PointType.Single;

        currentDistance = Random.Range(distanceSinglePointMin, distanceSinglePointMax);
        x = center.x + (currentDistance * Mathf.Cos(currentAngle * Mathf.PI / 180));
        y = center.y + (currentDistance * Mathf.Sin(currentAngle * Mathf.PI / 180));

        vertices.Add(new Vector2(x + center.x / 50, y + center.y / 50));
    }
    
    void GenerateNextTwoPoints()
    {
        float x1;
        float y1;
        float x2;
        float y2;

        currectPointType = PointType.Double;

        currentDistance = Random.Range(distanceDoublePointMin, distanceDoublePointMax);
        x1 = center.x + (currentDistance * Mathf.Cos(currentAngle * Mathf.PI / 180));
        y1 = center.y + (currentDistance * Mathf.Sin(currentAngle * Mathf.PI / 180));

        currentDistance = Random.Range(distanceDoublePointMin, distanceDoublePointMax);

        x2 = center.x + (currentDistance * Mathf.Cos(currentAngle * Mathf.PI / 180));
        y2 = center.y + (currentDistance * Mathf.Sin(currentAngle * Mathf.PI / 180));

        vertices.Add(new Vector2(x1 + center.x / 50, y1 + center.y / 50));
        vertices.Add(new Vector2(x2 + center.x / 50, y2 + center.y / 50));
    }

    void GenerateNextTriangle()
    {
        triangles.Add(0);

        ushort vertice1;
        ushort vertice2;
        
        if (currectPointType == PointType.Single)
        {
            vertice1 = (ushort)(vertices.Count - 1);
            vertice2 = (ushort)(vertices.Count - 2);
        }
        else
        {
            vertice1 = (ushort)(vertices.Count - 2);
            vertice2 = (ushort)(vertices.Count - 3);
        }

        triangles.Add(vertice1);
        triangles.Add(vertice2);
    }
    
    void GenerateLastTriangle()
    {
        triangles.Add(0);
        triangles.Add(1);

        ushort vertice;

        if (currectPointType == PointType.Single)
        {
            vertice = (ushort)(vertices.Count - 1);
        }
        else
        {
            vertice = (ushort)(vertices.Count - 2);
        }

        triangles.Add(vertice);
    }

    int GenerateNextAngle()
    {
        int auxAngle;
        auxAngle = Random.Range(angleMin, angleMax);
        currentAngle += auxAngle;
        return auxAngle;
    }

    void ChangeSprite()
    {
        spriteRenderer.sprite.OverrideGeometry(vertices.ToArray(), triangles.ToArray());
    }

    void EditCollider()
    {
        vertices.RemoveAt(0);
        foreach (Vector2 vertice in vertices)
        {
            verticesAux.Add(vertice / 100);
        }

        edgeCollider2D.points = verticesAux.ToArray();
    }
}
