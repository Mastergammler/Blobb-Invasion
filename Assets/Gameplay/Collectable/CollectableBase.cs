using System.Linq.Expressions;
using UnityEngine;
using System.Collections;

public abstract class CollectableBase : MonoBehaviour,ICollectable
{

    public AudioClip CollectionSound;

    public CollectableType Type { get { return mType; } }
    protected CollectableType mType;
    private CollectionCallback mCallback;
    public delegate void CollectionCallback();
    private bool mIsAlreadyCollected = false;

    public void RequestCallback(CollectionCallback cb)
    {
        mCallback = cb;
    }


    public CollectableType Collect()
    {
        // double triggering happens because of physics collider and trigger collider for enemies
        if(mIsAlreadyCollected) return CollectableType.NONE;

        mIsAlreadyCollected = true;
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(SelfDestruct());
        playCollectionSound();
        mCallback?.Invoke();
        return Type;
    }

    private void playCollectionSound()
    {
        if(CollectionSound != null)
        {
            AudioSource source = GetComponent<AudioSource>();
            source.clip = CollectionSound;
            source.loop = false;
            source.Play();
        }
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        yield return null;
    }
}
