using System.Collections;
using UnityEngine;

public class FoamOutlineTransition : MonoBehaviour
{
    private Material _material;

    private void Awake()
    {
        RiverTransition.OnRiverTransition += RiverTransition_OnRiverTransition;
        _material = GetComponent<SpriteRenderer>().material;
    }

    private void RiverTransition_OnRiverTransition(object sender, RiverTransition.OnRiverTransitionArgs args)
    {
        _material.SetColor("Color2", args.color);
        StartCoroutine(Transition(args.transitionSpeed));
    }

    private IEnumerator Transition(float transitionSpeed)
    {
        float curLerp = 0.0f;

        while (curLerp != 1.0f)
        {
            curLerp = Mathf.Clamp01(curLerp + (transitionSpeed * Time.deltaTime));

            _material.SetFloat("lerpValue", curLerp);

            yield return null;
        }

        _material.SetColor("Color1", _material.GetColor("Color2"));
        _material.SetFloat("lerpValue", 0.0f);
    }

    private void OnDestroy()
    {
        RiverTransition.OnRiverTransition -= RiverTransition_OnRiverTransition;
    }
}
