using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Spine.Unity;

namespace UnitySpineTest
{
    public class AnimPlayer : MonoBehaviour
    {
        [Header("UI")]
        public Button playButton = null;
        public Button addButton = null;
        public Button clearButton = null;
        public Toggle isLoopToggle = null;
        public Slider speedSlider = null;
        public TMP_Text speedValueText = null;
        public TMP_Dropdown animDropdown = null;
        public TMP_Dropdown trackDropdown = null;
        public TMP_Text logger = null;

        [Header("Animation")]
        public SkeletonAnimation skeletalAnim = null;
        public Spine.Unity.SkeletonDataAsset skeletolDataAsset = null;
        private Spine.AnimationState _state = null;

        private string animSelected = string.Empty;
        private int trackSelected = 0;

        private List<TrackInfo> trackInfo = new List<TrackInfo>();

        private void Start()
        {
            playButton.onClick.AddListener(() => Play());
            addButton.onClick.AddListener(() => Add());
            clearButton.onClick.AddListener(() => Clear());

            isLoopToggle.onValueChanged.AddListener(OnIsLoopedValueChanged);

            speedSlider.onValueChanged.AddListener(OnSpeedChanged);
            speedSlider.value = 100;

            _state = skeletalAnim.state;

            PopulateAnimDropdown();
        }

        void PopulateAnimDropdown()
        {
            var options = new List<string>();
            var skeletonData = skeletolDataAsset.GetSkeletonData(false);
            foreach (var anims in skeletonData.Animations)
            {
                options.Add(anims.Name);
            }
            animDropdown.ClearOptions();
            animDropdown.AddOptions(options);

            animDropdown.onValueChanged.AddListener(OnAnimDropDownUpdated);

            ///

            var trackoptions = new List<string>();
            for (var i = 0; i < 10; i++)
            {
                trackoptions.Add($"track {i}");
            }
            trackDropdown.ClearOptions();
            trackDropdown.AddOptions(trackoptions);

            trackDropdown.onValueChanged.AddListener(OnTrackDropDownUpdated);

            ///

            OnAnimDropDownUpdated(0);
            OnTrackDropDownUpdated(0);
        }

        private void OnAnimDropDownUpdated(int index)
        {
            index = Mathf.Clamp(index, 0, animDropdown.options.Count);
            animDropdown.value = index;
            animSelected = animDropdown.options[index].text;
        }

        private void OnTrackDropDownUpdated(int index)
        {
            index = Mathf.Clamp(index, 0, animDropdown.options.Count);
            animDropdown.value = index;
            trackSelected = int.Parse(trackDropdown.options[index].text.Split(' ')[1]);

            var trackInfo = GetTrackInfo(trackSelected);
            if (trackInfo != null)
            {
                isLoopToggle.isOn = trackInfo.isLoop;
                speedSlider.value = trackInfo.speed;
            }
        }

        private void OnIsLoopedValueChanged(bool value)
        {
        }

        private void OnSpeedChanged(float value) => speedValueText.text = ((int)value).ToString();

        public void Add()
        {
            var isLoop = isLoopToggle.isOn;
            var speed = speedSlider.value / 100f;
            print($"track {trackSelected}, isLoop {isLoop}");

            var currentTrack = GetTrackInfo(trackSelected);
            if (currentTrack == null)
            {
                trackInfo.Add(new TrackInfo { track = trackSelected, anim = animSelected, isLoop = isLoop, speed = speed });
            }
            else
            {
                currentTrack.isLoop = isLoop;
                currentTrack.anim = animSelected;
                currentTrack.speed = speed;
            }

            Print();
        }

        public void Play()
        {
            foreach (var info in trackInfo)
            {
                _state.SetAnimation(info.track, info.anim, info.isLoop).TimeScale = info.speed;
            }
        }

        public void Clear()
        {
            trackInfo.Clear();
            Print();
        }

        private void Print()
        {
            logger.text = string.Empty;
            foreach (var track in trackInfo)
            {
                logger.text += $"track: {track.track} : {track.anim} @ {track.speed * 100}% speed, isloop: {track.isLoop}\n";
            }
        }

        private TrackInfo GetTrackInfo(int trackNo) => trackInfo.Find(obj => obj.track == trackNo);

        private class TrackInfo
        {
            public int track;
            public string anim;
            public float speed;
            public bool isLoop;
        }
    }
}
