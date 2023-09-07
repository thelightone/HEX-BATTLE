using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitConfig", menuName = "Data/UnitConfig", order = 51)]
public class UnitConfig : ScriptableObject
{
    public int health;
    public int damage;
    public int armor;
    public int goRange;

    public int missChance;
    public int vamp;
    public bool melee;

    public int cost;
    public int numberUnits;

    public SkillParent[] skills;
    //public Mesh mesh;

    //public AnimatorController animator;

    public Race race;
    public GameObject model;

    public enum Race
    {
        race1,
        race2
    }
}
