using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backoffice.Services
{
    public static class Greeking
    {
        #region Settings
        private const int minTitleWordCount = 3;
        private const int maxTitleWordCount = 9;

        private const int minParagraphsCount = 2;
        private const int maxParagraphsCount = 8;
        private const int minParagraphsWordCount = 20;
        private const int maxParagraphsWordCount = 60;

        private static Random Index = new Random();
        #endregion 

        #region Words
        private static string[] Words
        {
            get 
            {  
                var availableWords = "Lorem ipsum dolor sit amet consectetuer adipiscing elit Nunc feugiat Sed suscipit libero nec sem Vestibulum aliquet neque nec nisl Fusce turpis tortor blandit in posuere at hendrerit vitae magna Suspendisse nulla purus dapibus vel venenatis sed fermentum vitae nulla In suscipit nulla sagittis cursus tempor arcu pede vestibulum lacus at semper tortor ligula eget erat Cras scelerisque leo eu erat Quisque egestas turpis vel facilisis nonummy mauris enim rutrum quam vitae pharetra massa quam ac felis Vestibulum pede quam semper vitae interdum vel pulvinar ut diam Mauris aliquam elementum pede Suspendisse feugiat enim sit amet erat facilisis sagittis Duis laoreet turpis at risus Sed at est et dolor tristique scelerisque Maecenas dui Vivamus ut lorem Vestibulum lacus quam tempus et luctus at scelerisque nec libero";
                return availableWords.Split(' ');
            }
        }
        #endregion

        #region Greeking Functions
        public static string Name
        {
            get
            {
                return toUpperCase(getRandomWord());
            }
        }
        public static string FullName
        {
            get
            {
                var result = string.Empty;
                var wordCount = Index.Next(2, 3);
                for(var x = 0; x < wordCount; x++)
                {
                    if(!string.IsNullOrEmpty(result)) result += " ";
                    result += Name;
                }

                return result;
            }
        }
        public static string Company
        {
            get
            {
                var companyOptions = new string[] { "Co.", "LLC", "DBA", "Global", "" };
                var company = companyOptions[Index.Next(companyOptions.Length)];

                var result = string.Empty;
                var wordCount = Index.Next(1, 3);
                for(var x = 0; x < wordCount; x++)
                {
                    if(!string.IsNullOrEmpty(result)) result += " ";
                    result += toUpperCase(getRandomWord());
                }

                return result + " " + company;

            }
        }
        public static string Username
        {
            get
            {
                var joinOptions = new string[] { "_", "" };
                var join = joinOptions[Index.Next(joinOptions.Length)];

                var wordCount = Index.Next(1, 4);
                var words = new List<string>();


                var result = string.Empty;
                for(var x = 0; x < wordCount; x++)
                {
                    if(!string.IsNullOrEmpty(result) && Index.Next(2) == 1) result += join;
                    result += getRandomWord();
                }
                return result;
            }
        }
        public static string Email
        {
            get
            {
                return Username + "@" + getRandomWord(1) + "." + getRandomWord(2).Substring(0, 3);
            }
        }
        public static string Phone
        {
            get
            {
                var result = string.Empty;

                for(var x = 0; x < 10; x++)
                {
                    result += Index.Next(9).ToString();
                    if(x == 2 || x == 5) result += "-";
                }

                if(result[0] == '0') result = "8" + result.Substring(1);

                return result;
            }
        }
        public static string Server
        {
            get
            {
                var extensionOptions = new string[] { "com", "net", "org", "gov", "info" };
                var extension = extensionOptions[Index.Next(extensionOptions.Length)];

                var result = string.Empty;
                var wordCount = Index.Next(1, 3);
                for(var x = 0; x < wordCount; x++)
                {
                    if(!string.IsNullOrEmpty(result)) result += ".";
                    result += getRandomWord();
                }

                return result + "." + extension;
            }
        }
        public static string Url
        {
            get
            {
                return "http://" + Server;
            }
        }

        public static string Title
        {
            get
            {
                var wordCount = Index.Next(minTitleWordCount, maxTitleWordCount);
                var words = new List<string>();

                for(var x = 0; x < wordCount; x++) {
                    var word = getRandomWord();
            
                    if(word.Length < 5 && getRandomPercent() < 0.7M)
                    {
                        word = word.ToLower();
                    }

                    words.Add(word);
                }

                words[0] = toUpperCase(words[0]);

                var result = string.Empty;
                words.ForEach(c => 
                    {
                        if(!string.IsNullOrEmpty(result)) result += " ";
                        result += c;
                    });


                return result;
            }
        }
        public static string Paragraph
        {
            get
            {
                var wordCount = Index.Next(minParagraphsWordCount, maxParagraphsWordCount);
                var words = new List<string>();

                for(var x = 0; x < wordCount; x++) {
                    var word = getRandomWord();
                    if(getRandomPercent() > 0.85M)
                    {
                        word = getRandomWord() + ". " + toUpperCase(word);
                    }

                    words.Add(word);
                }

                words[0] = toUpperCase(words[0]);

                var result = string.Empty;
                words.ForEach(c => 
                    {
                        if(!string.IsNullOrEmpty(result)) result += " ";
                        result += c;
                    });


                return "<p>" + result + ".</p>";
            }
        }
        public static string Paragraphs
        {
            get
            {
                var paragraphCount = Index.Next(minParagraphsCount, maxParagraphsCount);
                var paragraphs = new List<string>();

                for(var x = 0; x < paragraphCount; x++) {
                    paragraphs.Add(Paragraph);
                }

                var result = string.Empty;
                paragraphs.ForEach(c => 
                    {
                        if(!string.IsNullOrEmpty(result)) result += " ";
                        result += c;
                    });


                return result;
            }
        }
        #endregion

        #region Helpers
        private static string getRandomWord()
        {
            return Words[Index.Next(0, Words.Length - 1)].ToLower();
        }
        private static string getRandomWord(int wordCount)
        {
            var result = string.Empty;
            for(var x = 0; x < wordCount; x++)
            {
                result += Words[Index.Next(0, Words.Length - 1)].ToLower();
            }
            return result;
        }
        private static decimal getRandomPercent() {
            return Convert.ToDecimal(Index.Next(0, 100) / 100M);
        }
        private static string toUpperCase(string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1);
        }
        #endregion
    }
}