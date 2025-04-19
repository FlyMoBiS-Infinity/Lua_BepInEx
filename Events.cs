using NLua;


using Loader_Lua;
namespace Eventter;


public class Event {
	public delegate object[] lua_stack (object[] args);
	public event lua_stack LuaStack;

	public object[] results;

	/// <summary>
	/// Run Each Registred Functions
	/// </summary>
	/// <param name="args"> Arguments For Lua Scripts </param>
	public object[] onEvent(params object[] args) {
		return LuaStack?.Invoke(args);
	}
	public void reg(LuaFunction lfun) {
		LuaStack += object[] (object[] args) => {return LoadScripts.TryCallFunction(lfun, args);};
	}
}