#pragma warning disable IDE0049 // ʹ�ÿ������

namespace HM.Common.UnitTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void MyTestMethod()
    {
        var foo = new Foo();

        string str1 = ToStringHelper.Build(foo);
        string str2 = ToStringHelper.Build(foo, info =>
        {
            return !info.GetGetMethod()?.IsStatic ?? false;
        });
        string str3 = ToStringHelper.Build(foo, info =>
        {
            return (info.GetGetMethod()?.IsPublic ?? false) && (!info.GetGetMethod()?.IsStatic ?? false);
        }, info =>
        {
            return info.IsPublic && !info.IsStatic;
        });

        System.Diagnostics.Debug.WriteLine(str1); // debug output
        System.Diagnostics.Debug.WriteLine(str2); // debug output
        System.Diagnostics.Debug.WriteLine(str3); // debug output
    }

    private class Foo
    {
        // Properties
        public static string StaticPublicProperty { get; set; } = "S-PublicProp";
        public string PublicProperty { get; set; } = "PublicProp";

        protected static string StaticProtectedProperty { get; set; } = "S-ProtectedProp";
        protected string ProtectedProperty { get; set; } = "ProtectedProp";

        private string StaticPrivateProperty { get; set; } = "S-PriProp";
        private string PrivateProperty { get; set; } = "PriProp";

        // Fields
        public static string StaticPublicField = "S-PublicField";
        public string PublicField = "PublicField";

        protected static string StaticProtectedField = "S-ProtectedField";
        protected string ProtectedField = "ProtectedField";

#pragma warning disable IDE0044 // ���ֻ�����η�
#pragma warning disable IDE1006 // ������ʽ
#pragma warning disable CS0414 // �ֶΡ�UnitTest1.Foo._staticPrivateField���ѱ���ֵ������δʹ�ù�����ֵ
#pragma warning disable IDE0051 // ɾ��δʹ�õ�˽�г�Ա
        private static string _staticPrivateField = "S-PriField";
#pragma warning restore IDE0051 // ɾ��δʹ�õ�˽�г�Ա
#pragma warning restore CS0414 // �ֶΡ�UnitTest1.Foo._staticPrivateField���ѱ���ֵ������δʹ�ù�����ֵ
#pragma warning restore IDE1006 // ������ʽ
#pragma warning restore IDE0044 // ���ֻ�����η�
#pragma warning disable IDE0044 // ���ֻ�����η�
#pragma warning disable IDE0051 // ɾ��δʹ�õ�˽�г�Ա
#pragma warning disable CS0414 
        private string _privateField = "PriField";
#pragma warning restore CS0414
#pragma warning restore IDE0051 // ɾ��δʹ�õ�˽�г�Ա
#pragma warning restore IDE0044 // ���ֻ�����η�
    }
}