using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LevelGenerator : MonoBehaviour
{
    [Range(0f, 1f)]
    public float [] radiusRange;
    [Range(0f, 1f)]
    public float [] ropeRange;

    float lastAngle = 0;
    public GameObject gearPrefab;
    public int score;

    public Text scoreNumber;
    
    public List <Gear> gears = new List<Gear>();

    public PlayerMovement playerMovement;

    void Start()
    {
        StartCoroutine(GenerateGears());   
        scoreNumber.text = ToRoman(1);
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
                    selectedConnectedGearParent = gears[UnityEngine.Random.Range(0, gears.Count - 1)];
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
        scoreNumber.text = ToRoman(score + 1);
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

    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
        if (number < 1) return string.Empty;            
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900); 
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);            
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new ArgumentOutOfRangeException("something bad happened");
    }
}