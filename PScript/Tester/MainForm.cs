using PScript;
using PScript.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PScript.Structure;

namespace Tester
{
    public partial class MainForm : Form
    {
        private ScriptEngine engine = new ScriptEngine();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var msgFunc = new Function(new ScriptFunctionDelegate(MsgBox));
            msgFunc.SetArguments(VariableKind.String, VariableKind.Number);

            var printFunc = new Function(new ScriptFunctionDelegate(Print));
            printFunc.SetArguments(VariableKind.String);
                
            var nthnFunc = new Function(new ScriptFunctionDelegate(Nothing));
            nthnFunc.SetArguments();

            engine.Functions.Add("msgbox", msgFunc);
            engine.Functions.Add("log", printFunc);
            engine.Functions.Add("nothing", nthnFunc);

            m_syntaxRichTextBox.Settings.Keywords.Add("msgbox");
            m_syntaxRichTextBox.Settings.Keywords.Add("log");
            m_syntaxRichTextBox.Settings.Keywords.Add("nothing");


            //Not yet implemented (comments)
            m_syntaxRichTextBox.Settings.Comment = "!";

            m_syntaxRichTextBox.Settings.KeywordColor = Color.Blue;
            m_syntaxRichTextBox.Settings.CommentColor = Color.Green;
            m_syntaxRichTextBox.Settings.StringColor = Color.Gray;
            m_syntaxRichTextBox.Settings.IntegerColor = Color.Red;

            m_syntaxRichTextBox.Settings.EnableStrings = true;
            m_syntaxRichTextBox.Settings.EnableIntegers = true;

            m_syntaxRichTextBox.CompileKeywords();
            m_syntaxRichTextBox.ProcessAllLines();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //try
            //{
                var txt = m_syntaxRichTextBox.Text;
                engine.Parse(txt);
                engine.Execute();
            //}
            //catch (PScript.Exceptions.ScriptException se)
            //{
            //    MessageBox.Show(se.ToString());
            //}
        }

        private Variable MsgBox(Variable[] args)
        {
            var str = (StringVariable)args[0];
            var num = (NumberVariable)args[1];

            string msg = str.Value + num.Value;

            MessageBox.Show(msg);

            return new StringVariable(msg);
        }
        private Variable Print(Variable[] args)
        {
            var str = (StringVariable)args[0];
            Console.WriteLine(str.Value);
            return new NullVariable();
        }
        private Variable Nothing(Variable[] args)
        {
            MessageBox.Show("Nothing");
            return new NullVariable();
        }
    }
}
