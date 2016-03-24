using System;
using System.Collections.Generic;
using PScript.Exceptions;
using PScript.Parsing;
using PScript.Structure;

namespace PScript
{
    public class ScriptEngine : IDisposable
    {
        private int index;
        private List<Token> tokens;

        private Dictionary<string, Variable> variables;
        private Dictionary<string, Function> functions;

        public Dictionary<string, Function> Functions
        {
            get { return functions; }
            set { functions = value; }
        }

        public ScriptEngine()
        {
            tokens = new List<Token>();
            variables = new Dictionary<string, Variable>();
            functions = new Dictionary<string, Function>();
        }

        public void Parse(string text)
        {
            tokens.Clear();
            variables.Clear();

            Tokenizer tok = new Tokenizer(text);

            while (true)
            {
                Token token = tok.Next();

                if (token.Kind == TokenKind.EOF)
                    break;

                if (token.Kind == TokenKind.EOL || token.Kind == TokenKind.WhiteSpace)
                    continue;

                tokens.Add(token);
            }
        }

        private Token GetNext()
        {
            if (index == (tokens.Count - 1))
                throw new ScriptException(null, "No more tokens to read");

            return tokens[++index];
        }

        public void Execute()
        {
            for (index = 0; index < tokens.Count; index++)
            {
                Token token = tokens[index];

                switch (token.Kind)
                {
                    case TokenKind.Symbol:
                        ExecuteSymbol(token);
                        break;
                    case TokenKind.Word:
                        ExecuteWord(token);
                        break;
                    default:
                        throw new ScriptException(token, "Bad next token");
                }
            }
        }

        private void ExecuteSymbol(Token token)
        {
            if (token.Value == "$")
            {
                var name = GetNext();

                if (name.Kind != TokenKind.Word)
                    throw new ScriptException(name, "Token after $ is not a word");

                if (variables.ContainsKey(name.Value))
                    throw new ScriptException(name, "Variable with this name already exists");

                var the_var = ParseAssigment();

                variables.Add(name.Value, the_var);
            }
            else
            {
                throw new ScriptException(token, "Syntax error");
            }
        }
        private void ExecuteWord(Token token)
        {
            if (variables.ContainsKey(token.Value)) //this is a variable
                variables[token.Value] = ParseAssigment();
            else //this is a function
                ParseFunction(token, true);
        }

        private Variable ParseValue(Token token)
        {
            string value = token.Value;
            Variable the_var = null;

            switch (token.Kind)
            {
                case TokenKind.QuotedString:
                    the_var = new StringVariable(value);
                    break;
                case TokenKind.Number:
                    int num = Convert.ToInt32(value);
                    the_var = new NumberVariable(num);
                    break;
                case TokenKind.Word:
                    if (value == "true")
                    {
                        the_var = new BoolVariable(true);
                    }
                    else if (value == "false")
                    {
                        the_var = new BoolVariable(false);
                    }
                    else if (value == "null")
                    {
                        the_var = new NullVariable();
                    }
                    else
                    {
                        if (variables.ContainsKey(value))
                        {
                            the_var = variables[value].Clone();
                        }
                        else if (functions.ContainsKey(value))
                        {
                            the_var = ParseFunction(token, false);
                        }
                    }
                    break;
            }

            if (the_var == null)
                throw new ScriptException(token, "Variable value is not string/number/bool/keyword");

            return the_var;
        }
        private Variable ParseAssigment()
        {
            var equal = GetNext();

            if (equal.Value != "=")
                throw new ScriptException(equal, "Token after variable name is not '='");

            var value_token = GetNext();

            Variable the_var = ParseValue(value_token);

            var colon = GetNext();

            if (colon.Value != ";")
                throw new ScriptException(colon, "Token after variable value is not ';'");

            return the_var;
        }
        private Variable ParseFunction(Token token, bool checkColon)
        {
            var next = GetNext();

            if (next.Value != "(")
                throw new ScriptException(next, "Token after function is not a '('");

            var function = functions[token.Value];

            var args = new List<Variable>();

            while (true)
            {
                var param = GetNext();
                string value = param.Value;

                if (param.Kind == TokenKind.Symbol)
                {
                    if (value == ")")
                        break;
                    else if (value != ",")
                        throw new ScriptException(param, "Symbol token inside arg list is not a ','");
                }
                else // not reserved value 
                {
                    Variable variable = ParseValue(param);
                    args.Add(variable);
                }
            }

            if (checkColon)
            {
                var colon = GetNext();

                if (colon.Value != ";")
                    throw new ScriptException(colon, "Token after variable value is not ';'");
            }

            return function.Execute(args.ToArray());
        }

        public void Dispose()
        {
            tokens.Clear();
            variables.Clear();
            functions.Clear();
        }
    }
}
