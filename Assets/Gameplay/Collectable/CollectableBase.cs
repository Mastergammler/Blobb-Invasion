using System.Linq.Expressions;
using UnityEngine;
using System.Collections;

public abstract class CollectableBase : MonoBehaviour,ICollectable
{
    public CollectableType Type { get { return mType; } }
    protected CollectableType mType;
    private CollectionCallback mCallback;
    public delegate void CollectionCallback();

    public void RequestCallback(CollectionCallback cb)
    {
        mCallback = cb;
    }

    public CollectableType Collect()
    {
        StartCoroutine(SelfDestruct());
        if(mCallback != null)
        {
            mCallback();
        }
        return Type;
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
        yield return null;
    }
}
