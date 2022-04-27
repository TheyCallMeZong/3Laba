using _3Laba;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MyLib
{
    public partial class UserControl1: Form
    {
        private Dictionary<string, string> fileNames = new Dictionary<string, string>();
        private List<MenuStructure> menus = new List<MenuStructure>();
        private MenuStructure menu = new MenuStructure();
        private MenuStructure menuHead = new MenuStructure();
        private List<MenuStructure> previos = new List<MenuStructure>();

        private int level = 0;
        private ToolStripMenuItem tool = new ToolStripMenuItem();
        private ToolStripMenuItem toolHead;
        private List<ToolStripMenuItem> toolsHead = new List<ToolStripMenuItem>();
        private List<ToolStripMenuItem> result = new List<ToolStripMenuItem>();
        private Dictionary<ToolStripMenuItem, int> previosToolsWithLevel = new Dictionary<ToolStripMenuItem, int>();

        public UserControl1(string path)
        {
            InitializeComponent();

            using (var reader = new StreamReader(path))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var text = line.Split(' ');
                    if (text.Count() == 3)
                    {
                        MenuStructure menu = new MenuStructure()
                        {
                            NumberHierarchy = Convert.ToInt32(text[0]),
                            Name = text[1],
                            Status = Convert.ToInt32(text[2]),
                        };

                        if (menu.NumberHierarchy > this.menu.NumberHierarchy)
                        {
                            this.menu.SubMenu.Add(menu);
                        }
                        else if (menu.NumberHierarchy > menuHead.NumberHierarchy)
                        {
                            var item = previos.LastOrDefault(x => x.NumberHierarchy == menu.NumberHierarchy - 1);
                            item.SubMenu.Add(menu);
                        }
                        else
                        {
                            menuHead = menu;
                            menus.Add(menuHead);
                        }
                        previos.Add(menu);
                        this.menu = menu;
                    }
                    else
                    {
                        MenuStructure menu = new MenuStructure()
                        {
                            NumberHierarchy = Convert.ToInt32(text[0]),
                            Name = text[1],
                            Status = Convert.ToInt32(text[2]),
                            MethodName = text[3]
                        };

                        if (menu.NumberHierarchy > menuHead.NumberHierarchy)
                        {
                            if (menu.NumberHierarchy > this.menu.NumberHierarchy)
                            {
                                this.menu.SubMenu.Add(menu);
                            }
                            else
                            {
                                var item = previos.LastOrDefault(x => x.NumberHierarchy == menu.NumberHierarchy - 1);
                                item.SubMenu.Add(menu);
                            }
                        }
                        else
                        {
                            menus.Add(menu);
                        }
                    }
                }
                Show(menus);
            }
        }

        private void Show(List<MenuStructure> menus)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                ToolStripMenuItem tool = new ToolStripMenuItem(menus[i].Name);
                if (menus[i].Status == 2)
                {
                    continue;
                }
                if (menus[i].MethodName != null && menus[i].Status != 1)
                {
                    fileNames.Add(menus[i].MethodName, menus[i].Name);
                    tool.Click += Click;
                }
                if (menus[i].SubMenu.Count > 0)
                {
                    if (toolHead == null)
                    {
                        toolHead = tool;
                        previosToolsWithLevel.Add(tool, level);
                        level++;
                        Show(menus[i].SubMenu);
                        level--;
                        result.Add(toolHead);
                        toolHead = null;
                        continue;
                    }
                    else
                    {
                        previosToolsWithLevel.Add(tool, level);
                        this.tool = previosToolsWithLevel.LastOrDefault(x => x.Value == level - 1).Key;
                        this.tool.DropDownItems.Add(tool);

                        if (toolHead != this.tool)
                        {
                            if (menus[i].SubMenu.Count == 3)
                            {
                                toolsHead.Add(this.tool);
                                toolHead.DropDownItems.AddRange(toolsHead.ToArray());
                            }
                        }
                        level++;
                        Show(menus[i].SubMenu);
                        level--;
                        continue;
                    }
                }

                if (toolHead != null)
                {
                    if (level == 1)
                    {
                        toolHead.DropDownItems.Add(tool);
                    }
                    else
                    {
                        var t = previosToolsWithLevel.LastOrDefault(x => x.Value == level - 1);
                        this.tool = t.Key;
                        this.tool.DropDownItems.Add(tool);
                    }
                }
                else
                {
                    if (result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            menuStrip1.Items.Add(item);
                        }
                        toolsHead = new List<ToolStripMenuItem>();
                        result = new List<ToolStripMenuItem>();
                    }
                    menuStrip1.Items.Add(tool);
                }
            }
        }

        private void Click(object sender, EventArgs e)
        {
            foreach (var item in fileNames)
            {
                if (item.Value == ((ToolStripMenuItem)sender).Text)
                {
                    MessageBox.Show($"Hello from {item.Key}");
                }
            }
        }
    }
}
