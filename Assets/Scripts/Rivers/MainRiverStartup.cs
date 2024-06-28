using System.Collections;
using UnityEngine;

public class MainRiverStartup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _water;
    [SerializeField] private GameObject _landTop;
    [SerializeField] private GameObject _landBot;
    [SerializeField] private Material _waterTransitionMaterial;
    [SerializeField] private Material _landTopTransitionMaterial;
    [SerializeField] private Material _landBotTransitionMaterial;
    [Header("Startup values")]
    [SerializeField] private Material _waterMaterial;
    [SerializeField] private Texture _landTextureTop;
    [SerializeField] private Texture _landTextureBot;

    // Buffer and bug fix for the first scale to, for the player, visibly move the river
    private float _initScale = 300.0f;

    private void Awake()
    {
        SetupWater();
        SetupLand();


        if (_water == null || _landTop == null || _landBot == null)
        {
            _water = GameObject.FindGameObjectWithTag("Water").transform;
            _landTop = GameObject.FindGameObjectWithTag("LandTop");
            _landBot = GameObject.FindGameObjectWithTag("LandBot");
        }
        MoveAndScaleRiver();
    }

    private void Start()
    {
        RiverTransition.ChangeIntersectionFoamColor(_waterMaterial.GetColor("HighlightColor"));
    }

    private void SetupWater()
    {
        _waterTransitionMaterial.SetColor("HighlightColor", _waterMaterial.GetColor("HighlightColor"));
        _waterTransitionMaterial.SetFloat("HighlightSize", _waterMaterial.GetFloat("HighlightSize"));
        _waterTransitionMaterial.SetVector("HighlightStretch", _waterMaterial.GetVector("HighlightStretch"));
        _waterTransitionMaterial.SetVector("IntervalStrength", _waterMaterial.GetVector("IntervalStrength"));
        _waterTransitionMaterial.SetFloat("IntervalSpeed", _waterMaterial.GetFloat("IntervalSpeed"));
        _waterTransitionMaterial.SetFloat("WavePointDensity", _waterMaterial.GetFloat("WavePointDensity"));
        _waterTransitionMaterial.SetFloat("WavePointSpeed", _waterMaterial.GetFloat("WavePointSpeed"));
        _waterTransitionMaterial.SetFloat("RefractionSpeed", _waterMaterial.GetFloat("RefractionSpeed"));
        _waterTransitionMaterial.SetVector("RefractionScale", _waterMaterial.GetVector("RefractionScale"));
        _waterTransitionMaterial.SetColor("RiverColor", _waterMaterial.GetColor("RiverColor"));
        _waterTransitionMaterial.SetColor("RiverDepthColor", _waterMaterial.GetColor("RiverDepthColor"));
        _waterTransitionMaterial.SetFloat("RiverDepthStart", _waterMaterial.GetFloat("RiverDepthStart"));
        _waterTransitionMaterial.SetFloat("DepthStrength", _waterMaterial.GetFloat("DepthStrength"));

        _waterTransitionMaterial.SetFloat("RiverLerp", 0.0f);
    }

    private void SetupLand()
    {
        _landTopTransitionMaterial.SetTexture("Land1", _landTopTransitionMaterial.GetTexture("Land2"));
        _landBotTransitionMaterial.SetTexture("Land1", _landBotTransitionMaterial.GetTexture("Land2"));

        _landTopTransitionMaterial.SetFloat("Lerp", 0.0f);
        _landBotTransitionMaterial.SetFloat("Lerp", 0.0f);
    }


    private void MoveAndScaleRiver()
    {
        _water.localScale = new Vector3(_water.localScale.x, _water.localScale.y + _initScale, 1.0f);
        float newXpos = _water.localPosition.x + _initScale / 2.0f;
        _water.localPosition = new Vector3(newXpos, _water.localPosition.y, _water.localPosition.z);

        SpriteRenderer landsprite = _landBot.GetComponent<SpriteRenderer>();
        landsprite.size = new Vector2(landsprite.size.x + _initScale * 2.9985f, landsprite.size.y);
        _landBot.transform.localPosition = new Vector3(newXpos, _landBot.transform.localPosition.y, _landBot.transform.localPosition.z);

        landsprite = _landTop.GetComponent<SpriteRenderer>();
        landsprite.size = new Vector2(landsprite.size.x + _initScale * 2.9985f, landsprite.size.y);
        _landTop.transform.localPosition = new Vector3(newXpos, _landTop.transform.localPosition.y, _landBot.transform.localPosition.z);
    }
}