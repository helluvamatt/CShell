using CShell.Framework.Services;
using ScriptCs;
using ScriptCs.Contracts;

namespace CShell.Hosting
{
    public class ReplScriptHost : ScriptHost
    {
        public ReplScriptHost(IScriptPackManager scriptPackManager, ScriptEnvironment environment)
            : base(scriptPackManager, environment)
        {
        }
    }

    public class ReplScriptHostFactory : IScriptHostFactory
    {
        public IScriptHost CreateScriptHost(IScriptPackManager scriptPackManager, string[] scriptArgs)
        {
            IConsole console = new MockConsole();
            Printers printers = new Printers(new ObjectSerializer());

            return new ReplScriptHost(scriptPackManager, new ScriptEnvironment(scriptArgs, console, printers));
        }
    }
}
