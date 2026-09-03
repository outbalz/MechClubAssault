
using UnityEngine;

public interface IItemable
{
    public string ModuleName { get; }
    public Sprite Icon { get; }
    public string Description { get; }
    public float Price { get; }
}
