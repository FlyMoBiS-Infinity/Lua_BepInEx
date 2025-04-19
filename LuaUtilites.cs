using NLua;

using System.Threading;


using Loader_Lua;
namespace Lua_Utilites;


static public class Head {
	static public void Wait(int milliseconds) {
		Thread.Sleep(milliseconds);
	}
	static public void Delay(int milliseconds, LuaFunction lfun) {
		new Thread(() => {Thread.Sleep(milliseconds); LoadScripts.TryCallFunction(lfun);});
	}
}