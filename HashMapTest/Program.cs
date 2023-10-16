using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace HashMapTest
{
    struct UserDataStruct
    {
        public int ID;
        public string Name;
        public UserDataStruct(int id, string name)
        {
            ID = id;
            Name = name;
        }

    }


    internal class Program
    {

        static Stopwatch sw;
        static Hashtable UserDataHash = new();
        static List<UserDataStruct> UserDataList = new();
        static Dictionary<int, string> UserDataDict = new();

        static void Main(string[] args)
        {
            sw = new();
            //new instance
            UserDataHash = new Hashtable();
            UserDataList = new List<UserDataStruct>();
            //adding data
            for (int i = 0; i < 40000000; i++)
            {
                //demonstration of speed difference
                UserDataHash.Add(i, "Hash User " + i);
                UserDataList.Add(new UserDataStruct(i, "List User " + i));
                UserDataDict.Add(i, "Dict User " + i);
            }
            //removing
            if (UserDataHash.ContainsKey(0))
            {
                UserDataHash.Remove(0);
            }
            //setting values
            if(UserDataHash.ContainsKey(1))
            {
                UserDataHash[1] = "Replacement Test";
            }
            //looping through
            /*foreach (DictionaryEntry item in UserDataHash)
            {
                Console.WriteLine("key: "+item.Key+" / Value"+item.Value);
            }*/
            //Access (the main reason for use as a near constant O(1) lookup times)
            Random randomuserGen = new();
            int randomUser = -1;

            sw.Start();
            float startTime = 0;
            float endTime = 0;
            float deltaTime = 0;

            int cycles = 5;
            int cycle = 0;
            string username = string.Empty;
            while (cycle < cycles)
            {
                randomUser = randomuserGen.Next(30000000, 40000000);

                startTime = sw.ElapsedMilliseconds;
                // access from list
                username = GetUserFromList(randomUser);
                endTime = sw.ElapsedMilliseconds;
                deltaTime = endTime - startTime;
                Console.WriteLine("Time taken to retrieve "+ username+" from list took "+ string.Format("{0:0.##}", deltaTime) + "ms");

                startTime = sw.ElapsedMilliseconds;
                //access from hash
                username = (string)UserDataHash[randomUser];
                endTime = sw.ElapsedMilliseconds;
                deltaTime = endTime - startTime;
                Console.WriteLine("Time taken to retrieve " + username + " from hash took " + string.Format("{0:0.##}", deltaTime) + "ms");

                startTime = sw.ElapsedMilliseconds;
                // access from list
                //username = GetUserFromDict(randomUser);
                username = UserDataDict.FirstOrDefault(e => e.Key == randomUser).Value;
                endTime = sw.ElapsedMilliseconds;
                deltaTime = endTime - startTime;
                Console.WriteLine("Time taken to retrieve " + username + " from dictionary took " + string.Format("{0:0.##}", deltaTime) + "ms\n");

                cycle++;
            }
        }
        static string GetUserFromList (int userId)
        {
            for (int i = 0; i < UserDataList.Count; i++)
            {
                if (UserDataList[i].ID == userId)
                {
                    return UserDataList[i].Name;
                }
            }
            return string.Empty;
        }
        static string GetUserFromDict(int userId)
        {
            if(UserDataDict.TryGetValue(userId, out string Username))
            {
                return Username;
            }
            return string.Empty;
        }
    }
}