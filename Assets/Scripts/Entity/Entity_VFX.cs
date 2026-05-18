using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;

    private Color originalColor;

    [Header("Crit Damage Vfx")]
    [SerializeField] private GameObject critDamageVfx;
    [SerializeField] private Color critDamageColor = new Color(1f, 0.84f, 0f);

    [Header("Impact Vfx")]
    [SerializeField] private GameObject hitImpactVfx;
    [SerializeField] private Color impactColor = Color.white;

    [Header("Damage Vfx")]
    [SerializeField] private Material damagedMat;
    private Material originalMat;
    private Coroutine damageVfxCo;

    [Header("Elemental Vfx")]
    [SerializeField] private Color chillVfx = new Color(0f, 0.75f, 1f);
    [SerializeField] private Color fireColor = new Color(1f, 0.27f, 0f);
    [SerializeField] private Color lightningColor = new Color(1f, 0.88f, 0.2f);
    private Coroutine elementalVfxCo;
    [Space]
    [SerializeField] private GameObject thunderStrikePrefab;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        originalMat = sr.material;
        originalColor = sr.color;
        //chillVfx = new Color(0.5f, 0.5f, 1f, 0.5f); // Light blue with some transparency
        //fireVfx = new Color(1f, 0.5f, 0.5f, 0.5f); // Light red with some transparency
        //lightningVfx = new Color(1f, 1f, 0.5f, 0.5f); // Light yellow with some transparency
    }

    //public void ThunderStrikeVfx(Transform target)
    //{
    //    Instantiate(thunderStrikePrefab, target.position, Quaternion.identity);
    //}

    public void GetImapctVfx(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critDamageVfx : hitImpactVfx;
        GameObject vfx = ObjectPool.instance.Spawn(hitPrefab.name, target.position, Quaternion.identity);

        vfx.GetComponentInChildren<SpriteRenderer>().color = isCrit ? critDamageColor : impactColor;
    }

    //public void GetElementVfx(float duration, ElementType element)
    //{
    //    if (element == ElementType.None)
    //        return;

    //    if (elementalVfxCo != null)
    //        StopCoroutine(elementalVfxCo);

    //    Color elementColor = GetElementColorVfx(element);

    //    elementalVfxCo = StartCoroutine(ElementVfxCo(duration, elementColor));
    //}

    //public Color GetElementColorVfx(ElementType element)
    //{
    //    Color elementColor = Color.white;

    //    switch (element)
    //    {
    //        case ElementType.Ice:
    //            elementColor = chillVfx;
    //            break;
    //        case ElementType.Fire:
    //            elementColor = fireVfx;
    //            break;
    //        case ElementType.Lightning:
    //            elementColor = lightningVfx;
    //            break;
    //    }

    //    return elementColor;
    //}

    //private IEnumerator ElementVfxCo(float duration, Color effectColor)
    //{
    //    float elapsed = 0f;
    //    float interval = 0.2f;

    //    bool toggle = false;

    //    Color lightColor = effectColor * 1.2f;
    //    Color darkColor = effectColor * .8f;

    //    while (elapsed < duration)
    //    {
    //        sr.color = toggle ? lightColor : darkColor;
    //        toggle = !toggle;

    //        yield return new WaitForSeconds(interval);
    //        elapsed += interval;
    //    }

    //    sr.color = originalColor;
    //}

    public void DamageVfx(float duration)
    {
        if (!gameObject.activeInHierarchy || !isActiveAndEnabled)
            return;

        //if (sr == null)
        //    return;

        //if (originalMat == null)
        //    originalMat = sr.material;

        if (damageVfxCo != null)
            StopCoroutine(damageVfxCo);

        damageVfxCo = StartCoroutine(DamageVfxCo(duration));
    }

    private IEnumerator DamageVfxCo(float duration)
    {
        sr.material = damagedMat;
        yield return new WaitForSeconds(duration);
        sr.material = originalMat;
    }

    private void OnEnable()
    {
        if (originalMat != null)
            sr.material = originalMat;
    }

    private void OnDisable()
    {
        if (damageVfxCo != null)
        {
            StopCoroutine(damageVfxCo);
            damageVfxCo = null;
        }
        //if (sr != null)
        //    sr.color = originalColor;
    }
}
