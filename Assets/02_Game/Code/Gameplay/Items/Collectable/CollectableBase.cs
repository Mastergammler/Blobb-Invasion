using System.Linq.Expressions;
using UnityEngine;
using System.Collections;

using BlobbInvasion.Core;
namespace BlobbInvasion.Gameplay.Items
{
    public abstract class CollectableBase : MonoBehaviour, ICollectable, IHighscoreEvent
    {

        public AudioClip CollectionSound;

        public CollectableType Type { get { return mType; } }
        protected CollectableType mType;
        private CollectionCallback mCallback;
        public event ScoreActionEvent ScoreEvent;
        public delegate void CollectionCallback();
        private bool mIsAlreadyCollected = false;

        public void RequestCallback(CollectionCallback cb)
        {
            mCallback = cb;
        }


        public CollectableType Collect()
        {
            // double triggering happens because of physics collider and trigger collider for enemies
            if (mIsAlreadyCollected) return CollectableType.NONE;

            mIsAlreadyCollected = true;

            SpriteRenderer ren = GetComponent<SpriteRenderer>();
            if (ren != null) ren.enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(SelfDestruct());
            playCollectionSound();
            mCallback?.Invoke();
            // todo adjust for all via template method
            ScoreEvent?.Invoke(ScoreType.COLLECTED_HEALTH);
            return Type;
        }

        private void playCollectionSound()
        {
            if (CollectionSound != null)
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

}