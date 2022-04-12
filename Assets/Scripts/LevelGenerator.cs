using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{
    [Range(0f, 1f)]
    public float [] radiusRange;
    [Range(0f, 1f)]
    public float [] ropeRange;

    float lastAngle = 0;
    public GameObject gearPrefab;
    public int score;
    
    public List <Gear> gears = new List<Gear>();

    public PlayerMovement playerMovement;

    void Start()
    {
        StartCoroutine(GenerateGears());   
    }

    IEnumerator GenerateGears()
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
            int currentScore = score;
            if (i > 2)
                yield return new WaitUntil(() => score == currentScore + 1);
        } while (i < 5);
    }

    float CalculateTotalAngle(float wedgeAngle)
    {
        return lastAngle + wedgeAngle;
    }

    Gear CreateGear()
    {
        Gear newGear = GameObject.Instantiate(gearPrefab).GetComponentInChildren<Gear>();
        gears.Add(newGear);
        return newGear;
    }

    public Gear NextGear()
    {
        score++;
        Debug.Log(score);
        return gears[score];
    }

    public Gear NeareastGearFromPlayer()
    {
        float minDistance = 10000;
        float currentDistance;
        Gear closestGear = gears[0];
        foreach (Gear currentGear in gears)
        {
            currentDistance = Vector2.Distance(currentGear.transform.parent.position, playerMovement.transform.position);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                closestGear = currentGear;
            }
        }
        return closestGear;
    }
}