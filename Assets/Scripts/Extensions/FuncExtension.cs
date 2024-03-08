using System;

namespace Extensions
{
    /// <summary>
    /// A static class that stores the extension functions for <see cref="Unity.VisualScripting.Func{T1,T2,T3,T4,T5,TResult}"/>;
    /// </summary>
    public static class FuncExtension
    {
        /// <summary>
        /// Evaluate if all the functions in <see cref="Func{TResult}"/> with return type of <see cref="bool"/> are true.
        /// </summary>
        /// 
        /// <param name="funcs">The <see cref="Func{TResult}"/> to evaluate.</param>
        /// 
        /// <returns>
        /// <c>true</c> if all the functions in <paramref name="funcs"/> are true.
        /// Otherwise, <c>false</c>.
        /// </returns>
        public static bool AllTrue(this Func<bool> funcs)
        {
            foreach (var func in funcs.GetInvocationList())
            {
                if (!(bool)func.DynamicInvoke())
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}