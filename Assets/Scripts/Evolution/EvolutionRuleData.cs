using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionRuleData", menuName = "Evolution/EvolutionRuleData")]
public class EvolutionRuleData : ScriptableObject
{
    public TileType actualType;
    public int actualEvol;
    public TileType requiredType;
    public int requiredEvol;
    public int requiredCount;
    public TileType newType;
    public int newEvol;
}