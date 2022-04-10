using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gear : MonoBehaviour
{
    const int TEXTURE_SIZE = 5000;
    private SpriteRenderer spriteRenderer;
    // public Texture2D texture2D;

    private Vector2 center;
    
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

    public Gear RandomizeGear()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        var texture2D = new Texture2D(TEXTURE_SIZE, TEXTURE_SIZE);
        center = new Vector2 (TEXTURE_SIZE / 2, TEXTURE_SIZE / 2);
        spriteRenderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
        spriteRenderer.color = Color.black;

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

    void GenerateNextPoint()
    {
        float x;
        float y;

        currectPointType = PointType.Single;

        currentDistance = Random.Range(distanceSinglePointMin, distanceSinglePointMax);
        x = center.x + (currentDistance * Mathf.Cos(currentAngle * Mathf.PI / 180));
        y = center.y + (currentDistance * Mathf.Sin(currentAngle * Mathf.PI / 180));

        vertices.Add(new Vector2(x, y));
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

        vertices.Add(new Vector2(x1, y1));
        vertices.Add(new Vector2(x2, y2));
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

        GetComponent<PolygonCollider2D>().points = verticesAux.ToArray();
    }
}
