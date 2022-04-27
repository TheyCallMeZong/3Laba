using System.Reflection;

namespace _3Laba
{
    internal static class Program
    {
        static void Main()
        {
            string fileName = @"D:\����\2Laba\2Laba\TextFile1.txt";
            string libraryName = "MyLib.dll";
            var typeName = "MyLib.UserControl1";
            string methodName = "UserControl1";
            ApplicationConfiguration.Initialize();
            #region ������� ����������
            //Application.Run(new UserControl1(@"D:\����\2Laba\2Laba\TextFile1.txt"));
            #endregion

            #region ����� ����������

            Assembly assembly = Assembly.LoadFrom(libraryName);
            var type = assembly.GetType(typeName);
            if(type is not null)
            {
                object? obj = Activator.CreateInstance(type, fileName);
                MethodInfo? init = type.GetMethods().FirstOrDefault(x => x.Name == methodName);
                if(obj is Form form)
                {
                    Application.Run(form);
                }
            }
            #endregion
        }
    }
}