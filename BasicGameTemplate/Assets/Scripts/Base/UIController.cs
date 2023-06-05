using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject WinPanel, LosePanel, InGamePanel, TutorialPanel;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private List<string> moneyMulti = new();
    [SerializeField] private GameObject coin, money;

    [Header("Buttons")]
    [SerializeField] private Button _nextBoardBtn; //
    [SerializeField] private Button _backBoardBtn; //
    [SerializeField] private Button _transferBtn; //
    [SerializeField] private Button _menuBtn; //
    [SerializeField] private Button _resumeBtn; // 
    [SerializeField] private Button _settingsBtn; //
    [SerializeField] private Button _backBtn; // 
    [SerializeField] private Button _restartBtn; //
    [Header("Slider and Toggles")]
    [SerializeField] private Button _volumeBtn; //
    public Sprite ChangedImage;
    private Sprite DefaultImage;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _effectToggle;
    [Header("Panels")]
    [SerializeField] private CanvasRenderer _menuPanel; //
    [SerializeField] private CanvasRenderer _settingsPanel; //
    [Header("Audios")]
    [SerializeField] private AudioSource _audioSourceTransferButton;
    [SerializeField] private AudioSource _audioSourceBoardButton;
    [SerializeField] private AudioSource _audioSourceBackground;

    private Canvas UICanvas;

    private LevelManager levelManager;

    private void Awake()
    {
        
        //ButtonInitialize();

        
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        ScriptInitialize();
        DefaultImage = _volumeBtn.image.sprite;
        GameManager.Instance.OnMoneyChange.Invoke();

        GameManager.Instance.LevelFail.AddListener(() => ShowPanel(LosePanel, true));
        GameManager.Instance.LevelSuccess.AddListener(() => ShowPanel(WinPanel, true));
        GameManager.Instance.GameReady.AddListener(GameReady);
        GameManager.Instance.OnMoneyChange.AddListener(SetMoneyText);


        _nextBoardBtn.onClick.AddListener(()=>
        {
            GameManager.Instance.NextButtonEvent.Invoke();
            _audioSourceBoardButton.Play();
            StartCoroutine(IsAction());

        });
        _backBoardBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.PreviousButtonEvent.Invoke();
            _audioSourceBoardButton.Play();
            StartCoroutine(IsAction());

        });
        _transferBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.TransferButtonEvent.Invoke();
            _audioSourceTransferButton.Play();
            StartCoroutine(IsAction());

        });
        _volumeBtn.onClick.AddListener(AudioSettings);
        _restartBtn.onClick.AddListener(ReloadScene);
        _menuBtn.onClick.AddListener(OpenMenu);
        _resumeBtn.onClick.AddListener(ResumeButton);
        _settingsBtn.onClick.AddListener(OpenSettings);
        _backBtn.onClick.AddListener(ReturnMenu);
        _volumeSlider.onValueChanged.AddListener(Sliderlistener);
        _musicToggle.onValueChanged.AddListener(MusicToggleListener);
        _effectToggle.onValueChanged.AddListener(EffectToggleListener);

    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.LevelFail.RemoveListener(() => ShowPanel(LosePanel, true));
            GameManager.Instance.LevelSuccess.RemoveListener(() => ShowPanel(WinPanel, true));
            GameManager.Instance.GameReady.RemoveListener(GameReady);

            _nextBoardBtn.onClick.RemoveListener(() =>
            {
                GameManager.Instance.NextButtonEvent.Invoke();
                _audioSourceBoardButton.Play();
                StartCoroutine(IsAction());

            });
            _backBoardBtn.onClick.RemoveListener(() =>
            {
                GameManager.Instance.PreviousButtonEvent.Invoke();
                _audioSourceBoardButton.Play();
                StartCoroutine(IsAction());

            });
            _transferBtn.onClick.RemoveListener(() =>
            {
                GameManager.Instance.TransferButtonEvent.Invoke();
                _audioSourceTransferButton.Play();
                StartCoroutine(IsAction());

            });
            _volumeBtn.onClick.RemoveListener(AudioSettings);
            _restartBtn.onClick.RemoveListener(ReloadScene);
            _menuBtn.onClick.RemoveListener(OpenMenu);
            _resumeBtn.onClick.RemoveListener(ResumeButton);
            _settingsBtn.onClick.RemoveListener(OpenSettings);
            _backBtn.onClick.RemoveListener(ReturnMenu);
            _volumeSlider.onValueChanged.RemoveListener(Sliderlistener);
            _musicToggle.onValueChanged.RemoveListener(MusicToggleListener);
            _effectToggle.onValueChanged.RemoveListener(EffectToggleListener);
        }
    }

    void ScriptInitialize()
    {
        levelManager = FindObjectOfType<LevelManager>();
        UICanvas = GetComponentInParent<Canvas>();
    }

    //void ButtonInitialize()
    //{
    //    Next = WinPanel.GetComponentInChildren<Button>();
    //    Restart = LosePanel.GetComponentInChildren<Button>();

    //    Next.onClick.AddListener(() => levelManager.LoadLevel(1));
    //    Restart.onClick.AddListener(() => levelManager.LoadLevel(0));
    //    btnRestart.onClick.AddListener(() =>levelManager.LoadLevel(0));
    //}

    void ShowPanel(GameObject panel, bool canvasMode = false)
    {
        panel.SetActive(true);
        GameObject panelChild = panel.transform.GetChild(0).gameObject;
        panelChild.transform.localScale = Vector3.zero;
        panelChild.SetActive(true);
        panelChild.transform.DOScale(Vector3.one, 0.5f);

        UICanvas.worldCamera = Camera.main;
        UICanvas.renderMode = canvasMode ? RenderMode.ScreenSpaceCamera : RenderMode.ScreenSpaceOverlay;
    }

    void GameReady()
    {
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        InGamePanel.SetActive(true);
        //ShowTutorial();
    }

    void SetMoneyText()
    {
        if (coin.activeSelf)
        {
            coin.transform.DORewind();
            coin.transform.DOPunchScale(Vector3.one, 0.5f, 2, 1);
        }


        if (money.activeSelf)
        {
            money.transform.DORewind();
            money.transform.DOPunchScale(Vector3.one, 0.5f, 2, 1);
        }

        int moneyDigit = GameManager.Instance.PlayerMoney.ToString().Length;
        int value = (moneyDigit - 1) / 3;
        if (value < 1)
        {
            moneyText.text = GameManager.Instance.PlayerMoney.ToString();
        }
        else
        {
            float temp = GameManager.Instance.PlayerMoney / Mathf.Pow(1000, value);
            moneyText.text = temp.ToString("F2") + " " + moneyMulti[value];
        }
    }

    public void ReloadScene()
    {
        //int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(activeSceneIndex);
        levelManager.LoadLevel(0);
    }
    public void OpenMenu()
    {
        _menuPanel.gameObject.SetActive(true);
        OnInGame(false);
    }
    public void ResumeButton()
    {
        _menuPanel.gameObject.SetActive(false);
        OnInGame(true);
    }
    public void OpenSettings()
    {
        _settingsPanel.gameObject.SetActive(true);
        _menuPanel.gameObject.SetActive(false);
        OnInGame(false);
    }
    public void ReturnMenu()
    {
        _settingsPanel.gameObject.SetActive(false);
        _menuPanel.gameObject.SetActive(true);
        OnInGame(false);
    }
    public void OnInGame(bool key)
    {
        if(key)
        {
            _menuBtn.gameObject.SetActive(true);
            _nextBoardBtn.gameObject.SetActive(true);
            _backBoardBtn.gameObject.SetActive(true);
            _transferBtn.gameObject.SetActive(true);
        }
        else
        {
            _menuBtn.gameObject.SetActive(false);
            _nextBoardBtn.gameObject.SetActive(false);
            _backBoardBtn.gameObject.SetActive(false);
            _transferBtn.gameObject.SetActive(false);
        }
    }
    public void OnSoundOn(bool key)
    {
        if(key)
        {
            _musicToggle.isOn = false;
            _effectToggle.isOn = false;
        }
        else
        {
            _musicToggle.isOn = true;
            _effectToggle.isOn = true;
        }
    }

    public void AudioSettings()
    {
        switch (!_audioSourceBackground.mute)
        {
            case true:
                _audioSourceBackground.mute = true;
                _audioSourceBoardButton.mute = true;
                _audioSourceTransferButton.mute = true;
                _volumeBtn.image.sprite = ChangedImage;
                //_volumeSlider.value = 0;
                break;
            case false:  
                _audioSourceBackground.mute = false;
                _audioSourceBoardButton.mute = false;
                _audioSourceTransferButton.mute = false;
                _volumeBtn.image.sprite = DefaultImage;
                break;
        }
    }
    public void Sliderlistener(float value)
    {
        value = _volumeSlider.value;
        _audioSourceBackground.volume = value;
        _audioSourceBoardButton.volume = value/2;
        _audioSourceTransferButton.volume = value/2;
        if(value!=0)
        {
            _audioSourceBackground.mute = false;
            _audioSourceBoardButton.mute = false;
            _audioSourceTransferButton.mute = false;
            _musicToggle.isOn = true;
            _effectToggle.isOn = true;
        }
        else
        {
            _musicToggle.isOn = false;
            _effectToggle.isOn = false;
        }
    }
    public void MusicToggleListener(bool key)
    {
        key = _musicToggle.isOn;
        if (key)
        {
            //_musicToggle.isOn = false;
            _audioSourceBackground.mute = false;
        }
        else
        {
            //_musicToggle.isOn = true;
            _audioSourceBackground.mute = true;   
        }
    }
    public void EffectToggleListener(bool key)
    {
        key = _effectToggle.isOn;
        if (key)
        {
            //_effectToggle.isOn = false;
            _audioSourceBoardButton.mute = false;
            _audioSourceTransferButton.mute = false;
        }
        else
        {
            //_effectToggle.isOn = true;
            _audioSourceBoardButton.mute = true;
            _audioSourceTransferButton.mute = true;
        }
    }
    public IEnumerator IsAction() // BU KOD UI CONTROLLER .CS DOSYASINA GECIRILECEK.
    {
        _nextBoardBtn.interactable = false;
        _backBoardBtn.interactable = false;
        _transferBtn.interactable = false;
        yield return new WaitForSeconds(BoardManager.Timer + 0.1f);
        if (TransferArea.transferAreaPoint)
        {
            _nextBoardBtn.interactable = true;
            _backBoardBtn.interactable = true;
            _transferBtn.interactable = true;
        }
        else
        {
            _transferBtn.interactable = true;
        }
        StopCoroutine(IsAction());
    }
}
