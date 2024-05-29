using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.BusinessLogic.Core
{
    interface Igay
    {
        string whoI();
    }

    public class Antoha : Igay
    {
        public string whoI()
        {
            return "I am Antoha";
        }
    }

    public class Ggay : Igay 
    {
        public string whoI()
        {
            return "I am gGay";
        }
    }

    internal class ModeratorAPI //unnecessary
    {
        public List<string> whoI(List<Igay> list) 
        {
            var result = new List<string>();
            foreach (var item in list)
            {
                result.Add(item.whoI());
            }
            return result;
        }
    }
}
