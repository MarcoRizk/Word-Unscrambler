using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordUnscrambler.Workers;
using WordUnscrambler.Data;

namespace WordUnscrambler
{
    class Program
    {
        private static readonly FileReader _fileReader = new FileReader();
        private static readonly WordMatcher _wordsMatcher = new WordMatcher();
        

        static void Main(string[] args)
        {
            try
            {
                bool continueWordUnscramle = true;
                do
                {
                    Console.WriteLine(Constants.OptionsOnHowToEnterScrambledWords);
                    var option = Console.ReadLine() ?? string.Empty;

                    switch (option.ToUpper())
                    {
                        case Constants.File:
                            Console.WriteLine(Constants.EnterScrambledWordsViaFile);
                            ExecuteScrampledWordsInFileScenario();
                            break;
                        case Constants.Manual:
                            Console.WriteLine(Constants.EnterScrambledWordsManually);
                            ExecuteScrampledWordsManualInputScenario();
                            break;
                        default:
                            Console.WriteLine(Constants.EnterScrambledOptionNotRecognized);
                            break;


                    }
                    var contineWordUnscrambleDecision = string.Empty;
                    do
                    {
                        Console.WriteLine(Constants.OptionsOnContinuingTheProgram);
                        contineWordUnscrambleDecision = Console.ReadLine() ?? string.Empty;

                    } while (
                        !contineWordUnscrambleDecision.Equals(Constants.Yes, StringComparison.OrdinalIgnoreCase) &&
                        !contineWordUnscrambleDecision.Equals(Constants.No, StringComparison.OrdinalIgnoreCase));

                    continueWordUnscramle = contineWordUnscrambleDecision.Equals(Constants.Yes, StringComparison.OrdinalIgnoreCase);

                } while (continueWordUnscramle);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private static void ExecuteScrampledWordsManualInputScenario()
        {
            var manualInput = Console.ReadLine() ?? string.Empty;
            string[] scrambledWords = manualInput.Split(',');
            DispalyMatchedUnscrambledWords(scrambledWords);
        }



        private static void ExecuteScrampledWordsInFileScenario()
        {
            try
            {
                var fileName = Console.ReadLine() ?? string.Empty;
                string[] scrambledWords = _fileReader.Read(fileName);
                DispalyMatchedUnscrambledWords(scrambledWords);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constants.ErrorScrambledWordsCannotBeLoaded + ex.Message);
            }
            
        }

        private static void DispalyMatchedUnscrambledWords(string[] scrambledWords)
        {
            string[] wordList = _fileReader.Read(Constants.wordListFilename);

            List<MatchedWord> matchedWords = _wordsMatcher.Match(scrambledWords,wordList);

            if (matchedWords.Any())
            {
                foreach (var matchedWord in matchedWords)
                {
                    Console.WriteLine(Constants.MatchFound, matchedWord.ScrambledWord, matchedWord.Word);

                }
            }
            else
            {
                Console.WriteLine(Constants.MatchNotFound);
            }
        }
    }
}
