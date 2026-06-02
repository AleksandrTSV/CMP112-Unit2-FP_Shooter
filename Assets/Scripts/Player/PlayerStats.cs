using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 3;      //For future development
    public int goal = 20;       //
    public void DecreaseGoal() //For future development
    {
        goal -= 1;
        goal = Mathf.Max(goal, 0);
    }

    public void DecreaseHealth() //For future development
    {
        health -= 1;
        health = Mathf.Max(health, 0);
    }
}
