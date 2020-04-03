using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

class TestClass
{
    public string model;
    public int id;

    private Random random = new Random();

    public TestClass(int modelSize)
    {
        model = RandomString(modelSize);
        id = random.Next(0,1000);
    }

    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
