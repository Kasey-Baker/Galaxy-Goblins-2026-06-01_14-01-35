using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonNavigationNoise : MonoBehaviour, ISelectHandler
{

    [SerializeField] GameObject selectSfxPlayer;
    [SerializeField] AudioClip selectSfxToPlay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (selectSfxPlayer != null && selectSfxToPlay != null)
        {
            GameObject mySfxPlayer = Instantiate(selectSfxPlayer);
            mySfxPlayer.GetComponent<PlayDeathSound>().SetSound(selectSfxToPlay, 0.5f);
            mySfxPlayer.GetComponent<PlayDeathSound>().SetAsPersistent();
        }

    }
}
