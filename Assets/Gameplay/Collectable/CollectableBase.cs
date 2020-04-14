using UnityEngine;
using System.Collections;

public abstract class CollectableBase : MonoBehaviour,ICollectable
{
    public CollectableType Type { get { return mType; } }
    protected CollectableType mType;

    public CollectableType Collect()
    {
        StartCoroutine(SelfDestruct());
        return Type;
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
        yield return null;
    }
}
