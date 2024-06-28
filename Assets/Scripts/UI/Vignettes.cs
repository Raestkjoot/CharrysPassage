using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vignettes : MonoBehaviour
{
    [SerializeField] private List<GameObject> _vignetteScenes;
    [SerializeField] private GameObject _panel;
    [SerializeField] private string _nextSceneName;
    [SerializeField] private List<GameObject> _activeScenes = new List<GameObject>();

    private int _currentScene = 0;
    private GameObject _activeScene;

    void Start()
    {
        _activeScene = Instantiate(_vignetteScenes[0], _panel.transform);
        _activeScenes.Add(_activeScene);
        _currentScene++;
    }

    void Update()
    {
        if (Input.GetButtonDown("IntroNext"))
        {
            NextScene(_currentScene);
        }
    }

    private void NextScene(int currentScene)
    {

        if (_activeScene.GetComponent<VignetteSceneManager>().GetIsDoneShowingText() == false)
        {
            _activeScene.GetComponent<VignetteSceneManager>().ShowAllText();
            //_activeScene.GetComponent<VignetteSceneManager>().PauseAudio();
            return;
        }

        StartCoroutine(_vignetteScenes[currentScene].GetComponent<VignetteSceneManager>().FadeOut());

        _currentScene++;

        if (_currentScene < _vignetteScenes.Count)
        {
            Destroy(_activeScene);
            _activeScene = Instantiate(_vignetteScenes[_currentScene], _panel.transform);
        }
        else
        {
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}
