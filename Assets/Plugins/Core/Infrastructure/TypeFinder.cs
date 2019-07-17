using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace uMVVM {
    public class TypeFinder {
        
        /// <summary>
        /// 根据类名,获得其类型Type
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Type ResolveType(string className) {

            Assembly assembly = Assembly.GetExecutingAssembly();
            Type type = assembly.GetTypes().FirstOrDefault(t => t.Name == className );
            if (type == null) {
                throw new Exception(string.Format("Cant't find Class by class name:'{0}'", className));
            }
            return type;
        }
    }
}
