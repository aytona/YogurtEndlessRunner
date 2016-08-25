using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelectScreen : Singleton<CharacterSelectScreen> {

    [Range(0, 2)]
    private int selectedChar = 0;

    public void SelectCharacter(int charNum)
    {
        selectedChar = charNum;
        if(selectedChar > 2 || selectedChar < 0)
        {
            selectedChar = 0;
        }
    }

    public int GetSelectedCharNum()
    {
        return selectedChar;
    }


}
