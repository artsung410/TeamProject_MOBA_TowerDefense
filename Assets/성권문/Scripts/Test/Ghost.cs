using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Assets/New Enemy")]
public class Ghost : ScriptableObject
{
    public int id;
    public int hp;
    public int strength;
    public int xpReward;
}
