namespace SEW3_5_3_Laufzeitanalyse_Playlist_gr2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int arrSize = 5; // Anzahl Lieder, die verarbeitet werden

            int searchDuration = 200; // in Sekunden
            // searchDuration = Convert.ToInt32(Console.ReadLine());

            int[] arr = new int[arrSize];
            for (int i = 0; i < arrSize; i++) // array init mit index
            {
                arr[i] = i;
            }

            List<Song> songs = new List<Song>();
            string path = "SEW3 05 Playlist.csv";

            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string line;
                    bool firstline = true;
                    int songNumber = 0; // Limitiert die Anzahl an Liedern, die eingelesen werden.
                    while ((line = sr.ReadLine()) != null && songNumber++ < arrSize)
                    {
                        if (firstline)
                        {
                            firstline = false;
                            songNumber--;
                            continue;
                        }
                        string[] values = line.Split(";");
                        string title = values[0];
                        string artist = values[1];
                        string album = values[2];
                        int duration = Convert.ToInt32(values[3].Replace("s", ""));
                        songs.Add(new Song(title, artist, album, duration));
                    }
                }
            }

            List<List<int>> finalList = new List<List<int>>();
            ulong counter = 0;
            recursiveTest(arr, new List<int>(), finalList, songs, 0, arrSize, ref counter, searchDuration); // Start des rekursiven Aufruf

            void recursiveTest(int[] arr, List<int> testList, List<List<int>> finalList, List<Song> songs ,int n, int maxSize, ref ulong counter, int searchDuration)
            {
                if (n >= maxSize) // Abbruchbedingung, nach letztem Element Abbruch
                {
                    return;
                }
                counter++;

                testList.Add(arr[n]); // Hinzufügen des aktuellen Elements zur TestListe

                /*
                Console.Write("[ "); // Ausgabe
                foreach (int x in testList)
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine("]");
                */

                // Checkt die Liste, ob Sie den Kriterien entspricht
                checkList(testList, finalList, songs, searchDuration);

                recursiveTest(arr, testList, finalList, songs, n + 1, maxSize, ref counter, searchDuration); // Rekursiver Aufruf mit n + 1

                testList.Remove(arr[n]);

                recursiveTest(arr, testList, finalList, songs, n + 1, maxSize, ref counter, searchDuration); // Rekursiver Aufruf mit n + 1
            }

            void checkList(List<int> testList, List<List<int>> finalList, List<Song> songs, int searchDuration)
            {
                if (finalList.Count < 10)
                {
                    finalList.Add(new List<int>(testList));
                }
                else
                {
                    int testSongsDuration = calculateDuration(testList, songs);
                    if (testSongsDuration > searchDuration)
                    {
                        return;
                    }
                    int diffTestSongs = searchDuration - testSongsDuration;
                    foreach (List<int> actualBestSongs in finalList)
                    {
                        int durActual = calculateDuration(actualBestSongs, songs);
                        int diffActual = durActual - searchDuration;
                        if (diffTestSongs < diffActual)
                        {
                            finalList.Insert(finalList.IndexOf(actualBestSongs), new List<int>(testList));
                            finalList.RemoveAt(finalList.Count - 1);
                            break;
                        }
                    }
                }
            }

            // gibt die Gesamtzeit der Song-Liste zurück.
            int calculateDuration(List<int> testList, List<Song> songs)
            {
                int d = 0;
                foreach (int i in testList)
                {
                    d += songs[i].Duration;
                }
                return d;
            }

            Console.WriteLine("Counter: " + counter);

            ///
            //for (int i = 0; i < arrSize; i++)
            //{
            //    Console.WriteLine(arr[i]);
            //    for (int j = i; j < arrSize; j++)
            //    {
            //        Console.Write(arr[j]);
            //    }
            //    Console.WriteLine();
            //}
        }
    }
    public class Song
    {
        public string Title { get; }
        public string Artist { get; }
        public string Album { get; }
        public int Duration { get; }
        public Song(string title, string artist, string album, int duration)
        {
            Title = title;
            Artist = artist;
            Album = album;
            Duration = duration;
        }
    }
}