using UnityEngine;

public class ElementMark : MonoBehaviour
{
    [SerializeField] private float markDuration = 5f;

    [Header("FX elementaires")]
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private GameObject iceEffect;
    [SerializeField] private GameObject electricEffect;

    private Element markedElement = Element.None;
    private float markTimer = 0f;

    public bool HasMark => markTimer > 0f && markedElement != Element.None;
    public Element MarkedElement => markedElement;

    private void Update()
    {
        if (markTimer > 0f)
        {
            markTimer -= Time.deltaTime;
            if (markTimer <= 0f) ClearMark();
        }
    }

    public void SetMark(Element element)
    {
        markedElement = element;
        markTimer = markDuration;
        ShowEffect(element);
    }

    public void ClearMark()
    {
        //enleve la mark et le timer a 0
        markedElement = Element.None;
        markTimer = 0f;
        ShowEffect(Element.None);
    }

    private void ShowEffect(Element element)
    {
        //joue les effects
        if (fireEffect != null) fireEffect.SetActive(element == Element.Fire);
        if (iceEffect != null) iceEffect.SetActive(element == Element.Ice);
        if (electricEffect != null) electricEffect.SetActive(element == Element.Lightning);
    }
}
