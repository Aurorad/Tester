using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Tester1._0._1
{
    public partial class TestingWindow : Window
    {
        private Test test;
        
        private StackPanel StackPanelListOfQuestions;
        private TextBlock[] textBlockListOfQuestions;
        private ScrollViewer scroll;
        public TestingWindow()
        {
            InitializeComponent();
            scroll = new ScrollViewer();
            StackPanelListOfQuestions = new StackPanel();
            //відкриття тесту
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = ".csv";
            openFile.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            openFile.Title = "Виберіть основний файл тесту, додаткові файли мають міститися в цій же папці";
            if (openFile.ShowDialog() == true)
            {
                test = new Test(openFile.FileName);
                ListOfQuestions();
                //CreatingNewQuestion();
            }
            
        }

        private void ListOfQuestions()
        {
            //deleting the list
            textBlockListOfQuestions = new TextBlock[test.questions.Count];
            for (int i = 0; i < textBlockListOfQuestions.Length; i++)
            {
                textBlockListOfQuestions[i] = new TextBlock
                {
                    Text = "",
                    Margin = new Thickness(10, 10, 10, 10),
                    TextTrimming = TextTrimming.CharacterEllipsis,
                };
                textBlockListOfQuestions[i].Text = String.Format(test.questions[i].number 
                                                                 + ". " + test.questions[i].question);
                StackPanelListOfQuestions.Children.Add(textBlockListOfQuestions[i]);
            }
            scroll.Content = StackPanelListOfQuestions;
            AddToGrid(1, 0, scroll, TestingGrid.ColumnDefinitions.Count);
        }

        private void AddToGrid(int row, int column, UIElement element, int rowSpan)
        {
            Grid.SetRowSpan(element, rowSpan);
            AddToGrid(row, column, element);
        }

        private void AddToGrid(int row, int column, UIElement elemen)
        {
            Grid.SetColumn(elemen, column);
            Grid.SetRow(elemen, row);
            TestingGrid.Children.Add(elemen);
        }
    }
}