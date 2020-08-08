using Sync.MessageFilter;
using Sync.Plugins;
using Sync.Tools;
using Sync.Tools.ConfigurationAttribute;

namespace OsuLastfmScrobbler
{
    internal class OsuScrobblerPlugin : Plugin
    {
        private static string name = "OsuScrobbler";
        public OsuScrobblerPlugin() : base(name, "Myon") {}
    }
    /// <summary>
    /// Default plugin confiuration
    /// </summary>
    public class Settings : IConfigurable
    {
        [String]
        public ConfigurationElement LastfmApiKey { get; set; } = "";
        
        [String]
        public ConfigurationElement LastfmApiSecret { get; set; } = "";
        
        [String]
        public ConfigurationElement LastfmUsername { get; set; } = "";
        
        [String]
        public ConfigurationElement LastfmPassword { get; set; } = "";
        
        [Bool]
        public ConfigurationElement DebugMode { get; set; } = "False";
        

        public static readonly Settings Instance = new Settings();
        private static readonly PluginConfigurationManager config = new PluginConfigurationManager(new OsuScrobblerPlugin());
        static Settings()
        {
            config.AddItem(Instance);
        }

        public void onConfigurationLoad()
        {
        }

        public void onConfigurationSave()
        {
        }

        public void onConfigurationReload()
        {
        }
    }
}