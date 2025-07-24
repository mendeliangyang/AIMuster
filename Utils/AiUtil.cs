using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using AIMuster.Config;
using AIMuster.Models;

namespace AIMuster.Utils
{
    public class AiUtil
    {

        public const string PromptCodeWeb = "$prompt";
        public static string EscapeJsString(string input,Dictionary<string,string> dic)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            foreach (var item in dic)
            {
                input = input.Replace(item.Key,item.Value);
            }
            return System.Text.Json.JsonSerializer.Serialize(input);
        }
    }
}
