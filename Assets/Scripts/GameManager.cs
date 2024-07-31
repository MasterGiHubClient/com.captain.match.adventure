using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] difficultyGroups;
    private GameObject _selectedGroup;
    [SerializeField]
    private GameObject gamePanel;
    [SerializeField]
    private GameObject difficultyPanel;
    [SerializeField]
    private Sprite[] itemsSprites;
    [SerializeField]
    private GameObject nextLevelButton;
    [SerializeField]
    private GameObject WinPanel;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource soundSource;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;
    private List<GameObject> items;
    private int _difficulty = 0;
    private int itemsSpawned;
    [SerializeField]
    private AudioSource _winSoundSource;
    private Button _firstItemSelected;
    private Button _secondItemSelected;
    private int _matchesNeeded;
    private int _nMoves = 0;
    [SerializeField]
    private Text _MovesTextField;
    [SerializeField]
    private Text _MoveTextFieldOnWin;
    // Start is called before the first frame update
    void Start()
    {
        float _volumeSound = PlayerPrefs.GetFloat("soundVolume", 1);
        float _volumeMusic = PlayerPrefs.GetFloat("musicVolume", 1);
        musicSource.volume = _volumeMusic;
        soundSource.volume = _volumeSound;
        musicSlider.value = _volumeMusic;
        soundSlider.value = _volumeSound;
    }
    public void itemClicked(Button clickedBtn)
    {
        soundSource.Play();
        _nMoves++;
        _MovesTextField.text = _nMoves.ToString();
        if (_firstItemSelected == null)
        {
            Debug.Log($"Item {clickedBtn.gameObject.name} clicked!");
            _firstItemSelected = clickedBtn;
            imageOfButton(_firstItemSelected).SetActive(true);
        }
        else if (_secondItemSelected == null && clickedBtn != _firstItemSelected)
        {
            _secondItemSelected = clickedBtn;
            Debug.Log($"Item {clickedBtn.gameObject.name} clicked & checking matches");
            imageOfButton(_secondItemSelected).SetActive(true); 
            StartCoroutine(CheckMatch());
        }
    }

    public void startGame(int difficulty)
    {
        soundSource.Play();
        _difficulty = difficulty;
        //starting the game only when a difficulty level is selected
        if (_difficulty > 0)
        {
            //initialization
            items = new List<GameObject>();
            //calculating how many items will be spawned (to be added to a list)
            if (_difficulty == 1) itemsSpawned = 8;
            else if (_difficulty == 2) itemsSpawned = 12;
            else if (_difficulty == 3) itemsSpawned = 20;
            _matchesNeeded = itemsSpawned / 2;
            //adjusting panels visibility accordingly
            difficultyPanel.SetActive(false);
            gamePanel.SetActive(true);
            //selecting the game object that holds the difficulty items (easy, middle, hard) depending on which difficulty the player selected
            //then making it visible
            _selectedGroup = difficultyGroups[_difficulty - 1];
            _selectedGroup.SetActive(true);
            //making a temp list of items sprites to use for the game (selecting randomly)
            List<Sprite> tempSprites = new List<Sprite>();
            for (int i=0;i<_matchesNeeded;i++)
            {
                var index = Random.Range(0,itemsSprites.Count()-1);
                while (tempSprites.Contains(itemsSprites[index]))
                     index = Random.Range(0, itemsSprites.Count() - 1);
                tempSprites.Add(itemsSprites[index]);
            }
            tempSprites.AddRange(tempSprites.ToArray());
            //Selecting the background parent object of items in the current difficulty panel
            int _counter = 0;
            while (tempSprites.Count > 0)
            {
                GameObject item = _selectedGroup.transform.Find($"item{_counter + 1}").gameObject;
                GameObject imageInside = item.transform.Find($"Image").gameObject;
                int index= Random.Range(0,tempSprites.Count()-1);
                imageInside.GetComponent<Image>().sprite = tempSprites[index];
                tempSprites.RemoveAt(index);
                _counter++;
            }
            //
            //for (int i = 0; i < itemsSpawned; i++) //Looping through the items 
            //{
            //    //Selecting the i+1'th item to add a random sprite to it
            //    GameObject item = _selectedGroup.transform.Find($"item{i + 1}").gameObject;
            //    GameObject imageInside = item.transform.Find($"Image").gameObject;
            //    int randomIndex = Random.Range(0, tempSprites.Count - 1);
            //    imageInside.GetComponent<Image>().sprite = tempSprites[randomIndex];               
            //    //removing the already added sprite from the list
            //    tempSprites.RemoveAt(randomIndex);
            //    items.Add(item);
            //}
            //tempSprites.Clear();
        }
    }
    public void changeSoundVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", soundSlider.value);
        soundSource.volume = soundSlider.value;
    }
    public void changeMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        musicSource.volume = musicSlider.value;
    }

    GameObject imageOfButton(Button pile) //return gameobject of the image inside the pile game object
    {
        return pile.gameObject.transform.Find("Image").gameObject;
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second

        if (imageOfButton(_firstItemSelected).GetComponent<Image>().sprite == imageOfButton(_secondItemSelected).GetComponent<Image>().sprite)
        {
            // gems selected are a MATCH! disable both and increment score
            _firstItemSelected.gameObject.SetActive(false);
            _secondItemSelected.gameObject.SetActive(false);
            //Play sound
            
            _matchesNeeded--;
            Debug.Log($"Matches needed : {_matchesNeeded}");
            if (_matchesNeeded == 0)
            {
                // PlayerPrefs.SetInt("currentLevel", _level + 1);
                _MoveTextFieldOnWin.text = _nMoves.ToString();
                _winSoundSource.Play();
                ShowWinningPanel();
            }
        }
        else
        {
            // No match
            imageOfButton(_firstItemSelected).SetActive(false);
            imageOfButton(_secondItemSelected).SetActive(false);
            //decrement score

        }

        _firstItemSelected = null;
        _secondItemSelected = null;
    }
    void ShowWinningPanel()
    {
        gamePanel.SetActive(false);
        WinPanel.SetActive(true);
    }
    public void LoadMenu()
    {
        soundSource.Play();
        SceneManager.LoadScene("MenuScene");
    }
    public void NextLevel()
    {
        soundSource.Play();
        _difficulty++;
        if (_difficulty < 3)
        {
            PlayerPrefs.SetInt("RestartDifficulty", _difficulty);
            SceneManager.LoadScene("PlayScene");
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
    public void repeatLevel()
    {
        soundSource.Play();
        PlayerPrefs.SetInt("RestartDifficulty", _difficulty);
        SceneManager.LoadScene("PlayScene");
    }
}
