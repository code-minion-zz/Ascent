using UnityEngine;
using System.Collections;

public class AISensor 
{
    public enum EType
    {
        FirstFound,
        Closest,
        All,
    }

    public enum EScope
    {
        Allies,
        Enemies,
        All
    }

    private EType type;
    private EScope scope;

    public AISensor(EType type, EScope scope)
    {
        this.type = type;
        this.scope = scope;
    }

    public bool Sense()
    {
        return false;
    }
}
