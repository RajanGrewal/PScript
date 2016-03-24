/********************************************************8
 *	Author: Andrew Deren
 *	Date: July, 2004
 *	http://www.adersoftware.com
 * 
 *	StringTokenizer class. You can use this class in any way you want
 * as long as this header remains in this file.
 * 
 **********************************************************/

//Class derived from above

using System;
using PScript.Exceptions;

namespace PScript.Parsing
{
    /// <summary>
    /// StringTokenizer tokenized string (or stream) into tokens.
    /// </summary>
    public class Tokenizer
    {
        const char EOF = (char)0;

        int line;
        int column;
        int pos;	// position within data

        string data;

        char[] symbolChars;

        int saveLine;
        int saveCol;
        int savePos;

        public Tokenizer(string data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            this.data = data;

            //this.symbolChars = new char[] { '=', '+', '-', '/', ',', '.', '*', '~', '!', '@', '#', '$', '%', '^', '&', '(', ')', '{', '}', '[', ']', ':', ';', '<', '>', '?', '|', '\\' };
            
            this.symbolChars = new char[] { '=', ',', '$','(', ')', '{', '}',';'};
            
            this.line = 1;
            this.column = 1;
            this.pos = 0;
        }

        /// <summary>
        /// gets or sets which characters are part of TokenKind.Symbol
        /// </summary>
        public char[] SymbolChars
        {
            get { return this.symbolChars; }
            set { this.symbolChars = value; }
        }

        protected char LA(int count)
        {
            if (pos + count >= data.Length)
                return EOF;
            else
                return data[pos + count];
        }

        protected char Consume()
        {
            char ret = data[pos];
            pos++;
            column++;

            return ret;
        }

        protected Token CreateToken(TokenKind kind)
        {
            string tokenData = data.Substring(savePos, pos - savePos);

            //Rid the quotations
            if (kind == TokenKind.QuotedString)
            {
                int length = tokenData.Length;
                tokenData = tokenData.Substring(1, length - 2);
            }

            return new Token(kind, tokenData, saveLine, saveCol);
        }

        public Token Next()
        {
            char ch = LA(0);
            switch (ch)
            {
                case EOF:
                    return new Token(TokenKind.EOF, string.Empty, line, column);

                case ' ':
                case '\t':
                    return ReadWhitespace();
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ReadNumber();

                case '\r':
                    {
                        StartRead();
                        Consume();

                        if (LA(0) == '\n')
                            Consume();	// on DOS/Windows we have \r\n for new line

                        line++;
                        column = 1;

                        return CreateToken(TokenKind.EOL);
                    }
                case '\n':
                    {
                        StartRead();
                        Consume();
                        line++;
                        column = 1;

                        return CreateToken(TokenKind.EOL);
                    }

                case '"':
                    {
                        return ReadString();
                    }

                default:
                    {
                        if (Char.IsLetter(ch) || ch == '_')
                            return ReadWord();
                        else if (IsSymbol(ch))
                        {
                            StartRead();
                            Consume();
                            return CreateToken(TokenKind.Symbol);
                        }
                        else
                        {
                            throw new ScriptException("Unknown token type");
                        }
                    }

            }
        }

        /// <summary>
        /// save read point positions so that CreateToken can use those
        /// </summary>
        private void StartRead()
        {
            saveLine = line;
            saveCol = column;
            savePos = pos;
        }

        /// <summary>
        /// reads all whitespace characters (does not include newline)
        /// </summary>
        /// <returns></returns>
        protected Token ReadWhitespace()
        {
            StartRead();

            Consume(); // consume the looked-ahead whitespace char

            while (true)
            {
                char ch = LA(0);
                if (ch == '\t' || ch == ' ')
                    Consume();
                else
                    break;
            }

            return CreateToken(TokenKind.WhiteSpace);

        }

        /// <summary>
        /// reads number. Number is: DIGIT+ ("." DIGIT*)?
        /// </summary>
        /// <returns></returns>
        protected Token ReadNumber()
        {
            StartRead();

            bool hadDot = false;

            Consume(); // read first digit

            while (true)
            {
                char ch = LA(0);
                if (Char.IsDigit(ch))
                    Consume();
                else if (ch == '.' && !hadDot)
                {
                    hadDot = true;
                    Consume();
                }
                else
                    break;
            }

            return CreateToken(TokenKind.Number);
        }

        /// <summary>
        /// reads word. Word contains any alpha character or _
        /// </summary>
        protected Token ReadWord()
        {
            StartRead();

            Consume(); // consume first character of the word

            while (true)
            {
                char ch = LA(0);
                if (Char.IsLetter(ch) || ch == '_')
                    Consume();
                else
                    break;
            }

            return CreateToken(TokenKind.Word);
        }

        /// <summary>
        /// reads all characters until next " is found.
        /// If "" (2 quotes) are found, then they are consumed as
        /// part of the string
        /// </summary>
        /// <returns></returns>
        protected Token ReadString()
        {
            StartRead();

            Consume(); // read "

            while (true)
            {
                char ch = LA(0);
                if (ch == EOF)
                    break;
                else if (ch == '\r')	// handle CR in strings
                {
                    Consume();
                    if (LA(0) == '\n')	// for DOS & windows
                        Consume();

                    line++;
                    column = 1;
                }
                else if (ch == '\n')	// new line in quoted string
                {
                    Consume();

                    line++;
                    column = 1;
                }
                else if (ch == '"')
                {
                    Consume();
                    if (LA(0) != '"')
                        break;	// done reading, and this quotes does not have escape character
                    else
                        Consume(); // consume second ", because first was just an escape
                }
                else
                    Consume();
            }

            return CreateToken(TokenKind.QuotedString);
        }

        /// <summary>
        /// checks whether c is a symbol character.
        /// </summary>
        protected bool IsSymbol(char c)
        {
            for (int i = 0; i < symbolChars.Length; i++)
                if (symbolChars[i] == c)
                    return true;

            return false;
        }
    }
}
