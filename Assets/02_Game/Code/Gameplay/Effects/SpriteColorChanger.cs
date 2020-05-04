using System.Collections;
using UnityEngine;

namespace BlobbInvasion.Gameplay.Effects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteColorChanger : MonoBehaviour, IColorChange
    {
        [SerializeField]
        private bool AutoChange;
        [SerializeField][Tooltip("Only needed when auto change back true")]
        private float ChangeBackTime = 0.2f;
        [SerializeField]
        private Color ColorChange;

        //###############
        //##  MEMBERS  ##
        //###############

        private Color mStartColor;
        private SpriteRenderer mRenderer;

        //################
        //##    MONO    ##
        //################

        private void Start() 
        {
            mRenderer = GetComponent<SpriteRenderer>();
            mStartColor = mRenderer.color;
        }

        //#################
        //##  INTERFACE  ##
        //#################

        public void ChangeColor()
        {
            mRenderer.color = ColorChange;
            if(AutoChange) StartCoroutine(autoChangeBack());
        }

        public void ChangeBack()
        {
            mRenderer.color = mStartColor;
        }

        private IEnumerator autoChangeBack()
        {
            yield return new WaitForSeconds(ChangeBackTime);
            ChangeBack();
            yield return null;
        }

        public bool AutoChangeBack => AutoChange;
    }
}