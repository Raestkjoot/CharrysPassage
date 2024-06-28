using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject boat;
    [SerializeField] private Transform boatFadeInTarget;
    [SerializeField] private Transform boatFadeOutTarget;
    [SerializeField] private float boatSpeed = 1.0f;

    [SerializeField] private string SceneToLoad;
    [SerializeField] private GameObject blackOutPanel;
    [SerializeField] private float blackOutTimer = 0.2f;
    [SerializeField] private AudioClip _introTheme;

    [SerializeField] private CanvasGroup _menuButtons;
    [SerializeField] private CanvasGroup _menuTitle;

    private float _fadeInDelay = 1.0f;
    private float _fadeInTime = 1.0f;

    private bool startTheGame = false;
    private Animator BoatAnimator;
    private Color tempColor;
    private float desiredAlpha = 1f;
    private float currentAlpha;

    private float _cumulativeTime;

    private Transform _boatCurrentTarget;
    private float _accTimeBoatMoveDelay;
    private float _BoatMoveDelay = 1.0f;

    private void Start()
    {
        BoatAnimator = boat.GetComponent<Animator>();
        tempColor = blackOutPanel.GetComponent<Image>().color;

        _menuTitle.alpha = 0;
        _menuButtons.alpha = 0;
        _menuButtons.gameObject.SetActive(false);

        blackOutPanel.SetActive(false);
        _boatCurrentTarget = boatFadeInTarget;
    }

    private void Update()
    {
        _cumulativeTime += Time.deltaTime;

        if(_cumulativeTime > _fadeInDelay / 2.0f)
        {
            _menuTitle.alpha += (1 / (_fadeInTime * 2.0f)) * Time.deltaTime;
            
            if (_cumulativeTime > _fadeInDelay * 2.0f)
            {
                _menuButtons.gameObject.SetActive(true);
                _menuButtons.alpha += (1 / _fadeInTime) * Time.deltaTime;
            }
        }

        boat.transform.position = Vector3.Lerp(boat.transform.position, _boatCurrentTarget.position, boatSpeed * Time.deltaTime);

        if (startTheGame == true)
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, blackOutTimer * Time.deltaTime);

            _accTimeBoatMoveDelay += Time.deltaTime;
            if (_accTimeBoatMoveDelay > _BoatMoveDelay)
            {
                _boatCurrentTarget = boatFadeOutTarget;
            }

            tempColor.a = currentAlpha;
            blackOutPanel.GetComponent<Image>().color = tempColor;
            if (blackOutPanel.GetComponent<Image>().color.a >= desiredAlpha)
            {
                SceneManager.LoadScene(SceneToLoad);
            }
        }

    }
    public void StartOnClick()
    {
        BoatAnimator.SetBool("Start", true);
        startTheGame = true;
        blackOutPanel.SetActive(true);
        MusicManager.GetInstance().ChangeMusic(_introTheme, 4.0f);
    }
    public void QuitOnClick()
    {
        Application.Quit();
    }


}
