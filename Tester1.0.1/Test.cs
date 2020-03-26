using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;

namespace Tester1._0._1
{
    public class Test
    {
        public List<Question> questions { get; }
        public Test()
        {
            questions = new List<Question>();
        }
        public Test(string path)
        {
            questions = new List<Question>();
            List<Question> listOfQuestions = ReadTheQuestionFile(path); //read from the first file
            List<varForChooseRightOne> listForChooseRightOnes = ReadTheChooseRightFile(path);//read the chooseRight file
            List<varForComparison> listForComparisons = ReadTheComparisonFile(path);//read the comparison file
            for (int i = 0; i < listOfQuestions.Count; i++)
            {
               
                if (listForChooseRightOnes.Count>0 && 
                    listOfQuestions[i].number == listForChooseRightOnes[0].number)
                {
                    List<varForChooseRightOne> list = new List<varForChooseRightOne>();
                    while (listForChooseRightOnes.Count>0 &&
                           listOfQuestions[i].number == listForChooseRightOnes[0].number)
                    {
                        list.Add(listForChooseRightOnes[0]);
                        listForChooseRightOnes.Remove(listForChooseRightOnes[0]);
                    }
                    questions.Add(new ChooseRightOne(listOfQuestions[i], list));
                }
                else if(listForComparisons.Count>0 &&
                        listOfQuestions[i].number== listForComparisons[0].number)
                {
                    List<varForComparison> list = new List<varForComparison>();
                    while (listForComparisons.Count>0 && 
                           listOfQuestions[i].number == listForComparisons[0].number)
                    {
                        list.Add(listForComparisons[0]);
                        listForComparisons.Remove(listForComparisons[0]);
                    }
                    questions.Add(new Comparison(listOfQuestions[i], list));
                }
            }
        }
        /*public Test(Question question)
        {
            questions = new List<Question>();
            questions.Add(question);
        }*/
       
        /// <summary>
        /// Метод додавання нового питання до тесту
        /// </summary>
        /// <param name="question"></param>
        public void AddToTest(Question question)
        {
            questions.Add(question);
        }
        /// <summary>
        /// Додавання питання типу вибір правильних до тесту
        /// </summary>
        /// <param name="question"></param>
        public void AddToTest(ChooseRightOne question)
        {
            questions.Add(question);
        }
        /// <summary>
        /// Додавання питання типу порівняння до тесту
        /// </summary>
        /// <param name="question"></param>
        public void AddToTest(Comparison question)
        {
            questions.Add(question);
        }
        /// <summary>
        /// Повертає номер наступного питання
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfNextQuestion()
        {
            return questions.Count + 1;
        }
        /// <summary>
        /// Метод видаляє питання з тесту
        /// </summary>
        /// <param name="numberOfQuestionToRemove"></param>
        public void RemoveQuestionFromList(int numberOfQuestionToRemove)
        {
            for(int i=0; i<questions.Count; i++)
            {
                if(questions[i].number == numberOfQuestionToRemove)
                {
                    questions.Remove(questions[i]);
                }
            }
            //зміна номерації
            for(int i=0; i<questions.Count; i++)
            {
                questions[i].number = i + 1;
            }
        }
        /// <summary>
        /// Метод, що редагує питання типу ChooseRigthOne
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="question"></param>
        /// <param name="score"></param>
        /// <param name="listChooseRight"></param>
        public void EditQuestion(int Index, string question, float score, List<varForChooseRightOne> listChooseRight)
        {
            /*RemoveQuestionFromList(Index);
            ChooseRightOne choose = new ChooseRightOne(Index, question, score);
            choose.AddListOptions(listChooseRight);
            this.AddToTest(choose);*/
            ChooseRightOne choose = new ChooseRightOne(Index, question, score);
            choose.AddListOptions(listChooseRight);
            questions[Index-1] = new Question();
            questions[Index - 1] = choose;
        }
        /// <summary>
        /// Метод, що редагує питання типу Comparison
        /// </summary>
        /// <param name="index"></param>
        /// <param name="question"></param>
        /// <param name="score"></param>
        /// <param name="listComparison"></param>
        public void EditQuestion(int index, string question, float score, List<varForComparison> listComparison)
        {
            Comparison comparison = new Comparison(index, question, score);
            comparison.AddListOptions(listComparison);
            questions[index - 1] = new Question();
            questions[index - 1] = comparison;
        }        
        /// <summary>
        /// Метод повертає строку - вивод тесту
        /// </summary>
        /// <returns></returns>
        public string Show()
        {
            string strToReturn = "";
            foreach(Question q in questions)
            {
                strToReturn += q.Show() + "\n";
            }
            return strToReturn;
        }

        public void SaveTestToFile(string path)
        {
            //створюємо 3 списка - список з самими питаннями, з відповідями типу вибір правильних і співставлення
            List<Question> listOfQuestions = new List<Question>();
            List<varForChooseRightOne> listOfChoose = new List<varForChooseRightOne>();
            List<varForComparison> listOfComparison = new List<varForComparison>();
            for (int i = 0; i < questions.Count; i++)
            {
                listOfQuestions.Add(questions[i]);
                if (questions[i] is ChooseRightOne)
                {
                    ChooseRightOne chooseRightOne = (ChooseRightOne) questions[i];
                    for (int j = 0; j < chooseRightOne.options.Count; j++)
                    {
                        listOfChoose.Add(chooseRightOne.options[j]);
                    }
                }
                else
                {
                    Comparison comparison = (Comparison) questions[i];
                    for (int j = 0; j < comparison.options.Count; j++)
                    {
                        listOfComparison.Add(comparison.options[j]);
                    }
                }
                
            }
            WriteToFile(path, listOfQuestions);
            WriteToChooseRightFile(path, listOfChoose);
            WriteToComparisonFile(path, listOfComparison);
        }
        /// <summary>
        /// Метод зчитування тесту з файлу
        /// </summary>
        /// <param name="path"></param>
      

        public List<Question> ReadTheQuestionFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            CsvReader csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.Delimiter = ",";
            var records = csv.GetRecords<Question>();
            return records.ToList();
        }

        public List<varForChooseRightOne> ReadTheChooseRightFile(string path)
        {
            path = ChangingThePathForChooseRight(path);
            StreamReader reader = new StreamReader(path);
            CsvReader csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.Delimiter = ",";
            var records = csv.GetRecords<varForChooseRightOne>();
            return records.ToList();
        }

        public List<varForComparison> ReadTheComparisonFile(string path)
        {
            path = ChangingThePathForComparison(path);
            StreamReader reader = new StreamReader(path);
            CsvReader csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.Delimiter = ",";
            var records = csv.GetRecords<varForComparison>();
            return records.ToList();
        }
        
        public void WriteToFile(string path, List<Question> listOfQuestions)
        {
            var records = listOfQuestions;

            StreamWriter writer = new StreamWriter(path);
            CsvWriter csv = new CsvWriter(writer);
            
            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.Delimiter = ",";
            csv.WriteRecords(records);
            writer.Close();
        }

        public void WriteToChooseRightFile(string path, List<varForChooseRightOne> chooseRightOnes)
        {
            path = ChangingThePathForChooseRight(path);
            var records = chooseRightOnes;
            StreamWriter writer = new StreamWriter(path);
            CsvWriter csv = new CsvWriter(writer);
            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.Delimiter = ",";
            csv.WriteRecords(records);
            writer.Close();
        }

        public void WriteToComparisonFile(string path, List<varForComparison> comparisons)
        {
            path = ChangingThePathForComparison(path);
            var records = comparisons;
            StreamWriter writer = new StreamWriter(path);
            CsvWriter csv = new CsvWriter(writer);
            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.Delimiter = ",";
            csv.WriteRecords(records);
            writer.Close();
        }

        
        /// <summary>
        /// Метод, що повертає шлях до файлу збереження варіантів відповіді для вибору правильних відповідей
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ChangingThePathForChooseRight(string path)
        {
            string toReturn="";
            string[] array = path.Split('\\');
            for (int i = 0; i < array.Length - 1; i++)
            {
                toReturn += array[i] + "\\";
            }
            string[] array1 = array[array.Length - 1].Split('.');
            toReturn += array1[0] + "ChooseRight.csv";
            return toReturn;
        }
        /// <summary>
        /// Метод, що повертає шлях до файлу збереження варіантів відповіді для порівняння
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ChangingThePathForComparison(string path)
        {
            string toReturn="";
            string[] array = path.Split('\\');
            for (int i = 0; i < array.Length - 1; i++)
            {
                toReturn += array[i] + "\\";
            }
            string[] array1 = array[array.Length - 1].Split('.');
            toReturn += array1[0] + "Comparison.csv";
            return toReturn;
        }
        
        
    }
}
