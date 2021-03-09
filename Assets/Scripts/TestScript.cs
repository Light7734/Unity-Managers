using UnityEngine;

public class TestScript : MonoBehaviour
{
    private AudioEmitter audioEmitter;
    private AudioEmitterST audioEmitterST;

    private void Awake()
    {
        audioEmitter = gameObject.AddComponent<AudioEmitter>();
        audioEmitterST = gameObject.AddComponent<AudioEmitterST>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            audioEmitterST.EmitSoundtrackCrossFade(AudioName.BG_Piano);

        if (Input.GetKeyDown(KeyCode.W))
            audioEmitterST.EmitSoundtrackCrossFade(AudioName.BG_Flute);

        if (Input.GetKeyDown(KeyCode.E))
            audioEmitter.EmitSound(AudioName.SFX_Drop);

        if (Input.GetKeyDown(KeyCode.R))
            audioEmitter.EmitSound(AudioName.SFX_Death);

    }

}
