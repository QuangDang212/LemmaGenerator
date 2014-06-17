﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LemmaSharp.Classes;
using Test.Classes;

namespace Test
{
    class Program
    {

        static void Main(string[] args)
        {
            var lemmatizer = CreateLemmatizerFromFile();

            // a bunch of words to lemmatize
            /*const string input = "Stanford University is located in California. It is a great university.";
            var words = Regex.Split(input, @"\W+").Where(w => !string.IsNullOrEmpty(w));*/
            /*var words = new[] { "going", "working", "finding", "got", "found", "heard", 
                "told", "said", "thought", "met", "saw", "left", "took", 
                "was", "were", "lost", "felt", "fell", "brought", "spoke", 
                "making", "thinking", "meeting", "running", "staring", "seeing"};*/
            //var words = new[] {"wrote", "Wrote", "WROTE", "Written", "did", "Did", "DID", "testing", "Testing", "TESTING"};
            var words = new[] {"took"};

            // for each word, print it's lemma
            foreach (var word in words)
            {
                var lemma = lemmatizer.Lemmatize(word);
                Console.WriteLine(word + " --> " + lemma);
            }

            // Create readable file
            var currentDirectory = Directory.GetCurrentDirectory();
            var dataFilePath = string.Format("{0}/{1}/{2}", currentDirectory, "../../Data/Uncompressed", "full7z-multext-en.lem");

            using (var fstream = File.Create(dataFilePath))
            {
                lemmatizer.Serialize(fstream, false);
            }

            // add examples
            var examples = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("working", "work"),
                new Tuple<string, string>("left", "leave"),
                new Tuple<string, string>("theft", "theft"),
            };
            foreach (var example in examples)
            {
                Console.WriteLine("Add word '{0}' to examples", example.Item1);
                lemmatizer.AddExample(example.Item1, example.Item2, 5);

                var lemma = lemmatizer.Lemmatize(example.Item1);
                Console.WriteLine("{0} --> {1}", example.Item1, lemma);
            }

            Console.WriteLine("OK");
            Console.ReadLine();
        }

        private static Lemmatizer CreatePreBuiltLemmatizer()
        {
            var lemmatizer = new LemmatizerPrebuiltFull(LanguagePrebuilt.English);
            return lemmatizer;
        }

        private static Lemmatizer CreateLemmatizerFromFile()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var dataFilePath = string.Format("{0}/{1}/{2}", currentDirectory, "../../Data/Custom", "english.lem");
            using (var stream = File.OpenRead(dataFilePath))
            {
                var lemmatizer = new Lemmatizer(stream);
                return lemmatizer;
            }
        }
    }
}
