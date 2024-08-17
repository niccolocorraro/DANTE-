using System.Collections;
using System.Collections.Generic;


[System.Serializable]


public class UserData
{
    public string username;
    public string password; // Should be encrypted
    
    private List<CollectibleItem> canti;
}
