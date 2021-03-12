using UnityEngine;

class SingletonManager
{ 
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        string output = "";
        bool failed = false;

        if (!AudioSourcePool.Init())
        {
            output += "[SingletonManager.Bootstrap]: AudioSourcePool.Init failed";
            failed = true;
        }
        
        if(failed)
        {
            // TODO: Collect more information about why the initialization failed
            //       and save the output data in a file to allow you to debug for
            //       game testers/users that don't have Unity

            Debug.LogError(output);
            Application.Quit();
        }
        else
            Debug.Log(Time.time);
    }
}
