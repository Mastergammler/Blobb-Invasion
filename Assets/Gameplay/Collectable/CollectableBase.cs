using System.Linq.Expressions;
using UnityEngine;
using System.Collections;

public abstract class CollectableBase : MonoBehaviour,ICollectable
{
    public CollectableType Type { get { return mType; } }
    protected CollectableType mType;
    private ICollectionCallback mCallback;

    public void RequestCallback(ICollectionCallback cb)
    {
        mCallback = cb;
    }

    public CollectableType Collect()
    {
        StartCoroutine(SelfDestruct());
        if(mCallback != null)
        {
            mCallback.HasBeenCollected();
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
