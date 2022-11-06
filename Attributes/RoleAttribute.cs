using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Authorisation2.Attributes;

public class RoleAttribute : TypeFilterAttribute 
{
    //public RoleAttribute (Type type, string role) : base(type)
    //shu kornishda yozib yuqoridan controlerdan Attributetypeni olib kelish mumkin
    // yoki quyidagicha srazi berib ketsa ham bo`ladi, faqat bu dynamic bo`lmaydi
    public RoleAttribute (string roleList) : base(typeof (AuthFilterAttribute))
    {
        var RoleList = roleList.Split(',').ToList();
        Arguments  =  new object [] { new List <string>(RoleList) };
    }
}
