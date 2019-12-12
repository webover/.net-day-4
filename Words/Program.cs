using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Words
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();

            long totalWords = 0;
            long totalDistinctWords = 0;
            long countSearchedWord = 0;
            string searchWord = "ana";

            List<string> files = new List<string>();

            foreach (string file in Directory.EnumerateFiles(@"D:\dev-env\.net-day-4\Words\files", "*.dat"))
            {
                files.Add(file);
            }

            ConcurrentDictionary<string, List<string>> concurrentDictionary = new ConcurrentDictionary<string, List<string>>();

            Parallel.For(0, files.Count, (index, state) => {

                Console.WriteLine("Beginning iteration {0}", index);
                List<string> words = new List<string>();
                HashSet<string> distinctWords = new HashSet<string>();
                int cntSearchedWord = 0;
                //search for a specific words (contains)
                foreach (string line in File.ReadAllLines(files[index]))
                {
                    if(line.Contains(searchWord))
                    {
                        countSearchedWord++;
                    }

                    foreach (string word in line.Trim().Split(" "))
                    {
                        words.Add(word);
                        distinctWords.Add(word);                                     
                    }
                }
                //group words by xs 
                concurrentDictionary.AddOrUpdate("xs", key => new List<string>(), (key, list) =>
                {
                    return list.Concat(distinctWords.Where(dw => dw.Length < 5).ToList()).ToList();
                });
                //group words by s
                concurrentDictionary.AddOrUpdate("s", key => new List<string>(), (key, list) =>
                {
                    return list.Concat(distinctWords.Where(dw => dw.Length >= 5 && dw.Length < 10).ToList()).ToList();
                });
                //group words by m
                concurrentDictionary.AddOrUpdate("m", key => new List<string>(), (key, list) =>
                {
                    return list.Concat(distinctWords.Where(dw => dw.Length >= 10 && dw.Length < 15).ToList()).ToList();
                });
                //group words by m
                concurrentDictionary.AddOrUpdate("l", key => new List<string>(), (key, list) =>
                {
                    return list.Concat(distinctWords.Where(dw => dw.Length >= 15).ToList()).ToList();
                });

                Console.WriteLine("Completed iteration {0}", index);
                //count of all words
                Interlocked.Add(ref totalWords, words.Count);
                //count distinct words
                Interlocked.Add(ref totalDistinctWords, distinctWords.Count);
                Interlocked.Add(ref countSearchedWord, cntSearchedWord);                             
            });

            Console.WriteLine($"{files.Count} files, {totalWords} total words and {totalDistinctWords} total distinct words");
            Console.WriteLine($"Word {searchWord} was found {countSearchedWord} times");
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.TotalSeconds} seconds");
        }
    }
}
