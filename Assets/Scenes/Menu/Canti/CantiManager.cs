using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CantiManager : MonoBehaviour
{
    public Button[] levelButtons;  // Array di bottoni
    public Sprite lockSprite;      // Sprite per i livelli bloccati
    public Sprite openSprite;      // Sprite per i livelli sbloccati

    // Lista dei link per i 33 canti dell'Inferno
    private List<string> cantoLinks = new List<string>()
    {
        "https://divinacommedia.weebly.com/inferno-canto-i.html",
        "https://divinacommedia.weebly.com/inferno-canto-ii.html",
        "https://divinacommedia.weebly.com/inferno-canto-iii.html",
        "https://divinacommedia.weebly.com/inferno-canto-iv.html",
        "https://divinacommedia.weebly.com/inferno-canto-v.html",
        "https://divinacommedia.weebly.com/inferno-canto-vi.html",
        "https://divinacommedia.weebly.com/inferno-canto-vii.html",
        "https://divinacommedia.weebly.com/inferno-canto-viii.html",
        "https://divinacommedia.weebly.com/inferno-canto-ix.html",
        "https://divinacommedia.weebly.com/inferno-canto-x.html",
        "https://divinacommedia.weebly.com/inferno-canto-xi.html",
        "https://divinacommedia.weebly.com/inferno-canto-xii.html",
        "https://divinacommedia.weebly.com/inferno-canto-xiii.html",
        "https://divinacommedia.weebly.com/inferno-canto-xiv.html",
        "https://divinacommedia.weebly.com/inferno-canto-xv.html",
        "https://divinacommedia.weebly.com/inferno-canto-xvi.html",
        "https://divinacommedia.weebly.com/inferno-canto-xvii.html",
        "https://divinacommedia.weebly.com/inferno-canto-xviii.html",
        "https://divinacommedia.weebly.com/inferno-canto-xix.html",
        "https://divinacommedia.weebly.com/inferno-canto-xx.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxi.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxii.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxiii.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxiv.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxv.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxvi.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxvii.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxviii.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxix.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxx.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxxi.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxxii.html",
        "https://divinacommedia.weebly.com/inferno-canto-xxxiii.html"
    };


    void Start()
    {
        // Controlla che ci siano 33 bottoni
        if (levelButtons.Length != 33)
        {
            Debug.LogError("Numero di bottoni non corrisponde a 33!");
            return;
        }

        // Verifica che i dati dell'utente siano caricati
        if (DifficultySelector.instance == null || 
            DifficultySelector.instance.userData == null || 
            DifficultySelector.instance.userData.collectables == null)
        {
            Debug.LogError("I dati dell'utente o dei collezionabili non sono disponibili.");
            return;
        }

        // Aggiorna l'aspetto dei bottoni in base ai collectables
        
    }

    // Metodo per aggiornare le sprite dei bottoni
    private void Update()
    {
        List<CollectableItem> collectables = DifficultySelector.instance.userData.collectables.list;

        // Controllo debug per verificare quanti collectables sono presenti
        Debug.Log($"Numero di collectables trovati: {collectables.Count}");

        for (int i = 0; i < levelButtons.Length; i++)
        {
           // Debug.Log(collectables[i].isFound);

            if (i >= collectables.Count)
            {
                Debug.LogWarning("Più bottoni rispetto ai collectables disponibili.");
                break;
            }

            // Verifica lo stato di `isFound` per ciascun collezionabile
            if (collectables[i].isFound)
            {
                // Collectable trovato: sblocca il livello
                levelButtons[i].interactable = true;
                levelButtons[i].GetComponent<Image>().sprite = openSprite;  // Imposta sprite di "open"
              //  Debug.Log($"Bottone {i + 1} sbloccato.");  // Messaggio di debug
            }
            else
            {
                // Collectable non trovato: blocca il livello
                levelButtons[i].interactable = false;
                levelButtons[i].GetComponent<Image>().sprite = lockSprite;  // Imposta sprite di "lock"
              //  Debug.Log($"Bottone {i + 1} bloccato.");  // Messaggio di debug
            }

            // Forza l'aggiornamento della UI per ogni bottone
            levelButtons[i].GetComponent<Image>().SetNativeSize();
        }

        // Forza il rebuild dell'UI
        Canvas.ForceUpdateCanvases();
    }


   
    // Funzione che apre il link in base al numero passato
    public void OpenCantoLink(int cantoNumber)
    {
        // Controlla se il numero del canto è valido
        if (cantoNumber >= 1 && cantoNumber <= 33)
        {
            // Prendi il link corrispondente dal numero del canto
            string url = cantoLinks[cantoNumber - 1];
            Application.OpenURL(url);
            Debug.Log("Opened URL: " + url);
        }
        else
        {
            Debug.LogError("Canto number is out of range. Please pass a number between 1 and 33.");
        }
    }

}
