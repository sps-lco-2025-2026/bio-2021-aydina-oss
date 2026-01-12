using System.Diagnostics;

// Q1 - create a class based on the data given 

DownPat dp = new DownPat("DE", "C");

dp.Calculate();

// test the output 
Debug.Assert(dp.First == "NO");
Debug.Assert(dp.Second == "YES");
Debug.Assert(dp.Third == "YES");


public class DownPat
{
    String firsthalf;

    String secondhalf;

    String answer1, answer2, answer3;

    public String First => answer1;
    public String Second => answer2;
    public String Third => answer3;
    public DownPat(String s1, String s2)
    {
        firsthalf=s1;
        secondhalf=s2;
    }
    public void Calculate()
    {
        for(int i = 1; i < First.Length; i++)
        {
            String fhalf=First.Substring(0,i);
            String shalf=First.Substring(i);


        }
    }

    private bool CheckAlph(string s1, string s2)
    {
         return s1.Min() > s2.Max();   
    }
    private String Reverse(string s1)
    {
        String result="";
        for(int i = s1.Length - 1; i > -1; i--)
        {
         result+=s1[i];
        }

        return result;  
    }
}

