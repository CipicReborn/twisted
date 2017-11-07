
public class DeletePlayerPrefs {

    [UnityEditor.MenuItem("Utils/Delete All PlayerPrefs")]
    static public void DeleteAllPlayerPrefs () {

        UnityEngine.PlayerPrefs.DeleteAll();
    }
}

