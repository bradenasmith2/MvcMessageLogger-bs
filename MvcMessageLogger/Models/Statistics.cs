using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMessageLogger.DataAccess;
namespace MvcMessageLogger.Models
{
    public class Statistics
    {
        public string statModel { get; set; }

        public List<User> MessageCountDescUser(MvcMessageLoggerContext context)
        {
            var usersDesc = new List<User>();

            foreach (var user in context.Users.OrderByDescending(e => e.Messages.Count()).Include(e => e.Messages))
            {
                usersDesc.Add(user);
            }
            return usersDesc;
        }

        public List<String> MostCommonPersonal(MvcMessageLoggerContext context, User user)
        {
            Statistics stat = new Statistics();
            var wordsList = new List<string>();

            foreach (var e in user.Messages)
            {
                wordsList.AddRange(e.Content.ToLower().Split());
            }
            stat.MostCommonPublic(wordsList);

            return stat.MostCommonPublic(wordsList);
        }

        //this will filter ANY word set, it is not tied to any user/message/stat
        public List<String> MostCommonPublic(List<String> wordsList)
        {
            var returnPhrase = new List<string>();

            for (int i = 0; i < wordsList.Count; i++)//counting through all messages
            {
                wordsList[i] = wordsList[i].Remove(wordsList[i].Count());//removing the message that was checked
            }

            var sorted = new Dictionary<string, int>();
            foreach (var m in wordsList)
            {
                if (sorted.ContainsKey(m.ToLower()))
                {
                    sorted[m] += 1;//if the word has already been added, it needs to +1 to its value
                }
                else
                {
                    sorted.Add(m, 1);//adds a word to the dictionary with a starting value of '1'
                }
            }
            var sortedOrdered = sorted.OrderByDescending(d => d.Value);//sort the dictionary based on value

            foreach (KeyValuePair<string, int> word in sortedOrdered)
            {
                if (returnPhrase.Count() >= 10) break;//will only list the 10 most common words, can be adjusted here
                string w = "";
                w += word.Key;//adding the key to an empty string
                w += ": ";//separate key: value
                w += word.Value.ToString();//adding the value to the string
                returnPhrase.Add(w);//should be word : 4
            }
            foreach (var e in returnPhrase)//not working
            {
                Console.WriteLine(e); //this wil print the personal common word properly, but when the method is invoked, it does not print properly.
            }

            return returnPhrase;
        }

            //foreach (KeyValuePair<DateTime, int> hour in countedHoursDesc)
            //{
            //    //stack overflow
            //    string h = "";
            //    if (hour.Key.Hour > 12)//if the Hour is greater than 12, i.e 14:23 (2:23)
            //    {
            //        h = (hour.Key.Hour - 12).ToString() + " PM"; //subtract 12, to get the nonmilitary time
            //    }
            //    else if (hour.Key.Hour == 12)//if the hour IS 12
            //    {
            //        h = (hour.Key.Hour).ToString() + " PM";//add PM
            //    }
            //    else if (hour.Key.Hour > 0)//if the hour is in between 0 and 12, it is AM
            //    {
            //        h = (hour.Key.Hour).ToString() + " AM";//so add AM
            //    }
            //    else
            //    {
            //        h = "12 AM";//everything else is specified
            //    }
            //    returnPhrase += h;
            //}

        public string ActiveHour(MvcMessageLoggerContext context)//works but is not formatted
        {
            string returnPhrase = "";
            int count = 0;
            var dictionary = new Dictionary<string, int>();
            var timesList = new List<string>();
            var messages = context.Messages.ToList();

            foreach (var message in messages)
            {
                timesList.Add(message.CreatedAt.Hour.ToString());
            }

            foreach (var time in timesList)
            {
                if (dictionary.ContainsKey(time))
                {
                    dictionary[time] += 1;//if the hour already exists in the key, add 1 to its value
                }
                else
                {
                    dictionary.Add(time, 1);//when a message comes in, create a kvp with hour: count
                }
            }
            var SingleTimeDictionary = dictionary.OrderByDescending(e => e.Value).First();
            returnPhrase = SingleTimeDictionary.ToString();

            var value = SingleTimeDictionary.Value.ToString();
            var key = SingleTimeDictionary.Key.ToString();
            var phrase = $"At: {key} there were {value} messages created.";

            return phrase;
        }
    }
}
