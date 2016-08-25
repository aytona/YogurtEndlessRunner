using UnityEngine;
using System.Collections;

public class LargeObstacleWarning : MonoBehaviour {

    public GameObject warningSign;
    public string baseName;
    public float effectDuration;
    public float effectDelay;

    void Awake()
    {
        string position = "WarningSign" + gameObject.name.Remove(0, baseName.Length);
        warningSign = GameObject.Find(position);
    }

    void OnEnable()
    {
        StartCoroutine(warningEffect(effectDuration, effectDelay));
    }

    void OnDisable()
    {
        StopCoroutine(warningEffect(effectDuration, effectDelay));
    }

    private IEnumerator warningEffect(float duration, float delay)
    {
        bool toggle = false;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            if (toggle)
                warningSign.GetComponent<WarningSignPosition>().sprite.enabled = true;
            else if (warningSign.GetComponent<WarningSignPosition>().sprite != null)
                warningSign.GetComponent<WarningSignPosition>().sprite.enabled = false;
            toggle = !toggle;

            yield return new WaitForSeconds(delay);
        }
        warningSign.GetComponent<WarningSignPosition>().sprite.enabled = false;
    }
}
