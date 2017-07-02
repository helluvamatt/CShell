namespace CShell.Hosting.Package
{
    using System.Collections.Generic;

    using ScriptCs.Contracts;

    public class ScriptPackResolver : IScriptPackResolver
    {
        private readonly IEnumerable<IScriptPack> _scriptPacks;

        public ScriptPackResolver(IEnumerable<IScriptPack> scriptPacks)
        {
            _scriptPacks = scriptPacks;
        }

        public IEnumerable<IScriptPack> GetPacks()
        {
            return _scriptPacks;
        }
    }
}