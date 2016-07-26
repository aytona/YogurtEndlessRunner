using UnityEngine;
using System.Collections;

public class LargeObstacleWarning : MonoBehaviour {

    public GameObject warningSign;
    public float effectDuration;
    public float effectDelay;

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
        bool toggle = true;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            warningSign.SetActive(toggle);
            toggle = !toggle;
            yield return new WaitForSeconds(delay);
        }
        warningSign.SetActive(false);
    }
}
