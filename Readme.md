# OsuLastfmScrobbler
Last.fm Scrobbler of Osu!.

## How to use?

1. Download [OsuLastfmScrobbler.zip](https://github.com/iMyon/OsuLastfmScrobbler/releases)
2. Extract zip file(prefer `<osu_dir>\OsuLastfmScrobbler`), then run `.exe`.
3. In the first run, this application generates `config.ini`, set your configuration and reopen the application.
4. This application will try to open `osu!.exe`, otherwise you can open osu yourself.

## Config.ini

[OsuScrobbler.Settings]

Setting Name|Description
------------|-------------------
LastfmApiKey|`apiKey` of Last.fm, you can create your API account at https://www.last.fm/api/account/create
LastfmApiSecret|`apiSecret` of Last.fm. 
LastfmUsername|Your Last.fm username.
LastfmPassword|Your Last.fm password.

Once your configuration is valid and osu is running, you can see the success info like this:
![](documents/images/start-success.png)

Enjoy yourself!

## Develop

prepare:
```shell
git submodule init
git submodule update
cd OsuSync
git clone https://github.com/OsuSync/OsuRTDataProvider.git
```
1. Edit `OsuSync\Sync\Sync.csproj`, replace all `..\packages` with `..\..\packages`.
2. Edit `OsuSync\OsuRTDataProvider\OsuRTDataProvider.csproj`, replace `$(SolutionDir)\Sync\bin\$(ConfigurationName)\Plugins` with `$(SolutionDir)OsuSync\$(ConfigurationName)\Plugins`.
3. Build `Sync`
4. Build `OsuRTDataProvider`
5. Build or run `OsuLastfmScrobbler`



## Thanks
[OsuSync](https://github.com/OsuSync)
