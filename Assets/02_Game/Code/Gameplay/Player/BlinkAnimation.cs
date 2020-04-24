using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class BlinkAnimation : MonoBehaviour,ISpriteMaterialChanger
{

    [SerializeField]
    private Material NewMaterial;
    [SerializeField]
    private float ChangeBackTime = 0.2f;
    private Material mDefaultMaterial;
    private SpriteRenderer mRenderer;

    // Start is called before the first frame update
    void Start()
    {
        mRenderer = GetComponent<SpriteRenderer>();
        mDefaultMaterial = mRenderer.material;
    }

    public void ChangeMaterial()
    {
        mRenderer.material = NewMaterial;
        StartCoroutine(changeMaterialBack());
    }

    private IEnumerator changeMaterialBack()
    {
        yield return new WaitForSeconds(ChangeBackTime);
        mRenderer.material = mDefaultMaterial;
        yield return null;
    }
}
