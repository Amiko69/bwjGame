using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Range(0f, 1f)]
    public float [] radiusRange;
    [Range(0f, 1f)]
    public float [] ropeRange;

    float lastAngle = 0;
    public Gear gearPrefab;
    
    List <Gear> gears = new List<Gear>();

    void Start()
    {
        int i=0;
        do
        {
            Gear newGear = CreateGear();
            newGear.RandomizeGear();
            // place new gear at connected possition
            i++;
        } while (i < 1);
        // } while (GearIsNotFinished());
    }

    float CalculateTotalAngle(float wedgeAngle)
    {
        return lastAngle + wedgeAngle;
    }

    Gear CreateGear()
    {
        Gear newGear = GameObject.Instantiate(gearPrefab).GetComponent<Gear>();
        
        // newWedge.GetComponent<Wedge>().SetRadius(Random.Range(radiusRange[0], radiusRange[1]));
        // newWedge.GetComponent<Wedge>().SetRope(Random.Range(ropeRange[0], ropeRange[1]));
        gears.Add(newGear);
        return newGear;
    }

    bool GearIsNotFinished()
    {
        return false;
    }
}