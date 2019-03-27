using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JsCall
{
    public class CefJavaScriptEx
    {
        /// <summary>
        /// 利用反射，将对象的属性、方法映射成注册JS方法的脚本，
        /// Object obj 需要被映射的对象，
        /// String jsName JS调用的对象名称，
        /// return String 返回注册JS的脚本
        /// </summary>
        /// <param name="obj">需要被映射的对象</param>
        /// <param name="jsName">JS调用的对象名称</param>
        /// <returns>返回注册JS的脚本</returns>
        public static String CreateJsCodeByObject(Object obj, String jsName)
        {
            String jsCode = "function " + jsName + @"() {}
                if (!" + jsName + ") " + jsName + @" = {};
                (function() {";

            try
            {
                Type objType = obj.GetType();
                MethodInfo[] methods = objType.GetMethods();
                String pm = "", fname = "", func = "";
                for (int i = 0, j = methods.Length; i < j; i++)
                {
                    pm = "";
                    fname = methods[i].Name;
                    ParameterInfo[] param = methods[i].GetParameters();
                    for (int k = 0, x = param.Length; k < x; k++)
                    {
                        pm += "arg" + k;
                        if (k < (x - 1))
                        {
                            pm += ", ";
                        }
                    } 
                    func = jsName + "." + fname + " = function(" + pm + @") {
                        native function " + fname + "(" + pm + @");
                        return " + fname + "(" + pm + @");
                    };";
                    
                    jsCode += func;
                }
               
            }catch(Exception e){
                System.Windows.Forms.MessageBox.Show(e.Message);
            }            

            jsCode += @"})();";
            return jsCode;
        }
        
    }
}
