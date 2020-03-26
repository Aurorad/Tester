using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Tester1._0._1
{
    /// <summary>
    /// Interaction logic for CreateTestWindow.xaml
    /// </summary>
    public partial class CreateTestWindow : Window
    {
        private static Test ourTest;
        private int type = -1; //0 - choose right, 1 - comparison
        private int numberOfOptions = -1;

        private ComboBox comboBox1;//type of Question
        private ComboBox comboBox2; // number of options
        private RowDefinition[] rowDefinitions;

        private Button btnSaveEdition;
        private Button btnNextQuestion;
        private TextBlock textBlockQuestion;
        private TextBox textBoxQuestion;
        private TextBlock textBlockScore;
        private TextBox textBoxScore;

        private CheckBox[] checkBoxes;
        private TextBox[] tb1;
        private TextBox[] tb2;

        private StackPanel StackPanelListOfQuestions;
        private TextBlock[] textBlockListOfQuestions;
        private int Index;
        private ScrollViewer scroll;
        public CreateTestWindow()
        {
            InitializeComponent();
            
            textBlockQuestion = new TextBlock
            {
                Text = "Питання:",
                Margin = new Thickness(10, 10, 10, 10),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            textBoxQuestion = new TextBox
            {
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                Margin = new Thickness(10, 10, 20, 10)
            };
            textBoxQuestion.SpellCheck.IsEnabled = true;
            textBlockScore = new TextBlock
            {
                Text = "Кількість балів:",
                Margin = new Thickness(10, 10, 10, 10),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            textBoxScore = new TextBox
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10, 10, 10, 10),
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            btnSaveEdition = new Button
            {
                Content = "Зберегти зміни",
                Margin = new Thickness(10, 10, 10, 10),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            btnNextQuestion = new Button
            {
                Content = "Зберегти запитання",
                Margin = new Thickness(10, 10, 10, 10),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            
            scroll = new ScrollViewer();
            StackPanelListOfQuestions = new StackPanel();

        }

        /// <summary>
        /// Обробка події натиску на кнопку "Створити новий тест" меню "Тест"
        /// Додає до CreateTestWindowGrid поля вибору типу тесту і поля вибору к-сті варіантів відповіді
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewTest_Click(object sender, RoutedEventArgs e)
        {
            ourTest = new Test();//перевірити чи нема багів
            CreatingNewQuestion();
        }

        private void CreatingNewQuestion()
        {
            try
            {
                DeleteAllFromGrid();
            }
            catch (Exception) { }
            //Row 0
            // creating  TextBlock 1
            TextBlock textBlock1 = new TextBlock
            {
                Text = "Тип питання:",
                Margin = new Thickness(10, 10, 10, 10),
                Width = 135
            };
            type = -1;//
            numberOfOptions = -1;//
            //comboBox1 with items and selectionChenged event
            comboBox1 = new ComboBox
            {
                Margin = new Thickness(10, 10, 10, 10),
                Width = 250
            };
            comboBox1.Items.Add("Вибір правильних");
            comboBox1.Items.Add("Співставлення");
            comboBox1.SelectionChanged += comboBox1_SelectionChanged;
            //textBlock2
            TextBlock textBlock2 = new TextBlock
            {
                Text = "Кількість варіантів:",
                Margin = new Thickness(10, 10, 10, 10),
                Width = 190
            };
            // comboBox1 with items and selectionChenged event
            comboBox2 = new ComboBox
            {
                Margin = new Thickness(10, 10, 10, 10),
                Width = 45
            };
            comboBox2.Items.Add("3");
            comboBox2.Items.Add("4");
            comboBox2.Items.Add("5");
            comboBox2.Items.Add("6");
            comboBox2.SelectionChanged += ComboBox2_SelectionChanged;
            //StackPanel, adding textBox1, textBox2, comboBox1, comboBox2 and adding StackPanel to CreateTestWindwoGrid
            StackPanel stackPanelRow0 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            stackPanelRow0.Children.Add(textBlock1);
            stackPanelRow0.Children.Add(comboBox1);
            stackPanelRow0.Children.Add(textBlock2);
            stackPanelRow0.Children.Add(comboBox2);
            AddToGrid(0, 2, 4, stackPanelRow0);
        }
        
        /// <summary>
        /// Обробка події зміни значення вибору типу питання
        /// Змінує значення поля типу питання - type 
        /// і виклик методу AddOptionsToGrid() за умови, що к-сть варіантів відповідей задана
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="args"></param>
        private void comboBox1_SelectionChanged(object selected, RoutedEventArgs args)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Вибір правильних":
                    type = 0;
                    break;
                case "Співставлення":
                    type = 1;
                    break;
            }
            //Якщо задано к-сть варіантів, то будуть додані подальші елементи гріду
            if(numberOfOptions != -1) AddOptionsToGrid();
        }
        /// <summary>
        /// Обробка події зміни значення вибору кількості варіантів відповідей
        /// Змінює значення к-сті варіантів - numberOfOptions
        /// і виклик методу AddOptionsToGrid() за умови, що тип питання вже задано
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numberOfOptions = int.Parse(comboBox2.SelectedItem.ToString());
            //Якщо тип питання вже задано, то будуть додані подальші елементи гріду
            if (type != -1) AddOptionsToGrid();
        }
        /// <summary>
        /// Метод, що додає до CreateTestWindowGrid Всі необхідні елементи
        /// </summary>
        private void AddOptionsToGrid()
        {
            //here should add delete method of all rows
            DeleteAllFromGrid();
            //creating rows
            rowDefinitions = new RowDefinition[2 + numberOfOptions];
            for (int i = 0; i < rowDefinitions.Length; i++)
            {
                rowDefinitions[i] = new RowDefinition();
            }
            rowDefinitions[0].Height = new GridLength(50);//це буде к-сть балів
            rowDefinitions[1 + numberOfOptions].Height = new GridLength(50);//це будуть кнопки
            //додавання до гріду
            for (int i = 0; i < rowDefinitions.Length; i++)
            {
                CreateTestWindowGrid.RowDefinitions.Add(rowDefinitions[i]);
            }
            ListOfQuestions();
            //row 1 -> text of question 
            AddToGrid(1, 2, 2, textBlockQuestion);
            textBoxQuestion.Text = "";//обнулення вмісту
            AddToGrid(1, 4, 2, textBoxQuestion);
            //row 2 -> score
            AddToGrid(2, 2, 2, textBlockScore);
            textBoxScore.Text = "";//обнулення вмісту
            AddToGrid(2, 4, textBoxScore);
            // row 4+numberOfOptions -> button
            btnNextQuestion.Click += BtnNextQuestionClick;
            AddToGrid(3 + numberOfOptions, 5, btnNextQuestion);
            //
            switch (type)
            {
                case 0:
                    ChooseRightGrid();
                    break;
                case 1:
                    ComparisonGrid();
                    break;
            }
        }
        /// <summary>
        /// Метод, що додає до CreateTestWindowGrid поля для варіантів типу ChooseRight
        /// </summary>
        private void ChooseRightGrid()
        {
            checkBoxes = new CheckBox[numberOfOptions];
            tb1 = new TextBox[numberOfOptions];
            for (int i = 0; i < numberOfOptions; i++)
            {
                //checkBoxes
                checkBoxes[i] = new CheckBox
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                AddToGrid(3 + i, 2, checkBoxes[i]);
                //tb1
                tb1[i] = new TextBox
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new System.Windows.Thickness(10, 10, 20, 10),
                    Text = ""
                };
                tb1[i].SpellCheck.IsEnabled = true;
                
                AddToGrid(3 + i, 3, 3, tb1[i]);
            }
        }
        /// <summary>
        /// Метод, що додає до CreateTestWindowGrid поля для варіантів типу Comparison
        /// </summary>
        private void ComparisonGrid()
        {
            tb1 = new TextBox[numberOfOptions];
            tb2 = new TextBox[numberOfOptions];
            for (int i = 0; i < numberOfOptions; i++)
            {
                tb1[i] = new TextBox
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new System.Windows.Thickness(20, 10, 50, 10),
                    Text = ""
                };
                tb1[i].SpellCheck.IsEnabled = true;
                AddToGrid(3 + i, 2, 3, tb1[i]);
                tb2[i] = new TextBox
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new System.Windows.Thickness(10, 10, 20, 10),
                    Text = ""
                };
                tb2[i].SpellCheck.IsEnabled = true;
                AddToGrid(3 + i, 5, tb2[i]);
                
            }
        }
        /// <summary>
        /// Метод, що прибирає з CreateTestWindowGrid Всі rows, окрім нульового
        /// </summary>
        private void DeleteAllFromGrid()
        {
            if (CreateTestWindowGrid.RowDefinitions.Count > 2)
            {
                try
                {
                    for (int i = 0; i < CreateTestWindowGrid.RowDefinitions.Count - 1; i++)
                    {
                        CreateTestWindowGrid.Children.Remove(checkBoxes[i]);
                        CreateTestWindowGrid.Children.Remove(tb1[i]);
                    }
                }
                catch (Exception) { }
                try
                {
                    for (int i = 0; i < CreateTestWindowGrid.RowDefinitions.Count - 1; i++)
                    {
                        CreateTestWindowGrid.Children.Remove(tb1[i]);
                        CreateTestWindowGrid.Children.Remove(tb2[i]);
                    }
                }
                catch (Exception) { }
                CreateTestWindowGrid.Children.Remove(textBlockQuestion);
                CreateTestWindowGrid.Children.Remove(textBoxQuestion);
                CreateTestWindowGrid.Children.Remove(textBlockScore);
                CreateTestWindowGrid.Children.Remove(textBoxScore);
                CreateTestWindowGrid.Children.Remove(btnNextQuestion);
                for (int i = 0; i < rowDefinitions.Length; i++)
                {
                    CreateTestWindowGrid.RowDefinitions.Remove(rowDefinitions[i]);
                }

                try
                {
                    CreateTestWindowGrid.Children.Remove(btnSaveEdition);
                }catch (Exception) { }
            }
        }
        /// <summary>
        /// Метод, що додає element до CreateTestWindowGrid 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="element"></param>
        private void AddToGrid(int row, int column, UIElement element)
        {
            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
            CreateTestWindowGrid.Children.Add(element);
        }
        /// <summary>
        /// Метод, що додає element до CreateTestWindowGrid і задає RowSpan
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="element"></param>
        /// <param name="rowSpan"></param>
        private void AddToGrid(int row, int column, UIElement element, int rowSpan)
        {
            Grid.SetRowSpan(element, rowSpan);
            AddToGrid(row, column, element);
        }
        /// <summary>
        /// Метод, що додає element до CreateTestWindowGrid і задає ColumnSpan
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="element"></param>
        private void AddToGrid(int row, int column, int columnSpan, UIElement element)
        {
            Grid.SetColumnSpan(element, columnSpan);
            AddToGrid(row, column, element);
        }
        /// <summary>
        /// Метод, що зчитує з форми варіанти відповідей (вибір правильних)
        /// Приймає текст питання і к-сть балів, повертає об'єкт класу ChooseRightOne
        /// </summary>
        /// <param name="question"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        private ChooseRightOne CreatingChooseRightOne(string question, float score)
        {
            
            ChooseRightOne var = new ChooseRightOne(ourTest.GetNumberOfNextQuestion(), question, score);
            List<varForChooseRightOne> list = new List<varForChooseRightOne>();
            for (int i = 0; i < numberOfOptions; i++)
            {
                bool answer;
                if (checkBoxes[i].IsChecked == true) answer = true;
                else answer = false;
                string answerOption = tb1[i].Text;
                list.Add(new varForChooseRightOne(ourTest.GetNumberOfNextQuestion(), answerOption, answer));
            }
            var.AddListOptions(list);
            return var;
        }
        /// <summary>
        /// Метод, що зчитує з форми варіанти відповідей для порівняння
        /// Приймає текст питання і к-сть балів, повератє об'єкт класу Comparison
        /// </summary>
        /// <param name="question"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        private Comparison CreatingComparison(string question, float score)
        {
            Comparison var = new Comparison(CreateTestWindow.ourTest.GetNumberOfNextQuestion(), question, score);
            List<varForComparison> list = new List<varForComparison>();
            for (int i = 0; i < numberOfOptions; i++)
            {
                string var1 = tb1[i].Text;
                string var2 = tb2[i].Text;
                list.Add(new varForComparison(CreateTestWindow.ourTest.GetNumberOfNextQuestion(), var1, var2));
            }
            var.AddListOptions(list);

            return var;
        }
        /// <summary>
        /// Метод обробки події натиску на кнопку "Далі"
        /// Зберігає створене запитання, додає його до тесту, додає питання в список питань, створює нову форму введення питання
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void BtnNextQuestionClick(object sender, RoutedEventArgs args)
        {
           // MessageBox.Show("Створення нового питання");
            string question = textBoxQuestion.Text;
            if (question == "")
            {
                MessageBox.Show("Введіть текст питання!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            float score;
            try
            {
                score = float.Parse(textBoxScore.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Кількість балів за питання було введено неправильно!", "Помилка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            switch (type)
            {
                case 0:
                    ChooseRightOne choose = CreatingChooseRightOne(question, score);
                    ourTest.AddToTest(choose);
                   // MessageBox.Show(String.Format("ChooseRight question have been added\n" + choose.Show()));
                    break;
                case 1:
                    Comparison comparison = CreatingComparison(question, score);
                    ourTest.AddToTest(comparison);
                   // MessageBox.Show(String.Format("Comparison question have been added\n" + comparison.Show()));
                    break;
            }
            ListOfQuestions();
            CreatingNewQuestion();
        }

        /// <summary>
        /// Метод що створює/оновлє вміст списку питань
        /// </summary>
        private void ListOfQuestions()
        {
            DeleteListOfQuestions();
            //creating contexMenu
            ContextMenu menu = new ContextMenu();
            MenuItem itemEdit = new MenuItem { Header = "Редагувати" };
            itemEdit.Click += ItemEdit_Click;
            menu.Items.Add(itemEdit);
            MenuItem itemRemove = new MenuItem { Header = "Видалити" };
            itemRemove.Click += ItemRemove_Click;
            menu.Items.Add(itemRemove);
            //add number of question and text to list
            textBlockListOfQuestions = new TextBlock[ourTest.questions.Count];
            for (int i = 0; i < textBlockListOfQuestions.Length; i++)
            {
                textBlockListOfQuestions[i] = new TextBlock
                {
                    Text = "",
                    Margin = new Thickness(10, 10, 10, 10),
                    TextTrimming = TextTrimming.CharacterEllipsis,
                };
                textBlockListOfQuestions[i].Text = String.Format(ourTest.questions[i].number + ". " + ourTest.questions[i].question);
                textBlockListOfQuestions[i].MouseRightButtonDown += TextBlockList_MouseRightButtonDown;
                textBlockListOfQuestions[i].ContextMenu = menu;
                StackPanelListOfQuestions.Children.Add(textBlockListOfQuestions[i]);
            }
            scroll.Content = StackPanelListOfQuestions;
            AddToGrid(1, 0, scroll, CreateTestWindowGrid.RowDefinitions.Count-1);

        }
        /// <summary>
        /// Метод обробки події натиску на поле "Видалити" контекстного меню списку питань
        /// Видаляє з тесту відповідне питання, оновлює список питань
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Ви впевнені, що хочете видалити питання №" + Index, "Запитання", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    ourTest.RemoveQuestionFromList(Index);
                    ListOfQuestions();
                    break;
                case MessageBoxResult.No:
                    ListOfQuestions();
                    break;
            }
            
        }

        /// <summary>
        /// Метод обробки події натиску на поле "Редагувати" контекстного меню списку питань
        /// відкриває редагування питання
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemEdit_Click(object sender, RoutedEventArgs e)
        {
           
            if (ourTest.questions[Index - 1] is ChooseRightOne)
            {
                type = 0;
            }
            else type = 1;

            //перевірка і очищення гріду
            try
            {
                DeleteAllFromGrid();
            }
            catch (Exception)
            {
            }

            //додавання строки з полями тип питання і к-сть варіантів відповідей 
            TextBlock textBlock1 = new TextBlock
            {
                Text = "Тип питання:",
                Margin = new Thickness(10, 10, 10, 10),
                Width = 135
            };
            comboBox1 = new ComboBox
            {
                Margin = new Thickness(10, 10, 10, 10),
                Width = 250
            };
            comboBox1.Items.Add("Вибір правильних");
            comboBox1.Items.Add("Співставлення");
            //textBlock2
            TextBlock textBlock2 = new TextBlock
            {
                Text = "Кількість варіантів:",
                Margin = new Thickness(10, 10, 10, 10),
                Width = 190
            };
            // comboBox1 with items and selectionChenged event
            comboBox2 = new ComboBox
            {
                Margin = new Thickness(10, 10, 10, 10),
                Width = 45
            };
            comboBox2.Items.Add("3");
            comboBox2.Items.Add("4");
            comboBox2.Items.Add("5");
            comboBox2.Items.Add("6");
            //StackPanel, adding textBox1, textBox2, comboBox1, comboBox2 and adding StackPanel to CreateTestWindwoGrid
            StackPanel stackPanelRow0 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            stackPanelRow0.Children.Add(textBlock1);
            stackPanelRow0.Children.Add(comboBox1);
            stackPanelRow0.Children.Add(textBlock2);
            stackPanelRow0.Children.Add(comboBox2);
            AddToGrid(0, 2, 4, stackPanelRow0);
            if (ourTest.questions[Index - 1] is ChooseRightOne) type = 0;
            else if (ourTest.questions[Index - 1] is Comparison) type = 1;
            switch (type)
            {
                case 0:
                    EditChooseRightOne();

                    break;
                case 1:
                    EditComparison();

                    break;
            }
        }
        private void EditChooseRightOne()
        {
            ChooseRightOne choose = ourTest.questions[Index - 1] as ChooseRightOne;
            numberOfOptions = choose.options.Count;
           
            //comboBox1 selected option choose right
            comboBox1.SelectedItem = "Вибір правильних";
            //comboBox2 selected option numberOfOptions
            comboBox2.SelectedItem = numberOfOptions.ToString();

            //creating rows
            rowDefinitions = new RowDefinition[2 + numberOfOptions]; 
            for (int i = 0; i < rowDefinitions.Length; i++)
            {
                rowDefinitions[i] = new RowDefinition();
            }
            rowDefinitions[0].Height = new GridLength(50);//це буде к-сть балів
            rowDefinitions[1 + numberOfOptions].Height = new GridLength(50);//це будуть кнопки
            //додавання до гріду
            for (int i = 0; i < rowDefinitions.Length; i++)
            {
                CreateTestWindowGrid.RowDefinitions.Add(rowDefinitions[i]);
            }
            //row 1 -> text of question 
            AddToGrid(1, 2, 2, textBlockQuestion);
            textBoxQuestion.Text = choose.question;//додавання тексту питання
            AddToGrid(1, 4, 2, textBoxQuestion);
            //row 2 -> score
            AddToGrid(2, 2, 2, textBlockScore);
            textBoxScore.Text = choose.score.ToString();//додавання к-сті балів за питання
            AddToGrid(2, 4, textBoxScore);
            // row 4+numberOfOptions -> buttons
            //here I will create new buttons
            btnSaveEdition.Click += btnSaveChangesClick;
            AddToGrid(4+numberOfOptions, 5, btnSaveEdition);
            // rows with options 
            checkBoxes = new CheckBox[numberOfOptions];
            tb1 = new TextBox[numberOfOptions];
            for (int i = 0; i < numberOfOptions; i++)
            {
                //checkBoxes
                checkBoxes[i] = new CheckBox
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    IsChecked = choose.options[i].answer//Забиваємо значеннями true/false з варіанту відповіді
                };
                
                AddToGrid(3 + i, 2, checkBoxes[i]);
                //tb1
                tb1[i] = new TextBox
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 10, 20, 10),
                    Text = choose.options[i].answerOption
                };
                tb1[i].SpellCheck.IsEnabled = true;
                AddToGrid(3 + i, 3, 3, tb1[i]);
            }
            ListOfQuestions();
        }
        private void EditComparison()
        {
            Comparison comparison = ourTest.questions[Index-1] as Comparison;
            numberOfOptions = comparison.options.Count;
            //comboBox1 selected option choose right
            comboBox1.SelectedItem = "Співставлення";
            //comboBox2 selected option numberOfOptions
            comboBox2.SelectedItem = numberOfOptions.ToString();

            //creating rows
            rowDefinitions = new RowDefinition[2 + numberOfOptions]; 
            for (int i = 0; i < rowDefinitions.Length; i++)
            {
                rowDefinitions[i] = new RowDefinition();
            }
            rowDefinitions[0].Height = new GridLength(50);//це буде к-сть балів
            rowDefinitions[1 + numberOfOptions].Height = new GridLength(50);//це будуть кнопки
            //додавання до гріду
            for (int i = 0; i < rowDefinitions.Length; i++)
            {
                CreateTestWindowGrid.RowDefinitions.Add(rowDefinitions[i]);
            }
            //row 1 -> text of question 
            AddToGrid(1, 2, 2, textBlockQuestion);
            textBoxQuestion.Text = comparison.question;//додавання тексту питання
            AddToGrid(1, 4, 2, textBoxQuestion);
            //row 2 -> score
            AddToGrid(2, 2, 2, textBlockScore);
            textBoxScore.Text = comparison.score.ToString();//додавання к-сті балів за питання
            AddToGrid(2, 4, textBoxScore);
            // row 4+numberOfOptions -> buttons
            //here I will create new buttons
            btnSaveEdition.Click += btnSaveChangesClick;
            AddToGrid(4+numberOfOptions, 5, btnSaveEdition);
            // rows with options 
            tb1 = new TextBox[numberOfOptions];
            tb2 = new TextBox[numberOfOptions];
            for (int i = 0; i < numberOfOptions; i++)
            {
                //tb1
                tb1[i] = new TextBox
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new System.Windows.Thickness(20, 10, 50, 10),
                    Text = comparison.options[i].var1
                };
                tb1[i].SpellCheck.IsEnabled = true;
                
                
                //tb2
                tb2[i] = new TextBox
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new System.Windows.Thickness(10, 10, 20, 10),
                    Text = comparison.options[i].var2
                };
                tb2[i].SpellCheck.IsEnabled = true;
                AddToGrid(3 + i, 2, 3, tb1[i]);
                AddToGrid(3 + i, 5, tb2[i]);
            }
            ListOfQuestions();
        }
        
        /// <summary>
        /// Метод обробки події натиску на кнопку збереження питання, що редагувалося
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveChangesClick(object sender, RoutedEventArgs e)
        {
            string question = textBoxQuestion.Text;
            float score;
            try
            {
                score = float.Parse(textBoxScore.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Кількість балів за питання було введено неправильно!", "Помилка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            switch (type)
            {
                case 0:
                    List<varForChooseRightOne> list = new List<varForChooseRightOne>();
                    for (int i = 0; i < numberOfOptions; i++)
                    {
                        bool answer = false;
                        if (checkBoxes[i].IsChecked == true) answer = true;
                        string answerOption = tb1[i].Text;
                        list.Add(new varForChooseRightOne(Index, answerOption, answer));
                    }
                    ourTest.EditQuestion(Index, question, score, list);
                    break;
                case 1:
                    List<varForComparison> list1= new List<varForComparison>();
                    for (int i = 0; i < numberOfOptions; i++)
                    {
                        string var1 = tb1[i].Text;
                        string var2 = tb2[i].Text;
                        list1.Add(new varForComparison(Index, var1, var2));
                    }
                    ourTest.EditQuestion(Index, question, score, list1);
                    break;
            }
            ListOfQuestions();
            CreatingNewQuestion();
        }

        /// <summary>
        /// Обробка події натиску правої кнопки миші на один з елементів списку питань, записує номер питання, на яке було натиснуто
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlockList_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Спочатку ми закинемо елемент, на який ми натиснули, в TextBlock
            TextBlock textBlock = e.Source as TextBlock;
            //Текст даного TextBlock-у є типу "номер. текст питання", тому розбиваємо по крапці і витягуємо номер питаня
            string text = textBlock.Text;
            string[] array = text.Split('.');
            Index = int.Parse(array[0]);
            //MessageBox.Show(Index.ToString());
        }

        /// <summary>
        /// Метод, що за наявності списку питань на формі, видаляє його
        /// </summary>
        private void DeleteListOfQuestions()
        {
            try
            {
                CreateTestWindowGrid.Children.Remove(scroll);
                for (int i = 0; i < textBlockListOfQuestions.Length; i++)
                {
                    StackPanelListOfQuestions.Children.Remove(textBlockListOfQuestions[i]);
                }
            }
            catch (Exception)
            { }
        }

        private void SaveTest_OnClick(object sender, RoutedEventArgs e)
        {
            //перевірка наявності тесту для зберігання
            if (ourTest.questions.Count == 0)
            {
                MessageBox.Show("Ви не можете зберегти тест, який ви не створили", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Test";
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                
                ourTest.SaveTestToFile(fileName);
               // MessageBox.Show("FileName = "+ fileName);
                

            }
        }

        private void OpenTest_Click(object sender, RoutedEventArgs e)
        {
            /*throw new NotImplementedException();*/
            //НЕОБХІДНО БУДЕ ЩЕ ДОДАТИ ПЕРЕВІРКУ ЧИ ТЕСТ (ЯКЩО ВІН Є) ЗБЕРЕЖЕНИЙ, ЗАПИТ НА ЗБЕРЕЖЕННЯ
            
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = ".csv";
            openFile.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            openFile.Title = "Виберіть основний файл тесту, додаткові файли мають міститися в цій же папці";
            if (openFile.ShowDialog() == true)
            {
                /*string fileName = openFile.FileName;*/
                // MessageBox.Show("FileName = "+ openFile.FileName);
                
                ourTest = new Test(openFile.FileName);
                //MessageBox.Show("Тест задано");
                ListOfQuestions();
                CreatingNewQuestion();
            }
            
        }
    }
}
