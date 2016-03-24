using System;
using PScript.Exceptions;

namespace PScript.Structure
{
    public delegate Variable ScriptFunctionDelegate(Variable[] args);

    public class Function
    {
        private VariableKind[] m_kinds;

        private ScriptFunctionDelegate m_func;

        public Function(ScriptFunctionDelegate func)
        {
            m_func = func;
        }

        public void SetArguments(params VariableKind[] kinds)
        {
            m_kinds = kinds;
        }

        public Variable Execute(Variable[] args)
        {
            if (args.Length != m_kinds.Length)
                throw new ScriptException("Mismatching argument count");

            for (int i = 0; i < args.Length; i++)
            {
                var cur_arg = args[i].Kind;
                var cur_kind = m_kinds[i];

                if(cur_arg != cur_kind)
                    throw new ScriptException("Bad argument type");
            }

            return m_func(args);
        }
    }
}
