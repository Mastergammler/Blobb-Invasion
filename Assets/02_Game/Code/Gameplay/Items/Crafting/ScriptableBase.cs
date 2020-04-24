using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableBase : ScriptableObject
{
    public string Name;
    public string Tooltip;
    public Sprite Art;
    public CollectableType Type;


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        ScriptableBase sb = obj as ScriptableBase;
        return Name.Equals(sb.Name) && Type == sb.Type;
    }
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
