using System;

namespace lo_novo
{
    public class FunOrString
    {
        private Func<Intention, bool> fun = null;
        private string str = null;

        public FunOrString(Func<Intention, bool> fun) { this.fun = fun; }
        public FunOrString(string s) { this.str = s; }

        public static implicit operator FunOrString(Func<Intention, bool> fun) { return new FunOrString(fun); }
        public static implicit operator FunOrString(string s) { return new FunOrString(s); }
        public static implicit operator Func<Intention, bool>(FunOrString fos) { return fos.fun; }
        public static implicit operator string(FunOrString fos) { return fos.str; }

        public Func<Intention, bool> GetFun() { return fun; }
        public string GetString() { return str; }

        public bool IsFun { get { return fun != null; } }
        public bool IsString { get { return str != null; } }
    }
}

