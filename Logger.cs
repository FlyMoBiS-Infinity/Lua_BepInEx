using BepInEx.Logging;

using System;


using Loader_Lua;
namespace Loader_Logger;


public class Dairy {
	static public ManualLogSource MLOG { get; private set; } = Logger.CreateLogSource($"{MyPluginInfo.PLUGIN_NAME} - {MyPluginInfo.PLUGIN_VERSION}");
	static public ManualLogSource LLOG { get; private set; } = Logger.CreateLogSource("Loader");
	static public ManualLogSource SLOG { get; private set; } = Logger.CreateLogSource("Lua Scripts");
	
	public event EventHandler<LogEventArgs> LogEvent;
	static public void Log (ManualLogSource logSource, LogLevel level, params object[] messages) {
		string result = "";
		foreach (var message in messages) {
			result = $"{result}{message}\t";
		}
		logSource.Log(level, result);
	}
}