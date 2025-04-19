using BepInEx;
using BepInEx.Configuration;


namespace Loader_Lua;


static public class Configuration {
	//private ConfigEntry<string> configGreeting;
	//configGreeting = Config.Bind("its Section Name",   // The section under which the option is shown
	//                             "its Value Name",     // The key of the configuration option in the configuration file
	//                             "Default Value",      // The default value
	//                             "Description");       // Description of the option to show in the config file
	//Configuration.NewBind("Ranks", "A", "A");
	static public ConfigFile CONFIG { get; private set; } = new ConfigFile(System.IO.Path.GetFullPath(
		$"{BepInEx.Paths.PluginPath}/{MyPluginInfo.PLUGIN_NAME}.cfg"), true, new BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION));
	static public void NewBind (string name, string value, string defaultValue = null, string description = "Description - None", string[] AcceptableValues = null) {
		CONFIG.Bind(new ConfigDefinition(name, value), defaultValue ?? value, new ConfigDescription(description, new AcceptableValueList<string>(AcceptableValues ?? new string[1]{""}) ) );
	}
	static public void Save() {
		CONFIG.Save();
	}
}