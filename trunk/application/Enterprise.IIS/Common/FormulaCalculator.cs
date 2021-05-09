using System;
using System.CodeDom.Compiler;
using System.Reflection;  

namespace Enterprise.IIS.Common
{
    /// <summary>  
    /// 动态求值  调用方法：string a = FormulaCalculator.Eval("(2000+100)*0.53*16.14+500").ToString();
    /// </summary>  
    public class FormulaCalculator
    {
        /// <summary>  
        /// 计算结果,如果表达式出错则抛出异常  
        /// </summary>  
        /// <param name="statement">表达式,如"1+2+3+4"</param>  
        /// <returns>结果</returns>  
        public static object Eval(string statement)
        {
            return EvaluatorType.InvokeMember(
                        "Eval",
                        BindingFlags.InvokeMethod,
                        null,
                        Evaluator,
                        new object[] { statement }
                     );
        }

        static FormulaCalculator()
        {
            //构造JScript的编译驱动代码  
            CodeDomProvider provider = CodeDomProvider.CreateProvider("JScript");

            var parameters = new CompilerParameters {GenerateInMemory = true};

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, JscriptSource);

            Assembly assembly = results.CompiledAssembly;
            EvaluatorType = assembly.GetType("Evaluator");

            Evaluator = Activator.CreateInstance(EvaluatorType);
        }

        private static readonly object Evaluator;
        private static readonly Type EvaluatorType;

        /// <summary>  
        /// JScript代码  
        /// </summary>  
        private const string JscriptSource = @"class Evaluator  
              {  
                  public function Eval(expr : String) : String   
                  {   
                     return eval(expr);   
                  }  
              }";
    }
}