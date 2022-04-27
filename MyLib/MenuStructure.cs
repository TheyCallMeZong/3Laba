using System.Collections.Generic;

namespace _3Laba
{
    public class MenuStructure
    {
        public int NumberHierarchy { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string MethodName { get; set; }
        public List<MenuStructure> SubMenu { get; set; }

        public MenuStructure()
        {
            SubMenu = new List<MenuStructure>();
        }
    }
}
