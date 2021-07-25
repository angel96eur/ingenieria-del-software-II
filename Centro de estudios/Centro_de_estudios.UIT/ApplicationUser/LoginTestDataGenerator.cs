using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Centro_de_estudios.UIT.ApplicationUser
{
    public class LoginTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
    {
        new object[] {"angel@uclm.com","Passsword1234%"},
        new object[] {"fran@uclm.com","APassword1g234%"},
        new object[] {"alvaro@uclm.com","APaessword1234%"},
    };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
