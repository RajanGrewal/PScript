using System;
namespace PScript.Structure
{
    public enum VariableKind
    {
        Null,
        Number,
        String,
        Bool
    }

    public abstract class Variable
    {
        public abstract VariableKind Kind {get;}
        public abstract Variable Clone();
    }

    public class NullVariable : Variable
    {
        public override VariableKind Kind
        {
            get { return VariableKind.Null; }
        }

        public override Variable Clone()
        {
            return new NullVariable();
        }
    }

    public class NumberVariable : Variable
    {
        public override VariableKind Kind
        {
            get { return VariableKind.Number; }
        }

        private int m_value;

        public int Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public NumberVariable(int value)
        {
            m_value = value;
        }

        public override Variable Clone()
        {
            return new NumberVariable(m_value);
        }
    }

    public class StringVariable : Variable
    {
        public override VariableKind Kind
        {
            get { return VariableKind.String; }
        }
        private string m_value;

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public StringVariable(string value)
        {
            m_value = value;
        }

        public override Variable Clone()
        {
            var temp = String.Copy(m_value);
            return new StringVariable(temp);
        }
    }

    public class BoolVariable : Variable
    {
        public override VariableKind Kind
        {
            get { return VariableKind.Bool; }
        }
        private bool m_value;

        public bool Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public BoolVariable(bool value)
        {
            m_value = value;
        }

        public override Variable Clone()
        {
            return new BoolVariable(m_value);
        }
    }
}
