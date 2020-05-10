using BlobbInvasion.Code.Gameplay.Character;
using UnityEngine;

namespace BlobbInvasion.Gameplay.Character
{

    [RequireComponent(typeof(Collider2D))]
    public class TagItemDetector : MonoBehaviour,IDetector
    {
        private string[] mTags;

        public void SetTagsFilter(params string[] tags)
        {
            mTags = tags;
        }

        private void Start()
        {
        }

        private void OnTriggerEnter2D(Collider2D other)
        {

            if(mTags == null || checkIfTagMatches(other.transform.tag))
            {
                Debug.Log("Detected new item: " + other.name);
                OnItemDetected?.Invoke(other);
            }
        }

        private bool checkIfTagMatches(string colliderTag)
        {
            foreach(string tag in mTags)
            {
                if(tag.Equals(colliderTag)) return true;
            }
            return false;
        }


        public event ItemDetected OnItemDetected;
    }
}