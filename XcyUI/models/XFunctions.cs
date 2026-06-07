namespace XcyUI.models
{
    public class XFunctions
    {
        public delegate void XFunction();
        public delegate T XFunction2<T>();
        public delegate void XFunction<T>(T t);
        public delegate void XFunction<T, Y>(T t, Y y);
        public delegate void XFunction<T, Y, Z>(T t, Y y,Z z);
        public delegate T XFunctionResult<T>(T t);
        
        public delegate T XFunctionResult<T,V>(V t);
        public delegate T XFunctionResult<T,V,Z>(V t,Z z);
    }
}
