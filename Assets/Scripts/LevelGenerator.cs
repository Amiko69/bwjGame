using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Range(0f, 1f)]
    public float [] radiusRange;
    [Range(0f, 1f)]
    public float [] ropeRange;

    float lastAngle = 0;
    public GameObject gearPrefab;
    
    List <Gear> gears = new List<Gear>();

    public PlayerMovement playerMovement;

    void Start()
    {

        int i=0;
        do
        {
            Gear newGear = CreateGear();
            newGear.RandomizeGear();
            if (i == 0)
            {
                playerMovement.AssignGear(newGear);
                newGear.DecidePositionFromOtherGear(null);
            }
            else
            {
                Gear selectedConnectedGearParent;
                do 
                {
                    selectedConnectedGearParent = gears[Random.Range(0, gears.Count - 1)];
                }
                while (selectedConnectedGearParent.hasChild);
                newGear.DecidePositionFromOtherGear(selectedConnectedGearParent);
                newGear.DecideRotationFromOtherGear(selectedConnectedGearParent);
            }
            i++;
        } while (i < 3);
        // } while (GearIsNotFinished());
    }

    float CalculateTotalAngle(float wedgeAngle)
    {
        return lastAngle + wedgeAngle;
    }

    Gear CreateGear()
    {
        Gear newGear = GameObject.Instantiate(gearPrefab).GetComponentInChildren<Gear>();
        
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