using System;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using OsuRTDataProvider.BeatmapInfo;
using OsuRTDataProvider.Listen;

namespace OsuLastfmScrobbler.client
{
    public class OsuScrobbler
    {
        public OsuScrobbler()
        {
            osuListenerManager = new OsuListenerManager();
            osuListenerManager.Start();
        }

        private OsuListenerManager osuListenerManager;
        private LastfmClient lastfmlient;
        private DateTime playStartTime;
        
        private Beatmap lastBeatMap;
        private Beatmap beatMap;
        private int playTime;
        private int lastPlayTime;
        private int timeChangeBeatmapSetID;
        private bool isChangeDuringTimeChange = false; // 在时间变化的过程中BeatmapSetID发生了变化
        
        public delegate void OnStartedEvt();
        public event OnStartedEvt OnStartedChanged;


        public async void Start()
        {
            lastfmlient = new LastfmClient(Settings.Instance.LastfmApiKey, Settings.Instance.LastfmApiSecret);
            var response = await lastfmlient.Auth.GetSessionTokenAsync(Settings.Instance.LastfmUsername, Settings.Instance.LastfmPassword);
            if (response.Success == false)
            {
                Logger.Error("Lastfm verify error, please open config.ini and setting your lastfm account info.");
                osuListenerManager.Stop();
                return;
            }
            OnStartedChanged?.Invoke();
            HandleListenerEvents();
        }
        
        private void HandleListenerEvents()
        {
            osuListenerManager.OnBeatmapChanged+= (b) =>
            {
                lastBeatMap = beatMap;
                beatMap = b;
                if (lastBeatMap == null)
                {
                    playStartTime = DateTime.Now;
                    UpdateNowPlaying();
                }
                if (lastBeatMap != null && lastBeatMap.BeatmapSetID != beatMap.BeatmapSetID)
                {
                    UpdateNowPlaying();
                    var bm = lastBeatMap;
                    scrobble(bm);
                }
                // Console.WriteLine($"{b.BeatmapSetID}/{b.BeatmapID}: {b.ArtistUnicode} - {b.TitleUnicode}");
            };
            osuListenerManager.OnPlayingTimeChanged += (ms) =>
            {
                lastPlayTime = playTime;
                playTime = ms;
                // 处理同一首歌时间重置的情况
                if (playTime < lastPlayTime && timeChangeBeatmapSetID == beatMap.BeatmapSetID)
                {
                    // Console.WriteLine("重试或循环播放" + playTime);
                    var bm = beatMap;
                    scrobble(bm);
                    UpdateNowPlaying();
                }
                timeChangeBeatmapSetID = beatMap.BeatmapSetID;
            };
        }

        private void scrobble(Beatmap bm)
        {
            var pt = playStartTime;
            playStartTime = DateTime.Now;
            var duration = (long) (DateTime.Now - pt).TotalMilliseconds;
            // 大于一分钟记录
            if (duration <= 60000) return;
            var artist = bm.ArtistUnicode == "" ? bm.Artist : bm.ArtistUnicode;
            var title = bm.TitleUnicode == "" ? bm.Title : bm.TitleUnicode;
            Logger.Info($"scrobbled: {artist} - {title}");
            lastfmlient.Track.ScrobbleAsync(new Scrobble(artist, null, title, DateTime.UtcNow));
        }

        private void UpdateNowPlaying()
        {
            var artist = beatMap.ArtistUnicode == "" ? beatMap.Artist : beatMap.ArtistUnicode;
            var title = beatMap.TitleUnicode == "" ? beatMap.Title : beatMap.TitleUnicode;
            Logger.Info($"now playing: {artist} - {title}");
            lastfmlient.Track.UpdateNowPlayingAsync(new Scrobble(artist, null, title, new DateTimeOffset()));
        }
    }
}