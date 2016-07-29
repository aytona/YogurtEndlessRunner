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
                warningSign.GetComponent<WarningSignPosition>().textMesh.text = "!";
            else if (warningSign.GetComponent<WarningSignPosition>().textMesh != null)
                warningSign.GetComponent<WarningSignPosition>().textMesh.text = "";
            toggle = !toggle;

            yield return new WaitForSeconds(delay);
        }
        warningSign.GetComponent<WarningSignPosition>().textMesh.text = "";
    }
}
