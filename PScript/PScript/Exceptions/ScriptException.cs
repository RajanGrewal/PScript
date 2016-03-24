using PScript.Parsing;
using System;

namespace PScript.Exceptions
{
    public sealed class ScriptException : Exception
    {
        private Token m_token;
        private string m_message;

        public Token Token
        {
            get
            {
                return m_token;
            }
        }
        
        public override string Message
        {
            get
            {
                if (m_token != null)
                    return string.Format("[Line {0}:{1}] {2}", m_token.Line, m_token.Column, m_message);
                else
                    return m_message;
            }
        }

        public ScriptException(Token token,string message)
        {
           
            m_token = token;
            m_message = message;
        }


        public ScriptException( string message)
        {
            m_message = message;
        }
    }
}
