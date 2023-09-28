using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource[] sfx;
    [SerializeField] AudioSource[] bgm;
    [SerializeField] float sfxMinDistance;

    public bool playBGM;

    bool canPlaySFX;
    public int bgmIndex;

    void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke(nameof(AllowSFX), 0.3f);
    }

    void Update()
    {
        if (!playBGM)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                //PlayBGM(bgmIndex);
                PlayRandomBGM();
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        if (!canPlaySFX) { return; }

        if (_sfxIndex < sfx.Length)
        {
            //if (sfx[_sfxIndex].isPlaying) { return; }

            if ((_source != null) &&
                (Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinDistance))
            { return; }

            sfx[_sfxIndex].pitch = Random.Range(0.9f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));

    IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > 0.1f)
        {
            _audio.volume -= _audio.volume * 0.2f;
            yield return new WaitForSeconds(0.25f);

            if (_audio.volume <= 0.1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    void AllowSFX() => canPlaySFX = true;
}
