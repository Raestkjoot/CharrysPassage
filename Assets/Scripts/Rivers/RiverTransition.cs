using System;
using System.Collections;
using UnityEngine;

public class RiverTransition : MonoBehaviour
{
    public static event EventHandler<OnRiverTransitionArgs> OnRiverTransition;

    public class OnRiverTransitionArgs : EventArgs
    {
        public Color color;
        public float transitionSpeed;
    }

    [SerializeField] private float _transitionSpeed = 0.5f;
    [Header("References")]
    [SerializeField] private Transform _water;
    [SerializeField] private GameObject _landTop;
    [SerializeField] private GameObject _landBot;
    [SerializeField] private Transform _assetsSpawnPosition;
    [Header("Change to")]
    [SerializeField] private GameObject _newAssetsPrefab;
    [SerializeField] private Material _newWaterMaterial;
    [SerializeField] private float _waterScaleY;
    [SerializeField] private Texture _newLandTextureTop;
    [SerializeField] private Texture _newLandTextureBot;
    [SerializeField] private float _destroyPreviousAssetsTimer = 15.0f;

    private Material _waterTransitionMaterial;
    private Material _landTopTransitionMaterial;
    private Material _landBotTransitionMaterial;
    private float _currentLerp = 0.0f;
    private static GameObject _curRiverAssets;

    // Used by MainRiverStartup.cs to set the intersection foam color to match
    // the tutorial river purple when the game starts.
    public static void ChangeIntersectionFoamColor(Color color)
    {
        OnRiverTransition?.Invoke(null, new OnRiverTransitionArgs
        {
            color = color,
            transitionSpeed = 1000.0f
        }); ;
    }

    private void Start()
    {
        if (_water == null || _landTop == null || _landBot == null)
        {
            _water = GameObject.FindGameObjectWithTag("Water").transform;
            _landTop = GameObject.FindGameObjectWithTag("LandTop");
            _landBot = GameObject.FindGameObjectWithTag("LandBot");
        }

        _waterTransitionMaterial = _water.GetComponent<SpriteRenderer>().material;
        _landTopTransitionMaterial = _landTop.GetComponent<SpriteRenderer>().material;
        _landBotTransitionMaterial = _landBot.GetComponent<SpriteRenderer>().material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        // Setup transition to new background
        SetupWaterFrom();
        SetupLandFrom();
        SetupWaterTo();
        SetupLandTo();
        MoveAndScaleRiver();

        // Instantiate new river assets and, after a delay, destroy the old ones
        if (_curRiverAssets != null) StartCoroutine(DelayedDestroy(_curRiverAssets, _destroyPreviousAssetsTimer));
        _curRiverAssets = Instantiate(_newAssetsPrefab, _assetsSpawnPosition.position, Quaternion.identity);

        // Transition background
        OnRiverTransition?.Invoke(this, new OnRiverTransitionArgs
        {
            color = _newWaterMaterial.GetColor("HighlightColor"),
            transitionSpeed = _transitionSpeed
        });
        while (_currentLerp != 1.0f)
        {
            _currentLerp = Mathf.Clamp01(_currentLerp + (_transitionSpeed * Time.deltaTime));

            _waterTransitionMaterial.SetFloat("RiverLerp", _currentLerp);
            _landTopTransitionMaterial.SetFloat("Lerp", _currentLerp);
            _landBotTransitionMaterial.SetFloat("Lerp", _currentLerp);

            yield return null;
        }
    }

    private void SetupWaterFrom()
    {
        if (_waterTransitionMaterial.GetFloat("RiverLerp") < 0.5f)
        {
            return;
        }

        _waterTransitionMaterial.SetColor("HighlightColor", _waterTransitionMaterial.GetColor("HighlightColor2"));
        _waterTransitionMaterial.SetFloat("HighlightSize", _waterTransitionMaterial.GetFloat("HighlightSize2"));
        _waterTransitionMaterial.SetVector("HighlightStretch", _waterTransitionMaterial.GetVector("HighlightStretch2"));
        _waterTransitionMaterial.SetVector("IntervalStrength", _waterTransitionMaterial.GetVector("IntervalStrength2"));
        _waterTransitionMaterial.SetFloat("IntervalSpeed", _waterTransitionMaterial.GetFloat("IntervalSpeed2"));
        _waterTransitionMaterial.SetFloat("WavePointDensity", _waterTransitionMaterial.GetFloat("WavePointDensity2"));
        _waterTransitionMaterial.SetFloat("WavePointSpeed", _waterTransitionMaterial.GetFloat("WavePointSpeed2"));
        _waterTransitionMaterial.SetFloat("RefractionSpeed", _waterTransitionMaterial.GetFloat("RefractionSpeed2"));
        _waterTransitionMaterial.SetVector("RefractionScale", _waterTransitionMaterial.GetVector("RefractionScale2"));
        _waterTransitionMaterial.SetColor("RiverColor", _waterTransitionMaterial.GetColor("RiverColor2"));
        _waterTransitionMaterial.SetColor("RiverDepthColor", _waterTransitionMaterial.GetColor("RiverDepthColor2"));
        _waterTransitionMaterial.SetFloat("RiverDepthStart", _waterTransitionMaterial.GetFloat("RiverDepthStart2"));
        _waterTransitionMaterial.SetFloat("DepthStrength", _waterTransitionMaterial.GetFloat("DepthStrength2"));

        _waterTransitionMaterial.SetFloat("RiverLerp", 0.0f);
    }

    private void SetupLandFrom()
    {
        if (_landTopTransitionMaterial.GetFloat("Lerp") < 0.5f)
        {
            return;
        }

        _landTopTransitionMaterial.SetTexture("Land1", _landTopTransitionMaterial.GetTexture("Land2"));
        _landBotTransitionMaterial.SetTexture("Land1", _landBotTransitionMaterial.GetTexture("Land2"));

        _landTopTransitionMaterial.SetFloat("Lerp", 0.0f);
        _landBotTransitionMaterial.SetFloat("Lerp", 0.0f);
    }

    private void SetupWaterTo()
    {
        _waterTransitionMaterial.SetColor("HighlightColor2", _newWaterMaterial.GetColor("HighlightColor"));
        _waterTransitionMaterial.SetFloat("HighlightSize2", _newWaterMaterial.GetFloat("HighlightSize"));
        _waterTransitionMaterial.SetVector("HighlightStretch2", _newWaterMaterial.GetVector("HighlightStretch"));
        _waterTransitionMaterial.SetVector("IntervalStrength2", _newWaterMaterial.GetVector("IntervalStrength"));
        _waterTransitionMaterial.SetFloat("IntervalSpeed2", _newWaterMaterial.GetFloat("IntervalSpeed"));
        _waterTransitionMaterial.SetFloat("WavePointDensity2", _newWaterMaterial.GetFloat("WavePointDensity"));
        _waterTransitionMaterial.SetFloat("WavePointSpeed2", _newWaterMaterial.GetFloat("WavePointSpeed"));
        _waterTransitionMaterial.SetFloat("RefractionSpeed2", _newWaterMaterial.GetFloat("RefractionSpeed"));
        _waterTransitionMaterial.SetVector("RefractionScale2", _newWaterMaterial.GetVector("RefractionScale"));
        _waterTransitionMaterial.SetColor("RiverColor2", _newWaterMaterial.GetColor("RiverColor"));
        _waterTransitionMaterial.SetColor("RiverDepthColor2", _newWaterMaterial.GetColor("RiverDepthColor"));
        _waterTransitionMaterial.SetFloat("RiverDepthStart2", _newWaterMaterial.GetFloat("RiverDepthStart"));
        _waterTransitionMaterial.SetFloat("DepthStrength2", _newWaterMaterial.GetFloat("DepthStrength"));
    }

    private void SetupLandTo()
    {
        _landTopTransitionMaterial.SetTexture("Land2", _newLandTextureTop);
        _landBotTransitionMaterial.SetTexture("Land2", _newLandTextureBot);
    }

    private void MoveAndScaleRiver()
    {
        _water.localScale = new Vector3(_water.localScale.x, _water.localScale.y + _waterScaleY, 1.0f);
        float newXpos = _water.localPosition.x + _waterScaleY / 2.0f;
        _water.localPosition = new Vector3(newXpos, _water.localPosition.y, _water.localPosition.z);

        SpriteRenderer landsprite = _landBot.GetComponent<SpriteRenderer>();
        landsprite.size = new Vector2(landsprite.size.x + _waterScaleY * 2.9985f, landsprite.size.y);
        _landBot.transform.localPosition = new Vector3(newXpos, _landBot.transform.localPosition.y, _landBot.transform.localPosition.z);

        landsprite = _landTop.GetComponent<SpriteRenderer>();
        landsprite.size = new Vector2(landsprite.size.x + _waterScaleY * 2.9985f, landsprite.size.y);
        _landTop.transform.localPosition = new Vector3(newXpos, _landTop.transform.localPosition.y, _landBot.transform.localPosition.z);
    }

    private IEnumerator DelayedDestroy(GameObject doomedGameObject, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        Destroy(doomedGameObject);
    }
}
